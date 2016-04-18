using UnityEngine;
using System.Collections;

public class TeleportToScene : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        Application.LoadLevel("World");
    }
}
