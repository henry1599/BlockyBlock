using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;

namespace BlockyBlock.Managers
{
    public class LevelMenuManager : MonoBehaviour
    {
        public static LevelMenuManager Instance {get; private set;}
        [SerializeField] LevelItemConfig m_LevelItemConfig;
        [SerializeField] Transform m_LevelContainer;
        [SerializeField] Vector3 m_CenterPosition;
        [SerializeField] float m_ScrollXOffset = 8;
        public LevelScroller Scroller;
        public float ScrollXOffset => m_ScrollXOffset;
        private List<LevelItem> m_LevelNodes;
        public int ItemCount => m_LevelNodes.Count;
        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            InitLevels();
        }
        void InitLevels()
        {
            m_LevelNodes = new List<LevelItem>();
            LevelItems levelItems = m_LevelItemConfig.LevelItems;
            LevelItem itemTemplate = m_LevelItemConfig.LevelItemTemplate;
            int idx = 0;
            foreach(KeyValuePair<LevelID, LevelItemData> entry in levelItems)
            {
                Vector3 spawnPosition = m_CenterPosition;
                spawnPosition.x += idx * m_ScrollXOffset;
                LevelItem itemInstance = Instantiate(itemTemplate, spawnPosition, Quaternion.identity, m_LevelContainer);
                
                itemInstance.Setup(entry.Key, entry.Value);
                m_LevelNodes.Add(itemInstance);

                idx ++;
            }
        }
    }
}
