using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Tracking;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;

namespace BlockyBlock.Managers
{
    public class GameTracking : MonoBehaviour
    {
        public static GameTracking Instance {get; private set;}
        [SerializeField] APIConfig apiConfig;
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        public void SendLevelFinishedRecordData(LevelFinishedRecordData recordData)
        {
            RequestTracking requestData = new(RecordDataType.LEVEL_FINISHED.ToString(), recordData, recordData.metadata);
            WWWManager.Instance.Post(requestData, WebType.TRACKING, APIType.TRACKER, false);
        }
        public void SendLevelTriggerRecordData(LevelTriggerRecordData recordData)
        {
            RequestTracking requestData = new(RecordDataType.LEVEL_TRIGGER.ToString(), recordData, recordData.metadata);
            WWWManager.Instance.Post(requestData, WebType.TRACKING, APIType.TRACKER, false);
        }
        public void SendSessionFinishedRecordData(SessionFinishedRecordData recordData)
        {
            RequestTracking requestData = new(RecordDataType.SESSION_FINISHED.ToString(), recordData, recordData.metadata);
            WWWManager.Instance.Post(requestData, WebType.TRACKING, APIType.TRACKER, false);
        }
        public void SendSessionTriggerRecordData(SessionTriggerRecordData recordData)
        {
            RequestTracking requestData = new(RecordDataType.SESSION_TRIGGER.ToString(), recordData, recordData.metadata);
            WWWManager.Instance.Post(requestData, WebType.TRACKING, APIType.TRACKER, false);
        }
        [System.Serializable]
        public class RequestTracking
        {
            public string activityName;
            public RecordData trackingData;
            public BlockyBlock.Tracking.Metadata metaData;
            public RequestTracking()
            {
                this.activityName = string.Empty;
                this.trackingData = new();
                this.metaData = null;
            }
            public RequestTracking(string name, RecordData data, BlockyBlock.Tracking.Metadata meta)
            {
                this.activityName = name;
                this.trackingData = data;
                this.metaData = meta;
            }
        }
    }
}
