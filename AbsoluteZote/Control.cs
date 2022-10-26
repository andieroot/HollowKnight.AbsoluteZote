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
            ("GG_Mighty_Zote", "Battle Control"),
            ("GG_Nosk_Hornet", "Battle Scene"),
            ("GG_Sly","Battle Scene"),
        };
    }
    public override void LoadPrefabs(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        LoadPrefabsDashSlash(preloadedObjects);
        LoadPrefabsGreatSlash(preloadedObjects);
        LoadPrefabsCycloneSlash(preloadedObjects);
        LoadPrefabsRoll(preloadedObjects);
        LoadPrefabsJump(preloadedObjects);
        LoadPrefabsFall(preloadedObjects);
        LoadPrefabsRoar(preloadedObjects);
        LoadPrefabsJumpSlash(preloadedObjects);
        LoadPrefabsCharge(preloadedObjects);
        LoadPrefabsEvade(preloadedObjects);
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
            fsm.AccessBoolVariable("rolled").Value = false;
            UpdateStateEnter1(fsm);
            UpdateStateRoar(fsm);
            UpdateStateRoarEnd(fsm);
            UpdateStateSendEvent(fsm);
            UpdateStateStun(fsm);
            UpdateStateMoveChoice3(fsm);
            UpdateFSMDashSlash(fsm);
            UpdateFSMGreatSlash(fsm);
            UpdateFSMCycloneSlash(fsm);
            UpdateFSMRoll(fsm);
            UpdateFSMJump(fsm);
            UpdateFSMFall(fsm);
            UpdateFSMJumpSlash(fsm);
            UpdateFSMCharge(fsm);
            UpdateFSMEvade(fsm);
        }
        UpdateFSMRoar(fsm);
    }
    private void UpdateStateEnter1(PlayMakerFSM fsm)
    {
        if (absoluteZote_.settings_.skipIntro == 0)
        {
            fsm.InsertCustomAction("Enter 1", absoluteZote_.title.HideHUD, 0);
        }
        fsm.AddCustomAction("Enter 1", () =>
        {
            fsm.gameObject.GetComponent<HealthManager>().hp = 3000;
            if (absoluteZote_.settings_.skipIntro == 0)
            {
                fsm.SetState("Enter Short");
            }
            else
            {
                fsm.SetState("Roar End");
            }
        });
    }
    private void UpdateStateRoar(PlayMakerFSM fsm)
    {
        if (absoluteZote_.settings_.skipIntro == 0)
        {
            fsm.InsertCustomAction("Roar", absoluteZote_.title.ShowTitle, 0);
        }
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
            fsm.gameObject.transform.Find("greatSlashChargeChargeEffect").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("greatSlashChargeNACharge").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("greatSlashChargeNACharged").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("greatSlashSlashFlash1").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("greatSlashSlashFlash2").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("gs1").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("gse1").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("gse2").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneSlashChargeChargeEffect").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneSlashChargeNACharge").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneSlashChargeNACharged").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneSlashSlashFlash1").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneSlashSlashFlash2").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneTink").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneTink2").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneEffect").gameObject.SetActive(false);
        }, 0);
    }
    private void UpdateStateRoarEnd(PlayMakerFSM fsm)
    {
        if (absoluteZote_.settings_.skipIntro == 0)
        {
            fsm.AddCustomAction("Roar End", absoluteZote_.title.HideTitle);
        }
    }
    private void UpdateStateStun(PlayMakerFSM fsm)
    {
        fsm.ChangeTransition("Stun", "TOOK DAMAGE", "Move Choice 3");
        fsm.ChangeTransition("Stun", "FINISHED", "Move Choice 3");
    }
    private void UpdateStateMoveChoice3(PlayMakerFSM fsm)
    {
        var index = 0;
        var last = new Dictionary<string, int>();
        var regluarMoves = new List<string>()
        {
            "Set Jumps",
            "FT Through",
            "Roar Check",
            "JS Antic",
            "Charge Antic",
            "Dash Slash Jump Antic",
            "Great Slash Jump Antic",
            "Cyclone Slash Jump Antic",
            "Great Slash Jump Antic",
            "Cyclone Slash Jump Antic"
        };
        foreach (var regluarMove in regluarMoves)
        {
            last[regluarMove] = -1;
        }
        fsm.InsertCustomAction("Move Choice 3", () =>
        {
            fsm.gameObject.transform.Find("gs1").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("gse1").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("gse2").gameObject.SetActive(false);
            if (fsm.gameObject.GetComponent<HealthManager>().hp < 1500 && !fsm.AccessBoolVariable("rolled").Value)
            {
                fsm.SetState("Roll Jump Antic");
                fsm.AccessBoolVariable("rolled").Value = true;
                return;
            }
            foreach (var regularMove in regluarMoves)
            {
                if ((index - last[regularMove]) > 1.5 * regularMove.Length)
                {
                    fsm.SetState(regularMove);
                    last[regularMove] = index;
                    index += 1;
                    return;
                }
            }
            var chosenMove = regluarMoves[UnityEngine.Random.Range(0, regluarMoves.Count)];
            fsm.SetState(chosenMove);
            last[chosenMove] = index;
            index += 1;
        }, 0);
    }
}
