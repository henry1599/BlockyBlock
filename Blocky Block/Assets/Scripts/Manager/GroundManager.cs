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

                    if (groundType == GroundType.BOX_ON_GROUND ||
                        groundType == GroundType.TREE)
                    {
                        GameObject stuffPrefab = ResourceLoader.Instance.Grounds[groundType];
                        Vector3 stuffPosition = grid.GetWorldPosition(i, j);
                        GameObject stuffInstance = Instantiate(stuffPrefab, stuffPosition, Quaternion.identity, transform);
                        stuffInstance.GetComponent<GrabableObject>()?.Setup(i, j, floorIdx, stuffPosition, Vector3.zero, transform);

                        GameObject groundPrefab = ResourceLoader.Instance.Grounds[GroundType.GROUND];
                        Vector3 groundPosition = grid.GetWorldPosition(i, j);
                        Ground groundInstance = Instantiate(groundPrefab, groundPosition, Quaternion.identity, transform).GetComponent<Ground>();

                        groundInstance.Setup(groundType, stuffInstance);
                        grid.GridArray[i, j] = groundInstance;
                    }
                    else if (groundType == GroundType.COLLECTIBLE)
                    {
                        GameObject collectiblePrefab = ResourceLoader.Instance.Grounds[groundType];
                        Vector3 collectiblePosition = grid.GetWorldPosition(i, j);
                        GameObject collectibleInstance = Instantiate(collectiblePrefab, collectiblePosition, Quaternion.identity, transform);
                        collectibleInstance.GetComponent<CollectibleObject>()?.Setup(i, j, floorIdx, collectiblePosition);

                        GameObject groundPrefab = ResourceLoader.Instance.Grounds[GroundType.GROUND];
                        Vector3 groundPosition = grid.GetWorldPosition(i, j);
                        Ground groundInstance = Instantiate(groundPrefab, groundPosition, Quaternion.identity, transform).GetComponent<Ground>();

                        groundInstance.Setup(groundType, collectibleInstance);
                        grid.GridArray[i, j] = groundInstance;
                    }
                    else if (groundType == GroundType.GROUND || groundType == GroundType.WATER || groundType == GroundType.SPACE)
                    {
                        GameObject groundPrefab = ResourceLoader.Instance.Grounds[groundType];
                        Vector3 groundPosition = grid.GetWorldPosition(i, j);
                        Ground groundInstance = Instantiate(groundPrefab, groundPosition, Quaternion.identity, transform).GetComponent<Ground>();

                        groundInstance.Setup(groundType, null);
                        grid.GridArray[i, j] = groundInstance;
                    }

                }
            }
            m_IsFinishedSpawnGround = true;
        }
    }

}
