using Mirror;
using UnityEngine;

public class SencVargdhdh : NetworkBehaviour
{
    public LayerMask playerMask;

    [SyncVar(hook = nameof(SetHealth))]
    public float _Health = 100f;
    public float Health = 100f;
    public void ChangeHealth(float newValue)
    {
        if (isServer)
            SyncHealth(newValue);
        else
            CmdSyncHealth(newValue);

        [Server]
        void SyncHealth(float newValue)
        { _Health = newValue; }
        [Command]
        void CmdSyncHealth(float newValue)
        { _Health = newValue; }
    }
    public void SetHealth(float oldValue, float newValue) { Health = newValue; }

    private void OnCollisionEnter(Collision col)
    {
        if ((~playerMask & (1 << col.gameObject.layer)) == 0)
        {
            Debug.Log("Damage");
            ChangeHealth(Health - 1);
        }
    }
}
