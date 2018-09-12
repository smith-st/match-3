using UnityEngine;

namespace Match3Game.Interfaces {
    public interface ICell:IDestroyable {

        Cell SetId(int id);
        Cell SetObject(ICellObject obj);
        ICell GetNeighbor(Direction direction);
        void AddNeighborCell(ICell cell, Direction direction);
        Vector2 MyPosition { get; }
        ICellObject MyObject { get; }
        bool IsEmpty { get; }

    }
}