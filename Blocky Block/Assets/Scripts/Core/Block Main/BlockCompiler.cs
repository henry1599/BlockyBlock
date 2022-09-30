using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Managers;
using BlockyBlock.Events;
using BlockyBlock.UI;
using BlockyBlock.Enums;

namespace BlockyBlock.Core
{
    public class BlockCompiler : MonoBehaviour
    {
        public static BlockCompiler Instance {get; private set;}
        [SerializeField] BlockParser m_Parser;
        public IDERunState IDEState
        {
            get => m_IDEState;
            set 
            {
                m_IDEState = value;
                EditorEvents.ON_BLOCK_EDITOR?.Invoke(m_IDEState != IDERunState.STOP);
            }
        } private IDERunState m_IDEState;
        public bool IsExecuting = false;
        int m_IdxFunction = 0;
        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            UnitEvents.ON_JUMP += HandleJump;
            ErrorEvents.ON_ERROR_HANDLING += HandleError;

            GameEvents.ON_WIN += HandleStopRunning;
            GameEvents.ON_LOSE += HandleStopRunning;
        }
        void OnDestroy()
        {
            UnitEvents.ON_JUMP -= HandleJump;
            ErrorEvents.ON_ERROR_HANDLING -= HandleError;

            GameEvents.ON_WIN -= HandleStopRunning;
            GameEvents.ON_LOSE -= HandleStopRunning;
        }
        void HandleStopRunning()
        {
            StopCoroutine(Cor_Play());
            StopCoroutine(Cor_Debug());
            IsExecuting = false;
            GameEvents.ON_EXECUTING_BLOCK?.Invoke(IsExecuting);
            IDEState = IDERunState.STOP;
            BlockEvents.ON_HIGHLIGHT?.Invoke(null, IDEState);
            m_IdxFunction = 0;
            m_Parser.HandleError();
        }
        void HandleJump(BlockFunctionJump _uiBlockJump)
        {
            m_IdxFunction = _uiBlockJump.IdxJumpTo;
        }
        public void Play() 
        {
            IsExecuting = true;
            GameEvents.ON_EXECUTING_BLOCK?.Invoke(IsExecuting);
            IDEState = IDERunState.MANNUAL;
            StartCoroutine(Cor_Play());
        }
        void HandleError()
        {
            StopCoroutine(Cor_Play());
            StopCoroutine(Cor_Debug());
            IsExecuting = false;
            GameEvents.ON_EXECUTING_BLOCK?.Invoke(IsExecuting);
            IDEState = IDERunState.STOP;
            BlockEvents.ON_HIGHLIGHT?.Invoke(null, IDEState);
            m_IdxFunction = 0;
            m_Parser.HandleError();
        }
        public void Stop() 
        {
            IsExecuting = false;
            GameEvents.ON_EXECUTING_BLOCK?.Invoke(IsExecuting);
            IDEState = IDERunState.STOP;
            BlockEvents.ON_HIGHLIGHT?.Invoke(null, IDEState);
            StopCoroutine(Cor_Play());
            StopCoroutine(Cor_Debug());
            m_IdxFunction = 0;
            m_Parser.Stop();
            UnitEvents.ON_RESET?.Invoke();
        }
        public void Debug() 
        {
            StopCoroutine(Cor_Play());
            IsExecuting = true;
            m_Parser.Debug();
            IDEState = IDERunState.DEBUGGING;
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
            IDEState = IDERunState.STOP;
            BlockEvents.ON_HIGHLIGHT?.Invoke(null, IDEState);
            m_IdxFunction = 0;
        }
    }
}
