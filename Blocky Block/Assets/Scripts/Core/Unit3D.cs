using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using BlockyBlock.Core;
using DG.Tweening;
using BlockyBlock.Managers;
using BlockyBlock.Configurations;

namespace BlockyBlock.Core
{
    public class Unit3D : MonoBehaviour
    {
        [SerializeField] UnitAnimation m_Animation;
        private Vector3 m_StartPosition;
        private UnitDirection m_StartDirection;
        private UnitDirection m_CurrentDirection;
        public UnitConfig m_Unit3DConfig;
        // Start is called before the first frame update
        void Start()
        {
            UnitEvents.ON_MOVE_FORWARD += MoveForward;
            UnitEvents.ON_TURN_LEFT += TurnLeft;
            UnitEvents.ON_TURN_RIGHT += TurnRight;

            UnitEvents.ON_STOP += HandleStop;
            UnitEvents.ON_RESET += HandleReset;
        }
        void OnDestroy()
        {
            UnitEvents.ON_MOVE_FORWARD -= MoveForward;
            UnitEvents.ON_TURN_LEFT -= TurnLeft;
            UnitEvents.ON_TURN_RIGHT -= TurnRight;
            
            UnitEvents.ON_STOP -= HandleStop;
            UnitEvents.ON_RESET -= HandleReset;
        }
        public void Setup(Vector3 _startPosition, UnitDirection _startDirection)
        {
            m_StartPosition = _startPosition;
            m_StartDirection = _startDirection;
            m_CurrentDirection = _startDirection;

            transform.position = m_StartPosition;
            transform.eulerAngles = m_Unit3DConfig.GetDataByDirection(_startDirection).Rotation;
        }
        void HandleStop()
        {
            m_Animation.Reset();
        }
        void HandleReset()
        {
            transform.DOKill(true);
            Setup(m_StartPosition, m_StartDirection);
            m_Animation.Reset();
        }
        public void MoveForward(BlockFunctionMoveForward _moveForward)
        {
            DirectionData directionData = m_Unit3DConfig.GetDataByDirection(m_CurrentDirection);
            Vector3 newPosition = transform.position + directionData.MoveDirection * m_Unit3DConfig.StepDistance;
            float moveTime = m_Unit3DConfig.MoveTime; 
            transform 
                .DOMove(
                    newPosition,
                    moveTime
                )
                .SetEase(Ease.Linear);
            m_Animation.TriggerAnimRunning();
        }
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
            DirectionData directionData = m_Unit3DConfig.GetDataByDirection(m_CurrentDirection);
            transform
                .DOLocalRotate(
                    directionData.Rotation,
                    m_Unit3DConfig.RotateTime
                );
            m_Animation.TriggerAnimTurnLeft();
        }
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
            DirectionData directionData = m_Unit3DConfig.GetDataByDirection(m_CurrentDirection);
            transform
                .DOLocalRotate(
                    directionData.Rotation,
                    m_Unit3DConfig.RotateTime
                );
            m_Animation.TriggerAnimTurnRight();
        }
    }
}
