namespace AbsoluteZote;
public partial class Control : Module
{
    public Control(AbsoluteZote absoluteZote) : base(absoluteZote)
    {
    }
    public override List<(string, string)> GetPreloadNames()
    {
        return new List<(string, string)>
        {
            ("GG_Grey_Prince_Zote", "Grey Prince"),
            ("GG_Nailmasters", "Brothers"),
            ("GG_Mighty_Zote","Battle Control"),
        };
    }
    public override void LoadPrefabs(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        LoadPrefabsDashSlash(preloadedObjects);
        LoadPrefabsRoll(preloadedObjects);
        LoadPrefabsJump(preloadedObjects);
        LoadPrefabsFall(preloadedObjects);
        LoadPrefabsRoar(preloadedObjects);
    }
    public override void UpdateHitInstance(HealthManager healthManager, HitInstance hitInstance)
    {
        if (IsGreyPrince(healthManager.gameObject))
        {
            UpdateHitInstanceRoll(healthManager, hitInstance);
        }
    }
    public override void UpdateFSM(PlayMakerFSM fsm)
    {
        if (IsGreyPrince(fsm.gameObject) && fsm.FsmName == "Control")
        {
            UpdateStateEnter1(fsm);
            UpdateStateRoar(fsm);
            UpdateStateRoarEnd(fsm);
            UpdateStateSendEvent(fsm);
            UpdateStateStun(fsm);
            UpdateStateMoveChoice3(fsm);
            UpdateFSMDashSlash(fsm);
            UpdateFSMRoll(fsm);
            UpdateFSMJump(fsm);
            UpdateFSMFall(fsm);
        }
        UpdateFSMRoar(fsm);
    }
    private void UpdateStateEnter1(PlayMakerFSM fsm)
    {
        fsm.InsertCustomAction("Enter 1", absoluteZote_.title.HideHUD, 0);
    }
    private void UpdateStateRoar(PlayMakerFSM fsm)
    {
        fsm.InsertCustomAction("Roar", absoluteZote_.title.ShowTitle, 0);
    }
    private void UpdateStateSendEvent(PlayMakerFSM fsm)
    {
        fsm.InsertCustomAction("Send Event", () =>
        {
            fsm.gameObject.transform.Find("dashSlashChargeChargeEffect").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("dashSlashChargeNACharge").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("dashSlashChargeNACharged").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("dashSlashSlashFlash1").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("dashSlashSlashFlash2").gameObject.SetActive(false);
        }, 0);
    }
    private void UpdateStateRoarEnd(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Roar End", absoluteZote_.title.HideTitle);
    }
    private void UpdateStateStun(PlayMakerFSM fsm)
    {
        fsm.ChangeTransition("Stun", "TOOK DAMAGE", "Move Choice 3");
        fsm.ChangeTransition("Stun", "FINISHED", "Move Choice 3");
    }
    private void UpdateStateMoveChoice3(PlayMakerFSM fsm)
    {
        //2400: Blade Dance Antic
        //1600: Roll Jump Antic
        //800: Laser Net Antic
        //Any: Set Jumps
        //Any: FT Through
        //Any: B Roar Antic
        fsm.InsertCustomAction("Move Choice 3", () =>
        {
            fsm.SetState("B Roar Antic");
        }, 0);
    }
}
