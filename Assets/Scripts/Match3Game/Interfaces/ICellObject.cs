using Match3Game.Interfaces;
using UnityEngine;

namespace Match3Game {
    public interface ICellObject:IDestroyable {
        void SetColor();
        void SetColor(CellObjectColor color);
        CellObjectColor MyColor { get; }
        Transform Transform{ get; }
        

    }
}