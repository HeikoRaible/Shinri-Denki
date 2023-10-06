using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour {
    private int selection;

    private Image btn0;
    private Image btn1;
    private Image btn2;

    void Start() {
        btn0 = transform.GetChild(0).gameObject.GetComponent<Image>();
        btn1 = transform.GetChild(1).gameObject.GetComponent<Image>();
        btn2 = transform.GetChild(2).gameObject.GetComponent<Image>();

        selection = 0;
    }

    void Update() {
        ChangeColors();

        if (Input.GetKeyDown(KeyCode.W)) {
            switch (selection) {
                case 0:
                    selection = 2;
                    break;
                case 1:
                    selection = 0;
                    break;
                case 2:
                    selection = 1;
                    break;
            }
        } else if (Input.GetKeyDown(KeyCode.S)) {
            switch (selection) {
                case 0:
                    selection = 1;
                    
                    break;
                case 1:
                    selection = 2;
                    break;
                case 2:
                    selection = 0;
                    break;
            }
        } else if (Input.GetKey(KeyCode.Space)) {
            switch (selection) {
                case 0:
                    StartButton();
                    break;
                case 1:
                    ContinueButton();
                    break;
                case 2:
                    ExitButton();
                    break;
            }
        }
    }

    private void ChangeColors() {
        btn0.color = (selection == 0) ? new Color(0.5f,0.5f,0.5f,1f) : new Color(1f,1f,1f,1f);
        btn1.color = (selection == 1) ? new Color(0.5f,0.5f,0.5f,1f) : new Color(1f,1f,1f,1f);
        btn2.color = (selection == 2) ? new Color(0.5f,0.5f,0.5f,1f) : new Color(1f,1f,1f,1f);
    }

    public void StartButton() {
        GameObject.FindWithTag("GameController").GetComponent<GameController>().LoadLevel(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void ContinueButton() {
        int lvl = PlayerPrefs.GetInt("Progress");
        if (lvl != 0) {GameObject.FindWithTag("GameController").GetComponent<GameController>().Continue(lvl);}
    }

    public void ExitButton() {
        Application.Quit();
    }
}
