using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    [HideInInspector] public Text information_text;
    [HideInInspector] public Text jumps_text;

    private PlayerController player;

    void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        information_text = transform.GetChild(0).GetComponent<Text>();
        jumps_text = transform.GetChild(1).GetComponent<Text>();
    }

    void Update() {
        jumps_text.enabled = player.items.Contains("JumpBoots") ? true : false;
        jumps_text.text = "Jumps: " + player.jumpsLeft;    
    }

    public void InformationOn() {
        information_text.enabled = true;
    }

    public void InformationOff() {
        information_text.enabled = false;
    }
}
