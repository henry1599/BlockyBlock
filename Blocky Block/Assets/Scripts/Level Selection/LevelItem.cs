using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using BlockyBlock.Managers;
using BlockyBlock.Utils;
using BlockyBlock.Events;

namespace BlockyBlock
{
    public class LevelItem : MonoBehaviour
    {
        [SerializeField] TMP_Text m_LevelNameDisplayText;
        private int m_LevelID;
        public void Setup(LevelID _levelId, LevelItemData _data)
        {
            m_LevelID = (int)_levelId;
            m_LevelNameDisplayText.text = _data.LevelNameDisplay;
        }
        public void OnClick()
        {
            UIUtils.LockInput();
            GameManager.Instance.TransitionIn(() => 
                {
                    UIUtils.UnlockInput();
                    GameEvents.LOAD_LEVEL?.Invoke((LevelID)m_LevelID);
                }
            );
        }
    }
}
