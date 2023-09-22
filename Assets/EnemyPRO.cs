using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class EnemyPRO : MonoBehaviour
{
    public NavMeshAgent pathfinder;
    private Transform _transform;

    [SerializeField] private float timeWander = 10f;
    private float timeLeftWander = 10f;
    [SerializeField] private float wanderDistance = 10f;

    [SerializeField] private LayerMask layerPlayer;
    private bool playerDetect;
    [SerializeField] private float Damage;
    [SerializeField] private float timePlayerReturnRegen;


    private void Start()
    {
        _transform = gameObject.transform;

        timeLeftWander = timeWander;
    }
    private void Update()
    {
        timeLeftWander -= Time.deltaTime;

        if (timeLeftWander <= 0f && !playerDetect)
        {
            pathfinder.SetDestination(RandomNavSphere(transform.position, wanderDistance));
            timeLeftWander = timeWander;
        }
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float distance)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);
        return navHit.position;
    }

    public void Target(Transform targ)
    {
        if (targ != null && !playerDetect)
            pathfinder.SetDestination(targ.position);

        timeLeftWander = timeWander;
    }



    private void OnTriggerStay(Collider col)
    {
        if ((~layerPlayer.value & (1 << col.gameObject.layer)) == 0)
        {
            pathfinder.SetDestination(col.transform.position);
            playerDetect = true;
            timeLeftWander = timeWander;

            Player pl = col.GetComponent<Player>();
            //pl.Health -= Damage;
            //pl.isRegenerable = false;
            //if (pl.Health < 0)
            //{
            //    pl.controller.active = false;
            //}
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if ((~layerPlayer.value & (1 << col.gameObject.layer)) == 0)
        {
            playerDetect = false;
            timeLeftWander = timeWander;
            StartCoroutine(PlayerRegen(col.GetComponent<Player>()));
        }
    }

    private IEnumerator PlayerRegen(Player player)
    {
        yield return new WaitForSeconds(timePlayerReturnRegen);
        //player.isRegenerable = true;

        yield return null;
    }
}