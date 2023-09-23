using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barsik : Enemy
{
    private bool isAgr;
    [SerializeField] private float radius;
    [SerializeField] private float power;
    private void Update()
    {
        _timeLeftWander -= Time.deltaTime;
        if (_timeLeftWander < 0 && !isAgr)
        {
            pathfinder.SetDestination(RandomNavSphere(transform.position, wanderDistance));
            _timeLeftWander = timeWander;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if ((~playerLayer & (1 << col.gameObject.layer)) == 0)
        {
            transform.LookAt(col.transform.position);
            isAgr = true;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if ((~playerLayer & (1 << col.gameObject.layer)) == 0)
        {
            pathfinder.SetDestination(col.transform.position);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if ((~playerLayer & (1 << col.gameObject.layer)) == 0)
        {
            isAgr = false;
        }
    }

    public void ExplosionPunch()
    {
        Collider[] collidersOnSphere = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < collidersOnSphere.Length; i++)
        {
            Rigidbody rigidbody = collidersOnSphere[i].attachedRigidbody;
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(power, transform.position, radius);
            }
        }

    }


}
