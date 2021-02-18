using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILifeCycle
{
    public GameObject AI; // AI Prefab
    public Dictionary<string, GameObject> AllAI; // Dictionary containing all AI instances in the Current Game
    public Transform AIGroup; // Gameobject housing all AI Gameobject
    public Transform AIGroup2; // Second Gameobject housing all AI Gameobject
    public GameObject ball; // Reference to Soccer Ball
    public Vector3 lastPosition; // Reference to Soccer Ball X frames ago
    public int frameDelay; // Frames since last position
    public float speed; // Speed of the AI Brain
    public ForceMode forceMode; // 
    private int index; // Random Name Generator Seed for Unique ID :)
    private int currentFrames; // Current frames within game

    /// <summary>
    /// Constructs a new AILifeCycle
    /// </summary>
    /// <param name="AI">AI Prefab</param>
    /// <param name="AIGroup">AI Grouping</param>
    /// <param name="ball">Soccer Ball in Scene</param>
    /// <param name="frameDelay">Frame Delay between ball position checks</param>
    /// <param name="speed">Speed of each AI</param>
    /// <param name="forcemode">Forcemode type for AI Rigidbody</param>
    public AILifeCycle(GameObject AI, Transform AIGroup, Transform AIGroup2, GameObject ball, int frameDelay, float speed, ForceMode forcemode)
    {
        this.AI = AI;
        AllAI = new Dictionary<string, GameObject>();
        this.AIGroup = AIGroup;
        this.AIGroup2 = AIGroup2;
        this.ball = ball;
        lastPosition = ball.transform.position;
        this.frameDelay = frameDelay;
        this.speed = speed;
        this.forceMode = forcemode;
        index = -1;
        currentFrames = 0;

        Awake();
    }

    public void Awake()
    {
        if (Service.AIManager == null) { Service.AIManager = this; }
    }

    /// <summary>
    /// Creates a new AI in right end of scene and adds to allAI
    /// </summary>
    public void CreationRight()
    {
        GameObject newAI = GameObject.Instantiate(AI, AIGroup.position, AIGroup.rotation);
        newAI.name = RandomNameGenerator();
        AllAI.Add(newAI.name, newAI);
    }

    /// <summary>
    /// Creates a new AI in left end of scene and adds to allAI
    /// </summary>
    public void CreationLeft()
    {
        GameObject newAI = GameObject.Instantiate(AI, AIGroup2.position, AIGroup2.rotation);
        newAI.name = RandomNameGenerator();
        AllAI.Add(newAI.name, newAI);
    }

    /// <summary>
    /// Updates each AI to run towards the ball's location a few frames ago (based on frame delay)
    /// </summary>
    public void Updating()
    {
        foreach(KeyValuePair<string, GameObject> ai in AllAI)
        {
            ai.Value.GetComponent<Rigidbody>().AddForce(speed * ai.Value.transform.forward, forceMode);
            ai.Value.transform.LookAt(new Vector3(lastPosition.x, 0.5f, lastPosition.z));
        }

        if (currentFrames == frameDelay) { lastPosition = ball.transform.position; }
        else { currentFrames++; }
    }

    /// <summary>
    /// Destroys AI based on UUID (not actually a UUID :[ )
    /// </summary>
    /// <param name="UUID">Unique ID in the Jeff Naming Scheme.</param>
    public void Destroy(string UUID)
    {
        GameObject.Destroy(AllAI[UUID]);
        AllAI.Remove(UUID);
    }

    public void DestroyAll()
    {
        foreach (KeyValuePair<string, GameObject> ai in AllAI)
        {
            GameObject.Destroy(ai.Value);
            AllAI.Remove(ai.Key);
        }
    }

    /// <summary>
    /// Generates Unique Random ID
    /// </summary>
    /// <returns> Unique Random ID</returns>
    public string RandomNameGenerator()
    {
        index++;
        return $"Jeff {index}";
    }
}
