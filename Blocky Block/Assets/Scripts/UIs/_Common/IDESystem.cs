using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlockyBlock.Events;
using Utility.SLayout;
using DG.Tweening;
using BlockyBlock.Managers;

namespace BlockyBlock.UI
{
    public class IDESystem : MonoBehaviour
    {
        [SerializeField] private RectTransform m_ThisRect;
        [Tooltip("The top padding for level title")]
        [SerializeField] private float m_TopPadding = 350;
        [Tooltip("The bottom padding for tools")]
        [SerializeField] private float m_BottomPadding = 90;
        [Tooltip("Scale duration")]
        [SerializeField] private float m_ScaleDuration = 0.5f;
        private float m_MinRectHeight;
        [Tooltip("The content that contain the SVerticalLayoutGroup")]
        [SerializeField] private SVerticalLayoutGroup m_ContentLayout;
        private float m_CurrentIDEHeight = 0;
        // Start is called before the first frame update
        void Start()
        {
            m_MinRectHeight = Screen.height;
            EditorEvents.ON_IDE_CONTENT_CHANGED += HandleContentChanged;
        }
        void OnDestroy()
        {
            EditorEvents.ON_IDE_CONTENT_CHANGED -= HandleContentChanged;
        }
        void HandleContentChanged(int _childCount)
        {
            float contentSpacing = m_ContentLayout.spacing;
            float contentTopPadding = m_ContentLayout.padding.top;
            m_CurrentIDEHeight = 
                m_TopPadding + contentTopPadding + contentSpacing * _childCount + m_BottomPadding;
            m_CurrentIDEHeight = Mathf.Max(m_CurrentIDEHeight, 1080);
            Vector2 finalSize = new Vector2(m_ThisRect.sizeDelta.x, m_CurrentIDEHeight);

            DOTween.To(() => m_ThisRect.sizeDelta, value => m_ThisRect.sizeDelta = value, finalSize, m_ScaleDuration);
        }
    }
}
