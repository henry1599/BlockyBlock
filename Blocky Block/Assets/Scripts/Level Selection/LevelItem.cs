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

namespace BlockyBlock
{

    public class LevelItem : MonoBehaviour
    {
        public static event System.Action<LevelID, Vector3> ON_LEVEL_NODE_CLICKED;
        private int m_LevelID;
        public int LevelId => m_LevelID;
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
            ON_LEVEL_NODE_CLICKED?.Invoke((LevelID)m_LevelID, transform.position);
        }
    }
}
