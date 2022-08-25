using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using BlockyBlock.Core;
using DG.Tweening;
using BlockyBlock.Managers;

namespace BlockyBlock.Core
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] UnitAnimation m_UnitAnimation;
        private Vector3 m_StartPosition;
        private UnitDirection m_StartDirection;
        private UnitDirection m_CurrentDirection;
        // Start is called before the first frame update
        void Start()
        {
            UnitEvents.ON_MOVE_FORWARD += MoveForward;
            UnitEvents.ON_TURN_LEFT += TurnLeft;
            UnitEvents.ON_TURN_RIGHT += TurnRight;
        }
        void OnDestroy()
        {
            UnitEvents.ON_MOVE_FORWARD -= MoveForward;
            UnitEvents.ON_TURN_LEFT -= TurnLeft;
            UnitEvents.ON_TURN_RIGHT -= TurnRight;
        }
        public void Setup(Vector3 _startPosition, UnitDirection _startDirection)
        {
            m_StartPosition = _startPosition;
            m_StartDirection = _startDirection;
            m_CurrentDirection = _startDirection;

            transform.position = m_StartPosition;
            if (m_UnitAnimation == null)
            {
                Debug.LogError("There is no UnitAnimation component reference to this object");
                return;
            }
            m_UnitAnimation.SetDirection(m_StartDirection);
        }
        public void MoveForward(BlockFunctionMoveForward _moveForward)
        {
            Vector3 newPosition = transform.position;
            float distanceStep = UnitManager.Instance.DistanceStepValue;
            float moveTime = UnitManager.Instance.UnitMoveTime;
            switch (m_CurrentDirection)
            {
                case UnitDirection.UP:
                    newPosition = new Vector3(
                        transform.position.x,
                        transform.position.y + distanceStep,
                        transform.position.z
                    );
                    break;
                case UnitDirection.LEFT:
                    newPosition = new Vector3(
                        transform.position.x - distanceStep,
                        transform.position.y,
                        transform.position.z
                    );
                    break;
                case UnitDirection.RIGHT:
                    newPosition = new Vector3(
                        transform.position.x + distanceStep,
                        transform.position.y,
                        transform.position.z
                    );
                    break;
                case UnitDirection.DOWN:
                    newPosition = new Vector3(
                        transform.position.x,
                        transform.position.y - distanceStep,
                        transform.position.z
                    );
                    break;
            }
            m_UnitAnimation.TriggerMovingBool(true);
            transform 
                .DOMove(
                    newPosition,
                    moveTime
                )
                .SetEase(Ease.Linear);
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
            m_UnitAnimation.SetDirection(m_CurrentDirection);
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
            m_UnitAnimation.SetDirection(m_CurrentDirection);
        }
    }
}
