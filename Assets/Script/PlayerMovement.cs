﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public SPlayerMovementTweak[] playerTweak = null;

    public Rigidbody rigidBody = null;

    private EPlayerMovement _currentPlayerMovement = EPlayerMovement.RUNNING;
    private float _currentSpeed = 0;
    public float playerSpeed = 5;

    private Transform _transform = null;

    private void Start()
    {
        _transform = transform;
        ChangeSpeed();
    }

    //call on client
    private void ChangeSpeed()
    {
        for(int i = 0; i < playerTweak.Length; i++)
        {
            if(playerTweak[i].movement == _currentPlayerMovement)
            {
                _currentSpeed = playerTweak[i].speed;
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        if (isServer && !Data.GetInstance().DEBUG) return;

        if (Input.GetKey(Data.GetInstance().forwardKeycode))
        {
            rigidBody.MovePosition(rigidBody.position + _transform.forward * playerSpeed * _currentSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(Data.GetInstance().backwardKeycode))
        {
            rigidBody.MovePosition(rigidBody.position - _transform.forward * playerSpeed * _currentSpeed * Time.deltaTime);
        }

        if (Input.GetKey(Data.GetInstance().rightKeycode))
        {
            rigidBody.MovePosition(rigidBody.position - _transform.right * playerSpeed * _currentSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(Data.GetInstance().leftKeycode))
        {
            rigidBody.MovePosition(rigidBody.position + _transform.right * playerSpeed * _currentSpeed * Time.deltaTime);
        }
    }
}