using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IngameDebugConsole;

namespace BlockyBlock.Cheat
{
    public class CheatMenu : MonoBehaviour
    {
        [Space(10)]
        [Header("Local Data Content")]
        [SerializeField] Button resetLocalDataButton;
        [SerializeField] Button resetUserProfileButton;
        [SerializeField] Button resetAccountButton;
        void OnEnable()
        {
            this.resetLocalDataButton.onClick.AddListener(() => OnResetLocalDataButtonClick());
            this.resetUserProfileButton.onClick.AddListener(() => OnResetUserProfileButtonClick());
            this.resetAccountButton.onClick.AddListener(() => OnResetAccountButtonClick());
        }
        void OnDisable()
        {
            this.resetLocalDataButton.onClick.RemoveListener(() => OnResetLocalDataButtonClick());
            this.resetUserProfileButton.onClick.RemoveListener(() => OnResetUserProfileButtonClick());
            this.resetAccountButton.onClick.RemoveListener(() => OnResetAccountButtonClick());
        }
        void OnResetLocalDataButtonClick()
        {
            PlayerPrefs.DeleteAll();
        }
        void OnResetUserProfileButtonClick()
        {
            PlayerPrefs.DeleteKey(Events.GameConstants.PROFILE_KEY);
        }
        void OnResetAccountButtonClick()
        {
            PlayerPrefs.DeleteKey(BackEnd.BEConstants.ACCESS_TOKEN_KEY);
            PlayerPrefs.DeleteKey(BackEnd.BEConstants.REFRESH_TOKEN_KEY);
        }
    }
}
