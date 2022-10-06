using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;

namespace Helpers.BlockyBlock 
{
    public static class HelperBlockyBlock
    {
        public static GroundType StringToGroundType(string _string)
        {
            _string = _string.ToLower();
            switch (_string)
            {
                case "ground":
                    return GroundType.GROUND;
                case "water":
                    return GroundType.WATER;
                case "box":
                    return GroundType.BOX;
                case "hole":
                    return GroundType.SPACE;
                case "star":
                    return GroundType.COLLECTIBLE;
                case "trap":
                    return GroundType.TRAP;
                default:
                    return GroundType.GROUND;
            }
        }
        public static string TurnDirectionToString(TurnDirection _direction)
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
        public static TurnDirection StringToTurnDirection(string _string)
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
