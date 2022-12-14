using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Upgrade
{
    float AddedfractionHealth = 0.2f;
    Health healthcomp;
    private void Start()
    {
        GameObject Player = GameObject.Find("Player");
        healthcomp = Player.GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Upgrade();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Sr.enabled == true)
        {
            Sr.color = new Color(Sr.color.r, Sr.color.g, Sr.color.b, 1f);
        }
    }

    void Upgrade()
    {
        if (healthcomp.Heal(AddedfractionHealth))
        {
            Destroy(gameObject);
        }
        else
        {
            Sr.color = new Color(Sr.color.r, Sr.color.g, Sr.color.b, 0.4f);
        }
    }
}
