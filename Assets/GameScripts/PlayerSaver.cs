using UnityEngine;
using System;

public class PlayerSaver : MonoBehaviour {

    public GameObject player;
    public GameObject PlCamera;
    public string sceneName = "MainScene";
    IniFile lastPosition = new IniFile("Saves/LastPosotion.dat");

    private void Awake()
    {
        Screen.lockCursor = true;

        string savedPosition = lastPosition.IniReadValue(sceneName, "position", "none");
        string savedRotation = lastPosition.IniReadValue(sceneName, "rotation", "none");
        string savedCamRotation = lastPosition.IniReadValue(sceneName, "camRotation", "none");

        if (savedPosition != "none")
        {
            string[] posArr = savedPosition.Split('|');
            string[] rotArr = savedRotation.Split('|');
            string[] camrotArr = savedRotation.Split('|');

            float x_pos = Convert.ToSingle(posArr[0]);
            float y_pos = Convert.ToSingle(posArr[1]);
            float z_pos = Convert.ToSingle(posArr[2]);

            float x_rot = Convert.ToSingle(rotArr[0]);
            float y_rot = Convert.ToSingle(rotArr[1]);
            float z_rot = Convert.ToSingle(rotArr[2]);
            float w_rot = Convert.ToSingle(rotArr[3]);

            float x_Crot = Convert.ToSingle(camrotArr[0]);
            float y_Crot = Convert.ToSingle(camrotArr[1]);
            float z_Crot = Convert.ToSingle(camrotArr[2]);
            float w_Crot = Convert.ToSingle(camrotArr[3]);

            player.transform.position = new Vector3(x_pos, y_pos, z_pos);

            if (savedRotation != "none")
            {
                player.transform.rotation = new Quaternion(x_rot, y_rot , z_rot, w_rot);
                PlCamera.transform.rotation = new Quaternion(x_Crot, y_Crot, z_Crot, w_Crot);
            }
        }
    }
}
