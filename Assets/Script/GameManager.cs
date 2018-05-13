using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    private Player[] _allPlayers = null;

    public GameObject weaponPrefab = null;

    public int playersToStart = 1;
    public int maxPlayers = 2;
    protected int _currentNumberPlayer = 0;

    [SyncVar]
    protected int _allyPlayers = 0;
    [SyncVar]
    protected int _allyPlayersAlive = 0;
    [SyncVar]
    protected int _axisPlayers = 0;
    [SyncVar]
    protected int _axisPlayersAlive = 0;

    public string gameName = "GameModeTest";

    public bool teamFire = false;

    public float timeToRestart = 5;
    public bool autoRestart = true;

    protected GameObject[] allyRespawnPoints = null;
    protected GameObject[] axisRespawnPoints = null;

    //call on server
    public void PlayerJoin(Player thePlayer)
    {
        _allPlayers[_currentNumberPlayer] = thePlayer;
        GameObject go = Instantiate(weaponPrefab);
        NetworkServer.Spawn(go);
        thePlayer.GetPlayerFire().SetWeapon(go.GetComponent<Weapon>());
        thePlayer.GetPlayerFire().AttackWeaponToSocket();
        thePlayer.GetPlayerFire().RpcAttackWeaponToSocket(thePlayer.netId, go.GetComponent<Weapon>().netId);
        _currentNumberPlayer++;
        OnPlayerPostJoin(thePlayer);
    }

    //call on server
    public virtual void OnPlayerPostJoin(Player thePlayer)
    {

    }

    protected void Start()
    {
        if (isServer)
        {
            _allPlayers = new Player[maxPlayers];
            allyRespawnPoints = GameObject.FindGameObjectsWithTag("AllySpawn");
            axisRespawnPoints = GameObject.FindGameObjectsWithTag("AxisSpawn");
        }
        GameModeStart();
    }

    //call on server
    public virtual void GameModeStart()
    {

    }

    //call on server
    public void SetTeamVictory(EPlayerFaction winningFaction)
    {
        for(int i = 0; i < _allPlayers.Length; i++)
        {
            if(_allPlayers[i] != null)
            {
                _allPlayers[i].RpcShowWin(true, winningFaction);
            }
        }

        if(autoRestart)
            RestartGame();
    }

    //call on server
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

        OnPlayerReady(thePlayer);
    }

    //call on server
    public void RestartGame()
    {
        StartCoroutine("Restarting");
    }
    
    protected IEnumerator Restarting()
    {
        yield return new WaitForSeconds(timeToRestart);
        _allyPlayersAlive = 0;
        _axisPlayersAlive = 0;
        for (int i = 0; i < _allPlayers.Length; i++)
        {
            if (_allPlayers[i] != null)
            {
                _allPlayers[i].RpcShowWin(false, EPlayerFaction.NONE);
                OnPlayerReady(_allPlayers[i]);
            }
        }
    }

    //call on server
    public virtual void OnPlayerReady(Player thePlayer)
    {
        //check if player can spawn
        thePlayer.Respawn();
        if (thePlayer.playerFaction == EPlayerFaction.ALLY)
        {
            _allyPlayersAlive++;
            thePlayer.GetPlayerMovement().RpcForcePosition(allyRespawnPoints[Random.Range(0, allyRespawnPoints.Length)].transform.position);
        }
        else if (thePlayer.playerFaction == EPlayerFaction.AXIS)
        {
            _axisPlayersAlive++;
            thePlayer.GetPlayerMovement().RpcForcePosition(axisRespawnPoints[Random.Range(0, axisRespawnPoints.Length)].transform.position);
        }
    }

    //call on server
    public virtual void OnPlayerDeath(Player thePlayer)
    {
        if (thePlayer.playerFaction == EPlayerFaction.ALLY)
        {
            _allyPlayersAlive--;
        }
        else if (thePlayer.playerFaction == EPlayerFaction.AXIS)
        {
            _axisPlayersAlive--;
        }
        thePlayer.Respawn();
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

    protected static GameManager _instance = null;
    public static GameManager GetInstance()
    {
        return _instance;
    }

    protected void Awake()
    {
        _instance = this;
    }
}
