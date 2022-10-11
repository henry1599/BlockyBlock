using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using BlockyBlock.Core;
using DG.Tweening;
using BlockyBlock.Managers;
using BlockyBlock.Configurations;
using Helpers;

namespace BlockyBlock.Core
{
    public class Unit3D : MonoBehaviour
    {
        [SerializeField] UnitAnimation m_Animation;
        [SerializeField] UnitVision m_UnitVision;
        [SerializeField] Transform m_GrabPivot;
        const float m_GrabDelay = 0.25f;
        const float m_UngrabDelay = 0.4f;
        const float m_PushDelay = 0.2f;
        private Vector3 m_StartPosition;
        private UnitDirection m_StartDirection;
        private UnitDirection m_CurrentDirection;
        private int m_CurrentFloor;
        private Vector2 m_CurrentCell;
        private Vector2 m_StartCell;
        private GameObject m_GrabbedObject;
        private bool IsGrabSomething 
        {
            get => m_IsGrabSomething;
            set 
            {
                m_IsGrabSomething = value;
                UpdateGrabStatus(value);
            }
        } bool m_IsGrabSomething;
        // Start is called before the first frame update
        void Start()
        {
            UnitEvents.ON_MOVE_FORWARD += MoveForward;
            UnitEvents.ON_TURN_LEFT += TurnLeft;
            UnitEvents.ON_TURN_RIGHT += TurnRight;
            UnitEvents.ON_PICK_UP += Pickup;
            UnitEvents.ON_PUT_DOWN += Putdown;
            UnitEvents.ON_PUSH += Push;
            UnitEvents.ON_JUMP_IF_GRAB_STH += JumpIfGrabSomething;
            UnitEvents.ON_JUMP_IF_STH_FRONT += JumpIfSthFront;

            UnitEvents.ON_STOP += HandleStop;
            UnitEvents.ON_RESET += HandleReset;
        }
        void OnDestroy()
        {
            UnitEvents.ON_MOVE_FORWARD -= MoveForward;
            UnitEvents.ON_TURN_LEFT -= TurnLeft;
            UnitEvents.ON_TURN_RIGHT -= TurnRight;
            UnitEvents.ON_PICK_UP -= Pickup;
            UnitEvents.ON_PUT_DOWN -= Putdown;
            UnitEvents.ON_PUSH -= Push;
            UnitEvents.ON_JUMP_IF_GRAB_STH -= JumpIfGrabSomething;
            UnitEvents.ON_JUMP_IF_STH_FRONT -= JumpIfSthFront;
            
            UnitEvents.ON_STOP -= HandleStop;
            UnitEvents.ON_RESET -= HandleReset;
        }
        public void Setup(Vector3 _startPosition, UnitDirection _startDirection, int _startX, int _startY)
        {
            m_StartCell = new Vector2(_startX, _startY);
            IsGrabSomething = false;
            m_CurrentCell = m_StartCell;
            m_StartPosition = _startPosition;
            m_StartDirection = _startDirection;
            m_CurrentDirection = _startDirection;

            transform.position = m_StartPosition;
            transform.eulerAngles = ConfigManager.Instance.UnitConfig.GetDataByDirection(_startDirection).Rotation;
        }
        void HandleStop()
        {
            m_Animation.Reset();
        }
        void HandleReset()
        {
            transform.DOKill(true);
            Setup(m_StartPosition, m_StartDirection, (int)m_StartCell.x, (int)m_StartCell.y);
            m_Animation.Reset();
        }
        void UpdateGrabStatus(bool _status)
        {
            if (_status == false)
            {
                m_GrabbedObject = null;
            }
            int weight = _status ? 1 : 0;
            m_Animation.TriggerAnimUpperLayer(weight);
        }

