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
                    {LevelID.LEVEL_MANNUAL_01, LevelStatus.LOCK},
                    {LevelID.LEVEL_MANNUAL_02, LevelStatus.LOCK},
                    {LevelID.LEVEL_MANNUAL_03, LevelStatus.LOCK},
                    {LevelID.LEVEL_MANNUAL_04, LevelStatus.LOCK},
                    {LevelID.LEVEL_MANNUAL_05, LevelStatus.LOCK},
                }
            );
            CustomizationData = new CustomizationData();
        }
    }
    public class CustomizationData
    {
        public int Body;
        public int BodyPart;
        public int Eyes;
        public int Gloves;
        public int HeadPart;
        public int Mouth;
        public int Nose;
        public int Ears;
        public int Glasses;
        public int Hair;
        public int Hat;
        public int Horn;
        public int Tail;
        public CustomizationData()
        {
            this.Body = 0;
            this.BodyPart = 0;
            this.Eyes = 0;
            this.Gloves = 0;
            this.HeadPart = 0;
            this.Mouth = 0;
            this.Nose = 0;
            this.Ears = 0;
            this.Glasses = 0;
            this.Hair = 0;
            this.Hat = 0;
            this.Horn = 0;
            this.Tail = 0;
        }
    }
}
