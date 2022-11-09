using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Managers;
using BlockyBlock.Events;

public class LevelNode : MonoBehaviour
{
    public LevelType LevelType;
    public LevelID LevelID;
    public void OnClick()
    {
        GameManager.Instance.TransitionIn(() => GameEvents.LOAD_LEVEL?.Invoke(LevelID));
    }
}
