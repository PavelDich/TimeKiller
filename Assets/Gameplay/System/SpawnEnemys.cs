using UnityEngine;
using Mirror;

public class SpawnEnemys : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public void Start()
    {
        NetworkServer.Spawn(Instantiate(enemyPrefab, transform));
    }
}
