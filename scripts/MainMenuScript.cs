using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MMCanvas;
    public GameObject INGAMEcanvas;
    public GameObject Missioncanvas;
    public GameObject PauseCanvas;

    public GameObject MMcamera;
    public GameObject INGAMEcamera;

    public AudioClip ingameAudio;
    public AudioSource audioSource;

    public GameObject player;
    public GameObject AudioManager;

    private void Start()
    {
        player.GetComponent<ThirdPersonController>().enabled = false;
        player.GetComponent<ThirdPersonAttackController>().enabled = false;
        //AudioManager.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey("k"))
        {
            player.GetComponent<ThirdPersonController>().enabled = true;
            player.GetComponent<ThirdPersonAttackController>().enabled = true;

            MMCanvas.SetActive(false);
            PauseCanvas.SetActive(true);
            INGAMEcanvas.SetActive(true);

            MMcamera.SetActive(false);
            INGAMEcamera.SetActive(true);

            audioSource.clip = ingameAudio;
            audioSource.Play();
            Invoke("ActivateEnemyAudio", 5f);
        }

        if (Input.GetKeyDown("i"))
        {
            Missioncanvas.SetActive(true);
        }
        else if (Input.GetKeyUp("i"))
        {
            Missioncanvas.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Escape) && Input.GetKey(KeyCode.E))
        {
            Application.Quit();
        }
    }

    void ActivateEnemyAudio()
    {
        //AudioManager.SetActive(true);
    }
}