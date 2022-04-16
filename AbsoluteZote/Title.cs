using System.Collections.Generic;
using UnityEngine;
using Modding;
using Satchel;


namespace AbsoluteZote
{
    public class Title
    {
        private readonly Mod mod_;
        public readonly Dictionary<string, GameObject> prefabs = new();
        public Title(Mod mod)
        {
            mod_ = mod;
        }
        private void Log(object message) => mod_.Log(message);
        public List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                 ("GG_Radiance", "Boss Control"),
            };
        }
        public void LoadPrefabs(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            var bossControl = preloadedObjects["GG_Radiance"]["Boss Control"];
            prefabs["bossTitle"] = bossControl.transform.Find("Boss Title").gameObject;
        }
        public void UpgradeFSM(PlayMakerFSM fsm)
        {
            if (fsm.gameObject.scene.name == "GG_Grey_Prince_Zote" && fsm.gameObject.name == "Grey Prince Title" && fsm.FsmName == "Control")
            {
                for (int i = 1; i <= 13; ++i)
                {
                    fsm.RemoveAction("Extra " + i.ToString(), 1);
                }
                fsm.RemoveAction("Main Title", 0);
            }
        }
    }
}