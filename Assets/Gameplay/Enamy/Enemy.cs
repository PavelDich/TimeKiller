using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NetworkBehaviour
{
    public NavMeshAgent pathfinder;
    public LayerMask playerLayer;
    public float wanderDistance = 10f;
    public float timeWander = 10f;
    public float _timeLeftWander = 0f;

    [SyncVar(hook = nameof(SetHealth))]
    public float _Health = 100f;
    public float Health = 100f;

    public void ChangeHealth(float newValue)
    {
        if (isServer)
            SyncHealth(newValue);
        else
            CmdSyncHealth(newValue);

        [Server]
        void SyncHealth(float newValue)
        { _Health = newValue; }
        [Command]
        void CmdSyncHealth(float newValue)
        { _Health = newValue; }
    }
    public void SetHealth(float oldValue, float newValue) { Health = newValue; }

    public Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);
        return navHit.position;
    }
}
