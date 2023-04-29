namespace AnyZote;

public partial class Control : Module
{
    private void LoadPrefabsCharge(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
    }
    private void UpdateFSMCharge(PlayMakerFSM fsm)
    {
        fsm.InsertCustomAction("Charge Start", () =>
        {
            fsm.AccessFloatVariable("Charge Timer").Value = (float)random.NextDouble() * 4;
        }, 6);
        fsm.InsertCustomAction("Charge Fall", () =>
        {
            fsm.SetState("Jump Antic");
        }, 0);
    }
}