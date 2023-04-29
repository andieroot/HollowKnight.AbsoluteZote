namespace AnyZote;

public partial class Control : Module
{
    private void LoadPrefabsJumpSlash(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
    }
    private void UpdateFSMJumpSlash(PlayMakerFSM fsm)
    {
        fsm.InsertCustomAction("Stomp", () =>
        {
            if (random.Next(2) == 1)
            {
                fsm.SetState("Shift Dir");
            }
        }, 0);
        void setWaveScale(PlayMakerFSM fsm)
        {
            var wave = fsm.FsmVariables.GetFsmGameObject("Shockwave").Value;
            wave.transform.SetScaleX(6);
        }
        fsm.InsertCustomAction("Slash Waves L", () => setWaveScale(fsm), 4);
        fsm.InsertCustomAction("Slash Waves R", () => setWaveScale(fsm), 4);
    }
}