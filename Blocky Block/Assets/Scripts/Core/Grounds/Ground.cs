using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;

namespace BlockyBlock.Core 
{
    [System.Serializable]
    public class Ground : MonoBehaviour
    {
        GroundType InitType;
        GameObject InitStuff;
        public GroundType Type;
        public GameObject Stuff {get; set;}
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
            Type = InitType;
            Stuff = InitStuff;
        }
        public void Setup(GroundType _type, GameObject _stuff = null)
        {
            InitType = Type = _type;
            InitStuff = Stuff = _stuff;
        }
    }
}
