using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class MeeleweaponScript : MonoBehaviour
{
    public GameObject hitbox;
    bool attacking= false;

    void Update()
    {
        var mouse = Mouse.current;

        if (mouse == null)
            return;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            attacking = !attacking;
        }

        if (attacking)
        {
            hitbox.SetActive(true);
            Invoke("hitboxactive", .5f);
        }
        else
        {
            hitbox.SetActive(false);
        }
    }

    void hitboxactive()
    {
        attacking = false;
    }
}
