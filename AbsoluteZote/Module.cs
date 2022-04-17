using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modding;
using Satchel;

namespace AbsoluteZote
{
    public abstract class Module
    {
        protected readonly AbsoluteZote absoluteZote_;
        protected readonly Dictionary<string, GameObject> prefabs = new();
        public Module(AbsoluteZote absoluteZote)
        {
            absoluteZote_ = absoluteZote;
            absoluteZote_.modules.Add(this);
        }
        private void Log(object message) => absoluteZote_.Log(message);
        public abstract List<(string, string)> GetPreloadNames();
        public abstract void LoadPrefabs(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects);
        public abstract void Initialize(UnityEngine.SceneManagement.Scene scene);
        public abstract void UpdateFSM(PlayMakerFSM fsm);
        public abstract string UpdateText(string key, string sheet, string text);
    }
}