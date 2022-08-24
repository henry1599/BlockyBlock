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
                if (m_Index >= FuncCount)
                {
                    StopExecution = true;
                    GameEvents.ON_CONTROL_BUTTON_TOGGLE?.Invoke(ControlButton.PLAY, false);
                    GameEvents.ON_CONTROL_BUTTON_TOGGLE?.Invoke(ControlButton.STOP, false);
                    GameEvents.ON_CONTROL_BUTTON_TOGGLE?.Invoke(ControlButton.DEBUG, false);
                    return;
                }
                if (StopExecution)
                {
                    return;
                }
                Functions[value].Execute();
            }
        }
        public int FuncCount;
        public bool StopExecution = false;
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
        IEnumerator Cor_Stop()
        {
            Functions.Clear();
            IsFinishParse = false;
            StopExecution = true;
            yield return new WaitForSeconds(0.5f);
            GameEvents.ON_CONTROL_BUTTON_TOGGLE?.Invoke(ControlButton.PLAY, false);
            GameEvents.ON_CONTROL_BUTTON_TOGGLE?.Invoke(ControlButton.STOP, false);
            GameEvents.ON_CONTROL_BUTTON_TOGGLE?.Invoke(ControlButton.DEBUG, false);
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
                    Functions.Add(
                        new BlockFunctionMoveForward(
                            _uiBlock
                        )
                    );
                    break;
                case BlockType.TURN_LEFT:
                    Functions.Add(
                        new BlockFunctionTurnLeft(
                            _uiBlock
                        )
                    );
                    break;
                case BlockType.TURN_RIGHT:
                    Functions.Add(
                        new BlockFunctionTurnRight(
                            _uiBlock
                        )
                    );
                    break;
                case BlockType.PICK_UP:
                    break;
                case BlockType.PUT_DOWN:
                    break;
                case BlockType.DO_UNTIL:
                    break;
            }
        }
    }
}
