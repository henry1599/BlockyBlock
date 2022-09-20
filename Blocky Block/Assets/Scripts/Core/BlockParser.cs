using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.UI;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using BlockyBlock.Configurations;
using BlockyBlock.Managers;

namespace BlockyBlock.Core
{
    public class BlockParser : MonoBehaviour
    {
        Transform m_IDEContent = null;
        public List<BlockFunction> Functions {get; set;} = new List<BlockFunction>();
        public bool IsFinishParse {get; set;}
        public float DelayTime {get; set;} = 0;
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
        public int FuncCount {get; set;}
        public bool StopExecution {
            get => m_StopExecution;
            set {
                m_StopExecution = value;
                if (value)
                {
                    BlockCompiler.Instance.IsExecuting = false;
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
            DelayTime = ConfigManager.Instance.BlockConfig.GetDelayTime(Functions[_value].BlockType);
            Functions[_value].Execute();
            BlockEvents.ON_HIGHLIGHT?.Invoke(Functions[_value].UIBlock, BlockCompiler.Instance.IDEState);
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
            yield return new WaitForSeconds(DelayTime);
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
                case BlockType.TURN:
                    HandleTurn((UIBlockTurn)_uiBlock);
                    break;
                case BlockType.PICK_UP:
                    break;
                case BlockType.PUT_DOWN:
                    break;
                case BlockType.DO_UNTIL:
                    break;
                case BlockType.JUMP:
                    HandleJump((UIBlockJump)_uiBlock);
                    break;
                case BlockType.SKIP:
                    HandleSkip((UIBlockSkip)_uiBlock);
                    break;
            }
        }
        void HandleMoveForward(UIBlockMove _uiBlock)
        {
            BlockFunctionMoveForward function = new BlockFunctionMoveForward(_uiBlock);
            function.Setup();
            Functions.Add(function);
        }
        void HandleTurn(UIBlockTurn _uiBlock)
        {
            UIBlockOptionTurn uiBlockOptionTurn = (UIBlockOptionTurn)_uiBlock.UIBlockOption;
            TurnDirection turnDirection = uiBlockOptionTurn.CurrentTurnDirection;
            
            switch (turnDirection)
            {
                case TurnDirection.LEFT:
                    BlockFunctionTurnLeft functionTurnLeft = new BlockFunctionTurnLeft(_uiBlock);
                    functionTurnLeft.Setup();
                    Functions.Add(functionTurnLeft);
                    break;
                case TurnDirection.RIGHT:
                    BlockFunctionTurnRight functionTurnRight = new BlockFunctionTurnRight(_uiBlock);
                    functionTurnRight.Setup();
                    Functions.Add(functionTurnRight);
                    break;
            }

        }
        void HandleJump(UIBlockJump _uiBlock)
        {
            BlockFunctionJump function = new BlockFunctionJump(_uiBlock);
            function.Setup();
            Functions.Add(function);
        }
        void HandleSkip(UIBlockSkip _uiBlock)
        {
            BlockFunctionSkip function = new BlockFunctionSkip(_uiBlock);
            function.Setup();
            Functions.Add(function);
        }
        void HandleDoUntil(UIBlock _uiBlock)
        {
            
        }
    }
}
