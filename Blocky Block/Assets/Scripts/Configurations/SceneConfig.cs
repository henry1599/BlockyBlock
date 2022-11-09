using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using BlockyBlock.Enums;
using System.Linq;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Scene Config", menuName = "Scriptable Object/Scene Config")]
    public class SceneConfig : ScriptableObject
    {
        public Level Level;
        public LevelID GetLevelIDBySceneName(string _sceneName)
        {
            return Level.FirstOrDefault(x => x.Value == _sceneName).Key;
        }
        public string GetSceneNameByID(LevelID _id)
        {
            return Level[_id];
        }
    }
    [System.Serializable]
    public class Level : SerializableDictionaryBase<LevelID, string> {}
}
