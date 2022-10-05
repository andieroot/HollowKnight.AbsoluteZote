﻿using System.Reflection;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using System.Collections.Generic;
using Vasi;
using SFCore;

namespace AbsoluteZote {
    public abstract class Module
    {
        protected readonly AbsoluteZote absoluteZote_;
        protected readonly Dictionary<string, object> prefabs = new Dictionary<string, object>();
        protected readonly System.Random random = new System.Random();
        public Module(AbsoluteZote absoluteZote)
        {
            absoluteZote_ = absoluteZote;
            absoluteZote_.modules.Add(this);
        }
        protected void Log(object message) => absoluteZote_.Log(message);
        protected void LogFSM(PlayMakerFSM fsm, System.Action function = null)
        {
            foreach (var state in fsm.FsmStates)
            {
                fsm.InsertCustomAction(state.Name, () =>
                {
                    Log("FSM: " + fsm.gameObject.name + "-" + fsm.FsmName + " entering state: " + state.Name + ".");
                    function?.Invoke();
                }, 0);
            }
        }
        protected void LogFSMState(PlayMakerFSM fsm, string state, System.Action function = null)
        {
            for (int i = fsm.GetState(state).Actions.Length; i >= 0; i--)
            {
                fsm.InsertCustomAction(state, () =>
                {
                    Log("State: " + fsm.FsmName + "-" + state + " entering action: " + i.ToString() + ".");
                    function?.Invoke();
                }, i);
            }
        }
        protected bool IsGreyPrince(GameObject gameObject)
        {
            return gameObject.scene.name == "GG_Grey_Prince_Zote" && gameObject.name == "Grey Prince";
        }
        public virtual List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)> { };
        }
        public virtual void LoadPrefabs(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
        }
        public virtual HitInstance UpdateHit(Fsm fsm, HitInstance hitInstance)
        {
            return hitInstance;
        }
        public virtual string UpdateText(string key, string sheet)
        {
            return null;
        }
        public virtual bool UpdateDreamnailReaction(EnemyDreamnailReaction enemyDreamnailReaction)
        {
            return false;
        }
        public virtual void UpdateHitInstance(HealthManager healthManager, HitInstance hitInstance)
        {
        }
        public virtual void UpdateFSM(PlayMakerFSM fsm)
        {
        }
        public virtual void Initialize(UnityEngine.SceneManagement.Scene scene)
        {
        }
    }
}