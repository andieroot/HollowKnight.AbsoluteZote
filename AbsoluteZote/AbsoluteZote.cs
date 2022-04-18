using System.Collections.Generic;
using UnityEngine;
using Modding;

namespace AbsoluteZote
{
    public class AbsoluteZote : Mod, ITogglableMod
    {
        private Boss boss;
        public Title title;
        private Statue statue;
        private Arena arena;
        public List<Module> modules = new();
        public AbsoluteZote() : base("AbsoluteZote")
        {
            boss = new(this);
            title = new(this);
            statue = new(this);
            arena = new(this);
        }
        public override string GetVersion() => "1.0.0.0";
        public override List<(string, string)> GetPreloadNames()
        {
            List<(string, string)> preloadNames = new();
            foreach (var module in modules)
            {
                foreach (var name in module.GetPreloadNames())
                {
                    preloadNames.Add(name);
                }
            }
            return preloadNames;
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.PlayMakerFSM.OnEnable += PlayMakerFSMOnEnable;
            ModHooks.LanguageGetHook += LanguageGetHook;
            ModHooks.HeroUpdateHook += HeroUpdateHook;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChanged;
            if (preloadedObjects != null)
            {
                foreach (var module in modules)
                {
                    module.LoadPrefabs(preloadedObjects);
                }
            }
        }
        public void Unload()
        {
            On.PlayMakerFSM.OnEnable -= PlayMakerFSMOnEnable;
            ModHooks.LanguageGetHook -= LanguageGetHook;
            ModHooks.HeroUpdateHook -= HeroUpdateHook;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= ActiveSceneChanged;
        }
        private void PlayMakerFSMOnEnable(On.PlayMakerFSM.orig_OnEnable original, PlayMakerFSM fsm)
        {
            foreach (var module in modules)
            {
                module.UpdateFSM(fsm);
            }
            original(fsm);
        }
        private string LanguageGetHook(string key, string sheet, string text)
        {
            foreach (var module in modules)
            {
                text = module.UpdateText(key, sheet, text);
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
        private void ActiveSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
        {
            foreach (var module in modules)
                module.Initialize(to);
        }
    }
}
