using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Utils;

namespace BlockyBlock.Managers
{
    public class LoadingManager : MonoBehaviour
    {
        public static LoadingManager Instance {get; private set;}
        [SerializeField] GameObject Canvas;
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            GameEvents.ON_LOADING += HandleLoading;
        }
        void OnDestroy()
        {
            GameEvents.ON_LOADING -= HandleLoading;
        }
        void HandleLoading(bool status)
        {
            if (status)
            {
                UIUtils.LockInput();
            }
            else
            {
                UIUtils.UnlockInput();
            }
            this.Canvas.SetActive(status);
        }
    }
}
