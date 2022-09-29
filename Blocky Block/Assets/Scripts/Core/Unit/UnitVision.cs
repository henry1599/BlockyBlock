using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Managers;
using BlockyBlock.Configurations;

namespace BlockyBlock.Core 
{
    public class UnitVision : MonoBehaviour
    {
        [SerializeField] Transform m_FrontPivot, m_GroundPivot, m_BeyondFrontPivot;
        [SerializeField] float m_FrontRadius, m_GroundRadius;
        [SerializeField] LayerMask m_InteractableLayer, m_WalkableLayer, m_CannotPushToLayer;
        public GameObject GetBeyondFrontObject()
        {
            Collider[] cols = Physics.OverlapSphere(m_BeyondFrontPivot.position, m_FrontRadius, m_CannotPushToLayer, QueryTriggerInteraction.Collide);
            if (cols.Length > 0)
            {
                return cols[0].gameObject;
            }
            else
            {
                return null;
            }
        }
        public GameObject GetFrontObject()
        {
            Collider[] cols = Physics.OverlapSphere(m_FrontPivot.position, m_FrontRadius, m_InteractableLayer, QueryTriggerInteraction.Collide);
            if (cols.Length > 0)
            {
                return cols[0].gameObject;
            }
            else
            {
                return null;
            }
        }
        public bool IsWalkable()
        {
            Collider[] colsGround = Physics.OverlapSphere(m_GroundPivot.position, m_GroundRadius, m_WalkableLayer, QueryTriggerInteraction.Collide);
            Collider[] colsFront = Physics.OverlapSphere(m_FrontPivot.position, m_FrontRadius, m_InteractableLayer, QueryTriggerInteraction.Collide);
            return colsGround.Length > 0 && colsFront.Length == 0;
        }
        public GameObject GetFrontObject(int _unitX, int _unitY, int _floorIdx, DirectionData _directionData)
        {
            Ground[,] gridArray = GridManager.Instance.Grids[_floorIdx].GridArray;
            int x = _unitX + _directionData.XIdx;
            if (x > gridArray.GetLength(0) - 1)
            {
                return null;
            }
            int y = _unitY + _directionData.YIdx;
            if (y > gridArray.GetLength(1) - 1)
            {
                return null;
            }
            if (gridArray[x, y].Stuff != null)
            {
                if (gridArray[x, y].Type == Enums.GroundType.BOX_IN_WATER)
                {
                    return null;
                }
                return gridArray[x, y].Stuff.gameObject;
            }
            else
            {
                return null;
            }
        }
        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(m_FrontPivot.position, m_FrontRadius);
            Gizmos.DrawWireSphere(m_GroundPivot.position, m_GroundRadius);
            Gizmos.DrawWireSphere(m_BeyondFrontPivot.position, m_FrontRadius);
        }
    }
}
