using System;
using System.Collections.Generic;
using Match3Game.Interfaces;
using UnityEngine;

namespace Match3Game.Builders {
    public static class CellsFieldBuilder  {
        private const int CellSize = 2;
        /// <summary>
        /// создает игровое поле из ячеек указаной размерностью 
        /// </summary>
        /// <param name="size">размер игрового поля</param>
        /// <param name="prefab">префаб ячейки</param>
        /// <param name="parent">родитель для ячеек</param>
        /// <returns>список всех созданных ячеек</returns>
        public static List<ICell> Build(int size, GameObject prefab, Transform parent) {
            if (size < 3)
                throw new Exception("Размер поля не может быть меньше 3x3");
            if (size > 15) 
                throw new Exception("Размер поля не может быть ,больше 15x15");
            if (prefab == null) 
                throw new Exception("Не задан префаб ячейки");
            
            var from = -(size * CellSize) / 2 + CellSize / 2;
            var cells = new List<ICell>(size*size);
            var id = 0;
            var i = -1;
            do {
                i++;
                var j = -1;
                do {
                    id++;
                    j++;
                    var pos = new Vector2(from + CellSize*j,from + CellSize*i);
                    var go = UnityEngine.Object.Instantiate(prefab,new Vector2(pos.x + parent.position.x,pos.y + parent.position.y),Quaternion.identity, parent);
                    cells.Add(go.GetComponent<ICell>().SetId(id));
                } while (j < size-1);
            } while (i < size-1);
            
            //задаем соседние ячейки
            for (i = 0; i < cells.Count; i++) {
                //справа
                var direction = i + 1;
                if (direction < cells.Count && (i+1)%size != 0) 
                    cells[i].AddNeighborCell(cells[direction],Direction.Right);
                
                //слева
                direction = i - 1;
                if (direction >= 0 && i%size != 0) 
                    cells[i].AddNeighborCell(cells[direction],Direction.Left);
                
                //вверху
                direction = i + size;
                if (direction < cells.Count) 
                    cells[i].AddNeighborCell(cells[direction],Direction.Top);

                //внизу
                direction = i - size;
                if (direction >= 0) 
                    cells[i].AddNeighborCell(cells[direction],Direction.Bottom);
            }
            return cells;
        }
    }
}