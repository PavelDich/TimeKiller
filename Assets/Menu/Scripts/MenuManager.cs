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

    public void Disconnect(int ScaneID)
    {
        Manager.networkMan.Disconnect();
        Manager.networkMan.LoadScene(ScaneID);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
