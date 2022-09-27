using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using TMPro;

namespace BlockyBlock.Managers
{
    public class UIErrorManager : MonoBehaviour
    {
        [SerializeField] TMP_Text m_ErrorText;
        // Start is called before the first frame update
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
            ErrorEvents.ON_ERROR_HANDLING?.Invoke();
            // m_ErrorText.text = ConfigManager.Instance.ErrorConfig.ErrorData[_type];
            print(ConfigManager.Instance.ErrorConfig.ErrorData[_type]);
        }
    }
}
    
