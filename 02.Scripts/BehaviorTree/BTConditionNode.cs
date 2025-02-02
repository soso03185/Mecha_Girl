using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTConditionNode : BTNode
{
    private readonly System.Func<bool> m_condition;

    public BTConditionNode(System.Func<bool> condition)
    {
        this.m_condition = condition;
    }

    public override BTNodeState Evaluate()
    {
        return m_condition() ? BTNodeState.Success : BTNodeState.Failure;
    }
}
