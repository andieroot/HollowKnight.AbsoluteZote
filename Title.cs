using System.Reflection;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using System.Collections.Generic;
using Vasi;
using SFCore;
using System.Collections;

namespace AbsoluteZote
{
    public class Title : Module
    {
        private readonly bool enabled = true;
        private GameObject superTitle;
        private GameObject title;
        private GameObject titleBackground;
        private GameObject dreamMsg;
        public Title(AbsoluteZote absoluteZote) : base(absoluteZote)
        {
        }
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
        {
             ("GG_Radiance", "Boss Control"),
             ("GG_Grey_Prince_Zote", "Mighty_Zote_0005_17"),
        };
        }
        public override void LoadPrefabs(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            var bossControl = preloadedObjects["GG_Radiance"]["Boss Control"];
            var title = bossControl.transform.Find("Boss Title").gameObject;
            title.GetComponent<SetTextMeshProGameText>().convName = "ABSOLUTE_ZOTE_MAIN";
            var superTitle = title.transform.Find("Boss Title (1)").gameObject;
            superTitle.GetComponent<SetTextMeshProGameText>().convName = "ABSOLUTE_ZOTE_SUPER";
            superTitle.name = "superTitle";
            prefabs["title"] = title;
            var titleBackground = preloadedObjects["GG_Grey_Prince_Zote"]["Mighty_Zote_0005_17"];
            var spriteRenderer = titleBackground.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerID = -349214895;
            var whiteFader = bossControl.transform.Find("White Fader").gameObject;
            spriteRenderer.sprite = whiteFader.GetComponent<UnityEngine.SpriteRenderer>().sprite;
            spriteRenderer.color = new Color(.3f, 0, .3f, 0);
            titleBackground.transform.position = new Vector3(0, 0, -16);
            titleBackground.transform.localScale = Vector3.one * 256;
            prefabs["titleBackground"] = titleBackground;
        }
        public override void UpdateFSM(PlayMakerFSM fsm)
        {
            if (fsm.gameObject.scene.name == "GG_Grey_Prince_Zote" && fsm.gameObject.name == "Grey Prince Title" && fsm.FsmName == "Control")
            {
                title = UnityEngine.Object.Instantiate(prefabs["title"] as GameObject);
                title.GetComponent<TMPro.TextMeshPro>().color = new Color(1, 1, 1);
                superTitle = title.transform.Find("superTitle").gameObject;
                superTitle.GetComponent<TMPro.TextMeshPro>().color = new Color(1, 1, 1);
                title.name = "title";
                titleBackground = UnityEngine.Object.Instantiate(prefabs["titleBackground"] as GameObject);
                titleBackground.SetActive(true);
                titleBackground.name = "titleBackground";
                var gameCameras = GameObject.Find("_GameCameras").gameObject;
                var hudCamera = gameCameras.transform.Find("HudCamera").gameObject;
                var dialogueManager = hudCamera.transform.Find("DialogueManager").gameObject;
                dreamMsg = dialogueManager.transform.Find("Dream Msg").gameObject;
                for (int i = 1; i <= 13; ++i)
                {
                    fsm.RemoveAction("Extra " + i.ToString(), 1);
                }
                fsm.RemoveAction("Main Title", 0);
            }
        }
        public override string UpdateText(string key, string sheet)
        {
            string text = null;
            if (key == "ABSOLUTE_ZOTE_MAIN" && sheet == "Titles")
            {
                if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                {
                    text = "无上左特";
                }
                else
                {
                    text = "ZOTE";
                }
            }
            else if (key == "ABSOLUTE_ZOTE_SUPER" && sheet == "Titles")
            {
                if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                {
                    text = "";
                }
                else
                {
                    text = "ABSOLUTE";
                }
            }
            return text;
        }
        public void HideHUD()
        {
            if (!enabled)
            {
                return;
            }
            foreach (var fsm in GameCameras.instance.hudCanvas.GetComponents<PlayMakerFSM>())
            {
                fsm.SendEvent("OUT");
            }
        }
        private IEnumerator ShowTitle_()
        {
            title.GetComponent<FadeGroup>().FadeUp();
            dreamMsg.GetComponent<PlayMakerFSM>().SendEvent("CANCEL ENEMY DREAM");
            var t = .1f;
            var n = (int)(t * 60);
            var color = titleBackground.GetComponent<SpriteRenderer>().color;
            for (var i = 0; i < n; ++i)
            {
                yield return new WaitForSeconds(t / n);
                titleBackground.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, (float)(i + 1) / n);
            }
        }
        public void ShowTitle()
        {
            if (!enabled)
            {
                return;
            }
            var coroutine = ShowTitle_();
            title.GetComponent<TMPro.TextMeshPro>().StartCoroutine(coroutine);
        }
        private IEnumerator HideTitle_()
        {
            title.GetComponent<FadeGroup>().FadeDown();
            var t = .1f;
            var n = (int)(t * 60);
            var color = titleBackground.GetComponent<SpriteRenderer>().color;
            for (var i = 0; i < n; ++i)
            {
                yield return new WaitForSeconds(t / n);
                titleBackground.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, (float)(n - i) / n);
            }
            foreach (var fsm in GameCameras.instance.hudCanvas.GetComponents<PlayMakerFSM>())
            {
                fsm.SendEvent("IN");
            }
        }
        public void HideTitle()
        {
            if (!enabled)
            {
                return;
            }
            var coroutine = HideTitle_();
            title.GetComponent<TMPro.TextMeshPro>().StartCoroutine(coroutine);
        }
    }
}