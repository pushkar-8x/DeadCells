using System.Collections.Generic;
using _Scripts.Tiles;
using UnityEngine;

namespace Tarodev_Pathfinding._Scripts.Grid.Scriptables {
    [CreateAssetMenu(fileName = "New Scriptable Hex Grid")]
    public class ScriptableHexGrid : ScriptableGrid {

        [SerializeField,Range(1,50)] private int _gridWidth = 16;
        [SerializeField,Range(1,50)] private int _gridDepth = 9;
        //Dictionary<Vector2, NodeBase> cornerTiles = 
        
        public override Dictionary<Vector2, NodeBase> GenerateGrid() {
            var tiles = new Dictionary<Vector2, NodeBase>();
            var cornertiles = new Dictionary<Vector2, NodeBase>();
            var grid = new GameObject {
                name = "Grid"
            };
            for (var r = 0; r < _gridDepth ; r++) {
                var rOffset = r >> 1;
                for (var q = -rOffset; q < _gridWidth - rOffset; q++) {
                    var tile = Instantiate(nodeBasePrefab,grid.transform);
                    if (IsCornerTile(q, r, _gridWidth, _gridDepth)) {
                        tile.Init(false ,new HexCoords(q,r));
                        cornertiles.Add(tile.Coords.Pos, tile);
                    }
                    else
                    {
                        tile.Init(DecideIfObstacle(), new HexCoords(q,r));
                    }
                    
                    tiles.Add(tile.Coords.Pos,tile);
                }
                
                
            }

            return tiles;
        }
        
        private bool IsCornerTile(int q, int r, int gridWidth, int gridDepth) {
            // Define your corner logic here
            // Example: a tile is a corner if it is at the boundaries of the grid
            if (r == 0 || r == gridDepth - 1) {
                return true;
            }
    
            var rOffset = r >> 1;
            if (q == -rOffset || q == gridWidth - rOffset - 1) {
                return true;
            }

            return false;
        }
    }
}