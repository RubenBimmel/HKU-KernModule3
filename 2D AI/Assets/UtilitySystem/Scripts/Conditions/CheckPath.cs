using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCheckPathCondition", menuName = "Utility system/Condition/Chack path")]
public class CheckPath : Condition {
    public TransformVariable aIController;
    bool equalsValue;

    // Called every frame the condition needs to be checked
    public override bool Check(Context context) {
        AIController ai = (AIController)context.GetTransform(aIController).GetComponent<AIController>();
        PlatformNavigator nav = (PlatformNavigator)ai.GetNavigationSystem();
        if (nav != null) {
            return nav.pathIsActive;
        }
        return false;
    }
}
