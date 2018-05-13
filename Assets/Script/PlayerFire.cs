using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerFire : NetworkBehaviour
{
    public Transform weaponSocket = null;

    public Weapon currentWeapon = null;

    [SyncVar]
    public bool _dead = true;

    //call on client
    [ClientRpc]
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
    }

    //call on client
    [Command]
    public void CmdFire()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }

    //call on client
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
        if (isLocalPlayer)
        {
            if (_dead) return;

            if (Input.GetKey(Data.GetInstance().fireKeycode))
            {
                if (currentWeapon != null)
                {
                    CmdFire();
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