        #region Move Forward
        public void MoveForward(BlockFunctionMoveForward _moveForward)
        { 
            Move();
        }
        void Move()
        {
            DirectionData directionData = ConfigManager.Instance.UnitConfig.GetDataByDirection(m_CurrentDirection);

            GameObject m_Collectible = m_UnitVision.GetFrontObject((int)m_CurrentCell.x, (int)m_CurrentCell.y, m_CurrentFloor, directionData);
            // GameObject m_Collectible = m_UnitVision.GetFrontObject();

            int nextX = (int)m_CurrentCell.x + directionData.XIdx;
            int nextY = (int)m_CurrentCell.y + directionData.YIdx;
            
            if (m_UnitVision.IsWalkable() == false)
            {
                ErrorEvents.ON_ERROR?.Invoke(ErrorType.INVALID_MOVE);
                return;
            }

            m_CurrentCell = new Vector2(nextX, nextY);
            Vector3 newPosition = GridManager.Instance.Grids[m_CurrentFloor].GetWorldPosition((int)m_CurrentCell.x, (int)m_CurrentCell.y);
            float moveTime = ConfigManager.Instance.UnitConfig.MoveTime; 
            if (m_Collectible?.GetComponent<CollectibleObject>() != null)
            {
                StartCoroutine(Collect(m_Collectible.GetComponent<CollectibleObject>(), 0.2f));
            }
            transform 
                .DOMove(
                    newPosition,
                    moveTime
                )
                .SetDelay(0.2f)
                .SetEase(Ease.Linear);
            m_Animation.TriggerAnimRunning();
        }
        IEnumerator Collect(CollectibleObject _collectibleObject, float _delay)
        {
            yield return Helper.GetWait(_delay);
            _collectibleObject.OnCollect();
            UnitEvents.ON_COLLECT_STUFF?.Invoke();
        }
        #endregion

        #region Turn Left
        public void TurnLeft(BlockFunctionTurnLeft _turnLeft)
        {
            switch (m_CurrentDirection)
            {
                case UnitDirection.UP:
                    m_CurrentDirection = UnitDirection.LEFT;
                    break;
                case UnitDirection.LEFT:
                    m_CurrentDirection = UnitDirection.DOWN;
                    break;
                case UnitDirection.RIGHT:
                    m_CurrentDirection = UnitDirection.UP;
                    break;
                case UnitDirection.DOWN:
                    m_CurrentDirection = UnitDirection.RIGHT;
                    break;
            }
            DirectionData directionData = ConfigManager.Instance.UnitConfig.GetDataByDirection(m_CurrentDirection);
            transform
                .DOLocalRotate(
                    directionData.Rotation,
                    ConfigManager.Instance.UnitConfig.RotateTime
                );
            m_Animation.TriggerAnimTurn();
        }
        #endregion

        #region Turn Right
        public void TurnRight(BlockFunctionTurnRight _turnRight)
        {
            
            switch (m_CurrentDirection)
            {
                case UnitDirection.UP:
                    m_CurrentDirection = UnitDirection.RIGHT;
                    break;
                case UnitDirection.LEFT:
                    m_CurrentDirection = UnitDirection.UP;
                    break;
                case UnitDirection.RIGHT:
                    m_CurrentDirection = UnitDirection.DOWN;
                    break;
                case UnitDirection.DOWN:
                    m_CurrentDirection = UnitDirection.LEFT;
                    break;
            }
            DirectionData directionData = ConfigManager.Instance.UnitConfig.GetDataByDirection(m_CurrentDirection);
            transform
                .DOLocalRotate(
                    directionData.Rotation,
                    ConfigManager.Instance.UnitConfig.RotateTime
                );
            m_Animation.TriggerAnimTurn();
        }
        #endregion

