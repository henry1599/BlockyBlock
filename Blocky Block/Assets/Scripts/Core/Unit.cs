using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Enums;

namespace BlockyBlock.Core
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] float m_DistanceStepValue = 1.1f;
        private Vector3 m_StartPosition;
        private UnitDirection m_StartDirection;
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

        // Update is called once per frame
        void Update()
        {
            
        }
        public void MoveForward()
        {
            print("Move Forward");
        }
        public void TurnLeft()
        {
            print("Turn Left");
        }
        public void TurnRight()
        {
            print("Turn Right");
        }
    }
}
