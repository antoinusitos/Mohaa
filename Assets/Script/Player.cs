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

    [SyncVar]
    public EPlayerFaction playerFaction = EPlayerFaction.NONE;

    [SyncVar]
    private bool _dead = true;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        Camera.main.gameObject.SetActive(false);
        cameraToActivate.SetActive(true);

        if (playerUI != null)
        {
            playerUI.SetGameName(GameManager.GetInstance().gameName);
        }
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

    private void Start()
    {
        //Add a component to take damage and apply it on player life (for collisions)
        _playerLife = GetComponentInChildren<PlayerLife>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerFire = GetComponent<PlayerFire>();
        _playerCamera = GetComponent<PlayerCamera>();
        if (isServer)
        {
            GameManager.GetInstance().PlayerJoin(this);
        }
    }

    //call on server
    public void Respawn()
    {
        _dead = false;
        if (_playerMovement != null)
        {
            _playerMovement.SetCanMove(true);
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
        if (isLocalPlayer)
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

            if (_dead) return;

            //put interactions here with F
        }
    }
    
    //call on client
    public void SetFaction(EPlayerFaction newFaction)
    {
        if (_playerCamera != null)
            _playerCamera.SetCursorLocked(true);
        CmdSetFaction(newFaction);
    }

    [Command]
    public void CmdSetFaction(EPlayerFaction newFaction)
    {
        playerFaction = newFaction;
        GameManager.GetInstance().PlayerJoinTeam(this);
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
}
