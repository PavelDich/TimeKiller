using UnityEngine;
using Mirror;

public class SpawnEnemys : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public void Start()
    {
        //GameObject go = Instantiate(enemyPrefab, transform)
        //NetworkServer.Spawn();
    }
}
