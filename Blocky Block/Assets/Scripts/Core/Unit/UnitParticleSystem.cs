using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.Core 
{
    public class UnitParticleSystem : MonoBehaviour
    {
        [SerializeField] ParticleSystem m_VfxSleep;
        void Start()
        {
            ToggleParSleep(false);
        }
        public void ToggleParSleep(bool _value)
        {
            m_VfxSleep.gameObject.SetActive(_value);
            if (_value)
            {
                m_VfxSleep.Play();
            }
            else
            {
                m_VfxSleep.Stop();
            }
        }
    }
}