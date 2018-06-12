using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public GameObject cameraToActivate = null;

    public PlayerUI playerUI = null;
    private PlayerLife _playerLife = null;
    private PlayerMovement _playerMovement = null;
    private PlayerFire _playerFire = null;
    private PlayerCamera _playerCamera = null;
    private PlayerNetwork _playerNetwork = null;

    [SyncVar]
    public EPlayerFaction playerFaction = EPlayerFaction.NONE;

    [SyncVar]
    private bool _dead = true;

    private bool _init = false;

    public GameObject playerObserver = null;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        Camera.main.gameObject.SetActive(false);
        cameraToActivate.SetActive(true);

        if (playerUI != null)
        {
            playerUI.SetGameName(GameManager.GetInstance().gameName);
        }

        GameManager.GetInstance().localPlayer = this;
    }

    [ClientRpc]
    public void RpcShowWin(bool newState, EPlayerFaction winningFaction)
    {
        if(isLocalPlayer)
        {
            if(playerUI != null)
            {
                playerUI.ShowWin(newState, winningFaction);
            }
        }
    }

    public void Init(NetworkInstanceId playerAskingID)
    {
        if (_init) return;

        _init = true;

        PlayerNetwork[] playerNetworks = FindObjectsOfType<PlayerNetwork>();
        for (int i = 0; i < playerNetworks.Length; i++)
        {
            if (playerNetworks[i].netId == playerAskingID)
            {
                playerObserver = playerNetworks[i].gameObject;
                break;
            }
        }

        _playerNetwork = playerObserver.GetComponent<PlayerNetwork>();
        _playerLife = GetComponentInChildren<PlayerLife>();
        _playerLife.SetPlayerNetwork(_playerNetwork);
        _playerMovement = GetComponent<PlayerMovement>();
        _playerFire = GetComponent<PlayerFire>();
        _playerFire.SetPlayerNetwork(_playerNetwork);
        _playerCamera = GetComponent<PlayerCamera>();
        GameManager.GetInstance().localPlayerNetwork = _playerNetwork;
        if (isServer)
        {
            GameManager.GetInstance().PlayerJoin(this);
            GameManager.GetInstance().ShowLog(_playerNetwork.playerName + " joined the fight !");
        }
    }

    //call on server
    public void Respawn()
    {
        _dead = false;
        if (_playerMovement != null)
        {
            _playerMovement.SetCanMove(true);
            RespawnPoint[] respawns = FindObjectsOfType<RespawnPoint>();
            for(int i = 0; i < respawns.Length; i++)
            {
                if(respawns[i].faction == playerFaction && respawns[i].isAvailable)
                {
                    respawns[i].isAvailable = false;
                    _playerMovement.RpcForcePosition(respawns[i].transform.position);
                    break;
                }
            }
        }
        if (_playerFire != null)
        {
            _playerFire.SetDead(false);
        }
        if (_playerCamera != null)
        {
            _playerCamera.SetDead(false);
        }
        if (_playerLife != null)
        {
            _playerLife.SetIsDead(false);
            _playerLife.RefillFullLife();
        }
    }

    private void Update()
    {
        if (isLocalPlayer || localPlayerAuthority)
        {
            if (playerUI != null)
            {
                if (_playerFire != null && _playerFire.GetCurrentWeapon() != null)
                {
                    playerUI.SetTextAmmo(_playerFire.GetCurrentWeapon().GetCurrentAmmo(), _playerFire.GetCurrentWeapon().GetTotalAmmo());
                    playerUI.SetTextWeaponName(_playerFire.GetCurrentWeapon().weaponName);
                }
                if(_playerLife != null)
                {
                    playerUI.SetPlayerLife(_playerLife.GetCurrentLife());
                }

                playerUI.SetPlayers(true, GameManager.GetInstance().GetPlayersAllyAlive(), GameManager.GetInstance().GetPlayersAlly());
                playerUI.SetPlayers(false, GameManager.GetInstance().GetPlayersAxisAlive(), GameManager.GetInstance().GetPlayersAxis());
            }

            if (Input.GetKeyDown(KeyCode.P))
                CmdSendLog("lol");

            if (_dead) return;

            //put interactions here with F
        }
    }

    private void CmdSendLog(string log)
    {
        GameManager.GetInstance().ShowLog(log);
    }

    //call on client
    public void SetFaction(EPlayerFaction newFaction)
    {
        if (_playerCamera != null)
            _playerCamera.SetCursorLocked(true);
        playerFaction = newFaction;
        GameManager.GetInstance().PlayerJoinTeam(this);
    }

    [ClientRpc]
    public void RpcShowLog(string log)
    {
        playerUI.ShowLog(log);
    }

    //Call on server
    public void Dead()
    {
        GameManager.GetInstance().OnPlayerDeath(this);
        _dead = true;
        if (_playerMovement != null)
        {
            _playerMovement.SetCanMove(false);
        }
        if (_playerFire != null)
        {
            _playerFire.SetDead(true);
        }
        if(_playerCamera != null)
        {
            _playerCamera.SetDead(true);
        }
        if (_playerLife != null)
        {
            _playerLife.SetIsDead(true);
        }
    }

    public PlayerFire GetPlayerFire()
    {
        return _playerFire;
    }

    public PlayerMovement GetPlayerMovement()
    {
        return _playerMovement;
    }
}
