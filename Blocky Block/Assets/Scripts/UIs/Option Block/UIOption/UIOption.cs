using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockyBlock.UI;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using DG.Tweening;
using Helpers;
using Helpers.BlockyBlock;

namespace BlockyBlock.UI 
{
    public class UIOption : MonoBehaviour
    {
        public BlockType m_Type;
        public float DropDuration;
        public float HeightForEachOption = 45;
        public RectTransform m_TopPanel;
        public RectTransform m_BelowPanel;
        public UIOptionItem UIOptionItemTemplate;
        public virtual void Setup(bool _status, Transform _rectSnapTo, List<string> _optionStrings)
        {
            ShowData(_optionStrings, m_Type);
            if (_status)
            {
                // m_ThisRect.anchoredPosition = _rectSnapTo.anchoredPosition;
                transform.position = _rectSnapTo.position;
                transform.DOScaleY(1, DropDuration).SetEase(Ease.OutBack);
            }
            else
            {
                transform.DOScaleY(0, DropDuration).SetEase(Ease.InBack);
            }
        }
        public virtual void ShowData(List<string> _optionStrings, BlockType _type)
        {
            float actualHeight = _optionStrings.Count * HeightForEachOption;

            Vector2 sizeDelta = new Vector2(m_TopPanel.sizeDelta.x, actualHeight);
            m_TopPanel.sizeDelta = sizeDelta;
            m_BelowPanel.sizeDelta = sizeDelta;

            Helper.DeletChildren(m_TopPanel.transform);

            foreach (string optionString in _optionStrings)
            {
                UIOptionItem itemInstance = Instantiate(UIOptionItemTemplate.gameObject, m_TopPanel.transform).GetComponent<UIOptionItem>();
                if (_type == BlockType.TURN)
                {
                    itemInstance.SetText((int)HelperBlockyBlock.StringToTurnDirection(optionString), optionString);
                }
                else if (_type == BlockType.JUMP_IF_STH_FRONT)
                {
                    itemInstance.SetText((int)HelperBlockyBlock.StringToGroundType(optionString), optionString);
                }
            }
        }
    }
}
