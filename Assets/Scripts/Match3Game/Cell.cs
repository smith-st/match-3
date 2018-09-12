using System.Collections.Generic;
using Match3Game.Events;
using Match3Game.Interfaces;
using UnityEngine;

namespace Match3Game{
	public class Cell : BaseObject, ISwipable, ICell {
		/// <summary>
		///ID ячейки
		/// </summary>
		public int MyId { get; private set; }
		/// <summary>
		/// позиция ячейки
		/// </summary>
		public Vector2 MyPosition {
			get {
				return gameObject.transform.position; 
			}
		}
		/// <summary>
		/// объект, который находится на ячейке
		/// </summary>
		public ICellObject MyObject {
			get {
				return _cellObject;
			}
		}
		/// <summary>
		/// ячейка пустая?
		/// </summary>
		public bool IsEmpty {
			get { return _cellObject == null; }
		}
		
		private readonly Dictionary<Direction,ICell> _neighborCells = new Dictionary<Direction, ICell>(4);
		private ICellObject _cellObject;
		/// <summary>
		/// задает новый объект для ячейки
		/// </summary>
		public Cell SetObject(ICellObject obj) {
			_cellObject = obj;
			return this;
		}
		/// <summary>
		/// задает id для ячейки
		/// </summary>
		public Cell SetId(int id) {
			MyId = id;
			return this;
		}
		/// <summary>
		/// возвращает соседа в указаном  направлении
		/// </summary>
		public ICell GetNeighbor(Direction direction) {
			return _neighborCells.ContainsKey(direction) ? _neighborCells[direction] : null;
		}
		/// <summary>
		/// задает соседа в указаном направлении
		/// </summary>
		public void AddNeighborCell(ICell cell, Direction direction) {
			_neighborCells.Add(direction,cell);
		}
		/// <summary>
		/// при клике мышкой
		/// </summary>
		private void OnMouseDown() {
			CellEvent.Down(this);
		}
		/// <summary>
		/// при наведении мышкой
		/// </summary>
		private void OnMouseEnter() {
			if (Input.GetMouseButton (0)) {
				CellEvent.Enter(this);
			}
		}
		/// <summary>
		/// при отпускании кнопки мышки
		/// </summary>
		private void OnMouseUp() {
			CellEvent.Up(this);
		}

		
		public override void DestroyObject() {
			if (_cellObject != null)
				_cellObject.DestroyObject();
			base.DestroyObject();
		}
	}
}
