using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EarthMove : MonoBehaviour {
    private int framesElapsed = 0;

    void Update() {
        Vector3 targetPosition = new Vector3(3f, 6f, transform.position.z);
        Vector3 targetSize = new Vector3(1f, 1f, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.001f);
        transform.localScale = Vector3.Lerp(transform.localScale, targetSize, 0.0005f);

        framesElapsed++;

        if (framesElapsed >= 400) {
            GameObject.FindWithTag("GameController").GetComponent<GameController>().LoadLevel(SceneManager.GetActiveScene().buildIndex+1);
        }
    }

}
