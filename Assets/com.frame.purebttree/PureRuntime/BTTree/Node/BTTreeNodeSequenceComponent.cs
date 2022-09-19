namespace JackFrame.BTTreeNS {

    internal class BTTreeNodeSequenceComponent {

        IBTTreeNode node;

        int activeIndex;
        int nextIndex;

        internal BTTreeNodeSequenceComponent(IBTTreeNode node) {
            this.node = node;
        }

        internal bool Evaluate() {

            // 准备进入下一个
            var children = node.Children;
            if (activeIndex >= children.Count) {
                Reset();
                return false;
            }

            if (activeIndex == nextIndex) {
                var next = children[nextIndex];
                if (next.CanEnter()) {
                    next.SetNodeStatus(BTTreeNodeStatus.Execute);
                    nextIndex += 1;
                    return true;
                }
            }

            // 执行当前的
            var activeChild = children[activeIndex];
            bool res = activeChild.Evaluate();
            if (res) {
                return true;
            } else {
                if (nextIndex >= children.Count) {
                    // 整个 Sequence 执行结束
                    Reset();
                    return false;
                } else {
                    // 还有待执行的
                    activeChild.SetNodeStatus(BTTreeNodeStatus.Idle);
                    activeIndex = nextIndex;
                    return true;
                }
            }

        }

        internal void Tick() {
            var children = node.Children;
            if (activeIndex >= children.Count) {
                return;
            }
            var activeChild = children[activeIndex];
            activeChild.Tick();
        }

        internal void Reset() {
            node.Children.ForEach(value => value.SetNodeStatus(BTTreeNodeStatus.Idle));
            activeIndex = 0;
            nextIndex = 0;
        }

    }
}