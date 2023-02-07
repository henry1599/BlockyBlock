using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using UnityEngine.UI;
using BlockyBlock.UI;

namespace BlockyBlock.Common
{
    public class ShopItem : MonoBehaviour
    {
        public RawImage rawImage;
        public int id;
        public CustomizationType type;
        public UICustomButton uiCustomButton;
        public void Setup(CustomizationType type, int id, Texture2D texture, System.Action cb = null)
        {
            this.type = type;
            this.id = id;
            this.rawImage.texture = texture;

            this.uiCustomButton.OnClick.AddListener(() => cb());
        }
    }
}
