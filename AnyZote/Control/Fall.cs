namespace AnyZote;

public partial class Control : Module
{
    private void LoadPrefabsFall(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        var battleScene = preloadedObjects["GG_Traitor_Lord"]["Battle Scene"];
        var traitorLord = battleScene.transform.Find("Wave 3").gameObject.transform.Find("Mantis Traitor Lord").gameObject;
        var fsm = traitorLord.LocateMyFSM("Mantis");
        var wave = fsm.GetAction<SpawnObjectFromGlobalPool>("Waves", 0).gameObject.Value;
        wave.transform.Find("slash_core").gameObject.transform.Find("hurtbox").gameObject.GetComponent<DamageHero>().damageDealt = 1;
        prefabs["traitorLordWave"] = wave;
    }
    private void UpdateFSMFall(PlayMakerFSM fsm)
    {
        fsm.AddState("Fall Next");
        fsm.InsertCustomAction("FT Through", () =>
        {
            (fsm.GetState("FT Through").Actions[6] as Wait).time = 0.75f;
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
        fsm.InsertCustomAction("Ft Waves", () =>
        {
            var prefab = prefabs["traitorLordWave"];
            var wave = UnityEngine.Object.Instantiate(prefab as GameObject);
            wave.transform.position = new Vector3(fsm.gameObject.transform.position.x, 2, 1);
            wave.GetComponent<Rigidbody2D>().velocity = new Vector2(12, 0);
            wave = UnityEngine.Object.Instantiate(prefab as GameObject);
            wave.transform.position = new Vector3(fsm.gameObject.transform.position.x, 2, 1);
            wave.transform.localScale = new Vector3(-1, 1, 1);
            wave.GetComponent<Rigidbody2D>().velocity = new Vector2(-12, 0);
            fsm.SendEvent("FINISHED");
        }, 1);
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