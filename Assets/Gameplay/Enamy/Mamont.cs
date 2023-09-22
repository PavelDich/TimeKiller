using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mamont : Enemy
{

    private bool _mamontIsDead;
    private float _sphereRadius = 15;
    [SerializeField] private GameObject mamont;
    [SerializeField] private LayerMask playerLayer;
    
    public void MamontJerk()
    {
        Collider[] hitColiders = Physics.OverlapSphere(mamont.transform.position, _sphereRadius, playerLayer);
        RaycastHit hit;
        Physics.Raycast(hitColiders[0].transform.position, hitColiders[0].transform.forward, out hit);
        if (_mamontIsDead == true)
        {
            return;
        }
        foreach (var item in hitColiders)
        {
            if (item != null)
            {
                mamont.transform.LookAt(hitColiders[0].transform.position);
                mamont.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 10);
            }
        }


    }




}
