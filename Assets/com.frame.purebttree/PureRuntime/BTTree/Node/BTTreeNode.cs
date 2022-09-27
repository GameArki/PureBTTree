using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace JackFrame.BTTreeNS {

    public sealed class BTTreeNode : IBTTreeNode {

        // if nodeType == Action
        //      no children
        // else
        //      no action
        BTTreeNodeType nodeType;
        public BTTreeNodeType NodeType => nodeType;

        BTTreeNodeStatus nodeStatus;
        public BTTreeNodeStatus NodeStatus => nodeStatus;
        public void SetNodeStatus(BTTreeNodeStatus status) => nodeStatus = status;

        IBTTreePrecondition precondition;

        List<BTTreeNode> children;
        List<BTTreeNode> IBTTreeNode.Children => children;
        public List<BTTreeNode> GetChildren => children;

        BTTreeNodeSelectorComponent selectorComponent;
        BTTreeNodeSequenceComponent sequenceComponent;
        BTTreeNodeParallelComponent parallelComponent;
        BTTreeNodeActionComponent actionComponent;

        bool isActivated;
        public bool IsActivated => isActivated;

        internal BTTreeNode(BTTreeNodeType nodeType, IBTTreePrecondition precondition, IBTTreeAction action) {

            this.precondition = precondition;

            this.nodeType = nodeType;
            if (nodeType == BTTreeNodeType.Selector) {
                this.children = new List<BTTreeNode>();
                this.selectorComponent = new BTTreeNodeSelectorComponent(this);
            } else if (nodeType == BTTreeNodeType.Sequence) {
                this.children = new List<BTTreeNode>();
                this.sequenceComponent = new BTTreeNodeSequenceComponent(this);
            } else if (nodeType == BTTreeNodeType.ParallelAnd) {
                this.children = new List<BTTreeNode>();
                this.parallelComponent = new BTTreeNodeParallelComponent(this, true);
            } else if (nodeType == BTTreeNodeType.ParallelOr) {
                this.children = new List<BTTreeNode>();
                this.parallelComponent = new BTTreeNodeParallelComponent(this, false);
            } else if (nodeType == BTTreeNodeType.Action) {
                this.children = new List<BTTreeNode>(0);
                this.actionComponent = new BTTreeNodeActionComponent(this, action);
            }
        }

        public void Activate() {
            this.isActivated = true;
            children?.ForEach(value => value.Activate());
        }

        public void Deactivate() {
            this.isActivated = false;
            children?.ForEach(value => value.Deactivate());
        }

        public void AddChild(BTTreeNode node) {
            if (nodeType == BTTreeNodeType.Action) {
                throw new System.Exception("Dont Add Child Into An Action");
            }
            children.Add(node);
            if (nodeType == BTTreeNodeType.ParallelOr || nodeType == BTTreeNodeType.ParallelAnd) {
                parallelComponent.AddRes();
            }
        }

        public void RemoveChild(BTTreeNode node) {
            int index = children.FindIndex(value => value == node);
            if (index != -1) {
                var child = children[index];
                if (nodeType == BTTreeNodeType.ParallelOr || nodeType == BTTreeNodeType.ParallelAnd) {
                    parallelComponent.RemoveResAt(index);
                } else if (nodeType == BTTreeNodeType.Selector) {
                    selectorComponent.Remove(child);
                }
                children.RemoveAt(index);
            }
        }

        internal bool CanEnter() {
            if (!isActivated) {
                return false;
            }
            return precondition.CanEnter();
        }

        internal bool Evaluate() {

            if (!isActivated) {
                return false;
            }

            if (nodeType == BTTreeNodeType.Selector) {
                return selectorComponent.Evaluate();
            } else if (nodeType == BTTreeNodeType.Sequence) {
                return sequenceComponent.Evaluate();
            } else if (nodeType == BTTreeNodeType.Action) {
                return actionComponent.Evaluate();
            } else if (nodeType == BTTreeNodeType.ParallelAnd) {
                return parallelComponent.Evaluate();
            } else {
                throw new System.Exception("Node 节点类型不可为 None");
            }

        }

        internal void Tick() {

            if (!isActivated) {
                return;
            }

            if (nodeType == BTTreeNodeType.Selector) {
                selectorComponent.Tick();
            } else if (nodeType == BTTreeNodeType.Sequence) {
                sequenceComponent.Tick();
            } else if (nodeType == BTTreeNodeType.Action) {
                actionComponent.Tick();
            } else if (nodeType == BTTreeNodeType.ParallelAnd) {
                parallelComponent.Tick();
            } else {
                throw new System.Exception("Node 节点类型不可为 None");
            }

        }

        public void Reset() {
            children.ForEach(value => value.Reset());
            selectorComponent?.Reset();
            sequenceComponent?.Reset();
            parallelComponent?.Reset();
            actionComponent?.Reset();
            nodeStatus = BTTreeNodeStatus.Idle;
        }

        public string GetString() {
            StringBuilder sb = new StringBuilder();
            sb.Append(nodeType.ToString());
            if (nodeType == BTTreeNodeType.Action) {
                sb.Append($": {actionComponent.GetString()}");
            }
            return sb.ToString();
        }

    }

}