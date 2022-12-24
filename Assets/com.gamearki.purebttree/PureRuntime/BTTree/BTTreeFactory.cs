namespace GameArki.BTTreeNS {

    public static class BTTreeFactory {

        // ==== Container ====
        public static BTTreeNode CreateSelectorNode(IBTTreePrecondition precondition = null) {
            precondition = NotNullPrecondition(precondition);
            BTTreeNode node = new BTTreeNode(BTTreeNodeType.Selector, NotNullPrecondition(precondition), null);
            return node;
        }

        public static BTTreeNode CreateSequenceNode(IBTTreePrecondition precondition = null) {
            precondition = NotNullPrecondition(precondition);
            BTTreeNode node = new BTTreeNode(BTTreeNodeType.Sequence, precondition, null);
            return node;
        }

        public static BTTreeNode CreateParallelAndNode(IBTTreePrecondition precondition = null) {
            precondition = NotNullPrecondition(precondition);
            BTTreeNode node = new BTTreeNode(BTTreeNodeType.ParallelAnd, precondition, null);
            return node;
        }

        public static BTTreeNode CreateParallelOrNode(IBTTreePrecondition precondition = null) {
            precondition = NotNullPrecondition(precondition);
            BTTreeNode node = new BTTreeNode(BTTreeNodeType.ParallelOr, precondition, null);
            return node;
        }

        // ==== Action ====
        public static BTTreeNode CreateActionNode(IBTTreeAction action, IBTTreePrecondition precondition = null) {
            if (action == null) {
                throw new System.Exception("Action Cant Be Null");
            }
            precondition = NotNullPrecondition(precondition);
            BTTreeNode node = new BTTreeNode(BTTreeNodeType.Action, precondition, action);
            return node;
        }

        // ==== Precondition ====
        static IBTTreePrecondition NotNullPrecondition(IBTTreePrecondition precondition) {
            if (precondition == null) {
                return BTTreePrecondition_AlwaysTrue.Empty;
            } else {
                return precondition;
            }
        }

    }

}