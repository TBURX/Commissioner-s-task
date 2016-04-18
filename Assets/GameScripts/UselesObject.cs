using UnityEngine;
using System.Collections;

public class UselesObject : MonoBehaviour {

    public bool save = true;

    public void isUse()
    {
        Debug.Log("Ты использовал " + transform.name.Split('|')[0]);
        SystemOfCondition systemOfConditions = GameObject.Find("SystemOfConditions").GetComponent<SystemOfCondition>();
        systemOfConditions.writeCondition(int.Parse(transform.name.Split('|')[1]), "1");
        if (save == true)
        {
            systemOfConditions.Save();
        }
    }
}
