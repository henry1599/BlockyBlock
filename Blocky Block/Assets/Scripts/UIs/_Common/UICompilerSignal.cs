using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BlockyBlock.Events;

namespace BlockyBlock.UI
{
    public class UICompilerSignal : MonoBehaviour
    {
        [SerializeField] Transform m_PlayMode, m_DebugMode;
        void Start()
        {
            UnitEvents.ON_RESET += HandleResetIDE;
        }
        void OnDestroy()
        {
            UnitEvents.ON_RESET -= HandleResetIDE;
        }
        void HandleResetIDE()
        {
            m_DebugMode.localScale = Vector3.zero;
            m_PlayMode.localScale = Vector3.zero;
        }
        public void TogglePlayMode()
        {
            m_PlayMode.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            m_DebugMode.DOScale(Vector3.zero, 0.1f).SetEase(Ease.Linear);
        }
        public void ToggleDebugMode()
        {
            m_DebugMode.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            m_PlayMode.DOScale(Vector3.zero, 0.1f).SetEase(Ease.Linear);
        }
        public void DisableSelf()
        {
            m_DebugMode.DOScale(Vector3.zero, 0.1f).SetEase(Ease.Linear);
            m_PlayMode.DOScale(Vector3.zero, 0.1f).SetEase(Ease.Linear);
        }
    }
}
