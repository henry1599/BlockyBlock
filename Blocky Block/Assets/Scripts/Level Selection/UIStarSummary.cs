using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BlockyBlock.UI
{
    public class UIStarSummary : MonoBehaviour
    {
        [SerializeField] Transform starImage;
        public void Show()
        {
            starImage.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        }
    }
}
