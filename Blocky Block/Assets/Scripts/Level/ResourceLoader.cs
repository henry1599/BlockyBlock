using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using BlockyBlock.Enums;

namespace BlockyBlock.Managers
{
    [System.Serializable]
    public class Grounds : SerializableDictionaryBase<GroundType, GameObject> {}
    public class ResourceLoader : MonoBehaviour
    {
        public static ResourceLoader Instance {get; private set;}
        public Grounds Grounds;
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
