using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Managers;
using BlockyBlock.Events;

namespace BlockyBlock.Core
{
    public class BlockCompiler : MonoBehaviour
    {
        [SerializeField] BlockParser m_Parser;
        int m_IdxFunction = 0;
        void Start()
        {
            UnitEvents.ON_JUMP += HandleJump;
        }
        void OnDestroy()
        {
            UnitEvents.ON_JUMP -= HandleJump;
        }
        void HandleJump(BlockFunctionJump _uiBlockJump)
        {
            m_IdxFunction = _uiBlockJump.IdxJumpTo;
        }
        public void Play() 
        {
            StartCoroutine(Cor_Play());
        }
        public void Stop() 
        {
            StopCoroutine(Cor_Play());
            StopCoroutine(Cor_Debug());
            m_IdxFunction = 0;
            m_Parser.Stop();
            UnitEvents.ON_RESET?.Invoke();
        }
        public void Debug() 
        {
            m_Parser.Debug();
            if (m_Parser.IsFinishParse == false)
            {
                m_Parser.Parse();
            }
            StartCoroutine(Cor_Debug());
        }
        IEnumerator Cor_Debug()
        {
            yield return new WaitUntil(() => m_Parser.IsFinishParse == true);
            m_Parser.Index = m_IdxFunction;
            m_IdxFunction ++;
        }
        IEnumerator Cor_Play()
        {
            m_Parser.Parse();
            yield return new WaitUntil(() => m_Parser.IsFinishParse == true);
            while (!m_Parser.StopExecution)
            {
                m_Parser.Index = m_IdxFunction;
                m_IdxFunction ++;
                yield return new WaitForSeconds(m_Parser.DelayTime);
            }
            m_IdxFunction = 0;
        }
    }
}
