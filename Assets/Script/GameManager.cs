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
    private int _playersAlive = 0;

    public void PlayerJoin(Player thePlayer)
    {
        _allPlayers[_currentNumberPlayer] = thePlayer;
        GameObject go = Instantiate(weaponPrefab);
        NetworkServer.Spawn(go);
        thePlayer.SetWeapon(go.GetComponent<Weapon>());
        thePlayer.RpcAttackWeaponToSocket(thePlayer.netId, go.GetComponent<Weapon>().netId);
        _currentNumberPlayer++;
        _playersAlive++;
    }

    private void Start()
    {
        if (isServer)
        {
            _allPlayers = new Player[maxPlayers];
        }
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
