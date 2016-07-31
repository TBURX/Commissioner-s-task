using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerSaver : MonoBehaviour
{

    public GameObject player;
    public GameObject PlCamera;
    //public string sceneName = "MainScene";
    public IniFile lastPosition = new IniFile("Saves/LastPosotion.dat");

    private void Awake()
    {
        string sceneName = lastPosition.IniReadValue("LastScene", "scene name", "MainScene");
        bool isGoToAnotherScene = bool.Parse(lastPosition.IniReadValue("LastScene", "isGoToAnotherScene", "false"));
        if (SceneManager.GetActiveScene().name != sceneName && !isGoToAnotherScene)
        {
            SceneManager.LoadScene(sceneName);
        }
        string savedPosition = lastPosition.IniReadValue(sceneName, "position", "none");
        string savedRotation = lastPosition.IniReadValue(sceneName, "rotation", "none");
        string savedCamPosition = lastPosition.IniReadValue(sceneName, "camPosition", "none");
        Debug.Log("loaded " + savedCamPosition);
        string savedCamRotation = lastPosition.IniReadValue(sceneName, "camRotation", "none");

        if (savedPosition != "none")
        {
            string[] posArr = savedPosition.Split('|');
            string[] rotArr = savedRotation.Split('|');
            string[] camPosArr = savedCamPosition.Split('|');
            string[] camRotArr = savedCamRotation.Split('|');

            float x_pos = Convert.ToSingle(posArr[0]);
            float y_pos = Convert.ToSingle(posArr[1]);
            float z_pos = Convert.ToSingle(posArr[2]);

            float x_rot = Convert.ToSingle(rotArr[0]);
            float y_rot = Convert.ToSingle(rotArr[1]);
            float z_rot = Convert.ToSingle(rotArr[2]);
            float w_rot = Convert.ToSingle(rotArr[3]);

            float x_Cpos = Convert.ToSingle(camPosArr[0]);
            float y_Cpos = Convert.ToSingle(camPosArr[1]);
            float z_Cpos = Convert.ToSingle(camPosArr[2]);
            /*
            float x_Crot = Convert.ToSingle(camRotArr[0]);
            float y_Crot = Convert.ToSingle(camRotArr[1]);
            float z_Crot = Convert.ToSingle(camRotArr[2]);
            float w_Crot = Convert.ToSingle(camRotArr[3]);
            */
            player.transform.position = new Vector3(x_pos, y_pos, z_pos);
            player.transform.rotation = new Quaternion(x_rot, y_rot, z_rot, w_rot);
            if (isGoToAnotherScene)
            {
                player.transform.localEulerAngles = new Vector3(player.transform.localEulerAngles.x, player.transform.localEulerAngles.y + 180, player.transform.localEulerAngles.z);
            }
            PlCamera.transform.localPosition = new Vector3(x_Cpos, y_Cpos, z_Cpos);
            Debug.Log("loaded " + PlCamera.transform.position);
            //PlCamera.transform.localRotation = new Quaternion(x_Crot, y_Crot, z_Crot, w_Crot);
        }
    }

    public void GoToAnotherScene(string sceneName)
    {
        lastPosition.IniWriteValue("LastScene", "scene name", sceneName);
        lastPosition.IniWriteValue(SceneManager.GetActiveScene().name, "position", player.transform.position.x + "|" + player.transform.position.y + "|" + player.transform.position.z);
        lastPosition.IniWriteValue(SceneManager.GetActiveScene().name, "rotation", player.transform.rotation.x + "|" + player.transform.rotation.y + "|" + player.transform.rotation.z + "|" + player.transform.rotation.w);
        lastPosition.IniWriteValue(SceneManager.GetActiveScene().name, "camPosition", PlCamera.transform.localPosition.x + "|" + PlCamera.transform.localPosition.y + "|" + PlCamera.transform.localPosition.z);
        lastPosition.IniWriteValue("LastScene", "isGoToAnotherScene", "true");
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        lastPosition.IniWriteValue("LastScene", "scene name", SceneManager.GetActiveScene().name);
        lastPosition.IniWriteValue(SceneManager.GetActiveScene().name, "position", player.transform.position.x + "|" + player.transform.position.y + "|" + player.transform.position.z);
        lastPosition.IniWriteValue(SceneManager.GetActiveScene().name, "rotation", player.transform.rotation.x + "|" + player.transform.rotation.y + "|" + player.transform.rotation.z + "|" + player.transform.rotation.w);
        lastPosition.IniWriteValue(SceneManager.GetActiveScene().name, "camPosition", PlCamera.transform.localPosition.x + "|" + PlCamera.transform.localPosition.y + "|" + PlCamera.transform.localPosition.z);
        lastPosition.IniWriteValue("LastScene", "isGoToAnotherScene", "false");
    }
}