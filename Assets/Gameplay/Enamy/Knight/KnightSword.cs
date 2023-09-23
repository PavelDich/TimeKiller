using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSword : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Knight Damage");
    }
}
