using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using BlockyBlock.UI;

namespace BlockyBlock.Managers
{
    public class UIHomeManager : MonoBehaviour
    {
        public HomeState State 
        {
            get => m_State;
            set
            {
                m_State = value;
                HomeEvents.ON_STAGE_CHANGED?.Invoke(value);
            }
        } [SerializeField] HomeState m_State;
        public void OnStartButtonClick()
        {
            // * Call Event to load Level
            // GameEvents.LOAD_LEVEL?.Invoke(LevelID.LEVEL_TEST_3D);
            State = HomeState.LEVEL_TYPE_SELECTION;
        }
        public void OnBackButtonClick()
        {
            State = HomeState.MAIN;
        }
        public void OnExitButtonClick()
        {
            Application.Quit();
        }
    }
}
