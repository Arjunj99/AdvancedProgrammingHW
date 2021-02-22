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
        // Implement AIBaseRun
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
