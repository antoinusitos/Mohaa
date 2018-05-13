using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MME : GameManager
{
    public override void GameModeStart()
    {
        gameName = "MME";
    }

    public override void OnPlayerPostJoin(Player thePlayer)
    {
        base.OnPlayerPostJoin(thePlayer);
    }

    //call on server
    public override void OnPlayerReady(Player thePlayer)
    {
        base.OnPlayerReady(thePlayer);
    }

    //call on server
    public override void OnPlayerDeath(Player thePlayer)
    {
        if (thePlayer.playerFaction == EPlayerFaction.ALLY)
        {
            _allyPlayersAlive--;
            CheckPlayerTeam(EPlayerFaction.ALLY);
        }
        else if (thePlayer.playerFaction == EPlayerFaction.AXIS)
        {
            _axisPlayersAlive--;
            CheckPlayerTeam(EPlayerFaction.AXIS);
        }
    }

    //call on server
    private void CheckPlayerTeam(EPlayerFaction faction)
    {
        if (faction == EPlayerFaction.ALLY)
        {
            if(_allyPlayersAlive == 0)
            {
                //Axis Victory
                SetTeamVictory(EPlayerFaction.AXIS);
            }
        }
        else if (faction == EPlayerFaction.AXIS)
        {
            if (_axisPlayersAlive == 0)
            {
                //Ally Victory
                SetTeamVictory(EPlayerFaction.ALLY);
            }
        }
    }
}
