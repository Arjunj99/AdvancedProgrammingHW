using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AIScared : AIBase
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
        //behaviorTree = new Tree<AIScared>
        //(
        //    new Selector<AIScared>
        //    (
        //        new Sequence<AIScared>
        //        (
        //            new Apologize(),
        //            new Cower()
        //        ),
        //        new ScaredRun(),
        //        new ApologizeOnGoal()
        //    )
        //);
        return true;
    }
}

public class ScaredRun : RunBase
{
    public override void Run()
    {
        throw new System.NotImplementedException();
    }
}

public class ApologizeOnGoal : GoalBase
{
    public override void GoalAction()
    {
        throw new System.NotImplementedException();
    }
}

public class Cower : Node<AIScared>
{
    public override bool Update(AIScared context)
    {
        throw new System.NotImplementedException();
    }
}

public class Apologize : Node<AIScared>
{
    public override bool Update(AIScared context)
    {
        throw new System.NotImplementedException();
    }
}