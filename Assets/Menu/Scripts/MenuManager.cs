using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void Create(int ScaneId)
    {
        Manager.networkMan.Create(ScaneId);
    }

    public void Connect(int ScaneId)
    {
        Manager.networkMan.Connect(ScaneId, PlayerPrefs.GetString("ConnectIP"));
    }

    public void Disconnect()
    {
        Manager.networkMan.Disconnect();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
