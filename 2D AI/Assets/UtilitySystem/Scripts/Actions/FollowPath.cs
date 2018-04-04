using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFollowPathAction", menuName = "Utility system/Actions/Follow path")]
public class FollowPath : Action {

    public TransformVariable pawn;
    public TransformVariable aIController;

    // Called every frame the action is triggered
    public override void Execute(Context context) {
        SideScrollerPawn ssp = context.GetTransform(pawn).GetComponent<SideScrollerPawn>();
        AIController ai = (AIController)context.GetTransform(aIController).GetComponent<AIController>();
        NavigationSystem nav = ai.GetNavigationSystem();

        if (ssp != null && nav != null) {
            // Get and apply navigation direction
            Vector2 direction = nav.GetDirection();
            ssp.SetHorizontalInput(direction.x);

            // if the direction goes up, than the pawn should jump
            if (direction.y > 0) {
                ssp.Jump();
            }
        }
    }
}
