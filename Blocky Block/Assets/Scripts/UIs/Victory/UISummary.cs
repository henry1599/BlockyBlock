using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using BlockyBlock.Managers;
using Helpers;

namespace BlockyBlock.UI 
{
    public class UISummary : MonoBehaviour
    {
        [SerializeField] List<TMP_Text> conditions;
        public void Setup()
        {
            int minBlockUsed = LevelManager.Instance.CurrentLevelData.MinimumExecutionBlock;
            int minStepPassed = LevelManager.Instance.CurrentLevelData.MinimumExecutionStep;

            string[] conditionTexts = new string[3]
            {
                "Win the level",
                (new StringBuilder()).AppendFormat("Use at least {0} blocks", minBlockUsed).ToString(),
                (new StringBuilder()).AppendFormat("Run at least {0} steps", minStepPassed).ToString()
            };
            for (int i = 0; i < conditionTexts.Length; i++)
            {
                conditions[i].text = conditionTexts[i];
            }
            StartCoroutine(Cor_Validate());
        }
        IEnumerator Cor_Validate()
        {
            yield break;
        }
    }
}
