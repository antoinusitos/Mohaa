using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObserver : NetworkBehaviour
{
    private bool _isSelecting = false;

    public GameObject selectionUI = null;

    private int _teamSide = -1;
    private int _characterIndex = -1;

    public GameObject playerPrefab = null;

    private void Update()
    {
        if(
            Input.GetKeyDown(Data.GetInstance().menu) || 
            Input.GetKeyDown(Data.GetInstance().jump) || 
            Input.GetKeyDown(Data.GetInstance().validate)
        )
        {
            if(!_isSelecting)
            {
                _isSelecting = true;
                selectionUI.SetActive(true);
            }
        }

        if (Input.GetKeyDown(Data.GetInstance().menu))
        {
            // change the observed player
        }
    }

    //call client Side
    public void SetTeamSide(int team)
    {
        _teamSide = team;
    }

    //call client Side
    public void SelectCharacter(int characterIndex)
    {
        _characterIndex = characterIndex;
        StartCoroutine("WaitForAnswerCreationCharacter");
    }

    protected IEnumerator WaitForAnswerCreationCharacter()
    {
        yield return new WaitForSeconds(0.1f);
        CreatePlayer();
    }

    //call client side
    protected void CreatePlayer()
    {
        CmdCreatePlayer(_teamSide, _characterIndex, GetComponent<PlayerNetwork>().netId);
    }

    [Command]
    protected void CmdCreatePlayer(int teamSide, int characterIndex, NetworkInstanceId playerAskingID)
    {
        GameManager.GetInstance().CmdCreatePlayer(teamSide, characterIndex, GetComponent<PlayerNetwork>().netId);
    }
}
