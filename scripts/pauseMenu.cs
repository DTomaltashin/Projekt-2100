using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class pauseMenu : MonoBehaviour
{
    static bool gameisPaused = false;
    public GameObject pauseUI;
    public GameObject INgameUI;
    void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard == null)
            return;
        if(keyboard.escapeKey.wasPressedThisFrame)
        {
            if(gameisPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        pauseUI.SetActive(true);
        INgameUI.SetActive(false);
        Time.timeScale = 0f;
        gameisPaused = true;
    }
    void Resume()
    {
        pauseUI.SetActive(false);
        INgameUI.SetActive(true);
        Time.timeScale = 1f;
        gameisPaused = false;
    }
}
