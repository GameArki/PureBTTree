namespace JackFrame.BTTreeNS {

    internal class BTTreeNodeSelectorComponent {

        IBTTreeNode node;

        BTTreeNode activedChild;

        internal BTTreeNodeSelectorComponent(IBTTreeNode node) {
            this.node = node;
        }

        internal void Remove(BTTreeNode child) {
            if (activedChild == child) {
                activedChild = null;
            }
        }

        internal bool Evaluate() {

            if (activedChild == null) {
                var children = node.Children;
                for (int i = 0; i < children.Count; i += 1) {
                    var child = children[i];
                    if (child.CanEnter()) {
                        activedChild = child;
                        activedChild.SetNodeStatus(BTTreeNodeStatus.Execute);
                        return true;
                    }
                }
                return false;
            } else {
                bool res = activedChild.Evaluate();
                if (res) {
                    return true;
                } else {
                    Reset();
                    return false;
                }
            }

        }

        internal void Tick() {
            if (activedChild != null) {
                activedChild.Tick();
            }
        }

        internal void Reset() {
            node.Children.ForEach(value => value.SetNodeStatus(BTTreeNodeStatus.Idle));
            activedChild = null;
        }

    }
}