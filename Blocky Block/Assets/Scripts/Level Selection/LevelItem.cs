using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using BlockyBlock.Managers;
using BlockyBlock.Utils;
using BlockyBlock.Events;
using DG.Tweening;
using EPOOutline;

namespace BlockyBlock
{

    public class LevelItem : MonoBehaviour
    {
        private int m_LevelID;
        void OnMouseDown()
        {
            OnClick();
        }
        public void Setup(LevelID _levelId)
        {
            m_LevelID = (int)_levelId;
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
