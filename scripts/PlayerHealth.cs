using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    float hp = 100f;
    public Image Health;
    public GameObject DefeatCanvas;

    private void Start()
    {
        Health.fillAmount = 100f;
    }

    public void TakeDamage(int damageAmount)
    {
        Debug.Log(damageAmount);
        hp -= damageAmount;
        Debug.Log(hp);
        Health.fillAmount = hp/100f;
        if (hp == 0)
        {
            Invoke("Restart",1.5f);
            DefeatCanvas.SetActive(true);
        }
        else
        {
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Equals("EnemyHitbox"))
        {
            Debug.Log("hit");
            TakeDamage(20);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
