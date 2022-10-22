using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BlockyBlock.Events;

namespace BlockyBlock.UI 
{
    public enum HomeState
    {
        MAIN = 0,
        LEVEL_TYPE_SELECTION = 1
    }
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

        [Space(10)]
        [Header("Level Section")]
        [SerializeField] Transform m_MannualLevel;
        [SerializeField] Transform m_CustomLevel;
        [SerializeField] Transform m_BackButton;
        [SerializeField] float m_LevelSectionTransitionDuration;
        [SerializeField] Vector2 m_BackButtonShowPosition;
        [SerializeField] Vector2 m_BackButtonHidePosition;
        [SerializeField] Vector2 m_MannualLevelShowPosition;
        [SerializeField] Vector2 m_MannualLevelHidePosition;
        [SerializeField] Vector2 m_CustomLevelShowPosition;
        [SerializeField] Vector2 m_CustomLevelHidePosition;
        [SerializeField] Transform m_Camera;
        [SerializeField] float m_ShowCameraX;
        [SerializeField] float m_HideCameraX;
        void Awake()
        {
            HandleStageChanged(HomeState.MAIN);
        }
        void Start()
        {
            HomeEvents.ON_STAGE_CHANGED += HandleStageChanged;
        }
        void OnDestroy()
        {
            HomeEvents.ON_STAGE_CHANGED -= HandleStageChanged;
        }
        void HandleStageChanged(HomeState _state)
        {
            switch (_state)
            {
                case HomeState.MAIN:
                    HandleHideLevelSection(
                        () => HandleShowMainButtons(),
                        m_LevelSectionTransitionDuration
                    );
                    break;
                case HomeState.LEVEL_TYPE_SELECTION:
                    HandleHideMainButtons(
                        () => HandleShowLevelSection(),
                        m_AvatarDelayTransition + m_AvatarTransitionDuration
                    );
                    break;
            }
        }
        void HandleShowLevelSection(System.Action _cb = null, float _delay = 0)
        {
            m_MannualLevel.GetComponent<RectTransform>()
                .DOAnchorPos(m_MannualLevelShowPosition, m_LevelSectionTransitionDuration)
                .SetEase(Ease.OutBack);
            m_CustomLevel.GetComponent<RectTransform>()
                .DOAnchorPos(m_CustomLevelShowPosition, m_LevelSectionTransitionDuration)
                .SetEase(Ease.OutBack);
            m_BackButton.GetComponent<RectTransform>()
                .DOAnchorPos(m_BackButtonShowPosition, m_LevelSectionTransitionDuration)
                .SetEase(Ease.OutBack);
            m_Camera
                .DOMoveX(m_ShowCameraX, m_LevelSectionTransitionDuration)
                .SetEase(Ease.OutBack);
            DOVirtual.DelayedCall(_delay, () => _cb?.Invoke());
        }
        void HandleHideLevelSection(System.Action _cb = null, float _delay = 0)
        {
            m_MannualLevel.GetComponent<RectTransform>()
                .DOAnchorPos(m_MannualLevelHidePosition, m_LevelSectionTransitionDuration)
                .SetEase(Ease.InBack);
            m_CustomLevel.GetComponent<RectTransform>()
                .DOAnchorPos(m_CustomLevelHidePosition, m_LevelSectionTransitionDuration)
                .SetEase(Ease.InBack);
            m_BackButton.GetComponent<RectTransform>()
                .DOAnchorPos(m_BackButtonHidePosition, m_LevelSectionTransitionDuration)
                .SetEase(Ease.InBack);
            m_Camera
                .DOMoveX(m_HideCameraX, m_LevelSectionTransitionDuration)
                .SetEase(Ease.OutBack);
            DOVirtual.DelayedCall(_delay, () => _cb?.Invoke());
        }
        void HandleShowMainButtons(System.Action _cb = null, float _delay = 0)
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
            DOVirtual.DelayedCall(_delay, () => _cb?.Invoke());
        }

        void HandleHideMainButtons(System.Action _cb = null, float _delay = 0)
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
            DOVirtual.DelayedCall(_delay, () => _cb?.Invoke());
        }
    }
}
