namespace AnyZote;
public partial class Control : Module
{
    private List<GameObject> toSpit;
    private List<GameObject> turrets = new List<GameObject>();
    private List<GameObject> beams = new List<GameObject>();
    private List<GameObject> turrets2 = new List<GameObject>();
    private List<GameObject> beams2 = new List<GameObject>();
    public Control(AnyZote anyZote) : base(anyZote)
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
            ("GG_Traitor_Lord", "Battle Scene"),
            ("GG_Ghost_Markoth","Warrior"),
            ("GG_Radiance","Boss Control"),
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
        var markoth = preloadedObjects["GG_Ghost_Markoth"]["Warrior"].transform.Find("Ghost Warrior Markoth").gameObject;
        prefabs["Shield"] = markoth.LocateMyFSM("Shield Attack").GetAction<CreateObject>("Init", 1).gameObject.Value;
        var radiance = preloadedObjects["GG_Radiance"]["Boss Control"].transform.Find("Absolute Radiance").gameObject;
        var burst = radiance.transform.Find("Eye Beam Glow").gameObject.transform.Find("Burst 1").gameObject;
        prefabs["Beam"] = burst.transform.Find("Radiant Beam").gameObject;
        prefabs["Radiance"] = radiance;
        var ggStatueGrimm = preloadedObjects["GG_Workshop"]["GG_Statue_Grimm"];
        var Base = ggStatueGrimm.transform.Find("Base").gameObject;
        var Plaque = Base.transform.Find("Plaque").gameObject;
        var Plaque_Trophy_Right = Plaque.transform.Find("Plaque_Trophy_Right").gameObject;
        var Defeated_3 = Plaque_Trophy_Right.transform.Find("Defeated_3").gameObject;
        prefabs["beamGlow"] = Defeated_3;
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
        if (anyZote_.settings_.skipIntro == 0)
        {
            fsm.InsertCustomAction("Enter 1", anyZote_.title.HideHUD, 0);
        }
        fsm.AddCustomAction("Enter 1", () =>
        {
            fsm.gameObject.GetComponent<HealthManager>().hp = 3000;
            if (anyZote_.settings_.skipIntro == 0)
            {
                fsm.SetState("Enter Short");
            }
            else
            {
                fsm.SetState("Roar End");
            }
            Log("Stun before: " + fsm.gameObject.LocateMyFSM("Stun").AccessIntVariable("Stun Combo").Value);
            Log("Stun before: " + fsm.gameObject.LocateMyFSM("Stun").AccessIntVariable("Stun Hit Max").Value);
            fsm.gameObject.LocateMyFSM("Stun").AccessIntVariable("Stun Combo").Value = 65536;
            fsm.gameObject.LocateMyFSM("Stun").AccessIntVariable("Stun Hit Max").Value = 65536;
            Log("Stun after: " + fsm.gameObject.LocateMyFSM("Stun").AccessIntVariable("Stun Combo").Value);
            Log("Stun after: " + fsm.gameObject.LocateMyFSM("Stun").AccessIntVariable("Stun Hit Max").Value);
            var cnt = 6;
            float l = 8.19f, r = 44.61f;
            float g = (r - l) / (cnt - 1);
            turrets.Clear();
            beams.Clear();
            turrets2.Clear();
            beams2.Clear();
            for (int i = 0; i < cnt; i++)
            {
                var minion = prefabs["Turret Zoteling"] as GameObject;
                minion = UnityEngine.Object.Instantiate(minion);
                minion.SetActive(true);
                minion.SetActiveChildren(true);
                minion.transform.position = new Vector3(l + g * i, 19 + (float)random.NextDouble() / 2, fsm.gameObject.transform.position.z);
                turrets.Add(minion);
                var beam = prefabs["Beam"] as GameObject;
                beam = UnityEngine.Object.Instantiate(beam);
                beam.SetActive(true);
                beam.SetActiveChildren(true);
                beam.transform.position = new Vector3(minion.transform.position.x + 0.2f, minion.transform.position.y - 2.35f, minion.transform.position.z);
                beam.transform.rotation = Quaternion.Euler(0, 0, -90);
                beam.LocateMyFSM("Control").AddTransition("Fire", "FINISHED", "End");
                var radiance = prefabs["Radiance"] as GameObject;
                var action = radiance.LocateMyFSM("Attack Commands").GetAction<AudioPlayerOneShotSingle>("Aim", 3);
                action.spawnPoint = minion;
                action.delay = 0;
                beam.LocateMyFSM("Control").AddAction("Fire", action);
                var glow = prefabs["beamGlow"] as GameObject;
                glow = UnityEngine.Object.Instantiate(glow, minion.transform);
                glow.name = "glow";
                glow.transform.localScale *= 1.5f;
                glow.SetActive(false);
                glow.transform.Translate(-0.05f, 2, 0);
                glow.GetAddComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);
                var halo = radiance.transform.Find("Halo").gameObject;
                halo = UnityEngine.Object.Instantiate(halo, minion.transform);
                halo.name = "halo";
                halo.transform.localScale *= 0.2f;
                halo.SetActive(false);
                halo.transform.Translate(0.1f, 0.85f, 0);
                beam.LocateMyFSM("Control").AddCustomAction("Antic", () =>
                {
                    halo.SetActive(true);
                    glow.SetActive(true);
                });
                beam.LocateMyFSM("Control").AddCustomAction("End", () =>
                {
                    halo.SetActive(false);
                    glow.SetActive(false);
                });
                beams.Add(beam);
            }
            turrets2.Add(turrets[0]);
            turrets2.Add(turrets[turrets.Count - 1]);
            beams2.Add(beams[0]);
            beams2.Add(beams[beams.Count - 1]);
            fsm.gameObject.RefreshHPBar();
        });
    }
    private void UpdateStateRoar(PlayMakerFSM fsm)
    {
        if (anyZote_.settings_.skipIntro == 0)
        {
            fsm.InsertCustomAction("Roar", anyZote_.title.ShowTitle, 0);
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
        if (anyZote_.settings_.skipIntro == 0)
        {
            fsm.AddCustomAction("Roar End", anyZote_.title.HideTitle);
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
            "Cyclone Slash Jump Antic",
            "Set Jumps",
            "FT Through",
            "Roar Check",
            "JS Antic",
            "Charge Antic",
            "Dash Slash Jump Antic",
            "Great Slash Jump Antic",
            "Cyclone Slash Jump Antic",
            "Great Slash Jump Antic",
            "Cyclone Slash Jump Antic",
            "Dash Slash Jump Antic",
            "Great Slash Jump Antic",
            "Cyclone Slash Jump Antic",
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
            if (fsm.gameObject.GetComponent<HealthManager>().hp < 2800 && !fsm.AccessBoolVariable("wave1").Value)
            {
                fsm.SetState("Spit Set");
                fsm.AccessBoolVariable("wave1").Value = true;
                toSpit = new List<GameObject>
                {
                    prefabs["Fat Zoteling"] as GameObject,
                    prefabs["Salubra Zoteling"] as GameObject,
                };
                return;
            }
            if (fsm.gameObject.GetComponent<HealthManager>().hp < 1800 && !fsm.AccessBoolVariable("wave2").Value)
            {
                fsm.SetState("Spit Set");
                fsm.AccessBoolVariable("wave2").Value = true;
                toSpit = new List<GameObject>
                {
                    prefabs["Fat Zoteling"] as GameObject,
                    prefabs["Fat Zoteling"] as GameObject,
                };
                return;
            }
            if (fsm.gameObject.GetComponent<HealthManager>().hp < 1500 && !fsm.AccessBoolVariable("rolled").Value)
            {
                fsm.SetState("Roll Jump Antic");
                fsm.AccessBoolVariable("rolled").Value = true;
                return;
            }
            if (fsm.gameObject.GetComponent<HealthManager>().hp < 800 && !fsm.AccessBoolVariable("wave3").Value)
            {
                fsm.SetState("Spit Set");
                fsm.AccessBoolVariable("wave3").Value = true;
                toSpit = new List<GameObject>
                {
                     prefabs["Salubra Zoteling"] as GameObject,
                    prefabs["Salubra Zoteling"] as GameObject,
                     prefabs["Salubra Zoteling"] as GameObject,
                      prefabs["Salubra Zoteling"] as GameObject,
                };
                return;
            }
            List<GameObject>newAlive= new List<GameObject>();
            foreach(var go in alive)
            {
                if (go != null&& !go.name.Contains("Salubra")&&!go.name.Contains("Fat"))
                {
                    newAlive.Add(go);
                }
            }
            alive = newAlive;
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
            if (HeroController.instance.transform.position.y > 15.5f)
            {
                chosenMove = "Great Slash Jump Antic";
            }
            if (alive.Count == 0)
            {
                chosenMove = "Spit Set";
            }
            // regular spit
            if (chosenMove == "Spit Set")
            {
                GameObject[] candidates = [
                    prefabs["Tall Zoteling"] as GameObject,
                    prefabs["Normal Zoteling"] as GameObject,
                    prefabs["Ordeal Zoteling"] as GameObject,
                ];
                var candidate1 = candidates[UnityEngine.Random.Range(0, candidates.Length)];
                var candidate2 = candidates[UnityEngine.Random.Range(0, candidates.Length)];
                Log("Spit choice start");
                Log("spit 1" + candidate1.name);
                Log("Spit 2" + candidate2.name);
                Log("Spit choice end");
                toSpit = new List<GameObject>
                {
                    candidate1,
                    candidate2
                };
            }
            fsm.SetState(chosenMove);
            last[chosenMove] = index;
            index += 1;
        }, 0);
    }
}
