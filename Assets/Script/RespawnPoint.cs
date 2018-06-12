using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RespawnPoint : NetworkBehaviour
{
    public EPlayerFaction faction = EPlayerFaction.NONE;

    [SyncVar]
    public bool isAvailable = true;
}
