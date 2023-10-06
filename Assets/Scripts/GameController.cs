using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    private GameObject player;
    private PlayerController playerScript;
    private GameObject cam;
    private CameraMovement camScript;

    // current level data
    private LevelData levelData;

    void Start() {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
        cam = GameObject.FindWithTag("MainCamera");
        camScript = cam.GetComponent<CameraMovement>();
    }

    public void LoadLevel(int level) {
        SceneManager.LoadScene(level);
    }

    public void Continue(int level) {
        if (level > 4) {playerScript.items.Add("TimeGlove");}
        if (level > 12) {playerScript.items.Add("JumpBoots");}

        SceneManager.LoadScene(level);
    }
}
