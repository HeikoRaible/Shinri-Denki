using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontKill : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
}
