using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    private Camera ScrCam;
    List<GameObject> tables = new List<GameObject>();

    public Material normalTable, highlightedTable;

	// Use this for initialization
	void Start () {
        ScrCam = transform.gameObject.GetComponent<Camera>();
        tables.Add(transform.parent.Find("Играть").gameObject);
        tables.Add(transform.parent.Find("Загрузить").gameObject);
        tables.Add(transform.parent.Find("Настройки").gameObject);
        tables.Add(transform.parent.Find("Авторы").gameObject);
        tables.Add(transform.parent.Find("Выход").gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        GameObject go = getGameObjectFromCamRay();
        tables.ForEach(item => {
            if (item == go)
                item.transform.Find("Cube").GetComponent<MeshRenderer>().material = highlightedTable;
            else
                item.transform.Find("Cube").GetComponent<MeshRenderer>().material = normalTable;
        });
        if (go != null && Input.GetMouseButtonUp(0))
        {
            switch (go.name)
            {
                case "Играть": SceneManager.LoadScene("MainScene"); break;
                case "Выход": Application.Quit(); break;
            }
        }
    }

    GameObject getGameObjectFromCamRay()
    {
        Ray ray = ScrCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }
        else
            return null;
    }
}
