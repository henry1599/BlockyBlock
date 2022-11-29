using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IngameDebugConsole;

namespace BlockyBlock.Cheat
{
    public class CheatMenu : MonoBehaviour
    {
        [Header("Tab Buttons")]
        [SerializeField] Button localDataButton, consoleButton;

        [Space(5)]
        [Header("Contents")]
        [SerializeField] GameObject locaDataContent;
        [SerializeField] DebugLogManager consoleContent;
    }
}
