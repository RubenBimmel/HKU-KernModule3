using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralTiles {
    public enum EditMode {
        Add,
        Pick,
        Remove
    }

    public enum Layer {
        Background,
        Middleground,
        Foreground
    }

    [System.Serializable]
    public class Level : MonoBehaviour {

        [SerializeField]
        protected Grid[] layers;
        public int xSize = 2;
        public int ySize = 2;

        public Layer activeLayer;
        public Tile selectedTile;
        public EditMode mode;

        protected SpawnTile[] spawnTiles;

        // Called on initialization
        protected virtual void Awake() {
            // Create layers
            if (layers == null) {
                layers = new Grid[3];
            }
            for (int i = 0; i < 3; i++) {
                if (layers[i] == null) {
                    layers[i] = new GameObject().AddComponent<Grid>();
                    layers[i].Instantiate(xSize, ySize);
                    layers[i].name = ((Layer)i).ToString();
                    layers[i].transform.parent = transform;
                }
            }
            UpdateGridPositions();

            //Create a list of spawn tiles
            spawnTiles = CollectSpawnTiles();
        }

        // Set the position of the grids
        public void UpdateGridPositions() {
            for (int i = 0; i < 3; i++) {
                Vector3 position = transform.position;
                position += Vector3.left * (xSize / 2);
                position += Vector3.down * (ySize / 2);
                position += Vector3.forward * (1 - i);
                layers[i].transform.position = position;
            }
        }

        // Reset the size of the grid (x and y size should always be a multiple of 2)
        public void SetSize(int x, int y) {
            if (x % 2 == 1)  x--;
            if (x < 0)       x = 0;
            if (y % 2 == 1)  y--;
            if (y < 0)       y = 0;

            xSize = x;
            ySize = y;

            foreach (Grid grid in layers) {
                grid.SetSize(xSize, ySize);
            }
            UpdateGridPositions();
        }

        // Set a tile on a given position
        public void SetTile(Tile tile, int x, int y) {
            if (tile) {
                layers[(int)tile.layer].SetTile(tile, x, y);
            }
        }

        // Get a tile on a given position and layer
        public Tile GetTile(Layer layer, int x, int y) {
            return layers[(int)layer].GetTile(x,y);
        }

        // Remove a tile on a given position and layer
        public void RemoveTile(Layer layer, int x, int y) {
            layers[(int)layer].RemoveTile(x, y);
        }

        // Get the grid size
        public int[] GetSize () {
            return new int[] { xSize, ySize };
        }

        // Get the collision on a given position
        public CollisionType GetCollision (int x, int y) {
            if (layers[(int)Layer.Middleground].IsInRange(x, y)) {
                Tile tile = GetTile(Layer.Middleground, x, y);
                if (tile) {
                    return GetTile(Layer.Middleground, x, y).collision;
                }
                return CollisionType.None;
            }
            return CollisionType.None;
        }

        // Get the tile position of a world space position
        public int[] GetPositionInGrid (Vector3 position) {
            Vector2 relativePosition = position - transform.position;
            int xPos = Mathf.Clamp(Mathf.FloorToInt(relativePosition.x + .5f * xSize), 0, xSize);
            int yPos = Mathf.Clamp(Mathf.FloorToInt(relativePosition.y + .5f * ySize), 0, ySize);
            return new int[] { xPos, yPos };
        }

        // Get the world space position of a tile
        public Vector3 TransformCellPosition (int x, int y) {
            Vector3 position = new Vector3(x - .5f * xSize + .5f, y - .5f * ySize + .5f);
            return position + transform.position;
        }

        // Get all spawn tiles
        public SpawnTile[] GetSpawnTiles () {
            return spawnTiles;
        }

        // Create a list of all spawn tiles in this level
        public SpawnTile[] CollectSpawnTiles () {
            List<SpawnTile> tiles = new List<SpawnTile>();
            for (int i = 0; i < xSize; i++) {
                for (int j = 0; j < ySize; j++) {
                    Tile tile = GetTile(Layer.Middleground, i, j);
                    if (tile) {
                        SpawnTile spawnTile = tile.GetComponent<SpawnTile>();
                        if (spawnTile) {
                            tiles.Add(spawnTile);
                        }
                    }
                }
            }
            return tiles.ToArray();
        }
    }
}