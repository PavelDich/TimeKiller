using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NetworkMan : NetworkManager
{
    public bool playerSpawned;



    public void OnCreateCharacter(NetworkConnectionToClient conn, PosMessage message)
    {
        GameObject go = Instantiate(playerPrefab, message.vector3, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, go);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<PosMessage>(OnCreateCharacter);
    }

    public void ActivatePlayerSpawn()
    {
        PosMessage m = new PosMessage() { vector3 = Vector3.up };
        NetworkClient.Send(m);
        playerSpawned = true;
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
    }



    public void Create(int SceneID)
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            StartHost();
            LoadScene(SceneID);
        }
    }

    public void Connect(int SceneID, string IpAdress)
    {
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            networkAddress = IpAdress;
            StartClient();
            if (isNetworkActive) SceneManager.LoadScene(SceneID);
        }
    }

    public void Disconnect()
    {
        NetworkClient.Disconnect();
    }

    private void LoadScene(int Scane)
    {
        SceneManager.LoadScene(Scane);
    }

    public struct PosMessage : NetworkMessage
    {
        public Vector3 vector3;
    }
}
