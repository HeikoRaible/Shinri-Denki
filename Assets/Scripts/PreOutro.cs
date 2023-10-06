using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreOutro : MonoBehaviour {
    private AudioSource bgm;

    void Awake() {
        bgm = GameObject.FindWithTag("BGM").GetComponent<AudioSource>();
        StartCoroutine(FadeAudio());
    }

    private IEnumerator FadeAudio() {
        float t = 0;
        float factor = 1f;

        while (t < 1f) {
            t += Time.deltaTime * 0.4f * factor;
            bgm.volume = 1-t;
            yield return null;
        }
    }
}
