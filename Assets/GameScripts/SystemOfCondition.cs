using UnityEngine;
using System.Collections;
using System.IO;

public class SystemOfCondition : MonoBehaviour {

    public string[] arrayOfConditions;
    string conditions;
    IniFile conditionsFile = new IniFile("Settings/Conditions.dat");

	void Start () {
        if (!Directory.Exists("Settings"))
        {
            Directory.CreateDirectory("Settings");
        }

        conditions = conditionsFile.IniReadValue("Conditions", "allConditions", "null");
        arrayOfConditions = new string[conditions.Length];

        for (int i = 0; i < arrayOfConditions.Length; i++)
        {
            arrayOfConditions[i] = conditions[i].ToString();
        }

        Debug.Log(conditions);
    }

    public string readCondition(int i)
    {
        return arrayOfConditions[i];
    }

    public void writeCondition(int i, string str)
    {
        arrayOfConditions[i] = str;
    }

    private void onExit()
    {
        File.Delete("Settings/Conditions.dat");
    }

    public void Save()
    {          
        string tempConditions = "";
        for (int i = 0; i < conditions.Length; i++)
        {
            tempConditions += arrayOfConditions[i];
        }
        conditionsFile.IniWriteValue("Conditions", "allConditions", tempConditions);
    }
	
}
