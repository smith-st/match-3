using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Match3Game.Builders;
using Match3Game.Events;
using Match3Game.Interfaces;
using Smith.Extensions;
using UnityEngine;

namespace Match3Game.Controllers {
    public class GameController : MonoBehaviour {
        [SerializeField] private Transform _fieldContainer;
        private const float Speed = 0.3f;
        private PrefabFactory _pf;
        private UIController _ui;
        private List<ICell> _cells;
        private CellObjectBuilder _cellObjectBuilder;
        private bool _gameLock;
        private int _turns,_turnsTotal,_points, _pointsTotal, _fieldSize = 5;
        
        private void Awake() {
            
            _pf = FindObjectOfType<PrefabFactory>();
            if (_pf == null) 
                throw new Exception("Не найдена фабрика префабов");
            
            _ui = FindObjectOfType<UIController>();
            if (_ui == null) 
                throw new Exception("Не найден контроллер UI");

            GameEvent.OnSwipe += GameEventOnSwipe;
           
        }

        private void Start() {
            StartGame(_fieldSize);
        }

        public void StartGame() {
            StartGame(_fieldSize);
        }
        
        public void StartGame(float size) {
            StartGame((int)size);
        }
        /// <summary>
        /// запускает игру с указаной размерностью игрового поля
        /// </summary>
        /// <param name="fieldSize"></param>
        public void StartGame(int fieldSize) {
            _fieldSize = fieldSize;
            _ui.FieldSize(fieldSize);
            if (_cells != null && _cells.Count > 0) {
                foreach (var cell in _cells) 
                    cell.DestroyObject();
                _cells.Clear();
                _turns = 0;
                _points = 0;
                _ui.Reset();
                StopAllCoroutines();
                CancelInvoke();
            }

            _turnsTotal = fieldSize * fieldSize/2;
            _pointsTotal = fieldSize * fieldSize*2;
            _ui.PointsCount(_points,_pointsTotal);
            _ui.TurnsCount(_turns,_turnsTotal);
            _cells = CellsFieldBuilder.Build(fieldSize, _pf.Cell, _fieldContainer);
            if (fieldSize > 5)
                Camera.main.orthographicSize = fieldSize;
           
            _cellObjectBuilder = new CellObjectBuilder(_pf.CellObject);
            foreach (var cell in _cells) {
                var cellObj = _cellObjectBuilder.Build(cell.MyPosition);
                cell.SetObject(cellObj);
            }
            _gameLock = false;
            Invoke("CheckLinesForInvoke",Speed);
        }
        /// <summary>
        /// игра выиграна
        /// </summary>
        private void WinGame() {
            _gameLock = true;
            _ui.ShowWin();
        }
        /// <summary>
        /// игра проиграна
        /// </summary>
        private void LoseGame() {
            _gameLock = true;
            _ui.ShowLose();
        }
        /// <summary>
        /// игрок свайпнул по ячекам
        /// </summary>
        private void GameEventOnSwipe(ICell from, ICell to) {
            if (_gameLock || from.IsEmpty || to.IsEmpty) return;
            StartCoroutine(UserTurn(from,to));
        }
        
        /// <summary>
        /// ход игрока
        /// </summary>
        private IEnumerator UserTurn(ICell from, ICell to) {
            _gameLock = true;
            SwapObjectOnCells(from,to);
            yield return new WaitForSeconds(Speed);
            if (CheckLines()) {
                //есть линия
                _ui.TurnsCount(++_turns,_turnsTotal);
            }else {
                //возвращаем обратно
                _gameLock = true;
                SwapObjectOnCells(from,to);
                yield return new WaitForSeconds(Speed);
                _gameLock = false;
            }
        }
        /// <summary>
        /// обмен объектов между двумя ячейками
        /// </summary>
        private void SwapObjectOnCells(ICell from, ICell to) {
            from.MyObject.Transform.DOMove(to.MyPosition, Speed).SetEase(Ease.Linear);
            to.MyObject.Transform.DOMove(from.MyPosition, Speed).SetEase(Ease.Linear);
            var fromObj = from.MyObject;
            from.SetObject(to.MyObject);
            to.SetObject(fromObj);
        }
        /// <summary>
        /// уничтожение объектов на ячейке
        /// </summary>
        private void DestroyObjectOnCell(ICell cell) {
            if (cell.MyObject == null) return;
            cell.MyObject.DestroyObject();
            cell.SetObject(null);
            _points++;
        }
        
