using UnityEngine;

namespace Match3Game.Builders {
    public class CellObjectBuilder {
        private readonly GameObject _prefab;
        
        public CellObjectBuilder(GameObject prefab) {
            _prefab = prefab;
        }

        public ICellObject Build() {
            return Build(Vector2.zero);
        }
        /// <summary>
        /// Создает игровой елемент на указаной позиции
        /// </summary>
        public ICellObject Build(Vector2 position) {
            var go = Object.Instantiate(_prefab, position, Quaternion.identity);
            var cellObj = go.GetComponent<ICellObject>();
            cellObj.SetColor();
            return cellObj;
        }
    }
}