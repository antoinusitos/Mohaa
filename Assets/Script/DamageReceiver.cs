using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public PlayerLife damageReceiver = null;

    //call on server
    public void RedirectDamage(int damage)
    {
        if (damageReceiver != null)
            damageReceiver.TakeDamage(damage);
    }
}
