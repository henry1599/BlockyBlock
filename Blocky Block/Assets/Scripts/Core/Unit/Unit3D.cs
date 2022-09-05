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
        [SerializeField] UnitBound m_Bound;
        [Space(10)]
        [Header("From water to ground")]
        [SerializeField] float m_JumpPower;
        [Space(10)]
        [Header("VFX")]
        [SerializeField] GameObject m_VfxWaterSplash;
        private Vector3 m_StartPosition;
        private UnitDirection m_StartDirection;
        private UnitDirection m_CurrentDirection;
        public UnitConfig m_Unit3DConfig;
        private GroundType m_CurrentGround;
        private bool m_IsUnderwater = false;
        private GroundType CurrentGround 
        {
            get => m_CurrentGround;
            set 
            {
                m_IsUnderwater = value == GroundType.WATER;
                if (m_CurrentGround == GroundType.GROUND && value == GroundType.WATER)
                {
                    MoveFromGroundToWater();
                    m_CurrentGround = value;
                    return;
                }
                if (m_CurrentGround == GroundType.WATER && value == GroundType.GROUND)
                {
                    MoveFromWaterToGround();
                    m_CurrentGround = value;
                    return;
                }
                m_CurrentGround = value;
                MoveNormally(m_CurrentGround);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            UnitEvents.ON_MOVE_FORWARD += MoveForward;
            UnitEvents.ON_TURN_LEFT += TurnLeft;
            UnitEvents.ON_TURN_RIGHT += TurnRight;

            UnitEvents.ON_STOP += HandleStop;
            UnitEvents.ON_RESET += HandleReset;

            m_CurrentGround = m_Bound.CastBelow();
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
            
            m_CurrentGround = m_Bound.CastBelow();
        }
        void HandleStop()
        {
            m_Animation.Reset(m_IsUnderwater);
        }
        void HandleReset()
        {
            transform.DOKill(true);
            Setup(m_StartPosition, m_StartDirection);
            m_IsUnderwater = false;
            m_Animation.Reset(m_IsUnderwater);
        }
        public void MoveForward(BlockFunctionMoveForward _moveForward)
        {
            GroundType nextGround = m_Bound.CastFrontDown();
            switch (CurrentGround)
            {
                case GroundType.WATER:
                    nextGround = m_Bound.CastFrontUp();
                    break;
                case GroundType.GROUND:
                    nextGround = m_Bound.CastFrontDown();
                    break;
            }
            CurrentGround = nextGround;
        }
        void MoveFromGroundToWater()
        {
            DirectionData directionData = m_Unit3DConfig.GetDataByDirection(m_CurrentDirection);
            Vector3 newPosition = transform.position + directionData.MoveDirection * m_Unit3DConfig.StepDistance;
            float moveTime = m_Unit3DConfig.EnterWaterTime; 
            m_Animation.TriggerAnimGroundToWater();
            newPosition.y = -0.9f;
            transform 
                .DOMove(
                    newPosition,
                    moveTime
                )
                .SetEase(Ease.Linear)
                .OnComplete(() => transform.position = newPosition);
            StartCoroutine(SetDelay(() => Instantiate(m_VfxWaterSplash, new Vector3(newPosition.x, 0, newPosition.z), Quaternion.Euler(directionData.Rotation)), 0.3f));
        }
        IEnumerator SetDelay(System.Action _cb = null, float _delay = 0.0f)
        {
            yield return new WaitForSeconds(_delay);
            _cb?.Invoke();
        }
        void MoveFromWaterToGround()
        {
            DirectionData directionData = m_Unit3DConfig.GetDataByDirection(m_CurrentDirection);
            Vector3 newPosition = transform.position + directionData.MoveDirection * m_Unit3DConfig.StepDistance;
            float moveTime = m_Unit3DConfig.EnterWaterTime; 
            m_Animation.TriggerAnimWaterToGround();
            newPosition.y = 0f;
            transform 
                .DOLocalJump(
                    newPosition,
                    m_JumpPower,
                    1,
                    moveTime,
                    false
                )
                .SetEase(Ease.InOutSine)
                .OnComplete(() => transform.position = newPosition);
        }
        void MoveNormally(GroundType _currentGround)
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
            if (_currentGround == GroundType.GROUND)
            {
                m_Animation.TriggerAnimRunning();
            }
            else if (_currentGround == GroundType.WATER)
            {
                m_Animation.TriggerAnimSwimming();
            }
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
