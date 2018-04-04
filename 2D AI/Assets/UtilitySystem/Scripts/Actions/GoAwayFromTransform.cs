using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGoAwayFromTransformAction", menuName = "Utility system/Actions/Set target away from transform")]
public class GoAwayFromTransform : Action {

    public TransformVariable aIController;
    public TransformVariable enemy;
    public TransformVariable level;

    private float timer;

    // Called every frame the action is triggered
    public override void Execute(Context context) {
        SideScrollerPawn e = context.GetTransform(enemy).GetComponent<SideScrollerPawn>();
        ProceduralTiles.Level lvl = context.GetTransform(level).GetComponent<ProceduralTiles.Level>();

        AIController ai = (AIController)context.GetTransform(aIController).GetComponent<AIController>();
        NavigationSystem nav = ai.GetNavigationSystem();

        Vector3 pawnPosition = ai.GetPawn().transform.position;
        Vector3 targetPosition = pawnPosition;

        timer += Time.deltaTime;

        if (lvl) {
            // Get a list of all posible positions to go to
            ProceduralTiles.SpawnTile[] options = lvl.GetSpawnTiles();
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < options.Length; i++) {
                Vector3 pos = options[i].transform.position;
                for (int j = 0; j < positions.Count; j++) {
                    if (Vector3.Distance(pawnPosition, pos) < Vector3.Distance(pawnPosition, positions[j])) {
                        positions.Insert(j, pos);
                        break;
                    }
                }
                positions.Add(options[i].transform.position);
            }

            // Set the position the furthest away from the enemy as the new target
            for (int i = 0; i < 5; i++) {
                if (Vector3.Distance(e.transform.position, positions[i]) > Vector3.Distance(e.transform.position, targetPosition)) {
                    targetPosition = positions[i];
                }
            }
        }

        // Set the new target and start pathfinding
        if (nav != null && targetPosition != pawnPosition) {
            if (nav.GetTarget() != targetPosition || timer > 1f) {
                timer = 0;
                nav.SetTarget(targetPosition);

                PlatformNavigator platformerNav = (PlatformNavigator)nav;
                if (platformerNav != null && !platformerNav.aStarSearchIsRunning) {
                    ai.StartCoroutine(platformerNav.AStarSearch());
                }
            }
        }
    }
}
