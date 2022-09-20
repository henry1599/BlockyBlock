using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Core;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.Managers
{
    [System.Serializable]
    public class GroundsDictionary : SerializableDictionaryBase<int, List<Ground>> {}
    public class GroundManager : MonoBehaviour
    {
        public static GroundManager Instance {get; private set;}
        public bool m_IsFinishedSpawnGround = false;
        public GroundsDictionary Dictionary;
        void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            Dictionary = new GroundsDictionary();
            GameEvents.SETUP_GROUND += HandleSetupGround;
        }
        void OnDestroy()
        {
            GameEvents.SETUP_GROUND -= HandleSetupGround;
        }
        void HandleSetupGround(List<LevelGround> _levelGrounds)
        {
            StartCoroutine(Cor_SpawnGround(_levelGrounds));
        }
        IEnumerator Cor_SpawnGround(List<LevelGround> _levelGrounds)
        {
            yield return new WaitUntil(() => ResourceLoader.Instance != null);
            int idxFloor = 0;
            foreach (LevelGround levelGround in _levelGrounds)
            {
                List<Ground> grounds = new List<Ground>();
                foreach (GroundData _data in levelGround.groundDatas)
                {
                    Ground ground = Instantiate(ResourceLoader.Instance.Grounds[_data.groundType].gameObject, _data.position + Vector3.up * levelGround.Height, Quaternion.identity, transform).GetComponent<Ground>();
                    grounds.Add(ground);
                }
                Dictionary.TryAdd(idxFloor, grounds);
                idxFloor++;
            }
            m_IsFinishedSpawnGround = true;
        }
    }
}
