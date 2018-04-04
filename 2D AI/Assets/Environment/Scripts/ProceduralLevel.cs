using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralTiles {
    public class ProceduralLevel : Level {

        public Level[] segmentPool;
        public int segmentCount;
        public Level sides;
        public bool allowMirrored;
        private Level[] segments;
        
        // Called on initialization
        protected override void Awake() {
            // Fill list of random segments
            segments = new Level[segmentCount];
            for (int i = 0; i < segmentCount; i++) {
                segments[i] = segmentPool[Random.Range(0, segmentPool.Length)];
            }
            bool[] mirrored = new bool[segmentCount];
            if (allowMirrored) {
                for (int i = 0; i < segmentCount; i++) {
                    mirrored[i] = Random.Range(0, 2) == 1;
                }
            }

            base.Awake();

            // Set xSize and ySize
            int width = 0;
            int height = 0;
            foreach (Level segment in segments) {
                width += segment.xSize;
                height = Mathf.Max(height, segment.ySize);
            }
            if (sides) {
                width += sides.xSize * 2;
                height = Mathf.Max(height, sides.ySize);
            }
            SetSize(width, height);

            // Copy all tiles
            int dX = 0;

            //Left Side
            if (sides) {
                for (int i = 0; i < sides.xSize; i++) {
                    for (int j = 0; j < sides.ySize; j++) {
                        for (int layer = 0; layer < 3; layer++) {
                            Tile tile = sides.GetTile((Layer)layer, i, j);
                            SetTile(tile, i, j);
                        }
                    }
                }
                dX = sides.xSize;
            }

            //Segments
            for (int s = 0; s < segmentCount; s++) {
                for (int i = 0; i < segments[s].xSize; i++) {
                    for (int j = 0; j < segments[s].ySize; j++) {
                        for (int layer = 0; layer < 3; layer++) {
                            int x = mirrored[s] ? segments[s].xSize - i - 1 : i;
                            Tile tile = segments[s].GetTile((Layer)layer, x, j);
                            SetTile(tile, i + dX, j);
                        }
                    }
                }
                dX += segments[s].xSize;
            }

            //RightSide
            if (sides) {
                for (int i = 0; i < sides.xSize; i++) {
                    for (int j = 0; j < sides.ySize; j++) {
                        for (int layer = 0; layer < 3; layer++) {
                            Tile tile = sides.GetTile((Layer)layer, i, j);
                            SetTile(tile, i + dX, j);
                        }
                    }
                }
            }

            spawnTiles = CollectSpawnTiles();
        }
    }
}
