using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceScript : MonoBehaviour
{
    public AudioSource s;
    public GameObject text;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            //s.enabled = true;
            text.SetActive(true);
            Destroy(text, 5f);
        }

        Destroy(gameObject, 5f);
    }
}
