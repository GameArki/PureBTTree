namespace JackFrame.BTTreeNS {

    internal class BTTreeNodeActionComponent {

        enum BTTreeActionResult : byte {
            Ready,
            Running,
            End,
        }

        IBTTreeNode node;
        IBTTreeAction action;

        BTTreeActionResult actionResult;

        internal BTTreeNodeActionComponent(IBTTreeNode node, IBTTreeAction action) {
            this.node = node;
            this.action = action;
        }

        internal bool Evaluate() {
            if (actionResult == BTTreeActionResult.Running || actionResult == BTTreeActionResult.Ready) {
                return true;
            } else {
                return false;
            }
        }

        internal void Tick() {
            if (actionResult == BTTreeActionResult.Running) {
                bool res = action.Execute();
                if (!res) {
                    actionResult = BTTreeActionResult.End;
                    action.Exit();
                }
            } else if (actionResult == BTTreeActionResult.Ready) {
                action.Enter();
                actionResult = BTTreeActionResult.Running;
            }
        }

        public string GetString() {
            return action.GetType().Name.ToString();
        }

        internal void Reset() {
            actionResult = BTTreeActionResult.Ready;
        }

    }

}