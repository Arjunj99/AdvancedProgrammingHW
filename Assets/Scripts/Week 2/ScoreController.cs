using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public FiniteStateMachine<ScoreController> GameState;
    public TextMesh TitleText;
    public TextMesh GameOverText;
    public TextMesh ScoreText;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        GameState = new FiniteStateMachine<ScoreController>(this);
    }

    // Update is called once per frame
    void Update()
    {
        GameState.Update();
    }
}

public class StartGame : FiniteStateMachine<ScoreController>.State
{
    public override void OnEnter()
    {
        Context.TitleText.color = Color.white;
    }

    public override void OnExit()
    {
        Context.TitleText.color = Color.green;
    }

    public override void Update()
    {
    }
}

public class MidGame : FiniteStateMachine<ScoreController>.State
{
    private const int BASESCORE = 0;
    private const float MAXTIME = 30f;
    public float time;

    public override void OnEnter()
    {
        Context.score = BASESCORE;
        time = MAXTIME;
        Service.AIManager.CreationLeft();
        Service.AIManager.CreationRight();
    }

    public override void OnExit()
    {
        Service.AIManager.DestroyAll();
    }

    public override void Update()
    {
        time -= Time.deltaTime;
        if (time < 0) { }
    }
}

public class EndGame : FiniteStateMachine<ScoreController>.State
{
    public override void OnEnter()
    {
        Context.GameOverText.color = Color.white;
        Context.ScoreText.text = $"Score: {Service.ScoreController.score} points";
        Context.ScoreText.color = Color.white;
    }

    public override void OnExit()
    {
        Context.GameOverText.color = Color.green;
        Context.ScoreText.color = Color.green;
    }

    public override void Update()
    {
    }
}

public class StartGameEvent : AGPEvent
{
    public StartGameEvent() { }
}

public class EndGameEvent : AGPEvent
{
    public EndGameEvent() { }
}

public class RestartEvent : AGPEvent
{
    public RestartEvent() { }
}

public enum WhoScored
{
    playerScored, AIScored
}

public class ScoreEvent : AGPEvent
{
    public GameObject player;
    public WhoScored team;
    
    public ScoreEvent(GameObject player, WhoScored team)
    { 
        this.player = player;
        this.team = team;
    }
}


