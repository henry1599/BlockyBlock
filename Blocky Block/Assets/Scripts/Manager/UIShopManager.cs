using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using Helpers;
using BlockyBlock.Common;

namespace BlockyBlock.Managers
{
    public class UIShopManager : MonoBehaviour
    {
        [SerializeField] ShopConfig shopConfig;
        [SerializeField] Animator animator;
        [SerializeField] ShopItem itemTemplate;
        [SerializeField] Transform itemContainer;
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
            itemContainer.DeletChildren();
            List<ShopItemData> shopItemDatas = shopConfig.GetItemListByCustomizationType(type);
            foreach (var shopItem in shopItemDatas)
            {
                int idx = shopItem.id;
                Texture2D texture2D = shopItem.sprite;

                ShopItem shopItemInstance = Instantiate(this.itemTemplate, this.itemContainer);
                shopItemInstance.Setup(type, idx, texture2D, null);
            }
        }
        public void OnTabButtonClick(string tabName)
        {
            CustomizationType tabType = tabName.ToCustomizationType();
            if (tabType == CustomizationType.NONE)
                return;
            LoadContent(tabType);
            TriggerAnimOpen();
        }
        public void OnBackButtonClick()
        {
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
    }
}
