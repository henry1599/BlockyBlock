using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Tracking;
using System;

namespace BlockyBlock.Managers
{
    public class TrackingManager : MonoBehaviour
    {
        public static TrackingManager Instance {get; private set;}
        public TrackingHelper Helper => this.trackingHelper;
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
        void Start()
        {
            TrackingActionEvent.ON_GAME_ENTER += HandleGameEnter;
        }
        void OnDestroy()
        {
            TrackingActionEvent.ON_GAME_ENTER -= HandleGameEnter;
        }

        private void HandleGameEnter()
        {
            StartRecord(RecordDataType.SESSION_TRIGGER);
            StartRecord(RecordDataType.SESSION_FINISHED);
            StopRecord(RecordDataType.SESSION_TRIGGER);
        }

        public void StartRecord(RecordDataType dataType = RecordDataType.NONE)
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
        public void StopRecord(RecordDataType dataType = RecordDataType.NONE)
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
