using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SetNamePlayer : NetworkBehaviour
{
    public InputField inputName = null;

    public PlayerNetwork playerNetwork = null;

    public void Execute()
    {
        CmdSendName(inputName.text);
    }

    [Command]
    public void CmdSendName(string name)
    {
        playerNetwork.playerName = name;
    }
}
