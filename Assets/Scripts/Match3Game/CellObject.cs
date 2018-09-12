using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Match3Game {
    [RequireComponent(typeof(SpriteRenderer))]
    public class CellObject:BaseObject,ICellObject {
       
        public Transform Transform {
            get { return gameObject.transform; }
        }
        
        
        public CellObjectColor MyColor { get; private set; }
        private Dictionary<CellObjectColor, string> _cellObjectColors;
        
        private SpriteRenderer _spriteRenderer;

        private void Awake() {
            _cellObjectColors = new Dictionary<CellObjectColor, string> {
                {CellObjectColor.Blue,    "#0025FF"},
                {CellObjectColor.Green,   "#00FF08"},
                {CellObjectColor.Purple,  "#C800FF"},
                {CellObjectColor.Red,     "#FF0005"},
                {CellObjectColor.Yellow,  "#FFFF00"}
            };
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetColor() {
            var rnd = Random.Range(0, (int) CellObjectColor.Last);
            SetColor((CellObjectColor)rnd);
        }

        
        public void SetColor(CellObjectColor color) {
            MyColor = color;
            var colorHex = _cellObjectColors.Single(item => item.Key.Equals(color)).Value;
            Color newColor;
            ColorUtility.TryParseHtmlString(colorHex, out newColor);
            _spriteRenderer.color = newColor;
        }

    }
}