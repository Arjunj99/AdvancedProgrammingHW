using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AICocky : AIBase
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
        behaviorTree = new Tree<AICocky>
        (
            new Selector<AICocky>
            (
                new Sequence<AICocky>
                (
                    new YellInsults(),
                    new Gesture()
                ),
                new CockyRun(),
                new DanceOnGoal()
            )
        );
        return true;
    }
}

public class CockyRun : RunBase
{
    public override void Run()
    {
        throw new System.NotImplementedException();
    }
}

public class DanceOnGoal : GoalBase
{
    public override void GoalAction()
    {
        throw new System.NotImplementedException();
    }
}

public class YellInsults : Node<AICocky>
{
    public override bool Update(AICocky context)
    {
        throw new System.NotImplementedException();
    }
}

public class Gesture : Node<AICocky>
{
    public override bool Update(AICocky context)
    {
        throw new System.NotImplementedException();
    }
}
