using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : Enemy
{
    private float _resetIshoting = 10;
    private bool isAgr;
    private bool _isShooting = false;
    [SerializeField] private GameObject monkey;
    [SerializeField] private GameObject bullet;
    [SerializeField] Transform spawnBullet;
    [SerializeField] private float speedBullet;

    private void Update()
    {
        _timeLeftWander -= Time.deltaTime;
        if (_timeLeftWander < 0 && !isAgr)
        {
            pathfinder.SetDestination(RandomNavSphere(transform.position, wanderDistance));
            _timeLeftWander = timeWander;
        }
        if (_resetIshoting > 0 && _resetIshoting - Time.deltaTime >= 0)
        {
            _resetIshoting -= Time.deltaTime;
        }
        else
        {
            _resetIshoting = 0;
        }

    }

    private void OnTriggerEnter(Collider col)
    {
        if ((~playerLayer & (1 << col.gameObject.layer)) == 0)
        {
            isAgr = true;
            monkey.transform.LookAt(col.transform);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if ((~playerLayer & (1 << col.gameObject.layer)) == 0)
        {
            monkey.transform.LookAt(col.transform);
            if (_resetIshoting == 0)
            {
                Shoot();
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if ((~playerLayer & (1 << col.gameObject.layer)) == 0)
        {
            isAgr = false;
        }
    }

    public void Shoot()
    {
        Debug.Log("shot");
        if (_isShooting == true) return;
        _isShooting = true;
        GameObject bulletObject = Instantiate(bullet, spawnBullet.position, spawnBullet.rotation);
        Rigidbody rb = bulletObject.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speedBullet, ForceMode.Impulse);
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
