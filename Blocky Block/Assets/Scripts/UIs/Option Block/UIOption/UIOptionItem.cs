using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BlockyBlock.UI 
{
    public class UIOptionItem : MonoBehaviour
    {
        [SerializeField] TMP_Text m_Text;
        private int m_Idx;
        public static System.Action<int> ON_OPTION_ITEM_CLICKED;
        public void SetText(int _idx, string _text)
        {
            m_Idx = _idx;
            m_Text.text = _text;
        }
        public void OnClick()
        {
            ON_OPTION_ITEM_CLICKED?.Invoke(m_Idx);
        }
    }
}
