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
    public Vector3 startPosition;

    private void Awake()
    {
        new AILifeCycle(AI, AIGroup, AIGroup2, ball, frameDelay, speed, forceMode);

        if (Service.GameManager == null) { Service.GameManager = this; }
        else { Destroy(this.gameObject); }

        Service.EventManager.Register<ScoreEvent>(ReceiveScoreEvent);
        Service.EventManager.Register<EndGameEvent>(ReceiveEndGameEvent);

        startPosition = ball.transform.position;
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

    public void Unregister()
    {
        Service.EventManager.Unregister<ScoreEvent>(ReceiveScoreEvent);
        Service.EventManager.Unregister<EndGameEvent>(ReceiveEndGameEvent);

    }

    public void ResetPosition()
    {
        ball.transform.position = startPosition;
    }

    public void ReceiveScoreEvent(AGPEvent e)
    {
        ScoreEvent scoreEvent = (ScoreEvent)e;
        ResetPosition();
    }

    public void ReceiveEndGameEvent(AGPEvent e)
    {
        EndGameEvent endEvent = (EndGameEvent) e;
        ResetPosition();
    }
}
