using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.GameScripts.MainPerson;
using UnityStandardAssets.ImageEffects;
using System;

public class MenuController : MonoBehaviour {

    private GameObject mainMenu;
    private GameObject newGameMenu;
    private GameObject optionsMenu;
    private GameObject exitMenu;
    private GameObject controlsMenu;
    private GameObject headerText;
    private Dropdown resolutionsDropdown;
    private bool dropdownIsSet = false;
    private Toggle fullscreenToggle;
    public GameObject playerGui;

    public GameObject camera;

    public PonyController ponyController = new PonyController();

    private Blur blur;

    private bool quitAllowed = false;

    //main menu

    // Use this for initialization
    void Start ()
    {
        mainMenu = transform.Find("MainMenu").gameObject;
        newGameMenu = transform.Find("NewGameMenu").gameObject;
        optionsMenu = transform.Find("OptionsMenu").gameObject;
        exitMenu = transform.Find("ExitMenu").gameObject;
        controlsMenu = transform.Find("ControlsMenu").gameObject;
        resolutionsDropdown = optionsMenu.transform.Find("Options group").transform.Find("resolution").transform.Find("resolutionDropdown").GetComponent<Dropdown>();
        fullscreenToggle = optionsMenu.transform.Find("Options group").transform.Find("fullscreen").transform.Find("fullscreenToggle").GetComponent<Toggle>();
        headerText = transform.Find("headerText").gameObject;
        fullscreenToggle.isOn = Screen.fullScreen;
        blur = camera.GetComponent<Blur>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        ControlKeys();
        if (!dropdownIsSet)
        {
            setResolutionsDropdown();
        }
	}

    void Update ()
    {
        ResizeUI();
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
        scale = Math.Min(1,scale);//антиразмылин


        //заголовок
        defPosY = 55;
        defFontSize = 47;
        headerText.GetComponent<RectTransform>().position = new Vector3((float)Screen.width / 2, Screen.height - defPosY * scale);
        headerText.GetComponent<Text>().fontSize = Math.Min(61, (int)(scale * defFontSize));

        #region Главное меню

        if (mainMenu.activeSelf)
        {
            //кнопка "продолжить"
            defPosX = 30;
            defPosY = 130;
            defWidth = 600;
            defHeight = 285;
            GameObject continueButton = mainMenu.transform.Find("continueButton").gameObject;
            continueButton.GetComponent<RectTransform>().position = new Vector3(defPosX * scale + (float)(Screen.width - defScreenWidth * scale) / 2, Screen.height - defPosY * scale);
            continueButton.GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth * scale, defHeight * scale);

            //текст кнопки "продолжить"
            defFontSize = 21;
            defPosX = 25;
            defPosY = -140;
            GameObject continueText = mainMenu.transform.Find("continueButton").transform.Find("Text").gameObject;
            continueText.GetComponent<RectTransform>().position = continueButton.GetComponent<RectTransform>().position + new Vector3(scale * defPosX, scale * defPosY, 0);
            continueText.GetComponent<Text>().fontSize = (int)(scale * defFontSize);
            continueText.transform.Find("Image").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(continueText.transform.Find("Image").gameObject.GetComponent<RectTransform>().sizeDelta.x, scale * 1);

            //текст задания
            defFontSize = 21;
            defWidth = 550;
            defHeight = 90;
            GameObject scroller = continueButton.transform.Find("Scroller").gameObject;
            scroller.GetComponent<RectTransform>().position = continueText.GetComponent<RectTransform>().position + new Vector3(0, -continueText.GetComponent<RectTransform>().sizeDelta.y, 0);
            scroller.GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth * scale, defHeight * scale);
            scroller.transform.Find("ScrollRect").transform.Find("taskText").gameObject.GetComponent<Text>().fontSize = (int)(scale * defFontSize);

