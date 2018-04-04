using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour {

    public Controller controller;
    public Controller defaultController;
    protected Vector3 velocity;
    public new Camera camera;

    // Called on initialization
    protected virtual void Awake() {
        if (!controller) {
            SpawnDefaultController();
        }
        if (controller) {
            controller.Possess(this);
        }
    }

    // Called when a controller stops possessing a pawn
    protected virtual void OnUnPossess(Controller oldController) {
        if (camera) {
            camera.enabled = false;
        }
    }

    // Called when a controller starts possessing a pawn
    protected virtual void OnPossess(Controller newController) {
        if (newController.GetType() == typeof(PlayerController)) {
            if (camera) {
                camera.enabled = true;
            }
        }
    }

    // Used to set the controller through the pawn
    public void SetController(Controller newController) {
        OnUnPossess(controller);
        controller = newController;
        OnPossess(controller);
    }

    // Instantiates a prefab controller
    public void SpawnDefaultController() {
        if (defaultController) {
            Controller newController = Instantiate<Controller>(defaultController);
            newController.Possess(this);
        }
    }

    // Reposition pawn
    public virtual void Respawn (Vector3 position) {
        transform.position = position;
        velocity = Vector3.zero;
    }

    // Get the controller currently possessing this pawn
	public Controller PossessedBy() {
        return controller;
    }

    // Late update is called at the end of every frame
    protected virtual void LateUpdate() {
        // Applies the pawns velocity
        if (Time.time > .3f) {
            transform.Translate(velocity * Time.deltaTime);
        }
    }

    // Set velocity
    public void SetVelocity(Vector3 _velocity) {
        velocity = _velocity;
    }

    // Get velocity
    public Vector3 GetVelocity() {
        return velocity;
    }
}
