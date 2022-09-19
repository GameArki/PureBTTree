namespace JackFrame.BTTreeNS {

    public class BTTreePrecondition_AlwaysTrue : IBTTreePrecondition {

        public static IBTTreePrecondition Empty = new BTTreePrecondition_AlwaysTrue();

        public BTTreePrecondition_AlwaysTrue() {}

        public bool CanEnter() => true;

    }

}