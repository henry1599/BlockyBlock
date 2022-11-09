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
                if (GridManager.Instance.Grids.Count - 1 < unitData.Floor)
                {
                    Debug.LogError("Unit's floor does not match with the maximum floor on this level => Check LevelConfig for more detail");
                    yield break;
                }
                Vector3 startPosition = GridManager.Instance.Grids[unitData.Floor].GetWorldPosition(unitData.X, unitData.Y);
                UnitDirection startDirection = unitData.StartDirection;

                Unit3D unitInstance = Instantiate(m_Unit3DTemplate.gameObject, transform).GetComponent<Unit3D>();
                unitInstance.GetComponentInChildren<Animator>().runtimeAnimatorController = GameManager.Instance.LevelAnim;
                unitInstance.Setup(startPosition, startDirection, unitData.X, unitData.Y);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
