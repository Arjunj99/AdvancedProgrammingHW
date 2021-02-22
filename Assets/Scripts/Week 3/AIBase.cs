using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AIBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class RunAtBall : Node<AIBase>
{
    public override bool Update(AIBase context)
    {
        foreach (KeyValuePair<string, GameObject> ai in AllAI)
        {
            ai.Value.GetComponent<Rigidbody>().AddForce(speed * ai.Value.transform.forward, forceMode);
            ai.Value.transform.LookAt(new Vector3(lastPosition.x, 0.5f, lastPosition.z));
        }
    }
}
