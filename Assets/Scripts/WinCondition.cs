using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour {
    public AudioClip audioClear;

    private int blockCount;
    private int prevBlockCount;
    private GameObject door;
    private AudioSource my_audio;

    void Start() {
        my_audio = GetComponent<AudioSource>();
        door = GameObject.FindWithTag("Door");
        CheckWin();
        prevBlockCount = blockCount;
    }

    void LateUpdate() {
        if (blockCount != prevBlockCount) {
            my_audio.PlayOneShot(audioClear);
            prevBlockCount = blockCount;
        }
    }

    public void CheckWin() {
        blockCount = 0;
        foreach (Transform color in transform) {
            foreach (Transform block in color) {
                if (block.gameObject.activeSelf) {
                    blockCount += 1;			
                }
            }
        }
        if (blockCount == 0) {
            door.GetComponent<SpriteRenderer>().enabled = false;
            door.GetComponent<BoxCollider2D>().enabled = false;
            door.GetComponent<Door>().PlaySound();
        }
    }
}
