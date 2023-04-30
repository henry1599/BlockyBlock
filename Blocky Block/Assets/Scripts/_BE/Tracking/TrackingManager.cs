using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Tracking;
using System;
using BlockyBlock.Enums;

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
            TrackingActionEvent.ON_GAME_EXIT += HandleGameExit;
        }
        void OnDestroy()
        {
            TrackingActionEvent.ON_GAME_ENTER -= HandleGameEnter;
            TrackingActionEvent.ON_GAME_EXIT -= HandleGameExit;
        }

        private void HandleGameExit()
        {
            StopRecord(RecordDataType.SESSION_FINISHED);
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
        public void SetLevelPlay(LevelID levelID)
        {
            int level = (int)levelID % 1000;
            this.trackingHelper.SessionFinished.levelsPlay.Add(level);
        }
        public void SetEndCause(EndCause endCause)
        {
            this.trackingHelper.SessionFinished.endCause = endCause.ToString();
        }
        public void SetShopButtonClickCount()
        {
            this.trackingHelper.SessionFinished.shopButtonClickCount++;
        }
        public void SetCreditButtonClickCount()
        {
            this.trackingHelper.SessionFinished.creditButtonClickCount++;
        }
        public void SetMainGamePlayButtonClickCount()
        {
            this.trackingHelper.SessionFinished.mainGamePlayButtonClickCount++;
        }
        public void SetSettingButtonClickCount()
        {
            this.trackingHelper.SessionFinished.settingButtonClickCount++;
        }
        public void SetProfileButtonClickCount()
        {
            this.trackingHelper.SessionFinished.profileButtonClickCount++;
        }
        public void SetEntry()
        {
            this.trackingHelper.SessionFinished.entry = LevelEntry.None.LevelEntryToString();
        }
        public void SetIsProgress(bool isInProgress)
        {
            this.trackingHelper.SessionFinished.isProgress = isInProgress;
        }
    }
}
