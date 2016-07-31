using UnityEngine;
using System.Collections;

public class MyFPSController : UnityStandardAssets.Characters.FirstPerson.FirstPersonController {

    public PlayerSaver playerSaver;

    public override void openDoor()
    {
        playerSaver.GoToAnotherScene(m_RayTransform.name.Split('|')[0]);
    }
}
