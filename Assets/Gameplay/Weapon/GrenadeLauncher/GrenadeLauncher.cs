using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    
    [SerializeField] private GameObject bullet;
    [SerializeField] Transform spawnBullet;
    [SerializeField] private float speedBullet;
    private bool _isShooting = false;

    private void Update()
    {
        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    Shoot();
        //}
    }
    public void Shoot()
    {
        if (_isShooting == true) return;
        _isShooting = true;
        GameObject bulletObject = Instantiate(bullet, spawnBullet.position, spawnBullet.rotation);
        Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
        rb.AddForce(-transform.forward * speedBullet, ForceMode.Impulse);
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
        yield return new WaitForSecondsRealtime(1f);
        _isShooting = false;
    }

}
