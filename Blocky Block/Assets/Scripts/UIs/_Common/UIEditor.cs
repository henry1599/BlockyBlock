using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;

namespace BlockyBlock.Managers
{
    public class UIEditor : MonoBehaviour
    {
        [SerializeField] Animator m_Animator;
        bool m_Status = true;
        private static readonly int ShowKeyAnimation = Animator.StringToHash("show");
        // Start is called before the first frame update
        void Start()
        {
            EditorEvents.ON_PREVIEW_STATUS_TOGGLE += HandlePreviewStatusToggle;   
            EditorEvents.ON_FORCE_PREVIEW_STATUS_TOGGLE += HandlePreviewStatusForceToggle;         
        }
        void OnDestroy()
        {
            EditorEvents.ON_PREVIEW_STATUS_TOGGLE -= HandlePreviewStatusToggle;
            EditorEvents.ON_FORCE_PREVIEW_STATUS_TOGGLE -= HandlePreviewStatusForceToggle;
        }
        void HandlePreviewStatusToggle()
        {
            m_Status = !m_Status;
            m_Animator.SetBool(ShowKeyAnimation, m_Status);
        }

        void HandlePreviewStatusForceToggle(bool _status)
        {
            m_Status = _status;
            m_Animator.SetBool(ShowKeyAnimation, m_Status);
        }
    }
}
