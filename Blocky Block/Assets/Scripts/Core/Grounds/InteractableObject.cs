using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Managers;
using DG.Tweening;
using BlockyBlock.Enums;

namespace BlockyBlock.Core 
{
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] GroundType Type;
        [SerializeField] ParticleSystem m_VfxPut, m_VfxPutWater, m_VfxBeginPush, m_VfxEndPush;
        bool IsGrabbed = false;
        private int m_InitFloor;
        private int m_InitX, m_InitY;
        private Vector3 m_InitPosition;
        private Vector3 m_InitAngle;
        private Transform m_InitParent;

        private int m_CurrentX, m_CurrentY, m_CurrentFloor;
        void Start()
        {
            UnitEvents.ON_RESET += HandleReset;
        }
        void OnDestroy()
        {
            UnitEvents.ON_RESET -= HandleReset;
        }
        void Update()
        {
            if (IsGrabbed)
            {
                transform.localPosition = Vector3.zero;
                transform.localEulerAngles = Vector3.zero;
            }
        }
        void HandleReset()
        {
            Reset();
        }
        public void Setup(int _x, int _y, int _floor, Vector3 _position, Vector3 _angle, Transform _parent)
        {
            m_InitX = m_CurrentX = _x;
            m_InitY = m_CurrentY = _y;
            m_InitFloor = m_CurrentFloor = _floor;

            m_InitParent = _parent;
            m_InitPosition = _position;
            m_InitAngle = _angle;
        }
        public void Reset()
        {
            transform.SetParent(m_InitParent);
            transform.position = m_InitPosition;
            transform.eulerAngles = m_InitAngle;

            m_CurrentX = m_InitX;
            m_CurrentY = m_InitY;
            m_CurrentFloor = m_InitFloor;

            IsGrabbed = false;
            // UngrabSelf(m_InitX, m_InitY, m_InitFloor);
        }
        public void PushSelf(int _pushToX, int _pushToY, int _floor)
        {
            Vector3 pushPosition = GridManager.Instance.Grids[_floor].GetWorldPosition(_pushToX, _pushToY);
            if (GridManager.Instance.Grids[_floor].GridArray[_pushToX, _pushToY].Type == GroundType.WATER)
            {
                transform.DOLocalMove(pushPosition, 0.5f).SetEase(Ease.InOutSine)
                    .OnComplete(() => {
                        pushPosition += new Vector3(0, -0.8f, 0);
                        transform.DOLocalMove(pushPosition, 0.45f).SetEase(Ease.InOutSine);
                        DOVirtual.DelayedCall(0.25f, () => m_VfxPutWater.Play());
                        
                    });
            }
            else
            {
                transform.DOLocalMove(pushPosition, 0.5f).SetEase(Ease.InOutSine);
            }


            UpdateGrid(null);
            m_CurrentX = _pushToX;
            m_CurrentY = _pushToY;
            m_CurrentFloor = _floor;
            UpdateGrid(gameObject);
        }
        public void GrabSelf()
        {
            IsGrabbed = true;
            
            UpdateGrid(null);
        }
        public void UngrabSelf(int _putDownX, int _putDownY, int _floor, bool _playPar = false)
        {
            transform.SetParent(m_InitParent, true);
            transform.eulerAngles = Vector3.zero;
            Vector3 putPosition = GridManager.Instance.Grids[_floor].GetWorldPosition(_putDownX, _putDownY);
            bool isWater = false;
            if (GridManager.Instance.Grids[m_CurrentFloor].GridArray[_putDownX, _putDownY].Type == Enums.GroundType.WATER)
            {
                isWater = true;
                putPosition += new Vector3(0, -0.8f, 0);
            }
            transform.position = putPosition;
            if (_playPar)
            {
                if (isWater)
                    m_VfxPutWater.Play();
                else
                    m_VfxPut?.Play();
            }
            IsGrabbed = false;

            m_CurrentX = _putDownX;
            m_CurrentY = _putDownY;
            m_CurrentFloor = _floor;
            
            UpdateGrid(gameObject);
        }
        void UpdateGrid(GameObject _stuff)
        {
            // * Unchanged type
            if (GridManager.Instance.Grids[m_CurrentFloor].GridArray[m_CurrentX, m_CurrentY].Type == GroundType.BOX_IN_WATER)
            {
                return;
            }
            GridManager.Instance.Grids[m_CurrentFloor].GridArray[m_CurrentX, m_CurrentY].Stuff = _stuff;
            if (_stuff == null)
            {
                GroundType type = GridManager.Instance.Grids[m_CurrentFloor].GridArray[m_CurrentX, m_CurrentY].Type;
                type = (GroundType)((int)type & 0b000111);
                GridManager.Instance.Grids[m_CurrentFloor].GridArray[m_CurrentX, m_CurrentY].Type = type; 
            }
            else
            {
                GridManager.Instance.Grids[m_CurrentFloor].GridArray[m_CurrentX, m_CurrentY].Type |= Type;
            }

            if (GridManager.Instance.Grids[m_CurrentFloor].GridArray[m_CurrentX, m_CurrentY].Type == Enums.GroundType.WATER)
            {
                gameObject.tag = GameConstants.WALKABLE_TAG;
            }
            else
            {
                gameObject.tag = GameConstants.UNWALKABLE_TAG;
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Box"))
            {
                print("Box collide");
            }
        }
    }
}
