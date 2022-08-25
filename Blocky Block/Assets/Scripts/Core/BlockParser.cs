using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Enums;
using BlockyBlock.Events;

namespace BlockyBlock.Core
{
    public class BlockParser : MonoBehaviour
    {
        Transform m_IDEContent = null;
        public List<BlockFunction> Functions = new List<BlockFunction>();
        public bool IsFinishParse;
        int m_Index;
        public int Index 
        {
            get => m_Index;
            set
            {
                m_Index = value;
                UpdateIndex(value);
            }
        }
        public int FuncCount;
        public bool StopExecution {
            get => m_StopExecution;
            set {
                m_StopExecution = value;
                if (value)
                {
                    UnitEvents.ON_STOP?.Invoke();
                }
            }
        } private bool m_StopExecution = false;
        void UpdateIndex(int _value)
        {
            if (_value >= FuncCount)
            {
                StopExecution = true;
                return;
            }
            if (StopExecution)
            {
                return;
            }
            Functions[_value].Execute();
        }
        public void Parse()
        {
            m_Index = 0;
            Functions.Clear();
            IsFinishParse = false;
            StopExecution = false;
            StartCoroutine(Cor_Parse());
        }
        public void Stop() 
        {
            StartCoroutine(Cor_Stop());
        }
        public void Debug()
        {
            StartCoroutine(Cor_Debug());
        }
        IEnumerator Cor_Stop()
        {
            Functions.Clear();
            IsFinishParse = false;
            StopExecution = true;
            yield return new WaitForSeconds(0.5f);
            GameEvents.ON_CONTROL_BUTTON_TOGGLE_ALL?.Invoke(false);
        }
        IEnumerator Cor_Debug()
        {
            yield return new WaitForSeconds(0.25f);
            GameEvents.ON_CONTROL_BUTTON_TOGGLE_ALL?.Invoke(false);
        }
        IEnumerator Cor_GetIDE()
        {
            m_IDEContent = GameObject.FindGameObjectWithTag(GameConstants.IDE_CONTENT_TAG).transform;
            yield return new WaitUntil(() => m_IDEContent != null);
        }
        IEnumerator Cor_Parse()
        {
            yield return Cor_GetIDE();
            FuncCount = m_IDEContent.childCount - 1;
            foreach (Transform child in m_IDEContent)
            {
                UIBlock uiBlock = child.GetComponent<UIBlock>();
                if (uiBlock != null)
                {
                    // Parse here
                    FilterType(uiBlock);
                }
                else
                {
                    continue;
                }
            }
            StopExecution = FuncCount <= 0;
            m_Index = 0;
            IsFinishParse = true;
        }
        void FilterType(UIBlock _uiBlock)
        {
            switch (_uiBlock.Type)
            {
                case BlockType.MOVE_FORWARD:
                    HandleMoveForward((UIBlockMove)_uiBlock);
                    break;
                case BlockType.TURN_LEFT:
                    HandleTurnLeft((UIBlockTurnLeft)_uiBlock);
                    break;
                case BlockType.TURN_RIGHT:
                    HandleTurnRight((UIBlockTurnRight)_uiBlock);
                    break;
                case BlockType.PICK_UP:
                    break;
                case BlockType.PUT_DOWN:
                    break;
                case BlockType.DO_UNTIL:
                    break;
            }
        }
        void HandleMoveForward(UIBlockMove _uiBlock)
        {
            BlockFunctionMoveForward function = new BlockFunctionMoveForward(_uiBlock);
            function.Setup();
            Functions.Add(function);
        }
        void HandleTurnLeft(UIBlockTurnLeft _uiBlock)
        {
            BlockFunctionTurnLeft function = new BlockFunctionTurnLeft(_uiBlock);
            function.Setup();
            Functions.Add(function);
        }
        void HandleTurnRight(UIBlockTurnRight _uiBlock)
        {
            BlockFunctionTurnRight function = new BlockFunctionTurnRight(_uiBlock);
            function.Setup();
            Functions.Add(function);
        }
        void HandleDoUntil(UIBlock _uiBlock)
        {
            
        }
    }
}
