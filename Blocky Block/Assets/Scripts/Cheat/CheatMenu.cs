using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IngameDebugConsole;

namespace BlockyBlock.Cheat
{
    public class CheatMenu : MonoBehaviour
    {
        string[] titles = new string[]{
            "LOCAL DATA STORAGE",
            "INFO UI"
        };
        [Space(10)]
        [Header("Local Data Content")]
        [SerializeField] Button resetLocalDataButton;
        [SerializeField] Button resetUserProfileButton;
        [SerializeField] Button resetAccountButton;
        [Space(10)]
        [Header("Info UI")]
        [SerializeField] GameObject verionInfo;
        [SerializeField] GameObject consolePopup;
        [SerializeField] Button hideVersionInfo;
        [SerializeField] Button showVersionInfo;
        [SerializeField] Button hideConsolePopup;
        [SerializeField] Button showConsolePopup;
        void OnEnable()
        {
            this.resetLocalDataButton.onClick.AddListener(() => OnResetLocalDataButtonClick());
            this.resetUserProfileButton.onClick.AddListener(() => OnResetUserProfileButtonClick());
            this.resetAccountButton.onClick.AddListener(() => OnResetAccountButtonClick());

            this.hideVersionInfo.onClick.AddListener(() => OnHideVersionInfoButtonClick());
            this.showVersionInfo.onClick.AddListener(() => OnShowVersionInfoButtonClick());
            this.hideConsolePopup.onClick.AddListener(() => OnHideConsolePopupButtonClick());
            this.showConsolePopup.onClick.AddListener(() => OnShowConsolePopupButtonClick());
        }
        void OnDisable()
        {
            this.resetLocalDataButton.onClick.RemoveListener(() => OnResetLocalDataButtonClick());
            this.resetUserProfileButton.onClick.RemoveListener(() => OnResetUserProfileButtonClick());
            this.resetAccountButton.onClick.RemoveListener(() => OnResetAccountButtonClick());

            this.hideVersionInfo.onClick.RemoveListener(() => OnHideVersionInfoButtonClick());
            this.showVersionInfo.onClick.RemoveListener(() => OnShowVersionInfoButtonClick());
            this.hideConsolePopup.onClick.RemoveListener(() => OnHideConsolePopupButtonClick());
            this.showConsolePopup.onClick.RemoveListener(() => OnShowConsolePopupButtonClick());
        }
        #region LOCAL DATA STORAGE
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
        #endregion

        #region INFO UI
        void OnHideVersionInfoButtonClick()
        {
            this.verionInfo?.SetActive(false);
        }
        void OnShowVersionInfoButtonClick()
        {
            this.verionInfo?.SetActive(true);
        }
        void OnHideConsolePopupButtonClick()
        {
            this.consolePopup?.SetActive(false);
        }
        void OnShowConsolePopupButtonClick()
        {
            this.consolePopup?.SetActive(true);
        }
        #endregion
    }
}
