using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mamont : Enemy
{

    private bool _mamontIsDead;
    private float _sphereRadius = 15;
    private float timeLeftWander = 0f;
    [SerializeField] private GameObject mamont;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float wanderDistance = 10f;
    [SerializeField] private float timeWander = 10f;
    public NavMeshAgent pathfinder;

    private void Update()
    {
        timeLeftWander -= Time.deltaTime;
        if (timeLeftWander < 0)
        {
            MamontJerk();
            pathfinder.SetDestination(RandomNavSphere(transform.position, wanderDistance));
            timeLeftWander = timeWander;
        }
    }

    public void MamontJerk()
    {
        Collider[] hitColiders = Physics.OverlapSphere(mamont.transform.position, _sphereRadius, playerLayer);
        foreach (var item in hitColiders)
        {
            if (item != null)
            {
            Debug.Log("1");
                mamont.transform.LookAt(hitColiders[0].transform.position);
                mamont.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -10);
            }
        }


    }

    public Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        
        
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);
        return navHit.position;
    }
}
