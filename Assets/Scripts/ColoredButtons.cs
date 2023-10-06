using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredButtons : MonoBehaviour {

    void Update() {
        foreach (Transform button in transform) {
            bool activated = false;
            foreach (GameObject block in GameObject.FindGameObjectsWithTag("Block")) {
                if (block.transform.position == button.position) {
                    if (block.name == button.name) {
                        activated = true;
                    }
                }
            }
            if (activated) {
                GameObject door = button.GetChild(0).gameObject;
                door.GetComponent<SpriteRenderer>().enabled = false;
                door.GetComponent<BoxCollider2D>().enabled = false;
            } else {
                GameObject door = button.GetChild(0).gameObject;
                door.GetComponent<SpriteRenderer>().enabled = true;
                door.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
}
