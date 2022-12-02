using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BlockyBlock.UI 
{
    public class UICustomButton : MonoBehaviour
    {
        public bool Interactable {get; set;} = true;
        public UnityEvent OnClick;
    }
}
