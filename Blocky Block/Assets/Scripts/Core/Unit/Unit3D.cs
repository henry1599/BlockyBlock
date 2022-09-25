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

            GameObject m_Collectible = m_UnitVision.GetFontObject((int)m_CurrentCell.x, (int)m_CurrentCell.y, m_CurrentFloor, directionData);

            m_CurrentCell = new Vector2(m_CurrentCell.x + directionData.XIdx, m_CurrentCell.y + directionData.YIdx);
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
            m_GrabbedObject = m_UnitVision.GetFontObject((int)m_CurrentCell.x, (int)m_CurrentCell.y, m_CurrentFloor, directionData);
            if (m_GrabbedObject == null)
            {
                Debug.Log("Nothing to grab");
            }
            else
            {
                m_Animation.Reset();
                IsGrabSomething = true;
                m_Animation.TriggerAnimPickup();
                StartCoroutine(GrabStuff(m_GrabbedObject.GetComponent<GrabableObject>()));
            }
        }
        IEnumerator GrabStuff(GrabableObject _grabableObject)
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
                m_Animation.Reset();
                m_Animation.TriggerAnimPutdown();
                StartCoroutine(ResetGrab());
                StartCoroutine(UnGrabStuff(m_GrabbedObject.GetComponent<GrabableObject>()));
            }
            else
            {
                Debug.Log("Nothing to put down");
            }
        }
        IEnumerator UnGrabStuff(GrabableObject _grabableObject)
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
    }
}
