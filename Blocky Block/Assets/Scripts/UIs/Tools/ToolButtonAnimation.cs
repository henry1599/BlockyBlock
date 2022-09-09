using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.Tools 
{
    public class ToolButtonAnimation : MonoBehaviour
    {
        public static readonly int StatusKeyAnimation = Animator.StringToHash("status");
        [SerializeField] Animator m_Aimator;
        public void ToggleAnimStatus(bool _status)
        {
            m_Aimator?.SetBool(StatusKeyAnimation, _status);
        }
    }
}
