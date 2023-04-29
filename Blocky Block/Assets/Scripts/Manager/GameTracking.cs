using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Tracking;
using BlockyBlock.Configurations;
using BlockyBlock.Enums;
using BlockyBlock.BackEnd;

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
#if UNITY_EDITOR
            string logData = JsonUtility.ToJson(recordData);
            Debug.Log("[TRACK] : " + logData);
#endif
            RequestTrackingLevelFinished requestData = new(RecordDataType.LEVEL_FINISHED.ToString(), recordData, recordData.metadata);
            WWWManager.Instance.Post(requestData, WebType.TRACKING, APIType.TRACKER, (BEConstants.CONTENT_TYPE, BEConstants.CONTENT_VALUE), (BEConstants.CONTENT_TYPE_TRACKING, GameManager.Instance.AccessToken));
        }
        public void SendLevelTriggerRecordData(LevelTriggerRecordData recordData)
        {
#if UNITY_EDITOR
            string logData = JsonUtility.ToJson(recordData);
            Debug.Log("[TRACK] : " + logData);
#endif
            RequestTrackingLevelTrigger requestData = new(RecordDataType.LEVEL_TRIGGER.ToString(), recordData, recordData.metadata);
            WWWManager.Instance.Post(requestData, WebType.TRACKING, APIType.TRACKER, (BEConstants.CONTENT_TYPE, BEConstants.CONTENT_VALUE), (BEConstants.CONTENT_TYPE_TRACKING, GameManager.Instance.AccessToken));
        }
        public void SendSessionFinishedRecordData(SessionFinishedRecordData recordData)
        {
#if UNITY_EDITOR
            string logData = JsonUtility.ToJson(recordData);
            Debug.Log("[TRACK] : " + logData);
#endif
            RequestTrackingSessionFinished requestData = new(RecordDataType.SESSION_FINISHED.ToString(), recordData, recordData.metadata);
            WWWManager.Instance.Post(requestData, WebType.TRACKING, APIType.TRACKER, (BEConstants.CONTENT_TYPE, BEConstants.CONTENT_VALUE), (BEConstants.CONTENT_TYPE_TRACKING, GameManager.Instance.AccessToken));
        }
        public void SendSessionTriggerRecordData(SessionTriggerRecordData recordData)
        {
#if UNITY_EDITOR
            string logData = JsonUtility.ToJson(recordData);
            Debug.Log("[TRACK] : " + logData);
#endif
            RequestTrackingSessionTrigger requestData = new(RecordDataType.SESSION_TRIGGER.ToString(), recordData, recordData.metadata);
            WWWManager.Instance.Post(requestData, WebType.TRACKING, APIType.TRACKER, (BEConstants.CONTENT_TYPE, BEConstants.CONTENT_VALUE), (BEConstants.CONTENT_TYPE_TRACKING, GameManager.Instance.AccessToken));
        }
        [System.Serializable]
        public class RequestTracking
        {
            public string activityName;
            public BlockyBlock.Tracking.Metadata metaData;
            public RequestTracking()
            {
                this.activityName = string.Empty;
                this.metaData = null;
            }
            public RequestTracking(string name, BlockyBlock.Tracking.Metadata meta)
            {
                this.activityName = name;
                this.metaData = meta;
            }
        }
        [System.Serializable]
        public class RequestTrackingSessionTrigger : RequestTracking
        {
            public SessionTriggerRecordData trackingData;
            public RequestTrackingSessionTrigger()
            {
                this.trackingData = new();
            }
            public RequestTrackingSessionTrigger(string name, SessionTriggerRecordData data, BlockyBlock.Tracking.Metadata meta) : base(name, meta)
            {
                this.trackingData = data;
            }
        }
        [System.Serializable]
        public class RequestTrackingSessionFinished : RequestTracking
        {
            public SessionFinishedRecordData trackingData;
            public RequestTrackingSessionFinished() : base()
            {
                this.trackingData = new();
            }
            public RequestTrackingSessionFinished(string name, SessionFinishedRecordData data, BlockyBlock.Tracking.Metadata meta) : base(name, meta)
            {
                this.trackingData = data;
            }
        }
        [System.Serializable]
        public class RequestTrackingLevelTrigger : RequestTracking
        {
            public LevelTriggerRecordData trackingData;
            public RequestTrackingLevelTrigger() : base()
            {
                this.trackingData = new();
            }
            public RequestTrackingLevelTrigger(string name, LevelTriggerRecordData data, BlockyBlock.Tracking.Metadata meta) : base(name, meta)
            {
                this.trackingData = data;
            }
        }
        [System.Serializable]
        public class RequestTrackingLevelFinished : RequestTracking
        {
            public LevelFinishedRecordData trackingData;
            public RequestTrackingLevelFinished() : base()
            {
                this.trackingData = new();
            }
            public RequestTrackingLevelFinished(string name, LevelFinishedRecordData data, BlockyBlock.Tracking.Metadata meta) : base(name, meta)
            {
                this.trackingData = data;
            }
        }
    }
}
