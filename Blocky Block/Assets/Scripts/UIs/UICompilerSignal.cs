using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BlockyBlock.UI
{
    public class UICompilerSignal : MonoBehaviour
    {
        [SerializeField] Transform m_PlayMode, m_DebugMode;
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
