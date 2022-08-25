using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Managers;

namespace BlockyBlock.Core
{
    public class BlockCompiler : MonoBehaviour
    {
        [SerializeField] BlockParser m_Parser;
        float m_RunInterval;
        int m_IdxFunction = 0;
        void Start()
        {
            m_RunInterval = UnitManager.Instance.UnitMoveTime;
        }
        public void Play() 
        {
            StartCoroutine(Cor_Play());
        }
        public void Stop() 
        {
            m_IdxFunction = 0;
            m_Parser.Stop();
        }
        public void Debug() 
        {

        }
        IEnumerator Cor_Play()
        {
            m_Parser.Parse();
            yield return new WaitUntil(() => m_Parser.IsFinishParse == true);
            while (!m_Parser.StopExecution)
            {
                m_Parser.Index = m_IdxFunction;
                m_IdxFunction ++;
                yield return new WaitForSeconds(m_RunInterval);
            }
            m_IdxFunction = 0;
        }
    }
}
