using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{

    [SerializeField] private GameObject bullet;
    [SerializeField] Transform[] spawnBullet = new Transform[6];
    [SerializeField] private float speedBullet;
    private int counter = 0;
    private bool _isShooting = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }
    }
    public void Shoot()
    {
        if (_isShooting == true) return;
        _isShooting = true;
        GameObject bulletObject = Instantiate(bullet, spawnBullet[counter].position, spawnBullet[counter].rotation);
        Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
        rb.AddForce(spawnBullet[counter].forward * speedBullet, ForceMode.Impulse);
        StartCoroutine(DestroyBullet(bulletObject));
        StartCoroutine(ResetIsShooting());
        counter++;
        if (counter == 6)
        {
            counter = 0;
        }
    }

    private IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(5f);
        Destroy(bullet);
    }

    private IEnumerator ResetIsShooting()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        _isShooting = false;
    }
}
