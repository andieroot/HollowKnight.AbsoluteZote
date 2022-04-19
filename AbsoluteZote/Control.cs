using System.Collections.Generic;
using UnityEngine;
using Satchel;


namespace AbsoluteZote
{
    public class Control : Module
    {
        public Control(AbsoluteZote absoluteZote) : base(absoluteZote)
        {
        }
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
            };
        }
        public override void LoadPrefabs(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
        }
        public override void Initialize(UnityEngine.SceneManagement.Scene scene)
        {
        }
        public override void UpdateFSM(PlayMakerFSM fsm)
        {
            if (fsm.gameObject.scene.name == "GG_Grey_Prince_Zote" && fsm.gameObject.name == "Grey Prince" && fsm.FsmName == "Control")
            {
                fsm.InsertCustomAction("Enter 1", absoluteZote_.title.HideHUD, 0);
                fsm.InsertCustomAction("Roar", absoluteZote_.title.ShowTitle, 0);
                fsm.AddCustomAction("Roar End", absoluteZote_.title.HideTitle);
            }
        }
        public override string UpdateText(string key, string sheet, string text)
        {
            return text;
        }
    }
}