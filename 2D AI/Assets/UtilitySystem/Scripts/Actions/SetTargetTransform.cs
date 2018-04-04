using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetAction", menuName = "Utility system/Actions/Set target (Transform)")]
public class SetTargetTransform : Action {
    public TransformVariable aIController;
    public TransformVariable target;

    private float timer;
    private float interval = .25f;

    // Called every frame the action is triggered
    public override void Execute(Context context) {
        timer -= Time.deltaTime;

        AIController ai = (AIController)context.GetTransform(aIController).GetComponent<AIController>();
        NavigationSystem nav = ai.GetNavigationSystem();
        Transform t = context.GetTransform(target);

        SideScrollerPawn ssp = (SideScrollerPawn)ai.GetPawn();

        if (ssp && ssp.IsGrounded()) {
            if (nav != null && t) {
                Vector3 position = t.position + Vector3.up * .5f;
                PlatformNavigator platformerNav = (PlatformNavigator)nav;

                // If the target is changed and the interval time has passed
                if (nav.GetTarget() != position && timer < 0) {
                    timer = interval;
                    nav.SetTarget(position);

                    if (platformerNav != null) {
                        ai.StopAllCoroutines();
                        ai.StartCoroutine(platformerNav.AStarSearch());
                    }
                }

                // If there is no current path
                if (platformerNav != null && !platformerNav.pathIsActive && timer < 0) {
                    timer = interval;
                    ai.StopAllCoroutines();
                    ai.StartCoroutine(platformerNav.AStarSearch());
                }
            }
        }
    }
}
