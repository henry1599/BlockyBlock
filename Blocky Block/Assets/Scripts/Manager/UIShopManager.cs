using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using Helpers;
using BlockyBlock.Common;
using BlockyBlock.Events;
using UnityEngine.UI;

namespace BlockyBlock.Managers
{
    public class UIShopManager : MonoBehaviour
    {
        [SerializeField] ShopConfig shopConfig;
        [SerializeField] Animator animator;
        [SerializeField] ShopItem itemTemplate;
        [SerializeField] Button clearButtonTemplate;
        [SerializeField] Transform itemContainer;
        private Button clearButton;
        private readonly int OpenKey = Animator.StringToHash("isOpen");
        void Start()
        {
            LoadTab();
        }
        public void LoadTab()
        {
            TriggerAnimClose();
        }
        public void LoadContent(CustomizationType type)
        {
            if (this.clearButton != null)
            {
                this.clearButton.onClick.RemoveAllListeners();
                Destroy(this.clearButton.gameObject);
            }
            itemContainer.DeletChildren();
            this.clearButton = Instantiate(this.clearButtonTemplate, this.itemContainer).GetComponent<Button>();
            clearButton.onClick.AddListener(() => ProfileManager.Instance.ResetCustomization(type));
            List<ShopItemData> shopItemDatas = shopConfig.GetItemListByCustomizationType(type);
            foreach (var shopItem in shopItemDatas)
            {
                int idx = shopItem.id;
                Texture2D texture2D = shopItem.sprite;

                ShopItem shopItemInstance = Instantiate(this.itemTemplate, this.itemContainer);
                shopItemInstance.Setup(type, idx, texture2D);
            }
        }
        public void OnTabButtonClick(string tabName)
        {
            CustomizationType tabType = tabName.ToCustomizationType();
            if (tabType == CustomizationType.NONE)
                return;
            LoadContent(tabType);
            if (tabType == CustomizationType.TAIL)
            {
                ShopManager.Instance.OnRotateCharacterToTail();
            }
            else
            {
                ShopManager.Instance.OnResetCharacterTransform();
            }
            TriggerAnimOpen();
        }
        public void OnBackButtonClick()
        {
            ShopManager.Instance.OnResetCharacterTransform();
            TriggerAnimClose();
        }
        void TriggerAnimOpen()
        {
            this.animator.SetBool(OpenKey, true);
        }
        void TriggerAnimClose()
        {
            this.animator.SetBool(OpenKey, false);
        }
        public void OnBackSceneButtonClick()
        {
            GameManager.Instance.TransitionIn(() => GameEvents.LOAD_LEVEL?.Invoke(LevelID.HOME));
        }
    }
}
