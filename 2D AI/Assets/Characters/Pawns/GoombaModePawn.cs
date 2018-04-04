using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaModePawn : SideScrollerPawn {

    public bool alive = true;
    protected float downTime = 3f;
    protected int health = 100;
    protected GameObject sprites;

    // Called on initialization
    protected override void Awake() {
        base.Awake();
        sprites = transform.Find("Sprites").gameObject;
    }

    // Used to kill this pawn
    public void Kill() {
        Debug.Log(name + " is killed!");
        alive = false;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        if (sprites) {
            sprites.SetActive(false);
        }
        StartCoroutine("Revive");
    }

    // Respawns this pawn at a random position after the down time
    private IEnumerator Revive() {
        yield return new WaitForSeconds(downTime);
        while (true) {
            if (ProceduralTiles.SpawnTile.RespawnAvailable()) {
                ProceduralTiles.SpawnTile.RespawnAtRandomTile(this);
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Respawns the pawn at a given position
    public override void Respawn(Vector3 position) {
        alive = true;
        health = 100;
        if (sprites) {
            sprites.SetActive(true);
        }
        Invoke("ReactivateCollider", 1f);
        base.Respawn(position);
    }

    // Reactivates the collider
    protected void ReactivateCollider() {
        transform.GetComponent<BoxCollider2D>().enabled = true;
    }

    // Late update is called at the end of every frame
    protected override void LateUpdate() {
        // If the player is alive, update the pawns velocity
        if (alive) {
            base.LateUpdate();
        }
    }

    // Called when the pawn hits another pawn
    public override void HitOther(SideScrollerPawn other) {
        base.HitOther(other);
        // Health drops to prevent players from constantly hugging eachother
        health--;
        if (health < 0) {
            Kill();
        }
    }

    // Called when the pawn jumps on top of the other pawn
    protected override void JumpOnOther(SideScrollerPawn other) {
        // Kill the other pawn
        GoombaModePawn gmp = (GoombaModePawn)other;
        if (other) {
            gmp.Kill();
            health = 100;
        }
        base.JumpOnOther(other);
    }
}
