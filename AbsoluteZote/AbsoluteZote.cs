using System.Collections.Generic;
using UnityEngine;
using Modding;
using Satchel;


namespace AbsoluteZote
{
    public class AbsoluteZote : Mod
    {
        private Title title;
        public AbsoluteZote() : base("AbsoluteZote")
        {
            title = new(this);
        }
        public override string GetVersion() => "1.0";
        public override List<(string, string)> GetPreloadNames()
        {
            List<(string, string)> preloadNames = new();
            foreach (var name in title.GetPreloadNames())
                preloadNames.Add(name);
            return preloadNames;
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.PlayMakerFSM.OnEnable += PlayMakerFSMOnEnable;
            ModHooks.HeroUpdateHook += HeroUpdateHook;
            title.LoadPrefabs(preloadedObjects);
        }
        private void UpgradeFSM(PlayMakerFSM fsm)
        {
            if (fsm.gameObject.scene.name == "GG_Grey_Prince_Zote" && fsm.gameObject.name == "Grey Prince" && fsm.FsmName == "Control")
            {
                Object.Instantiate(title.prefabs["bossTitle"]);
                fsm.InsertCustomAction("Roar", () =>
                {
                    var bossTitle = GameObject.Find("Boss Title(Clone)");
                    bossTitle.GetComponent<FadeGroup>().FadeUp();
                }, 0);
                fsm.AddCustomAction("Roar End", () =>
                {
                    var bossTitle = GameObject.Find("Boss Title(Clone)");
                    bossTitle.GetComponent<FadeGroup>().FadeDown();
                });
            }
        }
        private void PlayMakerFSMOnEnable(On.PlayMakerFSM.orig_OnEnable original, PlayMakerFSM fsm)
        {
            UpgradeFSM(fsm);
            title.UpgradeFSM(fsm);
            original(fsm);
        }
        private void HeroUpdateHook()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GG_Grey_Prince_Zote");
            }
        }
    }
}
