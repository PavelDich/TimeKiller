using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BUM : MonoBehaviour
{
        [SerializeField] private float power = 50;
        [SerializeField] private float radius = 5;
    private void OnCollisionEnter(Collision collision)
    {
        ResetIsBOOM();
        Debug.Log("babax");
        this.gameObject.GetComponent<Rigidbody>().mass = power;
        Collider[] collidersOnSphere = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < collidersOnSphere.Length; i++)
        {
            Rigidbody rigidbody = collidersOnSphere[i].attachedRigidbody;
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(power, transform.position, radius);
            }
        }
    }

    private IEnumerator ResetIsBOOM()
    {
        yield return new WaitForSecondsRealtime(4f);
        
    }
}
