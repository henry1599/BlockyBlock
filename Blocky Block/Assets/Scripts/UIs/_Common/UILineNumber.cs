using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BlockyBlock.UI 
{
    public class UILineNumber : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_LineNumber;
        public void Unset()
        {
            m_LineNumber.text = "";
        }
        public void Setup(Transform _uiBlockTransform)
        {
            int line = _uiBlockTransform.GetSiblingIndex() + 1;
            m_LineNumber.text = line.ToString();
        }
    }
}
