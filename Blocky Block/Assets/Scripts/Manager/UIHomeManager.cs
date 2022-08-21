using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;

namespace BlockyBlock.Managers
{
    public class UIHomeManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void OnStartButtonClick()
        {
            // * Call Event to load Level
            GameEvents.LOAD_LEVEL?.Invoke(LevelID.LEVEL_01);
        }
    }
}
