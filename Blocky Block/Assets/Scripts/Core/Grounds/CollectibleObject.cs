using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Managers;
using DG.Tweening;
using BlockyBlock.Enums;

namespace BlockyBlock.Core 
{
    public class CollectibleObject : MonoBehaviour
    {
        [SerializeField] ParticleSystem m_VfxCollected;
        [SerializeField] GameObject m_CollectibleModel;
        bool IsCollected = false;
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
        void HandleReset()
        {
            Reset();
        }
        public void Setup(int _x, int _y, int _floor, Vector3 _position)
        {
            m_InitX = _x;
            m_InitY = _y;
            m_InitFloor = _floor;

            m_InitPosition = _position;
        }
        public void Reset()
        {
            m_CollectibleModel.SetActive(true);
            transform.position = m_InitPosition;
            transform.eulerAngles = m_InitAngle;
        }
        public void OnCollect()
        {
            print("Collect");
            m_CollectibleModel.SetActive(false);
            m_VfxCollected.Play();
            SoundManager.Instance.PlaySound(AudioPlayer.SoundID.STAR_COLLECTED);
            UpdateGrid();
        }
        void UpdateGrid()
        {
            // * Unchanged type
            if (GridManager.Instance.Grids[m_InitFloor].GridArray[m_InitX, m_InitY].Type == GroundType.BOX_IN_WATER)
            {
                return;
            }
            GroundType type = GridManager.Instance.Grids[m_InitFloor].GridArray[m_InitX, m_InitY].Type;
            type = (GroundType)((int)type & 0b000111);
            GridManager.Instance.Grids[m_InitFloor].GridArray[m_InitX, m_InitY].Type = type; 
        }
    }
}
