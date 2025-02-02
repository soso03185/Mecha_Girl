using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTActionNode : BTNode
{
    private readonly System.Func<BTNodeState> m_action;

    public BTActionNode(System.Func<BTNodeState> action)
    {
        m_action = action;
    }
    public override BTNodeState Evaluate()
    {
        return m_action();
    }
}
