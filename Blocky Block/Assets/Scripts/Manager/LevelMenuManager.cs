using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using Helpers;
using System.Linq;
using UnityEngine.AI;
using BlockyBlock.Utils;

namespace BlockyBlock.Managers
{
    public class LevelMenuManager : MonoBehaviour
    {
        public static LevelMenuManager Instance {get; private set;}
        private static readonly int IdleKeyAnimation = Animator.StringToHash("Idle");
        private static readonly int RunSlowKeyAnimation = Animator.StringToHash("Run Slow");
        private static readonly int RunMediumKeyAnimation = Animator.StringToHash("Run Medium");
        private static readonly int RunFastKeyAnimation = Animator.StringToHash("Run Fast");
        private static readonly int[] KeysAnim = new int[3]
        {
            RunSlowKeyAnimation,
            RunMediumKeyAnimation,
            RunFastKeyAnimation
        };
        [SerializeField] LevelItemConfig m_LevelItemConfig;
        [SerializeField] Transform[] m_LevelNodes;
        [SerializeField] NavMeshAgent m_Unit3D;
        [SerializeField] float[] m_Speeds;
        private Animator m_Anim;
        private bool m_IsMoving = false;
        private int m_ClickFactor = -1;
        private List<LevelItem> m_LevelItems;
        private LevelID m_ChosenNodeID;
        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            InitLevels(LoadChapterChosen());
            LevelItem.ON_LEVEL_NODE_CLICKED += HandleLevelNodeClicked;
        }
        void Update()
        {
            if (!m_IsMoving) 
            {
                m_Anim.CrossFade(IdleKeyAnimation, 0, 0);
                return;
            }
            float dist = m_Unit3D.remainingDistance; 
            if (dist != Mathf.Infinity && m_Unit3D.pathStatus == NavMeshPathStatus.PathComplete && m_Unit3D.remainingDistance <= 0.1f)
            {
                EnterLevel();
                m_IsMoving = false;
                m_Unit3D.speed = m_Speeds[m_ClickFactor];
                m_ClickFactor = -1;
            }
        }
        void OnDestroy()
        {
            LevelItem.ON_LEVEL_NODE_CLICKED -= HandleLevelNodeClicked;
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
            m_Anim = m_Unit3D.GetComponentInChildren<Animator>();
            m_Anim.runtimeAnimatorController = GameManager.Instance.LevelSelectionAnim;
        }
        void HandleLevelNodeClicked(LevelID _nodeID, Vector3 _position)
        {
            m_ChosenNodeID = _nodeID;
            m_IsMoving = true;
            m_ClickFactor++;
            if (m_ClickFactor > m_Speeds.Length - 1)
                m_ClickFactor = m_Speeds.Length - 1;
            
            m_Unit3D.speed = m_Speeds[m_ClickFactor];
            m_Anim.CrossFade(KeysAnim[m_ClickFactor], 0, 0);
            m_Unit3D.destination = _position;
        }
        void EnterLevel()
        {
            UIUtils.LockInput();
            m_LevelItems.ForEach(i => i.GetComponent<Collider>().enabled = false);
            GameManager.Instance.TransitionIn(() => 
                {
                    UIUtils.UnlockInput();
                    GameEvents.LOAD_LEVEL?.Invoke((LevelID)m_ChosenNodeID);
                }
            );
        }
    }
}
