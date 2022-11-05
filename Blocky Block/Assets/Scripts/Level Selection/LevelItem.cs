using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using BlockyBlock.Managers;
using BlockyBlock.Utils;
using BlockyBlock.Events;
using DG.Tweening;
using EPOOutline;

namespace BlockyBlock
{
    public class LevelItem : MonoBehaviour
    {
        [SerializeField] TMP_Text m_LevelNameDisplayText;
        [SerializeField] Vector3 m_InitBounds;
        [SerializeField] BoxCollider m_BoxCol;
        [SerializeField] Animator m_Animator;
        private static readonly int[] SelectionKeys = new int[10]
        {
            Animator.StringToHash("Selection 00"),
            Animator.StringToHash("Selection 01"),
            Animator.StringToHash("Selection 02"),
            Animator.StringToHash("Selection 03"),
            Animator.StringToHash("Selection 04"),
            Animator.StringToHash("Selection 05"),
            Animator.StringToHash("Selection 06"),
            Animator.StringToHash("Selection 07"),
            Animator.StringToHash("Selection 08"),
            Animator.StringToHash("Selection 09")
        };
        private int m_LevelID;
        bool m_IsHighlighted = false;
        void Start()
        {
            LevelSelectionEvents.ON_HIGHLIGHT_ITEM += HandleHighlight;
        }
        void OnDestroy()
        {
            LevelSelectionEvents.ON_HIGHLIGHT_ITEM -= HandleHighlight;
        }
        void HandleHighlight(LevelItem _item)
        {
            if (_item != this)
            {
                Unhighlight();
            }
            else
            {
                Highlight();
            }
        }
        void OnMouseDown()
        {
            if (!m_IsHighlighted)
            {
                LevelSelectionEvents.ON_HIGHLIGHT_ITEM?.Invoke(this);
                return;
            }
            OnClick();
        }
        void OnMouseEnter()
        {
            PlayRandomAnim();
            LevelSelectionEvents.ON_ITEM_HOVER?.Invoke(true);
        }
        void OnMouseExit()
        {
            LevelSelectionEvents.ON_ITEM_HOVER?.Invoke(false);
        }
        void Highlight()
        {
            if (m_IsHighlighted) return;
            m_IsHighlighted = true;
            PlayRandomAnim();
            transform.DOKill();
            transform 
                .DOScale(
                    Vector3.one * 2,
                    0.15f
                )
                .SetEase(Ease.InOutSine)
                .OnComplete(() => m_BoxCol.size = m_InitBounds / 2.2f);
            LevelMenuManager.Instance.Scroller.SnapTo(this);
        }
        void Unhighlight()
        {
            if (!m_IsHighlighted) return;
            m_IsHighlighted = false;
            transform.DOKill();
            transform 
                .DOScale(
                    Vector3.one,
                    0.15f
                )
                .SetEase(Ease.InOutSine)
                .OnComplete(() => m_BoxCol.size = m_InitBounds);
        }
        void PlayRandomAnim()
        {
            m_Animator.CrossFade(SelectionKeys[Random.Range(0, SelectionKeys.Length)], 0, 0);
        }
        public void Setup(LevelID _levelId, LevelItemData _data)
        {
            m_LevelID = (int)_levelId;
            m_LevelNameDisplayText.text = _data.LevelNameDisplay;
        }
        public void OnClick()
        {
            UIUtils.LockInput();
            GameManager.Instance.TransitionIn(() => 
                {
                    UIUtils.UnlockInput();
                    GameEvents.LOAD_LEVEL?.Invoke((LevelID)m_LevelID);
                }
            );
        }
    }
}
