using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;

namespace BlockyBlock
{
    public class ProfileData
    {
        public Dictionary<ChapterID, Dictionary<LevelID, LevelStatus>> UnlockedLevels;
        public CustomizationData CustomizationData;
        public ProfileData()
        {
            UnlockedLevels = new Dictionary<ChapterID, Dictionary<LevelID, LevelStatus>>();
            UnlockedLevels.TryAdd(
                ChapterID.CHAPTER_01,
                new Dictionary<LevelID, LevelStatus>(){
                    {LevelID.LEVEL_MANNUAL_00, LevelStatus.UNLOCK},
                    {LevelID.LEVEL_MANNUAL_01, LevelStatus.UNLOCK},
                    {LevelID.LEVEL_MANNUAL_02, LevelStatus.UNLOCK},
                    {LevelID.LEVEL_MANNUAL_03, LevelStatus.UNLOCK},
                    {LevelID.LEVEL_MANNUAL_04, LevelStatus.UNLOCK},
                    {LevelID.LEVEL_MANNUAL_05, LevelStatus.UNLOCK},
                }
            );
            CustomizationData = new CustomizationData();
        }
    }
    [System.Serializable]
    public class CustomizationData
    {
        public Dictionary<CustomizationType, int> Datas;
        public CustomizationData()
        {
            Datas = new Dictionary<CustomizationType, int>()
            {
                {CustomizationType.BODY, 0},
                {CustomizationType.BODY_PART, -1},
                {CustomizationType.EYES, 0},
                {CustomizationType.GLOVES, -1},
                {CustomizationType.MOUTH, 0},
                {CustomizationType.NOSE, -1},
                {CustomizationType.EARS, -1},
                {CustomizationType.GLASSES, -1},
                {CustomizationType.HAIR, -1},
                {CustomizationType.HAT, -1},
                {CustomizationType.HORN, -1},
                {CustomizationType.TAIL, -1}
            };
        }
    }
}
