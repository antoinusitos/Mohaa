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
                weapons[i].transform.SetParent(weaponSocket);
                weapons[i].transform.localPosition = Vector3.zero;
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
                    currentWeapon.CmdFire();
                }
            }
            else if (Input.GetKeyDown(Data.GetInstance().reload))
            {
                if (currentWeapon != null)
                {
                    currentWeapon.CmdReload();
                }
            }
        }
    }

    public void SetWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
    }
}
