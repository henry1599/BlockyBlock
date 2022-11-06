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
            StartCoroutine(Cor_LoadProfile());
        }
        void Start()
        {
            InitLevels(LoadChapterChosen());
        }
        IEnumerator Cor_LoadProfile()
        {
            yield return new WaitUntil(() => ProfileManager.Instance != null);
            ProfileManager.Instance.LoadProfile();
        }
        int LoadChapterChosen()
        {
            return PlayerPrefs.GetInt(GameConstants.CHAPTER_CHOSEN_KEY, 0);
        }
        void InitLevels(int _chapter)
        {
            ChapterID chapterID = (ChapterID)_chapter;
            m_LevelItems = new List<LevelItem>();
            Dictionary<LevelID, LevelStatus> levels = ProfileManager.Instance.ProfileData.UnlockedLevels[chapterID];
            List<LevelData> levelDatas = ConfigManager.Instance.LevelConfig.LevelDatas;
            List<LevelData> levelDatasInChapter = levelDatas.Where(lv => lv.ChapterID == chapterID).ToList();
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
