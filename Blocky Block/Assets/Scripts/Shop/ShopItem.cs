using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using UnityEngine.UI;
using BlockyBlock.UI;
using BlockyBlock.Managers;

namespace BlockyBlock.Common
{
    public class ShopItem : MonoBehaviour
    {
        public RawImage rawImage;
        public int id;
        public CustomizationType type;
        public UICustomButton uiCustomButton;
        public void Setup(CustomizationType type, int id, Texture2D texture)
        {
            this.type = type;
            this.id = id;
            this.rawImage.texture = texture;
        }
        public void OnClick()
        {
            ProfileManager.Instance.UnlockCustomization(type, id);
        }
    }
}
