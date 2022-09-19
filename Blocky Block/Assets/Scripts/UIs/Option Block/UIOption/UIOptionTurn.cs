using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockyBlock.UI;
using BlockyBlock.Enums;
using DG.Tweening;

namespace BlockyBlock.UI 
{
    public class UIOptionTurn : UIOption
    {
        public override void Setup(bool _status)
        {
            base.Setup(_status);
            if (_status)
            {
                transform.DOScaleY(1, DropDuration).SetEase(Ease.InOutSine);
            }
            else
            {
                transform.DOScaleY(0, DropDuration).SetEase(Ease.InOutSine);
            }
        }
    }
}
