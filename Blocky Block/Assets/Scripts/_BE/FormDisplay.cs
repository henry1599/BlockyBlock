using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using BlockyBlock.Utils;

namespace BlockyBlock.UI 
{
    public class FormDisplay : MonoBehaviour
    {
        public FormType Type;
        [SerializeField] protected Transform container;
        [SerializeField] protected float transitionDuration = 0.15f;
        void Start()
        {
            BEFormEvents.ON_ENABLED += HandleEnableForm;
        }
        void OnDestroy()
        {
            BEFormEvents.ON_ENABLED -= HandleEnableForm;
        }
        void HandleEnableForm(FormType type)
        {
            if (type != this.Type)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
        protected virtual void Open()
        {
            UIUtils.LockInput();
            container.DOScale(Vector3.one, transitionDuration).SetEase(Ease.InOutSine).OnComplete(() => UIUtils.UnlockInput());
        }
        protected virtual void Close()
        {
            UIUtils.LockInput();
            container.DOScale(Vector3.zero, transitionDuration).SetEase(Ease.InOutSine).OnComplete(() => UIUtils.UnlockInput());
        }
    }
}
