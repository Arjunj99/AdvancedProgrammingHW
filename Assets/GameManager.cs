using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject AI; // AI Prefab
    public Transform AIGroup; // Gameobject housing all AI Gameobject
    public Transform AIGroup2; // Gameobject housing all AI Gameobject
    public GameObject ball; // Reference to Soccer Ball
    public int frameDelay; // Frames since last position
    public float speed; // Speed of the AI Brain
    public ForceMode forceMode; // Forcemode Type

    private void Awake()
    {
        new AILifeCycle(AI, AIGroup, AIGroup2, ball, frameDelay, speed, forceMode);

        if (Service.GameManager == null) { Service.GameManager = this; }
        else { Destroy(this.gameObject); }
    }

    // Update is called once per frame
    void Update()
    {
        Service.AIManager.Updating();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Service.AIManager.CreationLeft();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Service.AIManager.CreationRight();
        }
    }
}
