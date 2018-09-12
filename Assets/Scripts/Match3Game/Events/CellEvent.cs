using System;
using Match3Game.Interfaces;

namespace Match3Game.Events{
	public static class CellEvent {

		public static event Action<ISwipable> 		OnDown;
		public static event Action<ISwipable> 		OnUp;
		public static event Action<ISwipable> 		OnEnter;
		
		public static void Down (ISwipable cell) {
			if (OnDown != null)
				OnDown (cell);
		}
		
		public static void Up (ISwipable cell) {
			if (OnUp != null)
				OnUp (cell);
		}
		
		public static void Enter (ISwipable cell) {
			if (OnEnter != null)
				OnEnter (cell);
		}

		
	}
}