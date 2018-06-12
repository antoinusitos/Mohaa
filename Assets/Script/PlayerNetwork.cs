using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetwork : NetworkBehaviour
{
    [SyncVar]
    public string playerName = "UNKNOWN";

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        GameManager.GetInstance().localPlayerNetwork = this;
    }

}
