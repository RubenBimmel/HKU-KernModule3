using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTargetAction", menuName = "Utility system/Actions/Set target (Vector3)")]
public class SetTarget : Action {
    public TransformVariable aIController;
    public Vector3 position;
    private float interval = .5f;
    private float timer;

    // Called every frame the action is triggered
    public override void Execute(Context context) {
        timer += Time.deltaTime;

        AIController ai = (AIController)context.GetTransform(aIController).GetComponent<AIController>();
        NavigationSystem nav = ai.GetNavigationSystem();
        if (nav != null) {
            // Set the new target if it is changed or the interval time has passed
            if (nav.GetTarget() != position || timer > interval) {
                timer = 0;
                nav.SetTarget(position);              

                PlatformNavigator platformerNav = (PlatformNavigator)nav;
                if (platformerNav != null && !platformerNav.aStarSearchIsRunning) {
                    ai.StartCoroutine(platformerNav.AStarSearch());
                }
            }
        }
    }
}
