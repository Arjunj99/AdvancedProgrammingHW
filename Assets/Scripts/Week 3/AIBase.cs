using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public abstract class AIBase : MonoBehaviour
{
    public float AISpeed;
    protected Tree<AIBase> behaviorTree;

    public abstract bool CreateTree();
}

public abstract class RunBase : Node<AIBase>
{
    public abstract void Run();

    public override bool Update(AIBase context)
    {
        Run();
        return true;
    }
}
