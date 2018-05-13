using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text ammoText = null;
    public Text weaponNameText = null;

    public void SetTextAmmo(int currentAmmo, int totalAmmo)
    {
        ammoText.text = currentAmmo + "/" + totalAmmo;
    }

    public void SetTextWeaponName(string newName)
    {
        weaponNameText.text = newName;
    }
}
