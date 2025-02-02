using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequenceNode : BTNode
{
    public override BTNodeState Evaluate()
    {
        bool isAnyChildRunning = false;

        foreach (BTNode node in m_children)
        {
            BTNodeState result = node.Evaluate();

            switch (result)
            {
                case BTNodeState.Failure:
                    return BTNodeState.Failure;

                case BTNodeState.Running: 
                    isAnyChildRunning = true;
                    break;
            }
        }

        return isAnyChildRunning ? BTNodeState.Running : BTNodeState.Success;
    }
}
