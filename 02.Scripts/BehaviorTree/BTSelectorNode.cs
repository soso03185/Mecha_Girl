using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelectorNode : BTNode
{
    public override BTNodeState Evaluate()
    {
        foreach (BTNode node in m_children)
        {
            BTNodeState result = node.Evaluate();

            switch (result)
            {
                case BTNodeState.Success:
                    return BTNodeState.Success;
                case BTNodeState.Running:
                    return BTNodeState.Running;
            }
        }

        return BTNodeState.Failure;
    }
}
