using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    private Player[] _allPlayers = null;

    public GameObject weaponPrefab = null;

    private void Start()
    {
        if (isServer)
            StartCoroutine("StartGame");
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2.0f);

        _allPlayers = FindObjectsOfType<Player>();

        for(int i = 0; i < _allPlayers.Length; i++)
        {
            GameObject go = Instantiate(weaponPrefab);
            NetworkServer.Spawn(go);
            _allPlayers[i].SetWeapon(go.GetComponent<Weapon>());
            _allPlayers[i].RpcAttackWeaponToSocket(_allPlayers[i].netId, go.GetComponent<Weapon>().netId);
        }
    }
}
