using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poop : MonoBehaviour
{
    public float Damage;
    public LayerMask playerLayer;
    private void OnTriggerEnter(Collider col)
    {
        if ((~playerLayer & (1 << col.gameObject.layer)) == 0)
        {
            Player pl = col.GetComponent<Player>();
            //pl.parameters.health -= Damage;
        }
    }
}
