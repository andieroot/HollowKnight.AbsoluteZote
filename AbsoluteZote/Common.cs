using Satchel;
using Modding;


namespace AbsoluteZote
{
    public static class Common { 
        public static void LogFSM(this Mod mod, PlayMakerFSM fsm, System.Action function = null)
        {
            foreach (var state in fsm.FsmStates)
            {
                FsmUtil.InsertCustomAction(fsm, state.Name, () =>
                {
                    mod.LogDebug("FSM: " + fsm.gameObject.name + "-" + fsm.FsmName + " entering state: " + state.Name + ".");
                    function?.Invoke();
                }, 0);
            }
        }
        public static void LogFSMState(this Mod mod, PlayMakerFSM fsm, string state, System.Action function = null)
        {
            for (int i = fsm.GetState(state).Actions.Length; i >= 0; i--)
            {
                FsmUtil.InsertCustomAction(fsm, state, () =>
                {
                    mod.LogDebug("State: " + fsm.FsmName + "-" + state + " entering action: " + i.ToString() + ".");
                    function?.Invoke();
                }, i);
            }
        }
    }
}