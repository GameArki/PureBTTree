namespace JackFrame.FSMNS {

    public interface IFSMState {

        int StateID { get; }
        void Enter();
        bool Execute();
        void Exit();

    }

}