        #region Pick up
        void Pickup(BlockFunctionPickup _pickup)
        {
            DoPickup();
        }
        void DoPickup()
        {
            DirectionData directionData = ConfigManager.Instance.UnitConfig.GetDataByDirection(m_CurrentDirection);
            // m_GrabbedObject = m_UnitVision.GetFrontObject((int)m_CurrentCell.x, (int)m_CurrentCell.y, m_CurrentFloor, directionData);
            m_GrabbedObject = m_UnitVision.GetFrontObject();
            if (m_GrabbedObject == null)
            {
                ErrorEvents.ON_ERROR?.Invoke(ErrorType.INVALID_PICK_UP);
            }
            else
            {
                m_Animation.Reset();
                IsGrabSomething = true;
                m_Animation.TriggerAnimPickup();
                StartCoroutine(GrabStuff(m_GrabbedObject.GetComponent<InteractableObject>()));
            }
        }
        IEnumerator GrabStuff(InteractableObject _grabableObject)
        {
            yield return Helper.GetWait(m_GrabDelay);
            _grabableObject.transform.SetParent(m_GrabPivot, true);
            _grabableObject.transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.InOutSine);
            _grabableObject.GrabSelf();
        }
        #endregion

        #region Put down
        void Putdown(BlockFunctionPutdown _putdown)
        {
            DoPutdown();
        }
        void DoPutdown()
        {
            if (IsGrabSomething)
            {
                DirectionData directionData = ConfigManager.Instance.UnitConfig.GetDataByDirection(m_CurrentDirection);
                int putIdxX = (int)m_CurrentCell.x + directionData.XIdx;
                int putIdxY = (int)m_CurrentCell.y + directionData.YIdx;
                
                if (ConfigManager.Instance.BehaviourConfig.BehaviourData[ErrorType.INVALID_PUT_DOWN_PLACE].GroundTypes
                        .Contains(GridManager.Instance.Grids[m_CurrentFloor].GridArray[putIdxX, putIdxY].Type))
                {
                    ErrorEvents.ON_ERROR?.Invoke(ErrorType.INVALID_PUT_DOWN_PLACE);
                    return;
                }
                m_Animation.Reset();
                m_Animation.TriggerAnimPutdown();
                StartCoroutine(ResetGrab());
                StartCoroutine(UnGrabStuff(m_GrabbedObject.GetComponent<InteractableObject>()));
            }
            else
            {
                ErrorEvents.ON_ERROR?.Invoke(ErrorType.INVALID_PUT_DOWN_NOTHING);
            }
        }
        IEnumerator UnGrabStuff(InteractableObject _grabableObject)
        {
            yield return Helper.GetWait(m_UngrabDelay * ConfigManager.Instance.BlockConfig.Blocks[BlockType.PUT_DOWN].ExecutionTime);
            DirectionData directionData = ConfigManager.Instance.UnitConfig.GetDataByDirection(m_CurrentDirection);
            int putIdxX = (int)m_CurrentCell.x + directionData.XIdx;
            int putIdxY = (int)m_CurrentCell.y + directionData.YIdx;
            
            _grabableObject.UngrabSelf(putIdxX, putIdxY, m_CurrentFloor, true);
        }
        IEnumerator ResetGrab()
        {
            yield return Helper.GetWait(0.95f * ConfigManager.Instance.BlockConfig.Blocks[BlockType.PUT_DOWN].ExecutionTime);
            IsGrabSomething = false;
        }
        #endregion
    
        #region Push
        void Push(BlockFunctionPush _push)
        {
            DirectionData directionData = ConfigManager.Instance.UnitConfig.GetDataByDirection(m_CurrentDirection);
            // GameObject pushableObject = m_UnitVision.GetFrontObject((int)m_CurrentCell.x, (int)m_CurrentCell.y, m_CurrentFloor, directionData);
            GameObject pushableObject = m_UnitVision.GetFrontObject();
            if (pushableObject == null)
            {
                ErrorEvents.ON_ERROR?.Invoke(ErrorType.INVALID_PUSH);
            }
            else
            {
                int pushIdxX = (int)m_CurrentCell.x + directionData.XIdx * 2;
                int pushIdxY = (int)m_CurrentCell.y + directionData.YIdx * 2;
                GameObject beyondObject = m_UnitVision.GetBeyondFrontObject();
                if (ConfigManager.Instance.BehaviourConfig.BehaviourData[ErrorType.INVALID_PUSH].GroundTypes
                        .Contains(GridManager.Instance.Grids[m_CurrentFloor].GridArray[pushIdxX, pushIdxY].Type))
                {
                    ErrorEvents.ON_ERROR?.Invoke(ErrorType.INVALID_PUSH);
                    return;
                }
                m_Animation.Reset();
                m_Animation.TriggerAnimPush();
                StartCoroutine(PushStuff(pushableObject.GetComponent<InteractableObject>(), pushIdxX, pushIdxY, m_CurrentFloor));
            }
        }
        IEnumerator PushStuff(InteractableObject _pushableObject, int _pushIdxX, int _pushIdxY, int _floor)
        {
            yield return Helper.GetWait(m_PushDelay);
            
            _pushableObject?.PushSelf(_pushIdxX, _pushIdxY, _floor);
        }
        #endregion

