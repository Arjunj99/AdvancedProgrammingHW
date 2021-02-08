using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerPlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public ForceMode forceMode;
    // Start is called before the first frame update
    void Awake()
    {
        if (Service.PlayerController == null) { Service.PlayerController = this; }
        else { Destroy(this.gameObject); }
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
}
