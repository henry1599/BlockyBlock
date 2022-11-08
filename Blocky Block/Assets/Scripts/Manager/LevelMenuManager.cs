using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using Helpers;
using System.Linq;

namespace BlockyBlock.Managers
{
    public class LevelMenuManager : MonoBehaviour
    {
        public static LevelMenuManager Instance {get; private set;}
        [SerializeField] LevelItemConfig m_LevelItemConfig;
        [SerializeField] Transform[] m_LevelNodes;
        private List<LevelItem> m_LevelItems;
        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            InitLevels(LoadChapterChosen());
        }
        public void OnBackButtonClick()
        {
            GameManager.Instance.TransitionIn(() => GameEvents.LOAD_LEVEL?.Invoke(LevelID.HOME));
        }
        int LoadChapterChosen()
        {
            return PlayerPrefs.GetInt(GameConstants.CHAPTER_CHOSEN_KEY, 0);
        }
        void InitLevels(int _chapter)
        {
            ChapterID chapterID = (ChapterID)_chapter;
            m_LevelItems = new List<LevelItem>();

            // * Get the """status""" of all levels based on player profile
            Dictionary<LevelID, LevelStatus> levels = ProfileManager.Instance.ProfileData.UnlockedLevels[chapterID];

            // * Get all levelDatas => To get all level infos in the CHOSEN CHAPTER
            List<LevelData> levelDatas = ConfigManager.Instance.LevelConfig.LevelDatas;

            // * Filter all levels in the chosen chapter
            List<LevelData> levelDatasInChapter = levelDatas.Where(lv => lv.ChapterID == chapterID).ToList();

            // * Traverse levelDatasInChapter in order to:
            // * To Intantiate the level node 3d in scene
            // * To Setup the levelID for each node
            // * To Setup the visual of status for each node
            foreach (var (data, idx) in levelDatasInChapter.WithIndex())
            {
                LevelID id = data.LevelID;
                LevelStatus status = levels[id];
                
                LevelItem itemInstance = Instantiate(m_LevelItemConfig.LevelItems[status], m_LevelNodes[idx]);
                itemInstance.transform.localPosition = Vector3.zero;
                itemInstance.Setup(id);

                m_LevelItems.Add(itemInstance);
            }
        }
    }
}
