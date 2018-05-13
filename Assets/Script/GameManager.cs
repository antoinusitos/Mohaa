using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    private Player[] _allPlayers = null;

    public GameObject weaponPrefab = null;

    public int playersToStart = 1;
    public int maxPlayers = 1;
    private int _currentNumberPlayer = 0;

    [SyncVar]
    private int _allyPlayers = 0;
    [SyncVar]
    private int _allyPlayersAlive = 0;
    [SyncVar]
    private int _axisPlayers = 0;
    [SyncVar]
    private int _axisPlayersAlive = 0;

    public string gameName = "MME";

    //call on server
    public void PlayerJoin(Player thePlayer)
    {
        _allPlayers[_currentNumberPlayer] = thePlayer;
        GameObject go = Instantiate(weaponPrefab);
        NetworkServer.Spawn(go);
        thePlayer.GetPlayerFire().SetWeapon(go.GetComponent<Weapon>());
        thePlayer.GetPlayerFire().RpcAttackWeaponToSocket(thePlayer.netId, go.GetComponent<Weapon>().netId);
        _currentNumberPlayer++;
    }

    private void Start()
    {
        if (isServer)
        {
            _allPlayers = new Player[maxPlayers];
        }
    }

    public void PlayerJoinTeam(Player thePlayer)
    {
        if (thePlayer.playerFaction == EPlayerFaction.ALLY)
        {
            _allyPlayers++;
        }
        else if (thePlayer.playerFaction == EPlayerFaction.AXIS)
        {
            _axisPlayers++;
        }

        thePlayer.Respawn();
        if (thePlayer.playerFaction == EPlayerFaction.ALLY)
        {
            _allyPlayersAlive++;
        }
        else if (thePlayer.playerFaction == EPlayerFaction.AXIS)
        {
            _axisPlayersAlive++;
        }
    }

    public int GetPlayersAlly()
    {
        return _allyPlayers;
    }

    public int GetPlayersAllyAlive()
    {
        return _allyPlayersAlive;
    }

    public int GetPlayersAxis()
    {
        return _axisPlayers;
    }

    public int GetPlayersAxisAlive()
    {
        return _axisPlayersAlive;
    }

    private static GameManager _instance = null;
    public static GameManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }
}
