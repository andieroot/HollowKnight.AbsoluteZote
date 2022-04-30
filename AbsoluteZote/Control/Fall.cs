namespace AbsoluteZote;

public partial class Control : Module
{
    private void LoadPrefabsFall(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
    }
    private void UpdateFSMFall(PlayMakerFSM fsm)
    {
        fsm.AddState("Fall Next");
        fsm.InsertCustomAction("FT Through", () =>
        {
            (fsm.GetState("FT Through").Actions[6] as Wait).time = 0.1f;
        }, 0);
        fsm.AddAction("FT Through", fsm.CreateTk2dPlayAnimationWithEvents(
            fsm.gameObject, "Jump", null));
        fsm.RemoveAction("FT Slam", 4);
        fsm.AddCustomAction("FT Slam", () =>
        {
            fsm.SendEvent("WAIT");
        });
        fsm.ChangeTransition("FT Slam", "WAIT", "Fall Next");
        UpdateStateFallNext(fsm);
    }
    private void UpdateStateFallNext(PlayMakerFSM fsm)
    {
        fsm.AddAction("Fall Next", fsm.CreateWait(0.75f, fsm.GetFSMEvent("1")));
        fsm.AddCustomAction("Fall Next", () =>
        {
            if (random.Next(2) == 1)
            {
                fsm.SetState("FT Through");
            }
        });
        fsm.AddTransition("Fall Next", "1", "FT Recover");
    }
}