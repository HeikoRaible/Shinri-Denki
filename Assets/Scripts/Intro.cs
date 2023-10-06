using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {
    public AudioClip audioAttack;
    public Sprite humanUp;

    private AudioSource my_audio;
    private AudioSource jungleAudio;

    private GameObject player;
    private PlayerController playerScript;
    private Animator playerAnimator;
    private SpriteRenderer fade;

    private Transform enemy;

    private float moveSpeed;
    private float gridSize;

    private bool action;
    private int act;

    void Awake() {
        my_audio = GetComponent<AudioSource>();
        jungleAudio = GameObject.FindWithTag("LevelData").gameObject.GetComponent<AudioSource>();

        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        playerAnimator = playerScript.animator;
        fade = GameObject.FindWithTag("Fade").gameObject.GetComponent<SpriteRenderer>();

        enemy = GameObject.FindWithTag("Enemy").transform;

        moveSpeed = playerScript.moveSpeed;
        gridSize = playerScript.gridSize;

        action = false;
        act = 0;
    }

    void FixedUpdate() {
        if(!action) {
            switch (act) {
                // wait
                case 0:
                    StartCoroutine(Wait(4));
                    break;
                // walk down
                case 1:
                    if (player.transform.position.y > 3f) {StartCoroutine(Move(new Vector2(0,-1)));}
                    else {act++;}
                    break;
                // wait
                case 2:
                    StartCoroutine(Wait(2));
                    break;
                // walk down
                case 3:
                    if (player.transform.position.y > 1f) {StartCoroutine(Move(new Vector2(0,-1)));}
                    else {act++;}
                    break;
                // wait
                case 4:
                    StartCoroutine(Wait(1));
                    break;
                // turn right
                case 5:
                    Turn(new Vector2(1,0));
                    break;
                // wait
                case 6:
                    StartCoroutine(Wait(2));
                    break;
                // turn left
                case 7:
                    Turn(new Vector2(-1,0));
                    break;
                // wait
                case 8:
                    StartCoroutine(Wait(2));
                    break;
                // walk down
                case 9:
                    if (player.transform.position.y > -1f) {StartCoroutine(Move(new Vector2(0,-1)));}
                    else {act++;}
                    break;
                // wait
                case 10:
                    StartCoroutine(Wait(1));
                    break;
                // walk down
                case 11:
                    if (player.transform.position.y > -3f) {StartCoroutine(Move(new Vector2(0,-1)));}
                    else {act++;}
                    break;
                // wait
                case 12:
                    StartCoroutine(Wait(2));
                    break;
                // turn left
                case 13:
                    Turn(new Vector2(-1,0));
                    break;
                // wait
                case 14:
                    StartCoroutine(Wait(1));
                    break;
                // walk left
                case 15:
                    if (player.transform.position.x > 1f) {StartCoroutine(Move(new Vector2(-1,0)));}
                    else {act++;}
                    break;
                // wait
                case 16:
                    StartCoroutine(Wait(6));
                    break;
                // turn right
                case 17:
                    Turn(new Vector2(1,0));
                    break;
                // wait
                case 18:
                    StartCoroutine(Wait(1));
                    break;
                // attacked
                case 19:
                    StartCoroutine(Attack());
                    break;
                // wait
                case 20:
                    StartCoroutine(Wait(2));
                    break;
                // fade off
                case 21:
                    Turn(new Vector2(0, -1));
                    StartCoroutine(FadeOff());
                    break;
                // wait
                case 22:
                    StartCoroutine(Wait(2));
                    break;
                // turn right
                case 23:
                    Turn(new Vector2(1, 0));
                    break;
                // wait
                case 24:
                    StartCoroutine(Wait(1));
                    break;
                // turn left
                case 25:
                    Turn(new Vector2(-1, 0));
                    break;
                // wait
                case 26:
                    StartCoroutine(Wait(1));
                    break;
                // walk right
                case 27:
                    if (player.transform.position.x < 2f) {StartCoroutine(Move(new Vector2(1,0)));}
                    else {act++;}
                    break;
                // walk up
                case 28:
                    if (player.transform.position.y < 5f) {StartCoroutine(Move(new Vector2(0,1)));}
                    else {act++;}
                    break;
                case 29:
                    GameObject.FindWithTag("GameController").GetComponent<GameController>().LoadLevel(SceneManager.GetActiveScene().buildIndex+1);
                    break;
            }
        }
    }

    private IEnumerator Attack() {
        action = true;
        Vector3 startPosition = enemy.position;

        float t = 0;
        float factor = 1f;

        my_audio.PlayOneShot(audioAttack);
 
        while (t < 1f) {
            t += Time.deltaTime * 3*(moveSpeed/gridSize) * factor;
            enemy.position = Vector3.Lerp(startPosition, player.transform.position, t);
            yield return null;
        }

        jungleAudio.volume = 0f;
        fade.color = new Color(0f,0f,0f,1f);
        enemy.position = new Vector3(2f, 1f, 0f);
        enemy.gameObject.GetComponent<SpriteRenderer>().sprite = humanUp;
        
        act++;
        action = false;
    }

    private IEnumerator FadeOff() {
        action = true;
        Vector3 startPosition = enemy.position;
        Vector3 endPosition = new Vector3(2f, 5f, 0f);

        float t = 0;
        float factor = 1f;

        while (t < 1f) {
            t += Time.deltaTime * 0.1f*(moveSpeed/gridSize) * factor;
            fade.color = Color.Lerp(new Color(0f,0f,0f,1f), new Color(0f,0f,0f,0f), t);
            enemy.position = Vector3.Lerp(startPosition, endPosition, t);
            jungleAudio.volume = t;
            yield return null;
        }

        enemy.gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
