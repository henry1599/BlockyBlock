using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Managers;

namespace BlockyBlock.Core
{
    [System.Serializable]
    public class Grid
    {
        int m_Width, m_Height;
        Ground[,] m_GridArray;
        Vector3 m_GridStartPosition;
        List<GroundData> m_GroundDatas;
        readonly float m_GroundSize = 1;
        public int Width {get => m_Width;}
        public int Height {get => m_Height;}
        public List<GroundData> GroundDatas {get => m_GroundDatas;}
        public Ground[,] GridArray {get => m_GridArray; set => m_GridArray = value;}
        public Vector3 GridStartPosition {get => m_GridStartPosition;}
        public Grid(int _width, int _height, Vector3 _startPosition)
        {
            m_GroundDatas = new List<GroundData>();

            m_Width = _width;
            m_Height = _height;
            m_GridStartPosition = _startPosition;

            m_GridArray = new Ground[_width, _height];
        }
        public Vector3 GetWorldPosition(int _iX, int _iY, int _floor)
        {
            return (new Vector3(_iX * m_GroundSize, _floor * ConfigManager.Instance.LevelConfig.SpaceEachFloor, _iY * m_GroundSize)) + m_GridStartPosition;
        }
    }
}
