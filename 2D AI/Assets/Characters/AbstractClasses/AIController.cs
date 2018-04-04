using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : Controller {

    protected NavigationSystem navigation;

    // Update is called every frame
    protected virtual void Update () {
        // Apply the navigation
        if (navigation != null) {
            pawn.SetVelocity(navigation.GetDirection());
        }
    }

    // Get connected navigation system instace
    public NavigationSystem GetNavigationSystem () {
        return navigation;
    }
}
