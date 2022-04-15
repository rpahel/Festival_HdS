using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public AudioMixer audioMixer;
    public PauseMenu pauseMenu;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume); 
    }

    public void HideMenu()
    {
        pauseMenu.settingsMenuIsOpen = false;
        settingsMenu.SetActive(false);
    }
}
