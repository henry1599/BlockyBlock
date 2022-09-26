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
        void Awake()
        {
            Instance = this;
        }
    }
}
