using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestCondition : MonoBehaviour {

    public SystemOfCondition systemOfConditions;
    public Text barrelText;
    public AudioSource audioS;

    int count = 0;

    bool[] cond = new bool[4];

    // Use this for initialization
    void Start () {
        cond[0] = false;
        cond[1] = false;
        cond[2] = false;
        cond[3] = false;
        audioS.Stop();
    }
	
	// Update is called once per frame
	void Update () {
	    if (systemOfConditions.readCondition(6) == "1" && cond[0] == false)
        {
            count += 1;
            cond[0] = true;
        }
        if (systemOfConditions.readCondition(7) == "1" && cond[1] == false)
        {
            count += 1;
            cond[1] = true;
        }
        if (systemOfConditions.readCondition(8) == "1" && cond[2] == false)
        {
            count += 1;
            cond[2] = true;
        }
        if (systemOfConditions.readCondition(9) == "1" && cond[3] == false)
        {
            count += 1;
            cond[3] = true;
        }
        if (count < 4)
        {
            barrelText.text = "Бочек найдено: " + count + "/4";
        }
        else
        {
            barrelText.text = "Бочек найдено: " + count + "/4" +
                "\n В этом мире призошла какая-то магия :)";
            if (audioS.isPlaying == false)
            {
                audioS.Play();
            }
        }
    }
}
