using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {
    public bool open;
    public AudioClip audioOpen;

    private GameObject player;
    private AudioSource my_audio;
    private Vector3 centerOffset;

    void Start() {
        player = GameObject.FindWithTag("Player");
        Vector2 playerCenterOffset = player.GetComponent<Collisions>().centerOffset;
        my_audio = gameObject.GetComponent<AudioSource>();
        centerOffset = new Vector3(playerCenterOffset.x, playerCenterOffset.y, 0);

        if (open) {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void FixedUpdate() {
        // if player steps on door, load next level
        if (transform.position == (player.transform.position + centerOffset)) {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().LoadLevel(SceneManager.GetActiveScene().buildIndex+1);
        }
    }

    public void PlaySound() {
        my_audio.PlayOneShot(audioOpen);
    }
}
