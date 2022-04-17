using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMaker;
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
            ModHooks.LanguageGetHook += LanguageGetHook;
            ModHooks.HeroUpdateHook += HeroUpdateHook;
            title.LoadPrefabs(preloadedObjects);
        }
        private void UpgradeFSM(PlayMakerFSM fsm)
        {
            if (fsm.gameObject.scene.name == "GG_Grey_Prince_Zote" && fsm.gameObject.name == "Grey Prince" && fsm.FsmName == "Control")
            {
                fsm.InsertCustomAction("Enter 1", () =>
                {
                    foreach (var f in GameCameras.instance.hudCanvas.GetComponentsInChildren<PlayMakerFSM>())
                    {
                        f.SendEvent("OUT");
                    }
                }, 0);
                var title_ = Object.Instantiate(title.prefabs["title"]);
                var background = Object.Instantiate(title.prefabs["background"]);
                fsm.InsertCustomAction("Roar", () =>
                {
                    var title = GameObject.Find("title(Clone)");
                    title.GetComponent<FadeGroup>().FadeUp();
                    background.SetActive(true);
                }, 0);
                fsm.AddCustomAction("Roar End", () =>
                {
                    var title = GameObject.Find("title(Clone)");
                    title.GetComponent<FadeGroup>().FadeDown();
                    background.SetActive(false);
                    foreach (var f in GameCameras.instance.hudCanvas.GetComponentsInChildren<PlayMakerFSM>())
                    {
                        f.SendEvent("IN");
                    }
                });
            }
        }
        private void PlayMakerFSMOnEnable(On.PlayMakerFSM.orig_OnEnable original, PlayMakerFSM fsm)
        {
            UpgradeFSM(fsm);
            title.UpgradeFSM(fsm);
            original(fsm);
        }
        private string LanguageGetHook(string key, string sheet, string text)
        {
            if (key == "ABSOLUTE_ZOTE_MAIN" && sheet == "Titles")
            {
                text = "无上左特";
            }
            return text;
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
