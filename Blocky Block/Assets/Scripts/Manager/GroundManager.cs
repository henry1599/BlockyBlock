using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Core;
using BlockyBlock.Enums;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.Managers
{
    public class GroundManager : MonoBehaviour
    {
        public static GroundManager Instance {get; private set;}
        public bool m_IsFinishedSpawnGround = false;
        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            GameEvents.SETUP_GROUND += HandleSetupGround;
        }
        void OnDestroy()
        {
            GameEvents.SETUP_GROUND -= HandleSetupGround;
        }
        void HandleSetupGround()
        {
            StartCoroutine(Cor_SpawnGround());
        }
        IEnumerator Cor_SpawnGround()
        {
            yield return new WaitUntil(() => ResourceLoader.Instance != null);
            List<Core.Grid> grids = GridManager.Instance.Grids;
            foreach (Core.Grid grid in grids)
            {
                foreach (GroundData groundData in grid.GroundDatas)
                {
                    int i = (int)groundData.idx.x;
                    int j = (int)groundData.idx.y;
                    GroundType groundType = groundData.groundType;
                    int floorIdx = groundData.floorIdx;

                    GameObject groundPrefab = ResourceLoader.Instance.Grounds[groundType];
                    Vector3 position = grid.GetWorldPosition(i, j, floorIdx);
                    Ground groundInstance = Instantiate(groundPrefab, position, Quaternion.identity, transform).GetComponent<Ground>();

                    grid.GridArray[i, j] = groundInstance;
                }
            }
            m_IsFinishedSpawnGround = true;
        }
    }
}
