using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralTiles {
    public class SequenceTile : Tile {

        // Called on initialization
        public override void Instantiate() {

            // Loop through sprites based on the x position of the tile
            spriteRenderer = transform.GetComponent<SpriteRenderer>();
            if (sprites != null && sprites.Length > 0) {
                int sprite = Mathf.FloorToInt(transform.position.x) % sprites.Length;
                if(sprite < 0) {
                    sprite = 3 + sprite;
                }
                spriteRenderer.sprite = sprites[sprite];
            }
        }
    }
}
