using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerPlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public ForceMode forceMode;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Awake()
    {
        if (Service.PlayerController == null) { Service.PlayerController = this; }
        else { Destroy(this.gameObject); }
        startPosition = transform.position;

        Service.EventManager.Register<ScoreEvent>(ReceiveScoreEvent);
        Service.EventManager.Register<EndGameEvent>(ReceiveEndGameEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * speed, forceMode);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-transform.right * speed, forceMode);
        }
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * speed, forceMode);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * speed, forceMode);
        }
    }

    public void Unregister()
    {
        Service.EventManager.Unregister<ScoreEvent>(ReceiveScoreEvent);
        Service.EventManager.Unregister<EndGameEvent>(ReceiveEndGameEvent);
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }

    public void ReceiveScoreEvent(AGPEvent e)
    {
        ScoreEvent scoreEvent = (ScoreEvent) e;
        ResetPosition();
    }

    public void ReceiveEndGameEvent(AGPEvent e)
    {
        EndGameEvent scoreEvent = (EndGameEvent) e;
        ResetPosition();
    }

    public void RecievePlayerGainedHealthEvent(AGPEvent e)
    {
        PlayerGainsHealth gainHealthEvent = (PlayerGainsHealth)e;
        // card.health += gainHealthEvent.healthRegained;
    }
}
