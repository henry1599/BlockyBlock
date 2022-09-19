using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;

namespace Helpers.BlockyBlock 
{
    public static class HelperBlockyBlock
    {
        public static string DirectionToString(TurnDirection _direction)
        {
            string result = "";
            switch (_direction)
            {
                case TurnDirection.LEFT:
                    result = "Left";
                    break;
                case TurnDirection.RIGHT:
                    result = "Right";
                    break;
            }
            return result;
        }
        public static TurnDirection StringToDirection(string _string)
        {
            TurnDirection result = TurnDirection.LEFT;
            _string = _string.ToLower();
            switch (_string)
            {
                case "left":
                    result = TurnDirection.LEFT;
                    break;
                case "right":
                    result = TurnDirection.RIGHT;
                    break;
            }
            return result;
        }
    }
}