        #region LineOperations

        private void CheckLinesForInvoke() {
            CheckLines();
        }
        /// <summary>
        /// проверяет есть ли линии из 3 и больше объектов одинкового цвета
        /// </summary>
        /// <returns></returns>
        private bool CheckLines() {
            var lines  = new List<List<ICell>>();
              
            //цикл по всем  ячейкам
            foreach (var cell in _cells) {
                FindCombo(cell, Direction.Left, Direction.Right, ref lines);
                FindCombo(cell, Direction.Bottom, Direction.Top, ref lines);
            }

            if (DestroyCombo(ref lines)) {
                Invoke("FillEmpty", Speed);
            }else {
                if (!TurnAvaliable()) {
                    Shuffle();
                    Invoke("CheckLinesForInvoke",Speed);
                }else {
                    
                    if (_points >= _pointsTotal) {
                        WinGame();
                    }else if(_turns>=_turnsTotal) {
                        LoseGame();
                    }else {
                        _gameLock = false;   
                    }
                }
                
            }

            _ui.PointsCount(_points,_pointsTotal);
            return lines.Count > 0;
        }
        /// <summary>
        /// находит комбинации объектов
        /// </summary>
        
        private static void FindCombo(ICell cell,Direction directionFrom, Direction directionTo, ref List<List<ICell>> lines) {
            if (cell.GetNeighbor(directionFrom) != null) return;
            var line = new List<ICell>();
            
            line.Clear();
            line.Add(cell);
            var obj = line[0].GetNeighbor(directionTo);
            do {
                if (obj == null || obj.MyObject == null) {
                    line.Clear();
                    return;
                }
                if (obj.MyObject.MyColor == line[0].MyObject.MyColor) {
                    //цвет первого объекта совпал с текущим
                    line.Add(obj);
                }else {
                    //цвет не совпал
                    if (line.Count >= 3) 
                        lines.Add(new List<ICell>(line));
                    line.Clear();
                    line.Add(obj);
                }
                obj = obj.GetNeighbor(directionTo);
            } while (obj!= null);
            if (line.Count >= 3) 
                lines.Add(new List<ICell>(line));
        }
        /// <summary>
        /// уничтожает комбинацию
        /// </summary>
        private bool DestroyCombo(ref List<List<ICell>> lines) {
            var del = false;
            var toDel = new List<ICell>();
            foreach (var line in lines) {
                //bonus 5
                if (line.Count == 4) {
                    //узнаем направление линии (вертикальная или горизонтальная)
                    if (
                        line[0].GetNeighbor(Direction.Top) == line[1]
                        || line[0].GetNeighbor(Direction.Bottom) == line[1]
                    ) {
                        //вертикальная
                        GetNeighborToEnd(line[0], Direction.Top,ref toDel);
                        GetNeighborToEnd(line[0].GetNeighbor(Direction.Bottom), Direction.Bottom,ref toDel);
                    }else {
                        //горизонтальная
                        GetNeighborToEnd(line[0], Direction.Right,ref toDel);
                        GetNeighborToEnd(line[0].GetNeighbor(Direction.Left), Direction.Left,ref toDel);
                    }
                }else if (line.Count >= 5) {
                    var result = _cells.Where(cell => cell.MyObject.MyColor == line[0].MyObject.MyColor).ToList();
                    foreach (var cell in result) {
                        if (!toDel.Contains(cell))
                            toDel.Add(cell);
                    }
                }
                else {
                    foreach (var cell in line) {
                        if (!toDel.Contains(cell))
                            toDel.Add(cell);
                    }
                }
            }

            if (toDel.Count > 0)
                del = true;
            foreach (var cell in toDel) 
                DestroyObjectOnCell(cell);
            
            return del;
        }
        /// <summary>
        /// собирает все ячейки до конца игрового поля в указаном нраправлении 
        /// </summary>
        
