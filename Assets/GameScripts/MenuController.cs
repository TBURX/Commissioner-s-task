using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.GameScripts.MainPerson;
using UnityStandardAssets.ImageEffects;
using System;

public class MenuController : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject newGameMenu;
    public GameObject optionsMenu;
    public GameObject exitMenu;
    public GameObject controlsMenu;
    public Dropdown resolutionsDropdown;
    private bool dropdownIsSet = false;
    public Toggle fullscreenToggle;
    public GameObject playerGui;

    public GameObject camera;

    public PonyController ponyController = new PonyController();

    private Blur blur;

    //main menu

    // Use this for initialization
    void Start ()
    {
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

        //заголовок главного меню
        defPosY = 55;
        float defFontSize = 47;
        GameObject header = mainMenu.transform.Find("mainMenuHeaderText").gameObject;
        header.GetComponent<RectTransform>().position = new Vector3((float)Screen.width / 2, Screen.height - defPosY / defScreenHeight * Screen.height);
        header.GetComponent<Text>().fontSize = (int)(defFontSize / defScreenHeight * Screen.height);



        //кнопка "продолжить"
        defPosX = 30;
        defPosY = 130;
        defWidth = 600;
        defHeight = 290;
        RectTransform continueButton = mainMenu.transform.Find("continueButton").gameObject.GetComponent<RectTransform>();
        continueButton.position = new Vector3(defPosX/defScreenWidth*Screen.width, Screen.height-defPosY/defScreenHeight*Screen.height);
        continueButton.sizeDelta = new Vector2(defWidth/defScreenWidth*Screen.width,defHeight/defScreenHeight*Screen.height);

        //кнопка "выход"
        defPosX = 950;
        defPosY = 445;
        defWidth = 300;
        defHeight = 215;
        RectTransform exitButton = mainMenu.transform.Find("exitButton").gameObject.GetComponent<RectTransform>();
        exitButton.position = new Vector3(defPosX / defScreenWidth * Screen.width, Screen.height - defPosY / defScreenHeight * Screen.height);
        exitButton.sizeDelta = new Vector2(defWidth / defScreenWidth * Screen.width, defHeight / defScreenHeight * Screen.height);
    }

    private void ControlKeys()
    {
        if(Input.GetKeyUp("escape"))
        {
            if (mainMenu.activeInHierarchy)
            {
                mainMenu.SetActive(false);
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
        Application.Quit();
    }
}
