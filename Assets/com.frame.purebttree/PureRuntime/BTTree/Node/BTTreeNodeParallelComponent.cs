using System.Collections.Generic;

namespace JackFrame.BTTreeNS {

    internal class BTTreeNodeParallelComponent {

        enum BTTreeNodeResult : byte {
            NodeReady,
            NodeRunning,
            NodeEnd,
        }

        IBTTreeNode node;

        List<BTTreeNodeResult> resList;

        bool isAnd;

        internal BTTreeNodeParallelComponent(IBTTreeNode node, bool isAnd) {
            this.node = node;
            this.isAnd = isAnd;
            this.resList = new List<BTTreeNodeResult>();
        }

        internal void AddRes() {
            resList.Add(BTTreeNodeResult.NodeReady);
        }

        internal void RemoveResAt(int index) {
            resList.RemoveAt(index);
        }

        internal bool Evaluate() {
            if (isAnd) {
                return EvaluateAnd();
            } else {
                return EvaluateOr();
            }
        }

        bool EvaluateAnd() {
            int endCount = 0;
            var children = node.Children;
            for (int i = 0; i < children.Count; i += 1) {
                var child = children[i];
                var res = resList[i];
                if (res == BTTreeNodeResult.NodeReady) {
                    bool can = child.CanEnter();
                    if (can) {
                        resList[i] = BTTreeNodeResult.NodeRunning;
                    }
                } else if (res == BTTreeNodeResult.NodeRunning) {
                    bool ev = child.Evaluate();
                    if (!ev) {
                        resList[i] = BTTreeNodeResult.NodeEnd;
                        endCount += 1;
                    }
                } else if (res == BTTreeNodeResult.NodeEnd) {
                    endCount += 1;
                }
            }

            if (endCount >= children.Count) {
                Reset();
                return false;
            } else {
                return true;
            }
        }

        bool EvaluateOr() {

            int endCount = 0;
            var children = node.Children;
            for (int i = 0; i < children.Count; i += 1) {
                var child = children[i];
                var res = resList[i];
                if (res == BTTreeNodeResult.NodeReady) {
                    bool can = child.CanEnter();
                    if (can) {
                        child.SetNodeStatus(BTTreeNodeStatus.Execute);
                        resList[i] = BTTreeNodeResult.NodeRunning;
                    }
                } else if (res == BTTreeNodeResult.NodeRunning) {
                    bool ev = child.Evaluate();
                    if (!ev) {
                        child.SetNodeStatus(BTTreeNodeStatus.Idle);
                        endCount += 1;
                        break;
                    }
                }
            }

            if (endCount > 0) {
                Reset();
                return false;
            } else {
                return true;
            }
        }

        internal void Tick() {
            var children = node.Children;
            for (int i = 0; i < children.Count; i += 1) {
                var res = resList[i];
                if (res == BTTreeNodeResult.NodeRunning) {
                    var child = children[i];
                    child.Tick();
                }
            }
        }

        internal void Reset() {
            for (int i = 0; i < resList.Count; i += 1) {
                resList[i] = BTTreeNodeResult.NodeReady;
            }
            node.Children.ForEach(value => value.Reset());
        }

    }
}