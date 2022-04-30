namespace AbsoluteZote;

public partial class Control : Module
{
    private void LoadPrefabsRoar(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        var battleControl = preloadedObjects["GG_Mighty_Zote"]["Battle Control"];
        var names = new List<(string, string, string)> { ("Zote Thwomp", "", "Thwomp Zoteling"), };
        foreach ((string group, string instance, string name) in names)
        {
            GameObject minion;
            if (instance != "")
            {
                minion = battleControl.transform.Find(group).gameObject.transform.Find(instance).gameObject;
            }
            else
            {
                minion = battleControl.transform.Find(group).gameObject;
            }
            UnityEngine.Object.Destroy(minion.GetComponent<PersistentBoolItem>());
            UnityEngine.Object.Destroy(minion.GetComponent<ConstrainPosition>());
            prefabs[name] = minion;
        }
    }
    private void UpdateFSMRoar(PlayMakerFSM fsm)
    {
        if (fsm.gameObject.scene.name == "GG_Grey_Prince_Zote" && fsm.gameObject.name.StartsWith("Zote Balloon ") && fsm.FsmName == "Control")
        {
            fsm.AddCustomAction("Set Pos", () =>
            {
                GameObject minion = prefabs["Thwomp Zoteling"] as GameObject;
                minion = UnityEngine.Object.Instantiate(minion);
                minion.SetActive(true);
                minion.SetActiveChildren(true);
                minion.transform.position = new Vector3(fsm.gameObject.transform.position.x, 23.4f, fsm.gameObject.transform.position.z);
                minion.transform.SetScaleX(0.5f * minion.transform.localScale.x);
                minion.transform.SetScaleY(0.5f * minion.transform.localScale.y);
                minion.transform.SetScaleZ(0.5f * minion.transform.localScale.z);
                fsm.SetState("Dormant");
            });
        }
        else if (fsm.gameObject.scene.name == "GG_Grey_Prince_Zote" && fsm.gameObject.name.StartsWith("Zote Balloon") && fsm.FsmName == "Control")
        {
            fsm.AddCustomAction("Set Pos", () =>
            {
                GameObject minion = prefabs["Thwomp Zoteling"] as GameObject;
                minion = UnityEngine.Object.Instantiate(minion);
                minion.SetActive(true);
                minion.SetActiveChildren(true);
                minion.transform.position = new Vector3(fsm.gameObject.transform.position.x, 23.4f, fsm.gameObject.transform.position.z);
                minion.transform.SetScaleX(0.5f * minion.transform.localScale.x);
                minion.transform.SetScaleY(0.5f * minion.transform.localScale.y);
                minion.transform.SetScaleZ(0.5f * minion.transform.localScale.z);
                minion = prefabs["Thwomp Zoteling"] as GameObject;
                minion = UnityEngine.Object.Instantiate(minion);
                minion.SetActive(true);
                minion.SetActiveChildren(true);
                minion.transform.position = new Vector3(26.4f + (float)(1 - random.NextDouble() * 2) * 10, 23.4f, fsm.gameObject.transform.position.z - 1e-2f);
                minion.transform.SetScaleX(1.25f * minion.transform.localScale.x);
                minion.transform.SetScaleY(1.25f * minion.transform.localScale.y);
                minion.transform.SetScaleZ(1.25f * minion.transform.localScale.z);
                fsm.SetState("Dormant");
            });
        }
        else if (fsm.gameObject.scene.name == "GG_Grey_Prince_Zote" && fsm.gameObject.name == "Zote Thwomp(Clone)" && fsm.FsmName == "Control")
        {
            fsm.AddTransition("Dormant", "FINISHED", "Set Pos");
            fsm.InsertCustomAction("Set Pos", () =>
            {
                fsm.FsmVariables.GetFsmFloat("X Pos").Value = fsm.gameObject.transform.position.x;
                fsm.gameObject.transform.Find("Enemy Crusher").gameObject.SetActive(false);
            }, 1);
            fsm.RemoveAction("Set Pos", 2);
            fsm.RemoveAction("Slam", 5);
            fsm.RemoveAction("Down", 6);
            fsm.InsertCustomAction("Waves", () =>
            {
                fsm.SendEvent("FINISHED");
            }, 0);
            fsm.RemoveAction("Rise", 7);
        }
    }
}