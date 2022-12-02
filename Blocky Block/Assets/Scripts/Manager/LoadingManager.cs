using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Events;
using BlockyBlock.Utils;
using TMPro;

namespace BlockyBlock.Managers
{
    public class LoadingManager : MonoBehaviour
    {
        public static LoadingManager Instance {get; private set;}
        [SerializeField] TMP_Text loadingText;
        [SerializeField] GameObject Canvas;
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            GameEvents.ON_LOADING += HandleLoading;
        }
        void OnDestroy()
        {
            GameEvents.ON_LOADING -= HandleLoading;
        }
        void HandleLoading(bool status, string message = "")
        {
            if (status)
            {
                UIUtils.LockInput();
            }
            else
            {
                UIUtils.UnlockInput();
            }
            loadingText.text = message;
            this.Canvas.SetActive(status);
        }
    }
}
