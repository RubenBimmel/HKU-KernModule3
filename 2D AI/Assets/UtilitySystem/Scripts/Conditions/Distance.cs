using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDistanceCondition", menuName = "Utility system/Condition/Distance")]
public class Distance : CompareFloats {
    public TransformVariable transform1;
    public TransformVariable transform2;
    public float value;

    // Called every frame the condition needs to be checked
    public override bool Check(Context context) {
        Transform object1 = context.GetTransform(transform1);
        Transform object2 = context.GetTransform(transform2);

        if (object1 && object2) {
            float distance = Vector3.Distance(object1.position, object2.position);

            return Compare(distance, value);
        }

        return false;
    }
}
