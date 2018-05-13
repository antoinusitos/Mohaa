using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public Weapon currentWeapon = null;

    public GameObject cameraToActivate = null;

    public PlayerUI playerUI = null;

    public Transform weaponSocket = null;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        Camera.main.gameObject.SetActive(false);
        cameraToActivate.SetActive(true);
    }

    private void Start()
    {
        if(isServer)
        {
            GameManager.GetInstance().PlayerJoin(this);
        }
    }

    //call on client
    [ClientRpc]
    public void RpcAttackWeaponToSocket(NetworkInstanceId playerID, NetworkInstanceId id)
    {
        if (netId != playerID) return;

        Weapon[] weapons = FindObjectsOfType<Weapon>();
        for(int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i].netId == id)
            {
                currentWeapon = weapons[i];
                currentWeapon.transform.SetParent(weaponSocket);
                currentWeapon.transform.localPosition = Vector3.zero;
                return;
            }
        }
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (playerUI != null)
            {
                if (currentWeapon != null)
                    playerUI.SetTextAmmo(currentWeapon.GetCurrentAmmo(), currentWeapon.GetTotalAmmo());
            }

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
}
