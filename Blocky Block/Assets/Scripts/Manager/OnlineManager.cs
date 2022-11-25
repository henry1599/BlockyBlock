using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.Managers
{
    public class OnlineManager : MonoBehaviour
    {
        public static readonly string DEFAULT_VALUE = "d3F4aU1";
        public static readonly string CONTENT_VALUE = "application/json";
        public static readonly string CONTENT_TYPE = "Content-Type";
        public static readonly string WEB_URL = "https://4jdwf133v0.execute-api.ap-southeast-1.amazonaws.com";
        public static OnlineManager Instance {get; private set;}
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }
}
