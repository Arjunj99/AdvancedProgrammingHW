using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public FiniteStateMachine<ScoreController> GameState;
    public TMP_Text TitleText;
    public TMP_Text GameOverText;
    public TMP_Text ScoreText;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        GameState = new FiniteStateMachine<ScoreController>(this);
        GameState.TransitionTo<StartGame>();
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
        Service.EventManager.Fire(new StartGameEvent());
    }

    public override void Update()
    {
        if (Input.anyKey)
        {
            TransitionTo<MidGame>();
        }
    }
}

public class MidGame : FiniteStateMachine<ScoreController>.State
{
    private const int BASESCORE = 0;
    private const float MAXTIME = 5f;
    public float time;

    public override void OnEnter()
    {
        Context.score = BASESCORE;
        time = MAXTIME;
        Service.AIManager.CreationLeft();
        Service.AIManager.CreationRight();
        Service.EventManager.Register<ScoreEvent>(ReceiveScoreEvent);
    }

    public override void OnExit()
    {
        Service.AIManager.DestroyAll();
        Service.EventManager.Fire(new EndGameEvent());
        Service.EventManager.Unregister<ScoreEvent>(ReceiveScoreEvent);
    }

    public override void Update()
    {
        time -= Time.deltaTime;
        Debug.Log(time);
        if (time < 0) { TransitionTo<EndGame>(); }
    }

    public void ReceiveScoreEvent(AGPEvent e)
    {
        ScoreEvent scoreEvent = (ScoreEvent) e;
        if (scoreEvent.team == WhoScored.AIScored) { Context.score--; }
        else { Context.score++; }
    }
}

public class EndGame : FiniteStateMachine<ScoreController>.State
{
    public override void OnEnter()
    {
        Context.GameOverText.color = Color.white;
        Context.ScoreText.text = $"Score: {Context.score} points";
        Context.ScoreText.color = Color.white;
    }

    public override void OnExit()
    {
        Context.GameOverText.color = Color.green;
        Context.ScoreText.color = Color.green;
        Service.EventManager.Fire(new RestartEvent());
    }

    public override void Update()
    {
        if (Input.anyKey)
        {
            TransitionTo<StartGame>();
        }
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
    public WhoScored team;
    
    public ScoreEvent(WhoScored team)
    { 
        this.team = team;
    }
}


