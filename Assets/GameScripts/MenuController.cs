﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.GameScripts.MainPerson;

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

    // Use this for initialization
    void Start ()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
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

    private void ControlKeys()
    {
        if(Input.GetKeyUp("escape"))
        {
            if (mainMenu.activeInHierarchy)
            {
                mainMenu.SetActive(false);
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
