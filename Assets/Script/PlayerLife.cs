using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLife : NetworkBehaviour
{
    public int maxLife = 100;
    [SyncVar]
    private int _currentLife = 100;

    private bool _dead = true;

    //call on server
    public void TakeDamage(int damage)
    {
        if (_dead) return;

        _currentLife -= damage;
        if(_currentLife <= 0)
        {
            _currentLife = 0;
            _dead = true;
            GetComponentInParent<Player>().Dead();
        }
    }

    public int GetCurrentLife()
    {
        return _currentLife;
    }

    //call on server
    public void SetIsDead(bool newState)
    {
        _dead = newState;
    }

    //call on server
    public void RefillFullLife()
    {
        _currentLife = maxLife;
    }

    //call on server
    public void RefillLife(int amount)
    {
        _currentLife += amount;
        if (_currentLife > maxLife)
            _currentLife = maxLife;
    }
}