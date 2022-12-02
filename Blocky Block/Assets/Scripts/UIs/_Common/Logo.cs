using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BlockyBlock
{
    public class Logo : MonoBehaviour
    {
        public Image LogoGraphic;
        public float TransitionDuration;
        public static System.Action ON_LOGO_FINISHED;
        // Start is called before the first frame update
        void Start()
        {
            LogoGraphic.transform.DOScale(Vector3.one * 2, TransitionDuration).SetEase(Ease.InOutSine);
            LogoGraphic.DOFade(0, TransitionDuration / 2).SetEase(Ease.InOutSine).SetDelay(TransitionDuration / 2)
            .OnComplete(() => ON_LOGO_FINISHED?.Invoke());
        }
    }
}
