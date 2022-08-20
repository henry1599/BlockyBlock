using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;

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
            LevelManager.Instance.CurrentLevelID = LevelID.LEVEL_01;
        }
    }
}
