using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using System;

namespace BlockyBlock.Events
{
    public class GameEvents
    {
        public static Action<LevelID> LOAD_LEVEL;
        public static Action<LevelData> SETUP_LEVEL;
    }
    public class GameConstants
    {
        public static readonly string UIBLOCK_OUTSIDE_CONTAINER_TAG = "Block Container";
    }
}
