using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Configurations;

namespace BlockyBlock.Managers
{
    public class ConfigManager : MonoBehaviour
    {
        public static ConfigManager Instance {get; private set;}
        public LevelConfig LevelConfig;
        public BlockConfig BlockConfig;
        public UnitConfig UnitConfig;
        public TurnDirectionConfig TurnDirectionConfig;
        public GroundFrontDataConfig GroundFrontDataConfig;
        public ErrorConfig ErrorConfig;
        public BehaviourConfig BehaviourConfig;
        public SceneConfig SceneConfig;
        void Awake()
        {
            Instance = this;
        }
    }
}