        #region Jump If Grab Something
        void JumpIfGrabSomething(BlockFunctionJumpIfGrabSth _blockFunction)
        {
            UnitEvents.ON_JUMP_IF_GRAB_STH_VALIDATE?.Invoke(_blockFunction, IsGrabSomething);
        }
        #endregion

        #region Jump If Something Front
        void JumpIfSthFront(BlockFunctionJumpIfSthFront _function)
        {
            DirectionData directionData = ConfigManager.Instance.UnitConfig.GetDataByDirection(m_CurrentDirection);

            ConditionDirection direction = _function.Direction;
            int nextX = (int)m_CurrentCell.x;
            int nextY = (int)m_CurrentCell.y;
            switch (direction)
            {
                case ConditionDirection.TOP_LEFT:
                    nextX += -1;
                    nextY += 1;
                    break;
                case ConditionDirection.TOP_RIGHT:
                    nextX += 1;
                    nextY += 1;
                    break;
                case ConditionDirection.CENTER_LEFT:
                    nextX += -1;
                    break;
                case ConditionDirection.CENTER_RIGHT:
                    nextX += 1;
                    break;
                case ConditionDirection.BOTTOM_LEFT:
                    nextX += -1;
                    nextY += -1;
                    break;
                case ConditionDirection.BOTTOM_MID:
                    nextY += -1;
                    break;
                case ConditionDirection.BOTTOM_RIGHT:
                    nextX += 1;
                    nextY += -1;
                    break;
                case ConditionDirection.TOP_MID:
                default:
                    nextY += 1;
                    break;
            }
            // int nextX = (int)m_CurrentCell.x + directionData.XIdx;
            // int nextY = (int)m_CurrentCell.y + directionData.YIdx;

            GroundType frontType = GridManager.Instance.Grids[m_CurrentFloor].GridArray[nextX, nextY].Type;
            GroundType frontGroundCheck = _function.GroundFront;

            bool result = false;
            switch (frontGroundCheck)
            {
                case GroundType.GROUND:
                    frontType = (GroundType)((int)frontType & 0b000111);
                    if (frontType == GroundType.GROUND)
                    {
                        result = true;
                    }
                    break;
                case GroundType.WATER:
                    frontType = (GroundType)((int)frontType & 0b000111);
                    if (frontType == GroundType.WATER)
                    {
                        result = true;
                    }
                    break;
                case GroundType.COLLECTIBLE:
                    frontType = (GroundType)((int)frontType & 0b111000);
                    if (frontType == GroundType.COLLECTIBLE)
                    {
                        result = true;
                    }
                    break;
                case GroundType.SPACE:
                    if (frontType == GroundType.SPACE)
                    {
                        result = true;
                    }
                    break;
                case GroundType.BOX:
                    frontType = (GroundType)((int)frontType & 0b111000);
                    if (frontType == GroundType.BOX)
                    {
                        result = true;
                    }
                    break;
                case GroundType.TRAP:
                    break;
            }
            UnitEvents.ON_JUMP_IF_STH_FRONT_VALIDATE?.Invoke(_function, result);
        }
        #endregion
    }
}
