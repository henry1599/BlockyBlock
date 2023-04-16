using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.Tracking
{
    public enum RecordDataType
    {
        NONE = -1,
        LEVEL_FINISHED = 0,
        LEVEL_TRIGGER,
        SESSION_FINISHED,
        SESSION_TRIGGER
    }
    [System.Serializable]
    public class RecordData
    {
        public RecordData() {}
    }
    [System.Serializable]
    public class LevelFinishedRecordData : RecordData
    {
        public string levelId;
        public string chapterId;
        public int debugButtonCount;
        public string endCause;
        public int pickUpBlockCount;
        public int pickUpBlockCountUse;
        public int playButtonCount;
        public int pushBlockCount;
        public int pushBlockCountUse;
        public int putDownBlockCount;
        public int putDownBlockCountUse;
        public int stepForwardBlockCount;
        public int stepForwardBlockCountUse;
        public int stopButtonCount;
        public int timeSpent;
        public int turnLeftBlockCount;
        public int turnLeftBlockCountUse;
        public int turnRightBlockCount;
        public int turnRightBlockCountUse;
        public int jumpBlockCount;
        public int jumpBlockCountUse;
        public int jumpIfBlockCount;
        public int jumpIfBlockCountUse;
        public int ifElseBlockCount;
        public int ifElseBlockCountUse;
        public Metadata metadata;
        public string entry;
        public LevelFinishedRecordData() : base()
        {
            this.levelId = string.Empty;
            this.chapterId = string.Empty;
            this.debugButtonCount = 0;
            this.endCause = string.Empty;
            this.pickUpBlockCount = 0;
            this.pickUpBlockCountUse = 0;
            this.playButtonCount = 0;
            this.pushBlockCount = 0;
            this.pushBlockCountUse = 0;
            this.putDownBlockCount = 0;
            this.putDownBlockCountUse = 0;
            this.stepForwardBlockCount = 0;
            this.stepForwardBlockCountUse = 0;
            this.stopButtonCount = 0;
            this.timeSpent = 0;
            this.turnLeftBlockCount = 0;
            this.turnLeftBlockCountUse = 0;
            this.turnRightBlockCount = 0;
            this.turnRightBlockCountUse = 0;
            this.jumpBlockCount = 0;
            this.jumpBlockCountUse = 0;
            this.jumpIfBlockCount = 0;
            this.jumpIfBlockCountUse = 0;
            this.ifElseBlockCount = 0;
            this.ifElseBlockCountUse = 0;
            this.metadata = null;
            this.entry = string.Empty;
        }
        public LevelFinishedRecordData(LevelFinishedRecordData data) : base()
        {
            this.levelId = data.levelId;
            this.chapterId = data.chapterId;
            this.debugButtonCount = data.debugButtonCount;
            this.endCause = data.endCause;
            this.pickUpBlockCount = data.pickUpBlockCount;
            this.pickUpBlockCountUse = data.pickUpBlockCountUse;
            this.playButtonCount = data.playButtonCount;
            this.pushBlockCount = data.pushBlockCount;
            this.pushBlockCountUse = data.pushBlockCountUse;
            this.putDownBlockCount = data.putDownBlockCount;
            this.putDownBlockCountUse = data.putDownBlockCountUse;
            this.stepForwardBlockCount = data.stepForwardBlockCount;
            this.stepForwardBlockCountUse = data.stepForwardBlockCountUse;
            this.stopButtonCount = data.stopButtonCount;
            this.timeSpent = data.timeSpent;
            this.turnLeftBlockCount = data.turnLeftBlockCount;
            this.turnLeftBlockCountUse = data.turnLeftBlockCountUse;
            this.turnRightBlockCount = data.turnRightBlockCount;
            this.turnRightBlockCountUse = data.turnRightBlockCountUse;
            this.jumpBlockCount = data.jumpBlockCount;
            this.jumpBlockCountUse = data.jumpBlockCountUse;
            this.jumpIfBlockCount = data.jumpIfBlockCount;
            this.jumpIfBlockCountUse = data.jumpIfBlockCountUse;
            this.ifElseBlockCount = data.ifElseBlockCount;
            this.ifElseBlockCountUse = data.ifElseBlockCountUse;
            this.metadata = data.metadata;
            this.entry = data.entry;
        }
    }
    [System.Serializable]
    public class LevelTriggerRecordData : RecordData
    {
        public string levelId;
        public string chapterId;
        public Metadata metadata;
        public string entry;
        public LevelTriggerRecordData() : base()
        {
            this.levelId = string.Empty;
            this.chapterId = string.Empty;
            this.metadata = null;
            this.entry = string.Empty;
        }
        public LevelTriggerRecordData(LevelTriggerRecordData data) : base()
        {
            this.levelId = data.levelId;
            this.chapterId = data.chapterId;
            this.metadata = data.metadata;
            this.entry = data.entry;
        }
    }
    [System.Serializable]
    public class SessionFinishedRecordData : RecordData
    {
        public List<int> levelsPlay;
        public string endCause;
        public int shopButtonClickCount;
        public int creditButtonClickCount;
        public int mainGamePlayButtonClickCount;
        public int settingButtonClickCount;
        public int profileButtonClickCount;
        public int timeSpent;
        public Metadata metadata;
        public string entry;
        public SessionFinishedRecordData() : base()
        {
            this.levelsPlay = new();
            this.endCause = string.Empty;
            this.shopButtonClickCount = 0;
            this.creditButtonClickCount = 0;
            this.mainGamePlayButtonClickCount = 0;
            this.settingButtonClickCount = 0;
            this.profileButtonClickCount = 0;
            this.timeSpent = 0;
            this.metadata = null;
            this.entry = string.Empty;
        }
        public SessionFinishedRecordData(SessionFinishedRecordData data) : base()
        {
            this.levelsPlay = data.levelsPlay;
            this.endCause = data.endCause;
            this.shopButtonClickCount = data.shopButtonClickCount;
            this.creditButtonClickCount = data.creditButtonClickCount;
            this.mainGamePlayButtonClickCount = data.mainGamePlayButtonClickCount;
            this.settingButtonClickCount = data.settingButtonClickCount;
            this.profileButtonClickCount = data.profileButtonClickCount;
            this.timeSpent = data.timeSpent;
            this.metadata = data.metadata;
            this.entry = data.entry;
        }
    }
    [System.Serializable]
    public class SessionTriggerRecordData : RecordData
    {
        public Metadata metadata;
        public SessionTriggerRecordData() : base()
        {
            this.metadata = null;
        }
        public SessionTriggerRecordData(SessionTriggerRecordData data) : base()
        {
            this.metadata = data.metadata;
        }
    }
    [System.Serializable]
    public class Metadata
    {
        
    }
}
