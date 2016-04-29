using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class MainMenuController : MonoBehaviour {

    private Camera ScrCam;
    List<GameObject> tables = new List<GameObject>();

    //UI
    public Canvas UILayer;
    private GameObject headerText;
    private GameObject optionsMenu;
    private GameObject authors;

    public Material normalTable, highlightedTable, pressedTable;


    private Dropdown resolutionsDropdown;
    private bool dropdownIsSet = false;
    private Toggle fullscreenToggle;

    // Use this for initialization
    void Start () {
        ScrCam = transform.gameObject.GetComponent<Camera>();
        tables.Add(transform.parent.Find("Играть").gameObject);
        tables.Add(transform.parent.Find("Загрузить").gameObject);
        tables.Add(transform.parent.Find("Настройки").gameObject);
        tables.Add(transform.parent.Find("Авторы").gameObject);
        tables.Add(transform.parent.Find("Выход").gameObject);

        headerText = UILayer.transform.Find("headerText").gameObject;
        optionsMenu = UILayer.transform.Find("OptionsMenu").gameObject;
        authors = UILayer.transform.Find("Authors").gameObject;
        resolutionsDropdown = optionsMenu.transform.Find("resolution").transform.Find("resolutionDropdown").GetComponent<Dropdown>();
        fullscreenToggle = optionsMenu.transform.Find("fullscreen").transform.Find("fullscreenToggle").GetComponent<Toggle>();
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    void FixedUpdate()
    {
        if (!dropdownIsSet)
        {
            setResolutionsDropdown();
        }
    }
    private void setResolutionsDropdown()
    {
        resolutionsDropdown.options.Clear();

        int i = 0;
        while (i < Screen.resolutions.Length && (Screen.resolutions[i].height != Screen.height || Screen.resolutions[i].width != Screen.width))
        {
            i++;
        }
        if (i >= Screen.resolutions.Length)
            i = 0;

        foreach (Resolution res in Screen.resolutions)
        {
            resolutionsDropdown.options.Add(new Dropdown.OptionData(res.width.ToString() + "x" + res.height.ToString()));
        }

        resolutionsDropdown.value = i;

        resolutionsDropdown.RefreshShownValue();
        dropdownIsSet = true;
    }

    public void applyResolution()
    {
        Screen.SetResolution(
            Screen.resolutions[resolutionsDropdown.value].width,
            Screen.resolutions[resolutionsDropdown.value].height,
            fullscreenToggle.isOn
            );
        dropdownIsSet = false;
    }

    // Update is called once per frame
    void Update () {
        ResizeUI();
        GameObject go = getGameObjectFromCamRay();
        tables.ForEach(item => {
            if (item == go)
            {
                if (Input.GetMouseButton(0))
                {
                        go.transform.Find("Cube").GetComponent<MeshRenderer>().material = pressedTable;
                }
                else
                    item.transform.Find("Cube").GetComponent<MeshRenderer>().material = highlightedTable;
            }
            else
            {
                if (
                    item.name == "Настройки" && optionsMenu.activeSelf ||
                    item.name == "Авторы" && authors.activeSelf
                )
                {
                    item.transform.Find("Cube").GetComponent<MeshRenderer>().material = highlightedTable;
                }
                else
                    item.transform.Find("Cube").GetComponent<MeshRenderer>().material = normalTable;
            }

        });
        if (Input.GetMouseButtonUp(0))
        {
            if (go != null)
            {
                switch (go.name)
                {
                    case "Играть": SceneManager.LoadScene("MainScene"); break;
                    case "Настройки": optionsMenu.SetActive(true); authors.SetActive(false); break;
                    case "Авторы": optionsMenu.SetActive(false); authors.SetActive(true); break;
                    case "Выход": Application.Quit(); break;
                }
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

    private void ResizeUI()
    {
        float defScreenWidth = 1280;
        float defScreenHeight = 720;
        float defPosX;
        float defPosY;
        float defWidth;
        float defHeight;
        float defFontSize;
        float scale = Screen.width / defScreenWidth < Screen.height / defScreenHeight
            ? Screen.width / defScreenWidth
            : Screen.height / defScreenHeight;
        scale = Math.Min(1, scale);

        //заголовок
        defPosY = 100;
        defFontSize = 47;
        headerText.GetComponent<RectTransform>().position = new Vector3((float)Screen.width / 2, Screen.height - defPosY * scale);
        headerText.GetComponent<Text>().fontSize = Math.Min(61, (int)(scale * defFontSize));

        #region меню настроек
        defPosX = 640;
        defPosY = 200;
        optionsMenu.GetComponent<RectTransform>().position = new Vector3((float)Screen.width / 2, Screen.height - defPosY * scale);
        optionsMenu.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 2 - 100 * scale, Screen.height - defPosY * scale - 100 * scale);


        #region resolution
        defHeight = 40;
        defPosY = 20;
        GameObject resolution = optionsMenu.transform.Find("resolution").gameObject;
        resolution.GetComponent<RectTransform>().sizeDelta = new Vector2(0, defHeight * scale);
        resolution.GetComponent<RectTransform>().localPosition = new Vector3(optionsMenu.GetComponent<RectTransform>().rect.width/2, - defPosY * scale, 0);

        defFontSize = 21;
        resolution.transform.Find("resolutionText").GetComponent<Text>().fontSize = (int)(defFontSize * scale);

        defWidth = 180;
        GameObject resolutionDropdown = resolution.transform.Find("resolutionDropdown").gameObject;
        resolutionDropdown.GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth * scale, 0);

        defFontSize = 21;
        resolutionDropdown.transform.Find("Label").GetComponent<Text>().fontSize = (int)(defFontSize * scale);

        defWidth = 40;
        defHeight = 40;
        resolutionDropdown.transform.Find("Arrow").GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth*scale,defHeight*scale);
        resolutionDropdown.transform.Find("Arrow").GetComponent<RectTransform>().localPosition = new Vector3(-defWidth*scale/2, 0,0);
        GameObject item = resolutionDropdown.transform.Find("Template")
            .transform.Find("Viewport")
            .transform.Find("Content")
            .transform.Find("Item").gameObject;

        defWidth = 40;
        defHeight = 40;
        item.transform.Find("Item Checkmark").GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth * scale, defHeight * scale);
        item.transform.Find("Item Checkmark").GetComponent<RectTransform>().localPosition = new Vector3(defWidth*scale/2, 0, 0);

        defFontSize = 21;
        item.transform.Find("Item Label").GetComponent<Text>().fontSize = (int)(defFontSize * scale);
        item.transform.Find("Item Label").GetComponent<RectTransform>().localPosition = new Vector3(defWidth*scale,0,0);
        #endregion

        #region fullscreen
        defHeight = 40;
        defPosY = 60;
        GameObject fullscreen = optionsMenu.transform.Find("fullscreen").gameObject;
        fullscreen.GetComponent<RectTransform>().sizeDelta = new Vector2(0, defHeight * scale);
        fullscreen.GetComponent<RectTransform>().localPosition = new Vector3(optionsMenu.GetComponent<RectTransform>().rect.width / 2, - defPosY * scale, 0);

        defFontSize = 21;
        fullscreen.transform.Find("fullscreenText").GetComponent<Text>().fontSize = (int)(defFontSize * scale);

        defWidth = 180;
        fullscreen.transform.Find("fullscreenToggle").GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth*scale, 0);
        defWidth = 40;
        defHeight = 40;
        fullscreen.transform.Find("fullscreenToggle").transform.Find("Background").GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth * scale, defHeight*scale);
        defWidth = 40;
        defHeight = 40;
        fullscreen.transform.Find("fullscreenToggle").transform.Find("Background").transform.Find("Checkmark").GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth * scale, defHeight * scale);
        #endregion

        //accept button
        defWidth = 160;
        defHeight = 40;
        optionsMenu.transform.Find("acceptButton").GetComponent<RectTransform>().localPosition = new Vector3(optionsMenu.transform.GetComponent<RectTransform>().rect.width/2, - optionsMenu.transform.GetComponent<RectTransform>().rect.height + 20);
        optionsMenu.transform.Find("acceptButton").GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth * scale, defHeight * scale);
        defFontSize = 21;
        optionsMenu.transform.Find("acceptButton").transform.Find("Text").GetComponent<Text>().fontSize = (int)(defFontSize * scale);
        #endregion

        #region авторы

        defPosX = 640;
        defPosY = 200;
        authors.GetComponent<RectTransform>().position = new Vector3((float)Screen.width / 2, Screen.height - defPosY * scale);
        authors.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/2-100*scale, Screen.height-defPosY*scale-100*scale);

        defFontSize = 21;
        authors.transform.Find("ScrollRect").Find("Text").GetComponent<Text>().fontSize = (int)(defFontSize*scale);

        #endregion

    }

}
