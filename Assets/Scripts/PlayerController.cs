using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

class PlayerController : MonoBehaviour {
    public bool debug = false;
    public int jumpsLeft = 0;

    public AudioClip audioJump = null;

    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float moveSpeed = 3f;
    [HideInInspector] public int gridSize = 1;
    [HideInInspector] public Vector3 resetPosition;
    [HideInInspector] public int jumpsStart = 0;
    [HideInInspector] public bool action;

    private Collisions collisions;
    private SpriteRenderer spriteRenderer;
    private AudioSource myAudio;

    private Vector2 input;

    // items
    public ArrayList items;

    void Start() {
        animator = GetComponent<Animator>();
        collisions = GetComponent<Collisions>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAudio = GetComponent<AudioSource>();

        resetPosition = transform.position;

        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);

        action = true;

        jumpsStart = jumpsLeft;

        items = new ArrayList();
    }

    private void FixedUpdate() {
        // get input
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) {
            input.y = 0;
        } else {
            input.x = 0;
        }

        if (debug && Input.GetKey(KeyCode.E)) {
            DEBUG();
        }

        // ACTIONS
        if (!action) {
            // RESET LEVEL
            if (Input.GetKey(KeyCode.Q) && items.Contains("TimeGlove")) {
                transform.position = resetPosition;
                jumpsLeft = jumpsStart;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // INTERACT
            } else if (Input.GetKey(KeyCode.Space)) {
                Interact();
            // MOVEMENT
            } else if (input != Vector2.zero) {
                MoveAndCollisions();
            }
        }
    }

    private void DEBUG() {
        GameObject.FindWithTag("GameController").GetComponent<GameController>().LoadLevel(SceneManager.GetActiveScene().buildIndex+1);
    }

    // interact
    private void Interact() {
        collisions.check();
 
        Collider2D left = collisions.hitLeft.collider;
        Collider2D right = collisions.hitRight.collider;
        Collider2D up = collisions.hitUp.collider;
        Collider2D down = collisions.hitDown.collider;

        Collisions collisionsScript;

        // Left
        if (direction.y == 0 && direction.x < 0 && left) {
            // Block
            if (left.tag == "Block") {
                left.gameObject.GetComponent<ThreeOrMore>().MoveAndCollisions(direction);
            // Rift
            } else if (jumpsLeft > 0 && left.tag == "Rift" && items.Contains("JumpBoots")) {
                collisionsScript = left.gameObject.GetComponent<Collisions>();
                collisionsScript.check();
                if (!collisionsScript.hitLeft) {
                    jumpsLeft--;
                    StartCoroutine(Move(true));
                }
            }
        // Right
        } else if (direction.y == 0 && direction.x > 0 && right) {
            // Block
            if (right.tag == "Block") {
                right.gameObject.GetComponent<ThreeOrMore>().MoveAndCollisions(direction);
            // Rift
            } else if (jumpsLeft > 0 && right.tag == "Rift" && items.Contains("JumpBoots")) {
                collisionsScript = right.gameObject.GetComponent<Collisions>();
                collisionsScript.check();
                if (!collisionsScript.hitRight) {
                    jumpsLeft--;
                    StartCoroutine(Move(true));
                }
            }
        // Up
        } else if (direction.x == 0 && direction.y > 0 && up) {
            // Block
            if (up.tag == "Block") {
                up.gameObject.GetComponent<ThreeOrMore>().MoveAndCollisions(direction);
            // Chest
            } else if (up.tag == "Chest") {
                string item = up.gameObject.GetComponent<Chest>().Open();
                if (item != null) {
                    items.Add(item);
                }
            // Rift
            } else if (jumpsLeft > 0 && up.tag == "Rift" && items.Contains("JumpBoots")) {
                collisionsScript = up.gameObject.GetComponent<Collisions>();
                collisionsScript.check();
                if (!collisionsScript.hitUp) {
                    jumpsLeft--;
                    StartCoroutine(Move(true));
                }
            }
        // Down
        } else if (direction.x == 0 && direction.y < 0 && down) {
            // Block
            if (down.tag == "Block") {
                down.gameObject.GetComponent<ThreeOrMore>().MoveAndCollisions(direction);
            // Rift
            } else if (jumpsLeft > 0 && down.tag == "Rift" && items.Contains("JumpBoots")) {
                collisionsScript = down.gameObject.GetComponent<Collisions>();
                collisionsScript.check();
                if (!collisionsScript.hitDown) {
                    jumpsLeft--;
                    StartCoroutine(Move(true));
                }
            }
        }
    }

    // move and collisions
    private void MoveAndCollisions() {
        // turn
        direction = new Vector2(input.x, input.y);
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);

        // move if possible
        if(!Input.GetKey(KeyCode.R)) {
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
                StartCoroutine(Move(false));
            }
        }
    }

    // move iterator coroutine
    public IEnumerator Move(bool jump) {
        action = true;
        if (!jump) {animator.SetBool("moving", true);}
        else {
            myAudio.PlayOneShot(audioJump);
            spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }

        BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;

        int grid = jump ? 2*gridSize : gridSize;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(
            startPosition.x + System.Math.Sign(direction.x) * grid,
            startPosition.y + System.Math.Sign(direction.y) * grid,
            startPosition.z);

        float t = 0;
        float factor = 1f;

        float ms = jump ? 3*moveSpeed : moveSpeed;
 
        while (t < 1f) {
            t += Time.deltaTime * (ms/grid) * factor;
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        boxCollider.enabled = true;

        if (!jump) {animator.SetBool("moving", false);}
        else {spriteRenderer.color = new Color(1f, 1f, 1f, 1f);}
        action = false;
    }
}
