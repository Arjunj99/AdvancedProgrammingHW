using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Vector3 position;
    public Vector3 rotation;
    public KeyCode forwardKey;
    public KeyCode backwardKey;
    public KeyCode rightKey;
    public KeyCode leftKey;
    public KeyCode gasJumpKey;
    public KeyCode iceKey; 
    public int narrativeState;

    public PlayerData()
    {
        position = Vector3.zero;
        rotation = Vector3.zero;
        forwardKey = KeyCode.W;
        backwardKey = KeyCode.S;
        rightKey = KeyCode.D;
        leftKey = KeyCode.A;
        gasJumpKey = KeyCode.Space;
        iceKey = KeyCode.LeftShift;
        narrativeState = 0;
    }

    public PlayerData(Vector3 position, Vector3 rotation, KeyCode forwardKey, KeyCode backwardKey, KeyCode rightKey, KeyCode leftKey, KeyCode gasJumpKey, KeyCode iceKey, int narrativeState)
    {
        this.position = position;
        this.rotation = rotation;
        this.forwardKey = forwardKey;
        this.backwardKey = backwardKey;
        this.rightKey = rightKey;
        this.leftKey = leftKey;
        this.gasJumpKey = gasJumpKey;
        this.iceKey = iceKey;
        this.narrativeState = narrativeState;
    }

    public PlayerData(PlayerMovement playerMovement)
    {
        this.position = playerMovement.gameObject.transform.position;
        this.rotation = playerMovement.gameObject.transform.rotation.eulerAngles;
        this.forwardKey = KeyCode.W;
        forwardKey = KeyCode.W;
        backwardKey = KeyCode.S;
        rightKey = KeyCode.D;
        leftKey = KeyCode.A;
        gasJumpKey = playerMovement.GasJumpKey;
        iceKey = playerMovement.IceSlideKey;
        narrativeState = 0;
    }

    public override string ToString()
    {
        return $"Position: {position}\nRotation: {rotation}\nForward Key: {forwardKey}\nBackward Key: {backwardKey}\nRight Key: " +
            $"{rightKey}\nLeft Key: {leftKey}\nGas Jump Key: {gasJumpKey}\nIce Slide Key: {iceKey}\nNarrative State: {narrativeState}";
    }
}
