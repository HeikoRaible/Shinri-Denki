using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour {
    public Vector2 centerOffset = new Vector2(0, 0);

    [HideInInspector] public RaycastHit2D hitLeft;
    [HideInInspector] public RaycastHit2D hitRight;
    [HideInInspector] public RaycastHit2D hitUp;
    [HideInInspector] public RaycastHit2D hitDown;

    private BoxCollider2D ownCollider;

    private float raycastMaxDistance = 0.5f;
    private float rayOffset = 0.5f;

    private Vector2 left = new Vector2(-1, 0);
    private Vector2 right = new Vector2(1, 0);
    private Vector2 up = new Vector2(0, 1);
    private Vector2 down = new Vector2(0, -1);

    void Start() {
        ownCollider = GetComponent<BoxCollider2D>();
    }

    public void check() {
        hitLeft = Raycast(left);
        hitRight = Raycast(right);
        hitUp = Raycast(up);
        hitDown = Raycast(down);
    }

    private RaycastHit2D Raycast(Vector2 direction) {
        Vector2 startingPosition = new Vector2(transform.position.x, transform.position.y);
        startingPosition += centerOffset;

        if (direction.y == 0) {startingPosition.x += direction.x*rayOffset;}
        else if (direction.x == 0) {startingPosition.y += direction.y*rayOffset;}

        Debug.DrawRay(startingPosition, direction, Color.green);

        ownCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(startingPosition, direction, raycastMaxDistance);
        ownCollider.enabled = true;
        return hit;
    }
}
