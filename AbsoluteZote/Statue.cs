using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modding;
using Satchel;

namespace AbsoluteZote
{
    public class Statue : Module
    {
        public Statue(AbsoluteZote absoluteZote) : base(absoluteZote)
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
        }
        public override string UpdateText(string key, string sheet, string text)
        {
            if (key == "NAME_GREY_PRINCE" && sheet == "Journal")
            {
                if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                    text = "无上左特";
                else
                    text = "ABSOLUTE ZOTE";
            }
            else if (key == "GG_S_MIGHTYZOTE" && sheet == "CP3")
            {
                if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                    text = "混沌之神";
                else
                    text = "God of chaos";
            }
            return text;
        }
    }
}