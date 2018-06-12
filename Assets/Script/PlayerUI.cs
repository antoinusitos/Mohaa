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

    public Image factionImage = null;
    public Sprite allySprite = null;
    public Sprite axisSprite = null;

    public Text winningText = null;

    private GameLog _gameLog = null;

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
            playersAlly.text = playersAlive + " / " + playersTotal + " [" + GameManager.GetInstance().GetAllyScore() + "]";
        else
            playersAxis.text = playersAlive + " / " + playersTotal + " [" + GameManager.GetInstance().GetAxisScore() + "]";
    }

    //call on client
    public void ShowLog(string log)
    {
        if (_gameLog == null)
            _gameLog = FindObjectOfType<GameLog>();

        _gameLog.ShowLog(log);
    }

    //call on server
    public void ChooseSide(int newFaction)
    {
        if(newFaction == 1)
        {
            factionImage.sprite = allySprite;
            GetComponentInParent<Player>().SetFaction(EPlayerFaction.ALLY);
        }
        else if (newFaction == 2)
        {
            factionImage.sprite = axisSprite;
            GetComponentInParent<Player>().SetFaction(EPlayerFaction.AXIS);
        }
    }

    public void ShowWin(bool newState, EPlayerFaction winningFaction)
    {
        if(newState)
        {
            winningText.gameObject.SetActive(true);
            winningText.text = winningFaction + " Win the Game !";
        }
        else
            winningText.gameObject.SetActive(false);
    }
}
