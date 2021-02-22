using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AIScared : AIBase
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        behaviorTree.Update(this);
    }

    public override bool CreateTree()
    {
        behaviorTree = new Tree<AIScared>
        (
            new Selector<AIScared>
            (
                new Sequence<AIScared>
                (
                    new Apologize(),
                    new Cower()
                ),
                new ScaredRun()
            )
        );
        return true;
    }
}

public class ScaredRun : RunBase
{
    public override void Run()
    {
        // Implement AIBaseRun
    }
}

public class Apologize : Node<AIScared>
{
    public override bool Update(AIScared context)
    {
        // Implement Yell Functionality
        throw new System.NotImplementedException();
    }
}

public class Cower : Node<AIScared>
{
    public override bool Update(AIScared context)
    {
        // Implement Gesture Functionality
        throw new System.NotImplementedException();
    }
}

