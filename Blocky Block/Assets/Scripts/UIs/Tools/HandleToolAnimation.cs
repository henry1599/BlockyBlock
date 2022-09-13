using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.Tools
{
    public class HandleToolAnimation : MonoBehaviour
    {
        [SerializeField] Animator m_Anim;
        private bool m_Status = true;
        private static readonly int StatusKeyAnimation = Animator.StringToHash("status");
        public void TriggerAnimStatus()
        {
            m_Status = !m_Status;
            m_Anim.SetBool(StatusKeyAnimation, m_Status);
        }
    }
}
