using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivBuildApp
{
    internal class GridEventArgs : EventArgs
    {
        public GearGridContent Grid { get; }
        public ItemType ItemType { get; }
        public int Index { get; }
        public GridEventArgs(GearGridContent grid, ItemType itemType, int index)
        {
            Grid = grid;
            ItemType = itemType;
            Index = index;
        }
        public GridEventArgs(GearGridContent grid, int index)
        {
            Grid = grid;
            ItemType = Lib.GetItemTypeFromElement(grid.Box);
            Index = index;
        }
    }
}
