using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerFire : NetworkBehaviour
{
    public Transform weaponSocket = null;

    public Weapon currentWeapon = null;

    private PlayerNetwork _playerNetwork = null;

    [SyncVar]
    public bool _dead = true;

    public void SetPlayerNetwork(PlayerNetwork newPlayerNetwork)
    {
        _playerNetwork = newPlayerNetwork;
    }

    //call on server
    /*public void AttackWeaponToSocket()
    {
        currentWeapon.transform.SetParent(weaponSocket);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.AttachTo(weaponSocket);
        return;
    }*/

    //call on client
    /*[ClientRpc]
    public void RpcAttackWeaponToSocket(NetworkInstanceId playerID, NetworkInstanceId id)
    {
        if (netId != playerID) return;

        Weapon[] weapons = FindObjectsOfType<Weapon>();
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].netId == id)
            {
                currentWeapon = weapons[i];
                currentWeapon.transform.SetParent(weaponSocket);
                currentWeapon.transform.localPosition = Vector3.zero;
                return;
            }
        }
    }*/

    //call on server
    public void Reset()
    {
        if (currentWeapon != null)
            currentWeapon.Reset();
    }

    [Command]
    public void CmdFire(string sender)
    {
        Debug.Log("isserver ?" + isServer);
        if (currentWeapon != null)
        {
            Debug.Log("isserver 2 ?" + isServer);
            currentWeapon.Fire(sender);
        }
    }

    [Command]
    public void CmdReload()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Reload();
        }
    }

    public void SetWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
    }

    public void SetDead(bool newState)
    {
        _dead = newState;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    private void Update()
    {
        if (isLocalPlayer || localPlayerAuthority)
        {
            if (_dead) return;

            if (Input.GetKey(Data.GetInstance().fireKeycode))
            {
                if (currentWeapon != null)
                {
                    CmdFire(_playerNetwork.playerName);
                }
            }
            else if (Input.GetKeyDown(Data.GetInstance().reload))
            {
                if (currentWeapon != null)
                {
                    CmdReload();
                }
            }
        }
    }
}
