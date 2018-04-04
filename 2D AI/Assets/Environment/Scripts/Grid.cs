using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralTiles {
    [System.Serializable]
    public class Grid : MonoBehaviour {
        
        [SerializeField]
        private Tile[] tiles;
        [SerializeField]
        private int xSize;
        [SerializeField]
        private int ySize;

        // Called on initialization
        public void Instantiate (int _xSize, int _ySize) {
            xSize = _xSize;
            ySize = _ySize;
            if (tiles == null) {
                tiles = new Tile[xSize * ySize];
            }
        }

        // Draw gizmos for a visual representation of the layer
        private void OnDrawGizmos () {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * ySize);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * xSize);
            Gizmos.DrawLine(transform.position + Vector3.up * ySize, transform.position + Vector3.up * ySize + Vector3.right * xSize);
            Gizmos.DrawLine(transform.position + Vector3.right * xSize, transform.position + Vector3.up * ySize + Vector3.right * xSize);
        }

        // Get the tile at a given position
        public Tile GetTile (int x, int y) {
            if (IsInRange(x, y)) {
                return tiles[x + y * xSize];
            }
            return null;
        }

        // Resize the level
        public void SetSize (int x, int y) {
            // Create a new grid and copy all of the tiles from the current grid
            Tile[] newTiles = new Tile[x * y];
            for (int i = 0; i < Mathf.Min(xSize, x); i++) {
                for (int j = 0; j < Mathf.Min(ySize, y); j++) {
                    if (GetTile(i,j) != null) {
                        newTiles[i + j * x] = GetTile(i, j);
                    }
                }
            }

            // Remove all of the tiles that are outside of the new grid
            for (int i = 0; i < xSize; i++) {
                for (int j = y; j < ySize; j++) {
                    RemoveTile(i, j);
                }
            }
            for (int i = x; i < xSize; i++) {
                for (int j = 0; j < ySize; j++) {
                    RemoveTile(i, j);
                }
            }

            // Set the new size and tiles list
            xSize = x;
            ySize = y;
            tiles = newTiles;
        }

        // Set a tile at a given position
        public void SetTile (Tile tile, int x, int y) {
            if (tile) {
                if (IsInRange(x, y)) {
                    RemoveTile(x, y);
                    if (!Application.isPlaying) {
                        tiles[x + y * xSize] = (Tile)UnityEditor.PrefabUtility.InstantiatePrefab(tile);
                    }
                    else {
                        tiles[x + y * xSize] = Instantiate<Tile>(tile);
                    }
                    tiles[x + y * xSize].transform.parent = transform;
                    tiles[x + y * xSize].transform.localPosition = new Vector3(x + .5f, y + .5f, 0);
                    tiles[x + y * xSize].Instantiate();                    
                }
            }
        }

        // Removes a tile at a given position
        public void RemoveTile (int x, int y) {
            if (IsInRange(x, y)) {
                if (GetTile(x, y)) {
                    GetTile(x, y).Remove();
                }
            }
        }

        // Check if a given position is within the grids range
        public bool IsInRange (int x, int y) {
            return x >= 0 && x < xSize && y >= 0 && y < ySize;
        }
    }
}
