using System;
using UnityEngine;

namespace Match3Game {
    public class PrefabFactory : MonoBehaviour{
        [SerializeField]
        private GameObject _cellPrefab;
        [SerializeField]
        private GameObject _cellObjectPrefab;

        private void Awake() {
            if (_cellPrefab == null)
                throw new Exception("Не задан прeфаб ячейки");
            if (_cellObjectPrefab== null)
                throw new Exception("Не задан прeфаб шара");
        }

        public GameObject Cell {
            get { return _cellPrefab; }
        }
        public GameObject CellObject {
            get { return _cellObjectPrefab; }
        }
    }
}