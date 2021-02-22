using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AICocky : AIBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool CreateTree()
    {
        behaviorTree = new Tree<AICocky>
        (
            new Selector<AICocky>
            (
                new Sequence<AICocky>
                (
                    new IsAngry(),
                    new RunAtRef()
                ),
                new RunAtBallAngry()
            )
        );
    }
}

public abstract class CockyRun : RunBase
{
    public override void Run()
    {
        // Implement AIBaseRun
    }
}

public class YellInsults : Node<AICocky>
{
    public override bool Update(AICocky context)
    {
        // Implement Yell Functionality
        return true;
    }
}

public class Gesture : Node<AICocky>
{
    public override bool Update(AICocky context)
    {
        // Implement Gesture Functionality
        return true;
    }
}
