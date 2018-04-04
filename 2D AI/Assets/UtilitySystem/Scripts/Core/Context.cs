using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context : MonoBehaviour {

    public TransformVariable[] transforms;
    public Transform[] transformReferences;

    // Check condition in this context
    public bool CheckCondition (Condition condition) {
        return condition.Check(this);
    }

    // Get the transform belonging to the reference
    public Transform GetTransform(TransformVariable variable) {
        for (int i = 0; i < transforms.Length; i++) {
            if (transforms[i] == variable) {
                return transformReferences[i];
            }
        }
        return null;
    }
}
