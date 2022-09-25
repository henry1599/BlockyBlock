using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;

namespace BlockyBlock.Core 
{
    [System.Serializable]
    public class Ground : MonoBehaviour
    {
        public GroundType Type;
        public Transform Position;
        public GameObject Stuff;
    }
}
