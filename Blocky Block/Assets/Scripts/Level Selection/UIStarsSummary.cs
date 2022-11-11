using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.UI
{
    public class UIStarsSummary : MonoBehaviour
    {
        [SerializeField] UIStarSummary[] stars;
        int currentIdx = 0;
        public void ShowStar()
        {
            stars[currentIdx].Show();
            currentIdx++;
        }
    }
}
