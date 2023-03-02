using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using BlockyBlock.UI;
using AudioPlayer;
using BlockyBlock.Utils;
using DG.Tweening;

namespace BlockyBlock.Managers
{
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager Instance {get; private set;}
        private static readonly string[] excitedAnims = new string[4]
        {
            "Excited01",
            "Excited02",
            "Excited03",
            "Excited04"
        };
        [SerializeField] GameObject vfxEquip;
        [SerializeField] Transform vfxContainer;
        public bool IsExciting {get; set;} = false;
        void Awake()
        {
            Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.TurnOffSeeBehind();
            GameManager.Instance.TransitionOut();
        }
        public void OnRotateCharacterToTail()
        {
            if (CustomizationManager.Instance == null)
                return;
            if (CustomizationManager.Instance.Display == null)
                return;
            CustomizationManager.Instance.Display.transform.DOKill();
            CustomizationManager.Instance.Display.transform.DOLocalRotate(
                new Vector3(0, 30, 0),
                0.25f,
                RotateMode.Fast
            );
        }
        public void OnResetCharacterTransform()
        {
            if (CustomizationManager.Instance == null)
                return;
            if (CustomizationManager.Instance.Display == null)
                return;
            CustomizationManager.Instance.Display.transform.DOKill();
            CustomizationManager.Instance.Display.transform.DOLocalRotate(
                new Vector3(0, 180, 0),
                0.25f,
                RotateMode.Fast
            );
        }
        public void OnExcited()
        {
            if (IsExciting)
                return;
            CustomizationManager.Instance.PlayAnim(excitedAnims[Random.Range(0, excitedAnims.Length)]);
            PlayVfx();
        }
        public void PlayVfx()
        {
            Instantiate(this.vfxEquip, this.vfxContainer);
        }
    }
}
