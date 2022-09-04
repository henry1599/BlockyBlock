using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.Core 
{
    public class UnitBound : MonoBehaviour
    {
        [SerializeField] Transform m_FrontPivot, m_BelowPivot;
        [SerializeField] float m_FrontRadius, m_BelowRadius;
        [SerializeField] LayerMask m_CastedLayer;
        public GroundType CastFront()
        {
            Collider[] cols = Physics.OverlapSphere(m_FrontPivot.position, m_FrontRadius, m_CastedLayer, QueryTriggerInteraction.Collide);
            foreach (Collider col in cols)
            {
                if (col.gameObject.CompareTag(GameConstants.GROUND_TAG))
                {
                    return GroundType.GROUND;
                }
                if (col.gameObject.CompareTag(GameConstants.WATER_TAG))
                {
                    return GroundType.WATER;
                }
            }
            return GroundType.GROUND;
        }
        
        public GroundType CastBelow()
        {
            Collider[] cols = Physics.OverlapSphere(m_BelowPivot.position, m_BelowRadius, m_CastedLayer, QueryTriggerInteraction.Collide);
            foreach (Collider col in cols)
            {
                if (col.gameObject.CompareTag(GameConstants.GROUND_TAG))
                {
                    return GroundType.GROUND;
                }
                if (col.gameObject.CompareTag(GameConstants.WATER_TAG))
                {
                    return GroundType.WATER;
                }
            }
            return GroundType.GROUND;
        }
        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(m_FrontPivot.position, m_FrontRadius);
            Gizmos.DrawWireSphere(m_BelowPivot.position, m_BelowRadius);
        }
    }
}
