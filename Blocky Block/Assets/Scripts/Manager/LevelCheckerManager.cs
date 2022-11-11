using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.Managers
{
    public class LevelCheckerManager : MonoBehaviour
    {
        public static LevelCheckerManager Instance {get; private set;}
        private int blockUsed = 0;
        private int stepPassed = 0;
        public int BlockUsed => this.blockUsed;
        public int StepPassed => this.stepPassed;
        void Awake()
        {
            Instance = this;
        }
        public void ConfirmBlockUsed()
        {
            this.blockUsed = UIManager.Instance.m_IDECodeContent.childCount - 1;
        }
        public void ConfirmStepPassed()
        {
            this.stepPassed = this.stepPassed < 0 ? 0 : this.stepPassed;
        }
        public void ResetStepPassed()
        {
            this.stepPassed = 0;
        }
        public void SetStepPassed(int value)
        {
            this.stepPassed += value;
        }
    }
}
