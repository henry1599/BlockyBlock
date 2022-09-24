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
        private int m_CurrentFloor;
        private bool m_IsUnderwater = false;
        private Vector2 m_CurrentCell;
        private Vector2 m_StartCell;
        // Start is called before the first frame update
        void Start()
        {
            UnitEvents.ON_MOVE_FORWARD += MoveForward;
            UnitEvents.ON_TURN_LEFT += TurnLeft;
            UnitEvents.ON_TURN_RIGHT += TurnRight;
            UnitEvents.ON_PICK_UP += Pickup;
            UnitEvents.ON_PUT_DOWN += Putdown;

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
            
            UnitEvents.ON_STOP -= HandleStop;
            UnitEvents.ON_RESET -= HandleReset;
        }
        public void Setup(Vector3 _startPosition, UnitDirection _startDirection, int _startX, int _startY)
        {
            m_StartCell = new Vector2(_startX, _startY);
            m_CurrentCell = m_StartCell;
            m_StartPosition = _startPosition;
            m_StartDirection = _startDirection;
            m_CurrentDirection = _startDirection;

            transform.position = m_StartPosition;
            transform.eulerAngles = ConfigManager.Instance.UnitConfig.GetDataByDirection(_startDirection).Rotation;
        }
        void HandleStop()
        {
            m_Animation.Reset(m_IsUnderwater);
        }
        void HandleReset()
        {
            transform.DOKill(true);
            Setup(m_StartPosition, m_StartDirection, (int)m_StartCell.x, (int)m_StartCell.y);
            m_IsUnderwater = false;
            m_Animation.Reset(m_IsUnderwater);
        }

        #region Move Forward
        public void MoveForward(BlockFunctionMoveForward _moveForward)
        {
            DirectionData directionData = ConfigManager.Instance.UnitConfig.GetDataByDirection(m_CurrentDirection);
            m_CurrentCell = new Vector2(m_CurrentCell.x + directionData.XIdx, m_CurrentCell.y + directionData.YIdx);
            Move();
        }
        void Move()
        {
            Vector3 newPosition = GridManager.Instance.Grids[m_CurrentFloor].GetWorldPosition((int)m_CurrentCell.x, (int)m_CurrentCell.y);
            float moveTime = ConfigManager.Instance.UnitConfig.MoveTime; 
            transform 
                .DOMove(
                    newPosition,
                    moveTime
                )
                .SetDelay(0.2f)
                .SetEase(Ease.Linear);
            m_Animation.TriggerAnimRunning();
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

        }
        #endregion

        #region Put down
        void Putdown(BlockFunctionPutdown _putdown)
        {
            
        }
        #endregion
    }
}
