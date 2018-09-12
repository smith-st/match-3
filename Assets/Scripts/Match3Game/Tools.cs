namespace Match3Game {
    public static class Tools {
        /// <summary>
        /// противоположное направление
        /// </summary>
        public static Direction OppositeNeighbor(Direction neighbor ) {
            switch (neighbor) {
                    case Direction.Bottom:
                        return Direction.Top;
                    case Direction.Top:
                        return Direction.Bottom;
                    case Direction.Right:
                        return Direction.Left;
                    case Direction.Left:
                        return Direction.Right;
                    default:
                        return Direction.Top;
            }
        }
        /// <summary>
        /// направление вправо от указаного  
        /// </summary>
        
        public static Direction NeighborAngle90Right(Direction neighbor ) {
            switch (neighbor) {
                    case Direction.Bottom:
                        return Direction.Left;
                    case Direction.Top:
                        return Direction.Right;
                    case Direction.Right:
                        return Direction.Bottom;
                    case Direction.Left:
                        return Direction.Top;
                    default:
                        return Direction.Top;
            }
        }
    }
}