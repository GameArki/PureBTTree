using System.Collections.Generic;

namespace GameArki.BTTreeNS {

    internal interface IBTTreeNode {
        List<BTTreeNode> Children { get; }
    }

}