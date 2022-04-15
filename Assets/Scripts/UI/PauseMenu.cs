using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    bool pauseMenuIsOpen = false;
    public bool settingsMenuIsOpen = false;
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuIsOpen = !pauseMenuIsOpen;

            if(!settingsMenuIsOpen)
                pauseMenu.SetActive(pauseMenuIsOpen);

            FindObjectOfType<EvaManager>().Pause();
        }

    }
    public void Options()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        settingsMenuIsOpen = true;
    }
    public void CloseOptions()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        settingsMenuIsOpen = false;
    }

    public void SetVolume()
    {
        audioMixer.SetFloat("Volume", volumeSlider.value);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
