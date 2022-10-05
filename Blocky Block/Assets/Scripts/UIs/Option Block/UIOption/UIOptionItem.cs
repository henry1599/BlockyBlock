using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BlockyBlock.UI 
{
    public class UIOptionItem : MonoBehaviour
    {
        [SerializeField] TMP_Text m_Text;
        protected int m_Idx;
        public void SetText(int _idx, string _text)
        {
            m_Idx = _idx;
            m_Text.text = _text;
        }
        public virtual void OnClick()
        {
        }
    }
}
