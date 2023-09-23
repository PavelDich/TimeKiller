using UnityEngine;
using Mirror;

public class SpawnEnemys : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public void Start()
    {
        Spawn();
    }

    [Server]
    public void Spawn()
    {
        GameObject go = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(go);
    }
}
