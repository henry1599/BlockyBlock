using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Configurations;
using BlockyBlock.Core;
using BlockyBlock.Enums;

namespace BlockyBlock.Managers
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager Instance {get; private set;}
        [SerializeField] Unit3D m_Unit3DTemplate;
        public float DistanceStepValue {get => m_DistanceStepValue; private set => m_DistanceStepValue = value;} [SerializeField] float m_DistanceStepValue = 1.1f;
        public float UnitMoveTime {get => m_UnitMoveTime; private set => m_UnitMoveTime = value;} [SerializeField] float m_UnitMoveTime = 1f;
        void Awake()
        {
            Instance = this;

            GameEvents.SETUP_LEVEL += HandleSetupLevel;
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }
        void OnDestroy()
        {
            GameEvents.SETUP_LEVEL -= HandleSetupLevel;
        }
        void HandleSetupLevel(LevelData _data)
        {
            StartCoroutine(SpawnUnit(_data));
        }
        IEnumerator SpawnUnit(LevelData _data)
        {
            yield return new WaitUntil(() => GroundManager.Instance.m_IsFinishedSpawnGround == true);
            foreach (UnitData unitData in _data.UnitDatas)
            {
                Vector3 startPosition = unitData.StartPosition + Vector3.up * ConfigManager.Instance.LevelConfig.SpaceEachFloor * unitData.Floor;
                UnitDirection startDirection = unitData.StartDirection;

                Unit3D unitInstance = Instantiate(m_Unit3DTemplate.gameObject, transform).GetComponent<Unit3D>();
                unitInstance.Setup(startPosition, startDirection);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
