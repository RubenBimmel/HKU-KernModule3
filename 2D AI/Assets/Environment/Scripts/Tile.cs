using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralTiles {

    public enum CollisionType {
        None,
        Block
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour {

        public Layer layer = Layer.Middleground;
        public CollisionType collision;
        public Sprite[] sprites;
        protected SpriteRenderer spriteRenderer;

        // Called to safely destroy this tile
        public void Remove () {
            if (Application.isEditor)
                DestroyImmediate(gameObject);
            else
                Destroy(gameObject);
        }

        // Called on initialisation
        public virtual void Instantiate() {
            // Pick a random sprite from the list
            spriteRenderer = transform.GetComponent<SpriteRenderer>();
            if (sprites != null && sprites.Length > 0) {
                int sprite = Random.Range(0, sprites.Length);
                spriteRenderer.sprite = sprites[sprite];
            }

            // Add a collider if collision is active
            if (collision != CollisionType.None) {
                gameObject.AddComponent<BoxCollider2D>().size = new Vector2(2, 2);
            }
        }
    }
}