using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockyBlock.Events;
using BlockyBlock.UI;
using BlockyBlock.Enums;
using TMPro;

namespace BlockyBlock.Managers
{
    public class UIErrorManager : MonoBehaviour
    {
        // Start is called before the first frame update
        public static UIErrorManager Instance {get; private set;}
        UIError m_UIError;
        void Awake()
        {
            // if (Instance != null)
            // {
            //     Destroy(gameObject);
            //     return;
            // }
            // DontDestroyOnLoad(gameObject);
            Instance = this;
            m_UIError = GameObject.FindGameObjectWithTag(GameConstants.UIERROR_TAG)?.GetComponent<UIError>();
        }
        void Start()
        {
            ErrorEvents.ON_ERROR += HandleError;
        }
        void OnDestroy()
        {
            ErrorEvents.ON_ERROR -= HandleError;    
        }
        void HandleError(ErrorType _type)
        {
            SoundManager.Instance.PlaySound(AudioPlayer.SoundID.ERROR_ALERT);
            ErrorEvents.ON_ERROR_HANDLING?.Invoke();
            string msg = ConfigManager.Instance.ErrorConfig.ErrorData[_type];
            m_UIError.Open(msg);
            GameEvents.ON_SHAKE_CAMERA?.Invoke();
        }
        public void Reset()
        {
            m_UIError.Close();
        }
    }
}
    
