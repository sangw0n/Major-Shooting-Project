using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject[] bulletPrefab;
    [SerializeField] private Transform bulletShootPos;

    [SerializeField] private float shootTime;
    [SerializeField] private float shootCurTime;

    private void Update()
    {
        Shoot()
;
    }

    private void Shoot()
    {
        if (shootCurTime >= shootTime && Input.GetKey(KeyCode.C))
        {
            shootCurTime = 0;
            int randomIndex = Random.Range(0, bulletPrefab.Length);
            Instantiate(bulletPrefab[randomIndex], bulletShootPos.position, Quaternion.identity);
        }
        else if(shootCurTime < shootTime)
            shootCurTime += Time.deltaTime;
    }

    
}
