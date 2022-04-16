using Modding;
using System.Collections.Generic;
using UnityEngine;
using Satchel;


namespace AbsoluteZote
{
    public class AbsoluteZote : Mod
    {
        public AbsoluteZote() : base("AbsoluteZote")
        {
        }
        public override string GetVersion() => "1.0";
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.PlayMakerFSM.OnEnable += PlayMakerFSMOnEnable;
        }
        private void PlayMakerFSMOnEnable(On.PlayMakerFSM.orig_OnEnable original, PlayMakerFSM fsm)
        {
            if (fsm.gameObject.scene.name == "GG_Radiance" && fsm.gameObject.name == "Boss Control" && fsm.FsmName == "Control")
            {
                this.LogFSM(fsm);
            }
            original(fsm);
        }
    }
}
