using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public void Awake()
    {
        Manager.gameManager = this;
    }

    [SyncVar(hook = nameof(SetIsGameStarted))]
    public bool _isGameStarted = false;
    public bool isGameStarted = false;
    public void ChangeIsGameStarted(bool newValue)
    {
        if (isServer)
            SyncisGameStarted(newValue);
        else
            CmdSyncIsGameStarted(newValue);

        [Server]
        void SyncisGameStarted(bool newValue)
        { _isGameStarted = newValue; }
        [Command]
        void CmdSyncIsGameStarted(bool newValue)
        { _isGameStarted = newValue; }
    }
    public void SetIsGameStarted(bool oldValue, bool newValue) { isGameStarted = newValue; }
}

