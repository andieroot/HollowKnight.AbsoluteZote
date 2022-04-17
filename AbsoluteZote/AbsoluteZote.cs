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
        private Boss boss;
        public Title title;
        public AbsoluteZote() : base("AbsoluteZote")
        {
            boss = new(this);
            title = new(this);
        }
        public override string GetVersion() => "1.0";
        public override List<(string, string)> GetPreloadNames()
        {
            List<(string, string)> preloadNames = new();
            foreach (var name in boss.GetPreloadNames())
                preloadNames.Add(name);
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
        private void PlayMakerFSMOnEnable(On.PlayMakerFSM.orig_OnEnable original, PlayMakerFSM fsm)
        {
            boss.UpdateFSM(fsm);
            title.UpdateFSM(fsm);
            original(fsm);
        }
        private string LanguageGetHook(string key, string sheet, string text)
        {
            text = boss.UpdateText(key, sheet, text);
            text = title.UpdateText(key, sheet, text);
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
