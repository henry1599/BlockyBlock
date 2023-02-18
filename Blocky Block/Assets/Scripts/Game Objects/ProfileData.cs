using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;

namespace BlockyBlock
{
    public class ProfileData
    {
        public string name;
        public int avatarIdx;
        public Dictionary<ChapterID, Dictionary<LevelID, LevelStatus>> unlockedLevels;
        public CustomizationData customizationData;
        public ProfileData()
        {
            unlockedLevels = new Dictionary<ChapterID, Dictionary<LevelID, LevelStatus>>();
            unlockedLevels.TryAdd(
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
            customizationData = new CustomizationData();
        }
    }
    [System.Serializable]
    public class CustomizationData
    {
        public Dictionary<CustomizationType, CustomizationStatus> datas;
        public CustomizationData()
        {
            datas = new Dictionary<CustomizationType, CustomizationStatus>()
            {
                {CustomizationType.BODY, new CustomizationStatus() {index = 0, isUnlock = true}},
                {CustomizationType.BODY_PART, new CustomizationStatus() {index = -1, isUnlock = true}},
                {CustomizationType.EYES, new CustomizationStatus() {index = 0, isUnlock = true}},
                {CustomizationType.GLOVES, new CustomizationStatus() {index = -1, isUnlock = true}},
                {CustomizationType.MOUTH, new CustomizationStatus() {index = 0, isUnlock = true}},
                {CustomizationType.NOSE, new CustomizationStatus() {index = -1, isUnlock = true}},
                {CustomizationType.EARS, new CustomizationStatus() {index = -1, isUnlock = true}},
                {CustomizationType.GLASSES, new CustomizationStatus() {index = -1, isUnlock = true}},
                {CustomizationType.HAIR, new CustomizationStatus() {index = -1, isUnlock = true}},
                {CustomizationType.HAT, new CustomizationStatus() {index = -1, isUnlock = true}},
                {CustomizationType.HORN, new CustomizationStatus() {index = -1, isUnlock = true}},
                {CustomizationType.TAIL, new CustomizationStatus() {index = -1, isUnlock = true}}
            };
        }
    }
    [System.Serializable]
    public class CustomizationStatus
    {
        public int index;
        public bool isUnlock;
        public CustomizationStatus()
        {
            index = 0;
            isUnlock = true;
        }
    }
}
