using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Managers;
using BlockyBlock.Configurations;

namespace BlockyBlock.Core 
{
    public class UnitVision : MonoBehaviour
    {
        public GameObject GetFontObject(int _unitX, int _unitY, int _floorIdx, DirectionData _directionData)
        {
            Ground[,] gridArray = GridManager.Instance.Grids[_floorIdx].GridArray;
            int x = _unitX + _directionData.XIdx;
            if (x > gridArray.GetLength(0) - 1)
            {
                return null;
            }
            int y = _unitY + _directionData.YIdx;
            if (y > gridArray.GetLength(1) - 1)
            {
                return null;
            }
            if (gridArray[x, y].Stuff != null)
            {
                return gridArray[x, y].Stuff.gameObject;
            }
            else
            {
                return null;
            }
        }
    }
}
