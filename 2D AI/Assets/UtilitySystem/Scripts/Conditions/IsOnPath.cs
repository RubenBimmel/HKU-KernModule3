using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIsOnPathCondition", menuName = "Utility system/Condition/IsOnPath")]
public class IsOnPath : Condition {
    public TransformVariable aIController;
    bool equalsValue;

    // Called every frame the condition needs to be checked
    public override bool Check(Context context) {
        AIController ai = (AIController)context.GetTransform(aIController).GetComponent<AIController>();
        PlatformNavigator nav = (PlatformNavigator) ai.GetNavigationSystem();
        if (nav != null) {
            return nav.IsOnPath();
        }
        return false;
    }
}
