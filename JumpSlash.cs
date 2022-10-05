using System.Reflection;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using System.Collections.Generic;
using Vasi;
using SFCore;

namespace AbsoluteZote
{
    public partial class Control : Module
    {
        private void LoadPrefabsJumpSlash(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
        }
        private void UpdateFSMJumpSlash(PlayMakerFSM fsm)
        {
            fsm.InsertCustomAction("Stomp", () =>
            {
                if (random.Next(2) == 1)
                {
                    fsm.SetState("Shift Dir");
                }
            }, 0);
            void setWaveScale(PlayMakerFSM fsm1)
            {
                var wave = fsm1.FsmVariables.GetFsmGameObject("Shockwave").Value;
                wave.transform.SetScaleX(6);
            }
            fsm.InsertCustomAction("Slash Waves L", () => setWaveScale(fsm), 4);
            fsm.InsertCustomAction("Slash Waves R", () => setWaveScale(fsm), 4);
        }
    }
}