using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {
    public Transform target;
    public AudioClip audioTeleport;

    private Transform player;
    private Vector3 centerOffset;
    private AudioSource my_audio;

    private bool teleporting;

    void Start() {
        player = GameObject.FindWithTag("Player").transform;
        Vector2 playerCenterOffset = player.GetComponent<Collisions>().centerOffset;
        my_audio = gameObject.GetComponent<AudioSource>();
        centerOffset = new Vector3(playerCenterOffset.x, playerCenterOffset.y, 0);

        teleporting = false;
    }

    void FixedUpdate() {
        // if player steps on teleporter, teleport to other teleporter
        if (!teleporting && transform.position == (player.position + centerOffset)) {
            teleporting = true;
            player.position = (target.position - centerOffset);
            my_audio.PlayOneShot(audioTeleport);
        }
    }

    void LateUpdate() {
        teleporting = false;
    }
}
