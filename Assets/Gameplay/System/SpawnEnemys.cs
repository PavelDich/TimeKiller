using UnityEngine;
using Mirror;

public class SpawnEnemys : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public void Start()
    {
<<<<<<< HEAD
        //GameObject go = Instantiate(enemyPrefab, transform)
        //NetworkServer.Spawn();
=======
        Spawn();
    }

    [Server]
    public void Spawn()
    {
        GameObject go = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(go);
>>>>>>> 8970cd61c09a49d9e3edf86de7d01ff4b3de0108
    }
}
