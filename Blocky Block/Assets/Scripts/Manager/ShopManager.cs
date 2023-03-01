using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Events;
using BlockyBlock.UI;
using AudioPlayer;
using BlockyBlock.Utils;

namespace BlockyBlock.Managers
{
    public class ShopManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.TransitionOut();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
