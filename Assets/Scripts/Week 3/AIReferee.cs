using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class AIReferee : AIBase
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
        behaviorTree = new Tree<AIReferee>
        (
            new Selector<AIReferee>
            (
                new Sequence<AIReferee>
                (
                    new RedCard(),
                    new RegulationCheck()
                ),
                new RefereeRun(),
                new AwardGoal()
            )
        );
        return true;
    }
}

public class RefereeRun : RunBase
{
    public override void Run()
    {
        throw new System.NotImplementedException();
    }
}

public class AwardGoal : GoalBase
{
    public override void GoalAction()
    {
        throw new System.NotImplementedException();
    }
}

public class RedCard : Node<AIReferee>
{
    public override bool Update(AIReferee context)
    {
        throw new System.NotImplementedException();
    }
}

public class RegulationCheck : Node<AIReferee>
{
    public override bool Update(AIReferee context)
    {
        throw new System.NotImplementedException();
    }
}