        private void GetNeighborToEnd(ICell cell, Direction direction, ref List<ICell> list) {
            if (cell == null) return;
            if (!list.Contains(cell))
                list.Add(cell);
            GetNeighborToEnd(cell.GetNeighbor(direction),direction,ref list);
        }
        /// <summary>
        /// заполняет пустые ячейки
        /// </summary>
        private void FillEmpty() {
            foreach (var cell in _cells) {
                var topCell = cell.GetNeighbor(Direction.Top);
                if (cell.IsEmpty && topCell == null) {
                    var cellObj = _cellObjectBuilder.Build(new Vector2(cell.MyPosition.x,cell.MyPosition.y + 1f));
                    cellObj.Transform.DOMove(cell.MyPosition,Speed);
                    cell.SetObject(cellObj);
                }else if (cell.IsEmpty && !topCell.IsEmpty) {
                    topCell.MyObject.Transform.DOMove(cell.MyPosition, Speed).SetEase(Ease.Linear);
                    cell.SetObject(topCell.MyObject);
                    topCell.SetObject(null);
                }
            }

            if (_cells.Count(cell => cell.IsEmpty) > 0) {
                Invoke("FillEmpty",Speed);
            }else {
                Invoke("CheckLinesForInvoke",Speed);
            }
        }

        /// <summary>
        /// проверка на наличие доступного хода
        /// </summary>
        /// <returns></returns>
        private bool TurnAvaliable() {
            foreach (var cell in _cells) {
                foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
                    //комбинация 2+1
                    var neighbor = cell.GetNeighbor(direction);
                    if (neighbor == null) continue;
                    if (cell.MyObject.MyColor == neighbor.MyObject.MyColor) {
                        var neighborForNeighbor = neighbor.GetNeighbor(direction);
                        if (neighborForNeighbor != null) {
                            foreach (Direction direction2 in Enum.GetValues(typeof(Direction))) {
                                if (
                                    direction2 != Tools.OppositeNeighbor(direction)
                                    && neighborForNeighbor.GetNeighbor(direction2) != null
                                    && neighborForNeighbor.GetNeighbor(direction2).MyObject.MyColor ==
                                    cell.MyObject.MyColor)
                                    return true;
                            }
                        }
                    }
                    //комбинация 1+1+1
                    var neighbor2 = cell.GetNeighbor(Tools.OppositeNeighbor(direction));
                    if (neighbor2 == null) continue;
                    neighbor = neighbor.GetNeighbor(Tools.NeighborAngle90Right(direction));
                    neighbor2 = neighbor2.GetNeighbor(Tools.NeighborAngle90Right(direction));
                    if (neighbor == null || neighbor2 == null ) continue;
                    if (
                        cell.MyObject.MyColor == neighbor.MyObject.MyColor
                        && cell.MyObject.MyColor == neighbor2.MyObject.MyColor)
                        return true;
                }
            }
            
            return false;
        }
        /// <summary>
        /// перемешывает объекты на игровом поле
        /// </summary>
        private void Shuffle() {
            var rnd = new List<int>(_cells.Count);
            for (var i = 0; i < _cells.Count; i++) 
                rnd.Add(i);
            rnd.Shuffle();
            for (var i = 0; i < rnd.Count/2; i++) 
                SwapObjectOnCells(_cells[rnd[i]],_cells[rnd[i+(rnd.Count/2)-1]]);
        }

        #endregion
       
    }
}