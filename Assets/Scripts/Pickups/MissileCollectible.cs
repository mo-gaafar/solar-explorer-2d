using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollectible : GunCollectible
{
    public GameObject MissilePrefab;
    private void Start()
    {
        CollectibleSpawnOffset = new Vector2(3, 3);
        SearchRadius = 5;
        GameObject Player = GameObject.Find("Player");
        gunController = Player.GetComponent<GunController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.enabled = false;
        Collider.enabled = false;
        TheGun = Instantiate(gun, transform.position, transform.rotation);

        //Set Up the projectile
        MissilePrefab = Instantiate(MissilePrefab);
        ((ProjectileGun)TheGun).SetProjectile(MissilePrefab);

        gunController.PickupGunCollectible(this);
    }



}
