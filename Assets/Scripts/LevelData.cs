using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelData : MonoBehaviour {
    public Vector2 playerPos;

    public int jumpsStart;

    public Vector2 camMaxPosition;
    public Vector2 camMinPosition;

    public bool storyScene = false;

    void Awake() {
        GameObject player = GameObject.FindWithTag("Player");
        PlayerController playerScript = player.GetComponent<PlayerController>();
        GameObject cam = GameObject.FindWithTag("MainCamera");
        CameraMovement camScript = cam.GetComponent<CameraMovement>();

        // save progress
        PlayerPrefs.SetInt("Progress", SceneManager.GetActiveScene().buildIndex);

        // set BGM
        if (storyScene) {GameObject.FindWithTag("BGM").GetComponent<AudioSource>().enabled = false;}
        else {GameObject.FindWithTag("BGM").GetComponent<AudioSource>().enabled = true;}

        // player
        player.transform.position = playerPos;
        playerScript.action = storyScene;
        playerScript.resetPosition = playerPos;
        playerScript.jumpsStart = jumpsStart;
        playerScript.jumpsLeft = jumpsStart;

        // cam
        cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, cam.transform.position.z);
        camScript.maxPosition = camMaxPosition;
        camScript.minPosition = camMinPosition;

        // ui
        GameObject.FindWithTag("UI").gameObject.GetComponent<Canvas>().enabled = storyScene ? false : true;

        // information
        if (SceneManager.GetActiveScene().buildIndex == 3) {GameObject.FindWithTag("UI").GetComponent<UI>().InformationOn();}
        else {GameObject.FindWithTag("UI").GetComponent<UI>().InformationOff();}
    }
}
