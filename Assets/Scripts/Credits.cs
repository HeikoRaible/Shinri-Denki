using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour {
    void Awake() {
        GameObject.FindWithTag("Player").gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GameObject.FindWithTag("MainCamera").gameObject.GetComponent<Camera>().enabled = false;
    }
}