            //кнопка "выход"
            defPosX = 950;
            defPosY = 445;
            defWidth = 300;
            defHeight = 230;
            GameObject exitButton = mainMenu.transform.Find("exitButton").gameObject;
            exitButton.GetComponent<RectTransform>().position = new Vector3(defPosX * scale + (float)(Screen.width - defScreenWidth * scale) / 2, Screen.height - defPosY * scale);
            exitButton.GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth * scale, defHeight * scale);

            //кнопка "сохранить"
            defPosX = 25;
            defPosY = 445;
            defWidth = 610;
            defHeight = 230;
            GameObject saveButton = mainMenu.transform.Find("saveButton").gameObject;
            saveButton.GetComponent<RectTransform>().position = new Vector3(defPosX * scale + (float)(Screen.width - defScreenWidth * scale) / 2, Screen.height - defPosY * scale);
            saveButton.GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth * scale, defHeight * scale);

            //кнопка "настройки"
            defPosX = 630;
            defPosY = 445;
            defWidth = 320;
            defHeight = 230;
            GameObject optionsButton = mainMenu.transform.Find("optionsButton").gameObject;
            optionsButton.GetComponent<RectTransform>().position = new Vector3(defPosX * scale + (float)(Screen.width - defScreenWidth * scale) / 2, Screen.height - defPosY * scale);
            optionsButton.GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth * scale, defHeight * scale);

            //кнопка "загрузить"
            defPosX = 625;
            defPosY = 180;
            defWidth = 625;
            defHeight = 240;
            GameObject loadButton = mainMenu.transform.Find("loadButton").gameObject;
            loadButton.GetComponent<RectTransform>().position = new Vector3(defPosX * scale + (float)(Screen.width - defScreenWidth * scale) / 2, Screen.height - defPosY * scale);
            loadButton.GetComponent<RectTransform>().sizeDelta = new Vector2(defWidth * scale, defHeight * scale);
        }
        #endregion

    }

    private void ControlKeys()
    {
        if(Input.GetKeyUp("escape"))
        {
            if (mainMenu.activeInHierarchy)
            {
                mainMenu.SetActive(false);
                headerText.SetActive(false);
                blur.enabled = false;
                if( playerGui != null) playerGui.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (ponyController != null)
                {
                    ponyController.menuIsActive = false;
                }
            }
            else
            {
                if (optionsMenu.activeInHierarchy)
                {
                    optionsMenu.SetActive(false);
                }
                else if (newGameMenu.activeInHierarchy)
                {
                    newGameMenu.SetActive(false);
                }
                else if (exitMenu.activeInHierarchy)
                {
                    exitMenu.SetActive(false);
                }
                else if (controlsMenu.activeInHierarchy)
                {
                    controlsMenu.SetActive(false);
                }
                mainMenu.SetActive(true);
                headerText.SetActive(true);
                blur.enabled = true;
                if (playerGui != null) playerGui.SetActive(false);
                Cursor.lockState=CursorLockMode.Confined;
                Cursor.visible = true;
                if (ponyController != null)
                    ponyController.menuIsActive = true;
            }
        }
    }

    public void CloseMenu()
    {
        mainMenu.SetActive(false);
        headerText.SetActive(false);
        blur.enabled = false;
        if (playerGui != null) playerGui.SetActive(true);
        newGameMenu.SetActive(false);
        optionsMenu.SetActive(false);
        exitMenu.SetActive(false);
        controlsMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (ponyController != null)
            ponyController.menuIsActive = false;
    }

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        newGameMenu.SetActive(false);
        optionsMenu.SetActive(false);
        exitMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void OpenNewGameMenu()
    {
        mainMenu.SetActive(false);
        newGameMenu.SetActive(true);
        optionsMenu.SetActive(false);
        exitMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void OpenOptionsMenu()
    {
        mainMenu.SetActive(false);
        newGameMenu.SetActive(false);
        optionsMenu.SetActive(true);
        exitMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void OpenExitMenu()
    {
        mainMenu.SetActive(false);
        newGameMenu.SetActive(false);
        optionsMenu.SetActive(false);
        exitMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }

    public void OpenControlsMenu()
    {
        mainMenu.SetActive(false);
        newGameMenu.SetActive(false);
        optionsMenu.SetActive(false);
        exitMenu.SetActive(false);
        controlsMenu.SetActive(true);
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

    public void NewGame()
    {
        SceneManager.LoadScene("World");//впоследствии надо заменить на загрузку новой игры
    }

    public void Exit()
    {
        quitAllowed = true;
        SceneManager.LoadScene("MainMenu");
    }

    void OnApplicationQuit()
    {
        if (!quitAllowed)
        {
            Application.CancelQuit();
            blur.enabled = true;
            ponyController.menuIsActive = true;
            playerGui.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            OpenExitMenu();
        }
    }
}
