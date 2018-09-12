using System.Collections.Generic;
using Match3Game.Events;
using Match3Game.Interfaces;
using UnityEngine;

namespace Match3Game{
	public class BaseObject : MonoBehaviour {
		/// <summary>
		/// уничтожает объект
		/// </summary>
		public virtual void DestroyObject() {
			Destroy(gameObject);
		}
	}
}
