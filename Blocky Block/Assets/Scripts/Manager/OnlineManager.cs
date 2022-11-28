using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.Managers
{
    public class OnlineManager : MonoBehaviour
    {
        public static OnlineManager Instance {get; private set;}
        public static string REGISTER_EMAIL = "";
        public static string VERIFICATION_TOKEN_REGISTER = "";
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
