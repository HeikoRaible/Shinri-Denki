using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeOrMore : MonoBehaviour {
    public AudioClip audioPush;

    [HideInInspector] public bool inRow;

    private WinCondition winCondition;
    private PlayerController player;
    private Collisions collisions;
    private Animator animator;
    private AudioSource my_audio;
    private Vector2 previousPosition;

    private bool action;
    private bool check;

    void Start() {
        winCondition = GameObject.FindWithTag("Blocks").GetComponent<WinCondition>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        collisions = GetComponent<Collisions>();
        animator = GetComponent<Animator>();
        my_audio = GetComponent<AudioSource>();

        previousPosition = new Vector2(transform.position.x, transform.position.y);
        action = false;
        check = false;
        inRow = false;
    }

    void FixedUpdate() {
        if (!action) {
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

            if (currentPosition != previousPosition) {
                CheckRow();
            }

            previousPosition = currentPosition;
        }
    }

    void LateUpdate() {
        check = false;
        if (inRow) {
            animator.SetTrigger("clear");
        }
    }

    private void CheckRow() {
        if (!check) {
            check = true;
            collisions.check();

            // colliders
            Collider2D left = collisions.hitLeft.collider;
            Collider2D right = collisions.hitRight.collider;
            Collider2D up = collisions.hitUp.collider;
            Collider2D down = collisions.hitDown.collider;

            // scripts
            ThreeOrMore leftS = null;
            ThreeOrMore rightS = null;
            ThreeOrMore upS = null;
            ThreeOrMore downS = null;

            if (left && left.gameObject.tag == "Block") {leftS = left.gameObject.GetComponent<ThreeOrMore>();}
            if (right && right.gameObject.tag == "Block") {rightS = right.gameObject.GetComponent<ThreeOrMore>();}
            if (up && up.gameObject.tag == "Block") {upS = up.gameObject.GetComponent<ThreeOrMore>();}
            if (down && down.gameObject.tag == "Block"){downS = down.gameObject.GetComponent<ThreeOrMore>();}

            // check left
            if (left && left.name == transform.name) {
                leftS.CheckRow();
            }
            // check right
            if (right && right.name == transform.name) {
                rightS.CheckRow();
            }
            // check left+right
            if (left && right) {
                // mark three in row
                if (transform.name == left.name && transform.name == right.name) {
                    inRow = true;
                    leftS.inRow = true;
                    rightS.inRow = true;
                }
            }

            // check up
            if (up && up.name == transform.name) {
                upS.CheckRow();
            }
            // check down
            if (down && down.name == transform.name) {
                downS.CheckRow();
            }
            // UP + DOWN
            if (up && down) {
                // mark three in row
                if (transform.name == up.name && up.name == down.name) {
                    inRow = true;
                    upS.inRow = true;
                    downS.inRow = true;
                }
            }
        }
    }

    // move and collisions
    public void MoveAndCollisions(Vector2 direction) {
        if (!action) {
            collisions.check();

            Collider2D left = collisions.hitLeft.collider;
            Collider2D right = collisions.hitRight.collider;
            Collider2D up = collisions.hitUp.collider;
            Collider2D down = collisions.hitDown.collider;

            bool noCollisions = true;
            if (
                // Left
                (direction.y == 0 && direction.x < 0 && left) ||
                // Right
                (direction.y == 0 && direction.x > 0 && right) ||
                // Up
                (direction.x == 0 && direction.y > 0 && up) ||
                // Down
                (direction.x == 0 && direction.y < 0 && down)
                ) {
                    noCollisions = false;
            }
            if (noCollisions) {
                StartCoroutine(Move(direction));
            }
        }
    }

    // move iterator coroutine
    public IEnumerator Move(Vector2 direction) {
        action = true;
        GetComponent<AudioSource>().PlayOneShot(audioPush);

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(
            startPosition.x + System.Math.Sign(direction.x) * player.gridSize,
            startPosition.y + System.Math.Sign(direction.y) * player.gridSize,
            startPosition.z);

        float t = 0;
        float factor = 1f;
 
        while (t < 1f) {
            t += Time.deltaTime * (player.moveSpeed/player.gridSize) * factor;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        action = false;
    }
}
