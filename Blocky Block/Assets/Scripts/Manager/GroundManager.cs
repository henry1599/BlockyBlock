using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;

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
        void HandleSetupGround(LevelGround _levelGround)
        {
            StartCoroutine(Cor_SpawnGround(_levelGround));
        }
        IEnumerator Cor_SpawnGround(LevelGround _levelGround)
        {
            yield return new WaitUntil(() => ResourceLoader.Instance != null);
            foreach (GroundData _data in _levelGround.groundDatas)
            {
                Instantiate(ResourceLoader.Instance.Grounds[_data.groundType].gameObject, _data.position, Quaternion.identity, transform);
            }
            m_IsFinishedSpawnGround = true;
        }
    }
}
