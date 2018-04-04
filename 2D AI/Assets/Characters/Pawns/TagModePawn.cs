using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagModePawn : SideScrollerPawn {

    public bool tagged;
    private bool tagIsActive;
    public float coolDownTime;
    public GameObject tagIndicator;

    // Called on initialization
    protected override void Awake() {
        tagIndicator.SetActive(tagged);
        tagIsActive = tagged;
        base.Awake();
    }

    // Tag this pawn
    public void Tag () {
        Invoke("ActivateTag", coolDownTime);
        tagged = true;
        tagIndicator.SetActive(true);
    }

    // Activate tag so that pawn can tag other players
    protected void ActivateTag() {
        tagIsActive = true;
    }

    // Gets called when pawn hits another pawn
    public override void HitOther(SideScrollerPawn other) {
        // Tag other player if tag is active
        if (tagIsActive) {
            TagModePawn tmp = other.GetComponent<TagModePawn>();
            if (tmp) {
                tmp.Tag();
                tagged = false;
                tagIsActive = false;
                tagIndicator.SetActive(false);
            }
        }
        base.HitOther(other);
    }
}
