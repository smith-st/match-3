using System;
using UnityEngine;
using UnityEngine.UI;

namespace Match3Game.Controllers {
    public class UIController:MonoBehaviour {
        [SerializeField]
        private Text _turns;
        [SerializeField]
        private Text _points;
        [SerializeField]
        private Text _fieldSize;
        [SerializeField]
        private GameObject _plashWin;
        [SerializeField]
        private GameObject _plashLose;

        private void Awake() {
            Reset();
        }
        /// <summary>
        /// количество ходов
        /// </summary>
        public void TurnsCount(int count,int total) {
            _turns.text = string.Format("Ходов: {0}/{1}",count.ToString(),total.ToString());
        }
        /// <summary>
        /// количество очков
        /// </summary>
        public void PointsCount(int count,int total) {
            _points.text = string.Format("Очков: {0}/{1}",count.ToString(),total.ToString());
        }
        /// <summary>
        /// размер поля
        /// </summary>
        public void FieldSize(int size) {
            _fieldSize.text = string.Format("Размер: {0}x{1}",size.ToString(),size.ToString());
        }
        /// <summary>
        /// сброс на нулевые значения
        /// </summary>
        public void Reset() {
            TurnsCount(0,0);
            PointsCount(0,0);
            _plashWin.SetActive(false);
            _plashLose.SetActive(false);
        }
        /// <summary>
        /// показывает окно выиграша
        /// </summary>
        public void ShowWin() {
            _plashWin.SetActive(true);
        }
        /// <summary>
        /// показывает окно проиграша
        /// </summary>
        public void ShowLose() {
            _plashLose.SetActive(true);
        }

    }
}