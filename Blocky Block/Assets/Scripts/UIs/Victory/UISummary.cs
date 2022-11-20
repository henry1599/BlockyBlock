using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using BlockyBlock.Managers;
using Helpers;
using DG.Tweening;

namespace BlockyBlock.UI 
{
    public class UISummary : MonoBehaviour
    {
        [SerializeField] UIStarsSummary starsSummary;
        [SerializeField] float delayEachStar;
        [SerializeField] List<TMP_Text> conditions;
        [SerializeField] Image[] conditionBorders;
        [SerializeField] Color unfinishedColor;
        [SerializeField] Color finishedColor;
        public void Setup(bool winGame = true, bool usedBlockPassed = false, bool stepPassed = false)
        {
            int minBlockUsed = LevelManager.Instance.CurrentLevelData.MinimumExecutionBlock;
            int minStepPassed = LevelManager.Instance.CurrentLevelData.MinimumExecutionStep;

            string[] conditionTexts = new string[3]
            {
                "Win the level",
                (new StringBuilder()).AppendFormat("Use at most {0} blocks", minBlockUsed).ToString(),
                (new StringBuilder()).AppendFormat("Run at most {0} steps", minStepPassed).ToString()
            };
            for (int i = 0; i < conditionTexts.Length; i++)
            {
                conditions[i].text = conditionTexts[i];
            }
            conditionBorders[0].color = winGame ? finishedColor : unfinishedColor;
            conditionBorders[1].color = usedBlockPassed ? finishedColor : unfinishedColor;
            conditionBorders[2].color = stepPassed ? finishedColor : unfinishedColor;

            int starCount = 0;
            if (winGame) starCount++;
            if (usedBlockPassed) starCount++;
            if (stepPassed) starCount++;

            StartCoroutine(Cor_Validate(starCount));
        }
        IEnumerator Cor_Validate(int stars)
        {
            yield return Helper.GetWait(0.5f);
            for (int i = 0; i < stars; i++)
            { 
                DOVirtual.DelayedCall(i * this.delayEachStar, () => 
                {
                    this.starsSummary.ShowStar();
                    SoundManager.Instance.PlaySound(AudioPlayer.SoundID.STAR_GAIN_VICTORY);
                });
            }
        }
    }
}
