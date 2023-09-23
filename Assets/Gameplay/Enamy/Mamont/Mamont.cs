using System.Security.Principal;
using Mirror;
using UnityEngine;

public class Mamont : Enemy
{
    private bool isAgr;

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
            pathfinder.speed = 50f;
            pathfinder.SetDestination(col.transform.position);
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

   
}
