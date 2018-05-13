using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text ammoText = null;
    public Text weaponNameText = null;

    public Slider lifeSlider = null;
    public Text lifeText = null;

    public Text gameName = null;

    public Text playersAlly = null;
    public Text playersAxis = null;

    //call on client
    public void SetTextAmmo(int currentAmmo, int totalAmmo)
    {
        ammoText.text = currentAmmo + "|" + totalAmmo;
    }

    //call on client
    public void SetTextWeaponName(string newName)
    {
        weaponNameText.text = newName;
    }

    //call on client
    public void SetPlayerLife(int currentLife)
    {
        lifeSlider.value = currentLife;
        lifeText.text = currentLife.ToString();
    }

    //call on client
    public void SetGameName(string newName)
    {
        gameName.text = "[" + newName + "]";
    }

    //call on client
    public void SetPlayers(bool ally, int playersAlive, int playersTotal)
    {
        if(ally)
            playersAlly.text = playersAlive + " / " + playersTotal;
        else
            playersAxis.text = playersAlive + " / " + playersTotal;
    }

    //call on client
    public void ChooseSide(int newFaction)
    {
        if(newFaction == 0)
            GetComponentInParent<Player>().SetFaction(EPlayerFaction.ALLY);
        else
            GetComponentInParent<Player>().SetFaction(EPlayerFaction.AXIS);
    }
}
