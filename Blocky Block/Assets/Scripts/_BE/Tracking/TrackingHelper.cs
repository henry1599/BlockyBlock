using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Managers;

namespace BlockyBlock.Tracking
{
    public class TrackingHelper : MonoBehaviour
    {
        public LevelFinishedRecordData LevelFinished => this.levelFinishedRecordData;
        private LevelFinishedRecordData levelFinishedRecordData;
        public LevelTriggerRecordData LevelTrigger => this.levelTriggerRecordData;
        private LevelTriggerRecordData levelTriggerRecordData;
        public SessionFinishedRecordData SessionFinished => this.sessionFinishedRecordData;
        private SessionFinishedRecordData sessionFinishedRecordData;
        public SessionTriggerRecordData SessionTrigger => this.sessionTriggerRecordData;
        private SessionTriggerRecordData sessionTriggerRecordData;
        public void StartRecordAll()
        {
            StartRecordLevelFinished();
            StartRecordLevelTrigger();
            StartRecordSessionFinished();
            StartRecordSessionTrigger();
        }
        public void StartRecordLevelFinished()
        {
            this.levelFinishedRecordData = new();
        }
        public void StopRecordLevelFinished()
        {
            GameTracking.Instance.SendLevelFinishedRecordData(new(this.levelFinishedRecordData));
        }
        public void StartRecordLevelTrigger()
        {
            this.levelTriggerRecordData = new();
        }
        public void StopRecordLevelTrigger()
        {
            GameTracking.Instance.SendLevelTriggerRecordData(new(this.levelTriggerRecordData));
        }
        public void StartRecordSessionFinished()
        {
            this.sessionFinishedRecordData = new();
        }
        public void StopRecordSessionFinished()
        {
            GameTracking.Instance.SendSessionFinishedRecordData(new(this.sessionFinishedRecordData));
        }
        public void StartRecordSessionTrigger()
        {
            this.sessionTriggerRecordData = new();
        }
        public void StopRecordSessionTrigger()
        {
            GameTracking.Instance.SendSessionTriggerRecordData(new(this.sessionTriggerRecordData));
        }
        public void StopRecordAll()
        {
            StopRecordLevelFinished();
            StopRecordLevelTrigger();
            StopRecordSessionFinished();
            StopRecordSessionTrigger();
        }
    }
}
