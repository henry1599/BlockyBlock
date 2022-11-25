using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using BlockyBlock.Enums;
using System.Collections.Generic;

namespace BlockyBlock.Managers
{
    public class FormManager : MonoBehaviour
    {
        [SerializeField] protected List<APIType> APIsHandle;
        protected bool isError;
        protected string errorMessage;
        void Start()
        {
            WWWManager.ON_ERROR += HandleError;
        }
        void OnDestroy()
        {
            WWWManager.ON_ERROR -= HandleError;
        }
        void HandleError(APIType apiType, string message)
        {
            if (!APIsHandle.Contains(apiType))
                return;
            this.isError = true;
            this.errorMessage = message;
            Debug.Log("Error raised : " + message);
        }
    }
}
