using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volume;
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Start()
    {
        // at the start of the game we take whole the resolutions our system support
        // and storage them in array.
        resolutions = Screen.resolutions;

        // we clear the last resolutions loaded in order to avoid bugs
        resolutionDropdown.ClearOptions();

        // to feed our dropdown we need to create a list with every resolution as a string
        List<string> options = new List<string>();

        // we'll feed our dropdown with this value who is going to store our screen resolution
        int currentResolutionValue = 0;

        // we loop the array of resolution to transform every resolution as an option string
        for(int resolution = 0; resolution < resolutions.Length; resolution++)
        {
            // feed the string list at every iteration
            options.Add(resolutions[resolution].width + "x" + resolutions[resolution].height);

            // we check here if the current resolution taken from the resolution array at the loop matches
            // with our screen resolution, we have to check the width and height
            if (resolutions[resolution].width == Screen.currentResolution.width &&
                resolutions[resolution].height == Screen.currentResolution.height)
                currentResolutionValue = resolution; // we store the array index as the resultion option
        }

        // we add the options to the dropdown
        resolutionDropdown.AddOptions(options);

        // fixed the resolution to our screen resolution
        resolutionDropdown.value = currentResolutionValue;

        // and apply the resolution to the screen
        resolutionDropdown.RefreshShownValue();

        
    }

    void Update()
    {
        volume.value = PlayerPrefs.GetFloat("Volume");
        
        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("Volume"));
    }

    // this method will update the resolution we select on the dropdown
    // we have to set this method in our dropdown object.
    public void SetResolution(int resolutionIndex)
    {
        // we take the resolution selected.
        Resolution resolution = resolutions[resolutionIndex];

        // and apply it on the screen.
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
