using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Outro : MonoBehaviour {
    public AudioClip audioAttack;
    public AudioClip audioLaugh;

    private AudioSource my_audio;

    private GameObject player;
    private PlayerController playerScript;
    private Animator playerAnimator;
    private SpriteRenderer fade;
    private CameraMovement cam;

    private Transform enemy;

    private float moveSpeed;
    private float gridSize;

    private bool action;
    private int act;

    void Awake() {
        my_audio = GetComponent<AudioSource>();

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerAnimator = playerScript.animator;
        cam = GameObject.FindWithTag("MainCamera").GetComponent<CameraMovement>();
        fade = GameObject.FindWithTag("Fade").gameObject.GetComponent<SpriteRenderer>();

        enemy = GameObject.FindWithTag("Enemy").transform;

        moveSpeed = playerScript.moveSpeed;
        gridSize = playerScript.gridSize;

        action = false;
        act = 0;
    }

    void Start() {
        cam.smoothing = 0.007f;
        cam.target = enemy;
    }

    void FixedUpdate() {
        if(!action) {
            switch (act) {
                // wait
                case 0:
                    StartCoroutine(Wait(4));
                    break;
                // walk up
                case 1:
                    if (player.transform.position.y < -3.5f) {StartCoroutine(Move(new Vector2(0,1)));}
                    else {act++;}
                    break;
                // wait
                case 2:
                    StartCoroutine(Wait(1));
                    break;
                // walk up
                case 3:
                    if (player.transform.position.y < -1.5f) {StartCoroutine(Move(new Vector2(0,1)));}
                    else {act++;}
                    break;
                // wait
                case 4:
                    StartCoroutine(Wait(1));
                    break;
                // attack
                case 5:
                    StartCoroutine(Attack());
                    break;
                // wait
                case 6:
                    StartCoroutine(Wait(2));
                    break;
                // fade off
                case 7:
                    Turn(new Vector2(0, -1));
                    StartCoroutine(FadeOff());
                    break;
                // wait
                case 8:
                    StartCoroutine(Wait(1));
                    break;
                // wait
                case 9:
                    StartCoroutine(Wait(3));
                    break;
                // credits
                case 10:
                    GameObject.FindWithTag("GameController").GetComponent<GameController>().LoadLevel(SceneManager.GetActiveScene().buildIndex+1);
                    break;
            }
        }
    }

    private IEnumerator Attack() {
        action = true;
        Vector3 startPosition = player.transform.position;

        float t = 0;
        float factor = 1f;

        my_audio.PlayOneShot(audioAttack);
 
        while (t < 1f) {
            t += Time.deltaTime * 3*(moveSpeed/gridSize) * factor;
            player.transform.position = Vector3.Lerp(startPosition, enemy.position - new Vector3(0, 0.5f*gridSize, 0), t);
            yield return null;
        }

        GameObject.FindWithTag("LevelData").gameObject.GetComponent<AudioSource>().volume = 0f;
        fade.color = new Color(0f,0f,0f,1f);

        player.transform.position = enemy.position;
        cam.target = player.transform;
        cam.transform.position = new Vector3(cam.transform.position.x, 2, cam.transform.position.z);
        cam.minPosition.y = 2;
        cam.maxPosition.y = 2;

        enemy.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        enemy.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        enemy.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        
        act++;
        action = false;
    }

    private IEnumerator FadeOff() {
        action = true;

        float t = 0;
        float factor = 1f;

        while (t < 1f) {
            t += Time.deltaTime * 0.1f*(moveSpeed/gridSize) * factor;
            fade.color = Color.Lerp(new Color(0f,0f,0f,1f), new Color(0f,0f,0f,0f), t);
            yield return null;
        }
        my_audio.PlayOneShot(audioLaugh);
        act++;
        action = false;
    }

    private IEnumerator Move(Vector2 direction) {
        action = true;
        playerAnimator.SetFloat("moveX", direction.x);
        playerAnimator.SetFloat("moveY", direction.y);
        playerAnimator.SetBool("moving", true);

        Vector3 startPosition = player.transform.position;
        Vector3 endPosition = new Vector3(
            startPosition.x + System.Math.Sign(direction.x) * gridSize,
            startPosition.y + System.Math.Sign(direction.y) * gridSize,
            startPosition.z);

        float t = 0;
        float factor = 1f;
 
        while (t < 1f) {
            t += Time.deltaTime * (moveSpeed/gridSize) * factor;
            player.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        playerAnimator.SetBool("moving", false);
        action = false;
    }

    private void Turn(Vector2 direction) {
        playerAnimator.SetFloat("moveX", direction.x);
        playerAnimator.SetFloat("moveY", direction.y);
        act++;
    }

    private IEnumerator Wait(int seconds) {
        action = true;
        yield return new WaitForSeconds(seconds);
        act++;
        action = false;
    }
}
