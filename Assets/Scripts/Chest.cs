using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour {
    public string content;
    public string text;
    public Sprite open;

    public AudioClip audioOpen;

    private PlayerController player;
    private AudioSource my_audio;
    private GameObject door;
    private bool opened;

    void Start() {
        opened = false;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        door = GameObject.FindWithTag("Door");
        my_audio = gameObject.GetComponent<AudioSource>();

        if (player.items.Contains(content)) {
            ChangeSprite();
            opened = true;
            OpenDoor();
        }
    }

    public string Open() {
        if (!opened) {
            opened = true;

            Text information = GameObject.FindWithTag("UI").GetComponent<UI>().information_text;
            information.enabled = true;
            information.text = text.Replace("\\n","\n");

            ChangeSprite();
            OpenDoor();
            my_audio.PlayOneShot(audioOpen);
            return content;
        } else {
            return null;
        }
    }

    private void ChangeSprite() {
        GetComponent<SpriteRenderer>().sprite = open;
    }

    private void OpenDoor() {
        door.GetComponent<SpriteRenderer>().enabled = false;
        door.GetComponent<BoxCollider2D>().enabled = false;
        door.GetComponent<Door>().PlaySound();
    }
}
