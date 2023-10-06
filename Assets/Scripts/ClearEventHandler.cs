using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearEventHandler : MonoBehaviour {
    private WinCondition winCondition;

    void Start() {
        winCondition = GameObject.FindWithTag("Blocks").GetComponent<WinCondition>();
    }

    private void Clear() {
        this.gameObject.SetActive(false);
        winCondition.CheckWin();
    }
}
