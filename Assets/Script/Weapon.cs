using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Weapon : NetworkBehaviour
{
    public int maxAmmo = 0;
    [SyncVar]
    private int _currentAmmo = 0;
    public int maxInMagazine = 0;
    [SyncVar]
    private int _currentAmmoInMagazine = 0;

    public float timeToReload = 0;
    private float _currentTimeReloading = 0;

    public float fireRate = 0.1f;
    private float _currentFireRate = 0;

    [SyncVar]
    private bool _isReloading = false;
    [SyncVar]
    private bool _canShoot = true;

    public string weaponName = "NONE";
    public int damage = 10;

    public Transform attachedTo = null;

    private void Start()
    {
        _currentAmmo = maxAmmo;
        _currentAmmoInMagazine = maxInMagazine;
    }

    public void AttachTo(Transform transf)
    {
        attachedTo = transf;
    }

    private void Update()
    {
        if(attachedTo != null)
            transform.position = attachedTo.position;

        if (!isServer) return;

        if(_isReloading)
        {
            _currentTimeReloading += Time.deltaTime;
            if(_currentTimeReloading >= timeToReload)
            {
                _currentTimeReloading = 0;
                _isReloading = false;
                SetBulletsAvailable();
            }
        }

        if(!_canShoot)
        {
            _currentFireRate += Time.deltaTime;
            if(_currentFireRate >= fireRate)
            {
                _currentFireRate = 0;
                _canShoot = true;
            }
        }
    }

    public void Reset()
    {
        _currentAmmo = maxAmmo;
        _currentAmmoInMagazine = maxInMagazine;
        _canShoot = true;
        _isReloading = false;
        _currentFireRate = 0;
    }

    //call on server
    private void SetBulletsAvailable()
    {
        if (_currentAmmo >= maxInMagazine)
        {
            _currentAmmo -= maxInMagazine - _currentAmmoInMagazine;
            _currentAmmoInMagazine = maxInMagazine;
        }
        else if(_currentAmmo > 0)  
        {
            _currentAmmoInMagazine = _currentAmmo;
            _currentAmmo = 0;
        }
    }

    //call on server
    public void Reload()
    {
        if (!isServer) return;

        if (_isReloading) return;

        if (_currentAmmo <= 0 || _currentAmmoInMagazine == maxInMagazine) return;

        _isReloading = true;
    }

    //call on server
    public void Fire()
    {
        if (!isServer) return;

        if (_isReloading) return;

        if (!_canShoot) return;

        if (_currentAmmoInMagazine <= 0) return;

        _currentFireRate = 0;
        _canShoot = false;

        _currentAmmoInMagazine--;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 5000))
        {
            DamageReceiver dr = hit.collider.GetComponent<DamageReceiver>();
            if(dr != null)
            {
                dr.RedirectDamage(damage);
            }
        }

        if (_currentAmmoInMagazine <= 0)
        {
            _currentAmmoInMagazine = 0;
            Reload();
        }
    }

    public int GetCurrentAmmo()
    {
        return _currentAmmoInMagazine;
    }

    public int GetTotalAmmo()
    {
        return _currentAmmo;
    }

    public float GetReloadTime()
    {
        return _currentTimeReloading;
    }

    public float GetRatioReloadTime()
    {
        return _currentTimeReloading / timeToReload;
    }
}
