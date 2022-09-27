using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Core;

namespace BlockyBlock.Managers
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance {get; private set;}
        public List<Core.Grid> Grids { get => m_Grids; set => m_Grids = value; } private List<Core.Grid> m_Grids;
        void Awake()
        {
            Instance = this;
            Grids = new List<Core.Grid>();
        }
        public void AddGrid(Core.Grid _grid)
        {
            Grids.Add(_grid);
        }
    }
}
