using Match3Game.Events;
using Match3Game.Interfaces;
using UnityEngine;


namespace Match3Game.Controllers {
	public class SwipeController : MonoBehaviour {
		private ISwipable _from, _to;
		
		private void Start () {
			CellEvent.OnDown += CellEventOnDown;
			CellEvent.OnEnter += CellEventOnEnter;
			CellEvent.OnUp += CellEventOnOnUp;
		}

		private void CellEventOnOnUp(ISwipable obj) {
			_from = null;
		}

		private void CellEventOnDown(ISwipable obj) {
			_from = obj;
		}
		
		private void CellEventOnEnter(ISwipable obj) {
			if (_from != null) {
				_to = obj;
				if (_from.MyId != _to.MyId) {
//					Debug.Log(string.Format("Swipe from {0} to {1}",_from.MyId.ToString(),_to.MyId.ToString()));
					GameEvent.Swipe(_from as ICell, _to as ICell);
					_from = null;
					_to = null;	
				}else {
					_to = null;
				}
			}
		}
	}
}
