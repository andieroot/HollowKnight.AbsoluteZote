using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modding;
using Satchel;

namespace AbsoluteZote
{
    public class Arena : Module
    {
        public Arena(AbsoluteZote absoluteZote) : base(absoluteZote)
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
            if (scene.name == "GG_Grey_Prince_Zote")
            {
                var ggArenaPrefab = GameObject.Find("GG_Arena_Prefab").gameObject;
                var crowd = ggArenaPrefab.transform.Find("Crowd").gameObject;
                crowd.SetActive(false);
            }
        }
        public override void UpdateFSM(PlayMakerFSM fsm)
        {
        }
        public override string UpdateText(string key, string sheet, string text)
        {
            return text;
        }
    }
}