using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SyncVar]
    public bool isGameStarted = false;
    public void ChangeIsGameStarted(bool newValue)
    {
        if (isServer)
            SyncIsGameStarted(newValue);
        else
            CmdSyncIsGameStarted(newValue);

        [Server]
        void SyncIsGameStarted(bool newValue)
        { isGameStarted = newValue; }
        [Command]
        void CmdSyncIsGameStarted(bool newValue)
        { isGameStarted = newValue; }
    }

    public void Awake()
    {
        Manager.gameManager = this;
    }
}
