using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Condition : ScriptableObject {
    // Called every frame the condition needs to be checked
    public abstract bool Check(Context context);
}
