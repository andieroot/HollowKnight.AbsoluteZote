using System.Collections.Generic;
using UnityEngine;
using Modding;
using Satchel;


namespace AbsoluteZote
{
    public class Boss
    {
        private readonly AbsoluteZote absoluteZote_;
        private readonly Dictionary<string, GameObject> prefabs = new();
        public Boss(AbsoluteZote absoluteZote)
        {
            absoluteZote_ = absoluteZote;
        }
        private void Log(object message) => absoluteZote_.Log(message);
        public List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
            };
        }
        public void LoadPrefabs(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
        }
        public void Instantiate()
        {

        }
        public void UpdateFSM(PlayMakerFSM fsm)
        {
            if (fsm.gameObject.scene.name == "GG_Grey_Prince_Zote" && fsm.gameObject.name == "Grey Prince" && fsm.FsmName == "Control")
            {
                fsm.InsertCustomAction("Enter 1", absoluteZote_.title.HideHUD, 0);
                fsm.InsertCustomAction("Roar", absoluteZote_.title.ShowTitle, 0);
                fsm.AddCustomAction("Roar End", absoluteZote_.title.HideTitle);
            }
        }
        public string UpdateText(string key, string sheet, string text)
        {
            return text;
        }
    }
}