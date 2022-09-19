using System.Collections.Generic;

namespace JackFrame.BTTreeNS {

    internal interface IBTTreeNode {
        List<BTTreeNode> Children { get; }
    }

}