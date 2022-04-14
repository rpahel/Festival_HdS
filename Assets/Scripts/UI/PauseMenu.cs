using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    bool menuIsOpen = false;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            menuIsOpen = !menuIsOpen;
            pauseMenu.SetActive(menuIsOpen);
        }

    }
    public void Options()
    {
        settingsMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
