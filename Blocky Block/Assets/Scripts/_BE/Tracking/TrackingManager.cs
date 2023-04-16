using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Tracking;

namespace BlockyBlock.Managers
{
    public class TrackingManager : MonoBehaviour
    {
        public static TrackingManager Instance {get; private set;}
        [SerializeField] private TrackingHelper trackingHelper;
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
        public void StartRecord(RecordDataType dataType)
        {
            switch(dataType)
            {
                case RecordDataType.LEVEL_FINISHED:
                    this.trackingHelper.StartRecordLevelFinished();
                    break;
                case RecordDataType.LEVEL_TRIGGER:
                    this.trackingHelper.StartRecordLevelTrigger();
                    break;
                case RecordDataType.SESSION_FINISHED:
                    this.trackingHelper.StartRecordSessionFinished();
                    break;
                case RecordDataType.SESSION_TRIGGER:
                    this.trackingHelper.StartRecordSessionTrigger();
                    break;
                default:
                    this.trackingHelper.StartRecordAll();
                    break;
            }
        }
        public void StopRecord(RecordDataType dataType)
        {
            switch(dataType)
            {
                case RecordDataType.LEVEL_FINISHED:
                    this.trackingHelper.StopRecordLevelFinished();
                    break;
                case RecordDataType.LEVEL_TRIGGER:
                    this.trackingHelper.StopRecordLevelTrigger();
                    break;
                case RecordDataType.SESSION_FINISHED:
                    this.trackingHelper.StopRecordSessionFinished();
                    break;
                case RecordDataType.SESSION_TRIGGER:
                    this.trackingHelper.StopRecordSessionTrigger();
                    break;
                default:
                    this.trackingHelper.StopRecordAll();
                    break;
            }
        }
    }
}
