using System;
using Match3Game.Interfaces;

namespace Match3Game.Events{
	public static class GameEvent{

		public static event Action<ICell,ICell> 		OnSwipe;
		
		public static void Swipe (ICell from, ICell to) {
			if (OnSwipe != null)
				OnSwipe (from,to);
		}
	}
}