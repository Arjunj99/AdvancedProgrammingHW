using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AIFriendly : AIBase
{
    protected void Awake()
    {
        CreateTree();
    }

    // Update is called once per frame
    void Update()
    {
        behaviorTree.Update(this);
    }

    public override bool CreateTree()
    {
        behaviorTree = new Tree<AIFriendly>
        (
            new Selector<AIFriendly>
            (
                new Sequence<AIFriendly>
                (
                    new Greet(),
                    new CheerOn()
                ),
                new FriendlyRun(),
                new CheerOnGoal()
            )
        );
        return true;
    }
}

public class FriendlyRun : RunBase
{
    public override void Run()
    {
        throw new System.NotImplementedException();
    }
}

public class CheerOnGoal : GoalBase
{
    public override void GoalAction()
    {
        throw new System.NotImplementedException();
    }
}

public class Greet : Node<AIFriendly>
{
    public override bool Update(AIFriendly context)
    {
        throw new System.NotImplementedException();
    }
}

public class CheerOn : Node<AIFriendly>
{
    public override bool Update(AIFriendly context)
    {
        throw new System.NotImplementedException();
    }
}