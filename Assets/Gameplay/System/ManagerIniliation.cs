using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerIniliation : MonoBehaviour
{
    public NetworkMan networkMan;

    private void Awake()
    {
        Manager.networkMan = networkMan;
    }
}
