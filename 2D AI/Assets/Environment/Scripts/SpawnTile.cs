using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralTiles {
    public class SpawnTile : Tile {

        private static List<SpawnTile> spawnTiles; // Static list with all spawn points

        // Called on initialization
        void Awake() {
            // Sprites are not visible during the game
            GetComponent<SpriteRenderer>().enabled = false;

            // Initialize spawn tiles list
            spawnTiles = new List<SpawnTile>();
        }

        // Use this for initialization
        void Start () {
            spawnTiles.Add(this);
        }

        // Static function to respawn a pawn at a random tile
        public static void RespawnAtRandomTile(Pawn pawn) {
            int s = Random.Range(0, spawnTiles.Count);
            spawnTiles[s].Respawn(pawn);
        }

        // Static function to check if spawn points are available
        public static bool RespawnAvailable () {
            return spawnTiles.Count > 0;
        }

        // Funtction to respawn a pawn at this spawn point
        public void Respawn (Pawn pawn) {
            Vector3 pos = transform.position;
            pos.z = pawn.transform.position.z;
            pawn.Respawn(pos);
        }
    }
}
