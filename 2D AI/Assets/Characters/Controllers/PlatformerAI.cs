using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerAI : AIController {
    public ProceduralTiles.Level level;
    public Agent agent;
    public Context context;

    // Use this for initialization
    protected void Start() {
        SideScrollerPawn SSPawn = (SideScrollerPawn)pawn;

        // Create new navigation system
        if (SSPawn) {
            navigation = new PlatformNavigator(SSPawn, level);
        }

        // Initialise agent
        if (agent) {
            agent.Initialise();
        }
    }

    // Update is called once per frame
    protected override void Update() {
        // Update agent
        if (agent && context) {
            agent.UpdateStates(context);
        }
    }
}
