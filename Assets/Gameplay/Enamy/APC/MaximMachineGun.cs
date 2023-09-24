using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaximMachineGun : MonoBehaviour
{

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject cube;
    [SerializeField] Transform spawnBullet;
    [SerializeField] private float speedBullet;
    private float _amountShoot = 10;
    private float reloadMachineGun = 15f;
    private bool _isShooting = false;

    private void FixedUpdate()
    {
        spawnBullet.LookAt(cube.transform.position);
        reloadMachineGun -= Time.deltaTime;
        if (reloadMachineGun <= 0)
        {
            for (int i = 0; i < _amountShoot; i++)
            {
                StartCoroutine(ShootWithDelay());
            }
            reloadMachineGun = 15f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Shoot();
        }
    }
    private IEnumerator ShootWithDelay()
    {
        for (int i = 0; i < _amountShoot; i++)
        {
            Shoot();
            yield return new WaitForSeconds(0.2f); 
        }
    }
    public void Shoot()
    {
        if (_isShooting == true) return;
        _isShooting = true;
        GameObject bulletObject = Instantiate(bullet, spawnBullet.position, spawnBullet.rotation);
        Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
        rb.AddForce(spawnBullet.forward * speedBullet, ForceMode.Impulse);
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
