using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Tools;
using BlockyBlock.Events;
using BlockyBlock.Enums;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.Managers
{
    [System.Serializable]
    public class CursorData : SerializableDictionaryBase<CursorType, Texture2D> {}
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager Instance {get; private set;}
        public CursorData CursorData;
        public CursorType CurrentCursorType {get; set;}
        public bool IsHome;
        void Awake()
        {
            Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            ToolEvents.ON_CURSOR_CHANGED += HandleCursorChanged;
            HandleCursorChanged(CursorType.SELECTION);
        }
        void OnDestroy()
        {
            ToolEvents.ON_CURSOR_CHANGED += HandleCursorChanged;
        }
        void HandleCursorChanged(CursorType _type)
        {
            CurrentCursorType = _type;
            Cursor.SetCursor(CursorData[_type], Vector3.zero, CursorMode.Auto);
        }
    }
}
