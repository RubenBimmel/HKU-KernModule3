using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAI : AIController {
    public Pawn target;

    // Called on initialization
    protected void Awake () {
        // Create linear navigation system
        navigation = new LinearNavigation(pawn);
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        // Keep navigation target updated with the other transform position
        navigation.SetTarget(target.transform.position);
    }
}
