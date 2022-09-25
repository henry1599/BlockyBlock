using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Managers;
using DG.Tweening;

namespace BlockyBlock.Core 
{
    public class GrabableObject : MonoBehaviour
    {
        bool IsGrabbed = false;
        private int m_InitFloor;
        private int m_InitX, m_InitY;
        private Vector3 m_InitPosition;
        private Vector3 m_InitAngle;
        private Transform m_InitParent;
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
            m_InitX = _x;
            m_InitY = _y;
            m_InitFloor = _floor;

            m_InitParent = _parent;
            m_InitPosition = _position;
            m_InitAngle = _angle;
        }
        public void Reset()
        {
            transform.SetParent(m_InitParent);
            transform.position = m_InitPosition;
            transform.eulerAngles = m_InitAngle;
            UngrabSelf(m_InitX, m_InitY, m_InitFloor);
        }
        public void GrabSelf()
        {
            IsGrabbed = true;
        }
        public void UngrabSelf(int _putDownX, int _putDownY, int _floor)
        {
            transform.SetParent(m_InitParent, true);
            transform.eulerAngles = Vector3.zero;
            Vector3 putPosition = GridManager.Instance.Grids[_floor].GetWorldPosition(_putDownX, _putDownY);
            transform.position = putPosition;
            IsGrabbed = false;
        }
    }
}
