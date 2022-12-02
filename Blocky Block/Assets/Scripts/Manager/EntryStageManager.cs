using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.Managers
{
    public class EntryStageManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.TransitionOut();
        }
    }
}
