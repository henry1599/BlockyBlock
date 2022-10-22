using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BlockyBlock.UI 
{
    public class UIHomeAnimation : MonoBehaviour
    {
        [Header("Main Button Field")]
        [Tooltip("List of main buttons on Home scene, from play to exit")]
        [SerializeField] Transform[] m_MainButtons;
        [SerializeField] float m_MainButtonTransitionDuration;
        [SerializeField] float m_MainButtonDelayTransition;
        [SerializeField] float m_MainButtonShowScale;
        [SerializeField] float m_MainButtonHideScale;

        [Space(10)]
        [Header("Avatar Field")]
        [SerializeField] Transform m_Avatar;
        [SerializeField] float m_AvatarTransitionDuration;
        [SerializeField] float m_AvatarDelayTransition;
        [SerializeField] Vector2 m_AvatarShowPosition;
        [SerializeField] Vector2 m_AvatarHidePosition;

        [Space(10)]
        [Header("Coin Field")]
        [SerializeField] Transform m_Coin;
        [SerializeField] float m_CoinTransitionDuration;
        [SerializeField] float m_CoinDelayTransition;
        [SerializeField] Vector2 m_CoinShowPosition;
        [SerializeField] Vector2 m_CoinHidePosition;

        public static event System.Action<bool> ON_STATUS_CHANGED;
        void Awake()
        {
            HandleShow();
        }
        void Start()
        {
            UIHomeAnimation.ON_STATUS_CHANGED += HandleStatusChanged;
        }
        void OnDestroy()
        {
            UIHomeAnimation.ON_STATUS_CHANGED -= HandleStatusChanged;
        }
        void HandleStatusChanged(bool _status)
        {
            if (_status)
                HandleShow();
            else
                HandleHide();
        }

        void HandleShow()
        {
            // * Main buttons
            int mainBtnIdx = 0;
            foreach (Transform tf in m_MainButtons)
            {
                tf
                    .DOScale(Vector3.one * m_MainButtonShowScale, m_MainButtonTransitionDuration)
                    .SetDelay(m_MainButtonDelayTransition * mainBtnIdx)
                    .SetEase(Ease.OutBack);
                mainBtnIdx ++;
            }

            // * Avatar
            m_Avatar.GetComponent<RectTransform>()
                .DOAnchorPos(m_AvatarShowPosition, m_AvatarTransitionDuration)
                .SetDelay(m_AvatarDelayTransition)
                .SetEase(Ease.OutBack);

            // * Coin
            m_Coin.GetComponent<RectTransform>()
                .DOAnchorPos(m_CoinShowPosition, m_CoinTransitionDuration)
                .SetDelay(m_CoinDelayTransition)
                .SetEase(Ease.OutBack);
        }

        void HandleHide()
        {
            // * Main buttons
            int mainBtnIdx = 0;
            foreach (Transform tf in m_MainButtons)
            {
                tf
                    .DOScale(Vector3.one * m_MainButtonHideScale, m_MainButtonTransitionDuration)
                    .SetDelay(m_MainButtonDelayTransition * mainBtnIdx)
                    .SetEase(Ease.InBack);
                mainBtnIdx ++;
            }

            // * Avatar
            m_Avatar.GetComponent<RectTransform>()
                .DOAnchorPos(m_AvatarHidePosition, m_AvatarTransitionDuration)
                .SetDelay(m_AvatarDelayTransition)
                .SetEase(Ease.InBack);

            // * Coin
            m_Coin.GetComponent<RectTransform>()
                .DOAnchorPos(m_CoinHidePosition, m_CoinTransitionDuration)
                .SetDelay(m_CoinDelayTransition)
                .SetEase(Ease.InBack);
        }
    }
}
