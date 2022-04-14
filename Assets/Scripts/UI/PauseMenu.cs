using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    bool pauseMenuIsOpen = false;
    public bool settingsMenuIsOpen = false;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            pauseMenuIsOpen = !pauseMenuIsOpen;

            if(!settingsMenuIsOpen)
                pauseMenu.SetActive(pauseMenuIsOpen);
        }

    }
    public void Options()
    {
        settingsMenu.SetActive(true);
        settingsMenuIsOpen = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
