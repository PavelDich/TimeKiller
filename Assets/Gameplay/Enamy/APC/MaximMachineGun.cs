using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaximMachineGun : MonoBehaviour
{

    [SerializeField] private GameObject bullet;
    [SerializeField] Transform spawnBullet;
    [SerializeField] private float speedBullet;
    private bool _isShooting = false;
    private float _resetIshoting = 10;

    public void Shoot()
    {
        if (_isShooting == true) return;
        _isShooting = true;
        GameObject bulletObject = Instantiate(bullet, spawnBullet.position, spawnBullet.rotation);
        Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
        rb.AddForce(-spawnBullet.forward * speedBullet, ForceMode.Impulse);
        _resetIshoting = 5;
        StartCoroutine(DestroyBullet(bulletObject));
        StartCoroutine(ResetIsShooting());
    }

    private IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(5f);
        Destroy(bullet);
    }

    private IEnumerator ResetIsShooting()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        _isShooting = false;
    }


}
