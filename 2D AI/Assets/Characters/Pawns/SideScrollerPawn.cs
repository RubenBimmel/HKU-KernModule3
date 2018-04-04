using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollerPawn : Pawn {

    public float speed;
    public float acceleration;
    public float jumpForce;
    public float gravityUp;
    public float gravityDown;
    public float maxFallSpeed;

    public Rect box;                //Box for collision detection
    public int horizontalRayCount;  //Amount of rays casted from the sides
    public int verticalRayCount;    //Amount of rays casted from the top or bottom
    public float edgeOffset;        //Offset of rays to the corners
    public float clampDistance;     //Distance to clamp towards a collider, prevents glitching

    protected bool grounded = true;

    // Draw gizoms for visual representation of the collider box
    void OnDrawGizmosSelected() {
        //Get box corner positions
        Vector3 LefTopcorner = transform.position + Vector3.right * box.xMin + Vector3.up * box.yMax;
        Vector3 RightTopcorner = transform.position + Vector3.right * box.xMax + Vector3.up * box.yMax;
        Vector3 LefBottomcorner = transform.position + Vector3.right * box.xMin + Vector3.up * box.yMin;
        Vector3 RightBottomcorner = transform.position + Vector3.right * box.xMax + Vector3.up * box.yMin;

        //Draw box lines
        Gizmos.DrawLine(LefTopcorner, RightTopcorner);
        Gizmos.DrawLine(RightTopcorner, RightBottomcorner);
        Gizmos.DrawLine(RightBottomcorner, LefBottomcorner);
        Gizmos.DrawLine(LefBottomcorner, LefTopcorner);
    }

    // Input for horizontal movement (walking)
    public void SetHorizontalInput (float input) {
        float xVelocity = Mathf.Clamp(input, -1, 1) * speed;
        velocity.x = Mathf.MoveTowards(velocity.x, xVelocity, acceleration * Time.deltaTime);
    }

    // Input for jumping
    public void Jump () {
        if (grounded) {
            velocity.y = jumpForce;
        }
    }

    // Called if pawn landed on top of another pawn
    protected virtual void JumpOnOther (SideScrollerPawn other) {
        velocity.y = jumpForce;
    }

    // Called if pawn hits another pawn
    public virtual void HitOther (SideScrollerPawn other) {

    }

    // Set vertical velocity
    protected void SetVerticalVelocity(float yVelocity) {
        velocity.y = yVelocity;
    }

    // Returns if pawn is grounded
    public bool IsGrounded() {
        return grounded;
    }

    // Late update is called in sync with physics
    protected override void LateUpdate() {
        //Add gravity
        if (!grounded) {
            float gravity = velocity.y > 0 ? gravityUp : gravityDown;
            velocity = new Vector2(velocity.x, Mathf.Max(velocity.y - gravity, -maxFallSpeed));
        }

        Transform hit = null;
        // Vertical collision detection (up)
        if (velocity.y > 0) {
            hit = CheckCollision(Vector3.up);
            if (hit) {
                velocity.y = 0;
                if (hit.tag == "Player" && hit.transform != transform) {
                    SideScrollerPawn player = hit.GetComponent<SideScrollerPawn>();
                    if (player) {
                        HitOther(player);
                        player.HitOther(this);
                    }
                }
            }
            grounded = false;
        }
        // Vertical collision detection (down)
        else {
            hit = CheckCollision(Vector3.down);
            if (hit) {
                velocity.y = 0;
                if (hit.tag == "Player" && transform.position.y - hit.transform.position.y >= 1.4f) {
                    SideScrollerPawn player = hit.GetComponent<SideScrollerPawn>();
                    if (player) {
                        HitOther(player);
                        player.HitOther(this);
                        JumpOnOther(player);
                    }
                }
                else {
                    grounded = true;
                }
            }
            else {
                grounded = false;
            }
        }

        // Horizontal collision detection (right)
        if (velocity.x > 0) {
            hit = CheckCollision(Vector3.right);
            if (hit) {
                velocity.x = 0;
                if (hit.tag == "Player" && hit.transform != transform) {
                    SideScrollerPawn player = hit.GetComponent<SideScrollerPawn>();
                    if (player) {
                        HitOther(player);
                        player.HitOther(this);
                    }
                }
            }
        }
        // Horizontal collision detection (left)
        else if (velocity.x < 0) {
            hit = CheckCollision(Vector3.left);
            if (hit) {
                velocity.x = 0;
                if (hit.tag == "Player" && hit.transform != transform) {
                    SideScrollerPawn player = hit.GetComponent<SideScrollerPawn>();
                    if (player) {
                        HitOther(player);
                        player.HitOther(this);
                    }
                }
            }
        }

        base.LateUpdate();
    }

    /*Check collision in a given direction
    //Directions should always be restricted to one axis*/
    protected Transform CheckCollision(Vector3 direction) {
        int rayCount = 0;
        Vector3 startPosition = Vector3.zero;
        Vector3 endPosition = Vector3.zero;
        float distance = 0;

        //Set ray properties based on direction
        if (direction.normalized == Vector3.up) {
            rayCount = horizontalRayCount;
            startPosition = transform.position + new Vector3(box.xMin, box.center.y);
            endPosition = transform.position + new Vector3(box.xMax, box.center.y);
            distance = box.yMax - box.center.y;
        }
        else if (direction.normalized == Vector3.right) {
            rayCount = verticalRayCount;
            startPosition = transform.position + new Vector3(box.center.x, box.yMax);
            endPosition = transform.position + new Vector3(box.center.x, box.yMin);
            distance = box.xMax - box.center.x;
        }
        else if (direction.normalized == Vector3.down) {
            rayCount = horizontalRayCount;
            startPosition = transform.position + new Vector3(box.xMin, box.center.y);
            endPosition = transform.position + new Vector3(box.xMax, box.center.y);
            distance = Mathf.Abs(box.yMin - box.center.y);
        }
        else if (direction.normalized == Vector3.left) {
            rayCount = verticalRayCount;
            startPosition = transform.position + new Vector3(box.center.x, box.yMin);
            endPosition = transform.position + new Vector3(box.center.x, box.yMax);
            distance = Mathf.Abs(box.xMin - box.center.x);
        }

        //Cast rays and store the distance in collider distance
        float colliderDistance = distance + clampDistance;
        Transform collider = null;
        for (int i = 0; i < rayCount; i++) {
            float t = i / (float)(rayCount - 1);
            t = t * (1 - 2 * edgeOffset) + edgeOffset;

            Vector3 position = Vector3.Lerp(startPosition, endPosition, t);
            RaycastHit2D hit = Physics2D.Raycast(position, direction, colliderDistance);
            Debug.DrawRay(position, direction * colliderDistance, Color.red);
            if (hit && hit.transform != transform) {
                if (hit.distance < colliderDistance) {
                    colliderDistance = hit.distance;
                    collider = hit.transform;
                }
            }
        }

        //Pawn is colliding if the colliderdistance is smaller then the max collider distance
        if (colliderDistance < distance + clampDistance) {
            transform.Translate(direction * (colliderDistance - distance));
            return collider;
        }

        return null;
    }

}
