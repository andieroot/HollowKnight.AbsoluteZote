using System.Collections.Generic;
using UnityEngine;
using Modding;
using Satchel;


namespace AbsoluteZote
{
    public class Title
    {
        private readonly AbsoluteZote absoluteZote_;
        private readonly Dictionary<string, GameObject> prefabs = new();
        private GameObject superTitle;
        private GameObject title;
        private GameObject titleBackground;
        private GameObject dreamMsg;
        public Title(AbsoluteZote absoluteZote)
        {
            absoluteZote_ = absoluteZote;
        }
        private void Log(object message) => absoluteZote_.Log(message);
        public List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                 ("GG_Radiance", "Boss Control"),
                 ("GG_Grey_Prince_Zote", "Mighty_Zote_0005_17"),
            };
        }
        public void LoadPrefabs(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            var bossControl = preloadedObjects["GG_Radiance"]["Boss Control"];
            var title = bossControl.transform.Find("Boss Title").gameObject;
            title.GetComponent<SetTextMeshProGameText>().convName = "ABSOLUTE_ZOTE_MAIN";
            var superTitle = title.transform.Find("Boss Title (1)").gameObject;
            superTitle.GetComponent<SetTextMeshProGameText>().convName = "ABSOLUTE_ZOTE_SUPER";
            superTitle.name = "superTitle";
            title.name = "title";
            prefabs["title"] = title;
            var titleBackground = preloadedObjects["GG_Grey_Prince_Zote"]["Mighty_Zote_0005_17"];
            var spriteRenderer = titleBackground.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerID = -349214895;
            var whiteFader = bossControl.transform.Find("White Fader").gameObject;
            spriteRenderer.sprite = whiteFader.GetComponent<UnityEngine.SpriteRenderer>().sprite;
            spriteRenderer.color = new Color(0.3f, 0, 0.3f);
            titleBackground.transform.position = new Vector3(0, 0, -16);
            titleBackground.transform.localScale = Vector3.one * 256;
            titleBackground.name = "titleBackground";
            prefabs["titleBackground"] = titleBackground;
        }
        public void UpdateFSM(PlayMakerFSM fsm)
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
        public string UpdateText(string key, string sheet, string text)
        {
            if (key == "ABSOLUTE_ZOTE_MAIN" && sheet == "Titles")
            {
                if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                    text = "无上左特";
                else
                    text = "Zote";
            }
            else if (key == "ABSOLUTE_ZOTE_SUPER" && sheet == "Titles")
            {
                if (Language.Language.CurrentLanguage() == Language.LanguageCode.ZH)
                    text = "";
                else
                    text = "Absolute";
            }
            return text;
        }
        public void InstantiateTitle()
        {
            title = Object.Instantiate(absoluteZote_.title.prefabs["title"]);
            title.GetComponent<TMPro.TextMeshPro>().color = new Color(1, 1, 1);
            superTitle = title.transform.Find("superTitle").gameObject;
            superTitle.GetComponent<TMPro.TextMeshPro>().color = new Color(1, 1, 1);
            title.name = "title";
            titleBackground = Object.Instantiate(absoluteZote_.title.prefabs["titleBackground"]);
            titleBackground.name = "titleBackground";
            var gameCameras = GameObject.Find("_GameCameras").gameObject;
            var hudCamera = gameCameras.transform.Find("HudCamera").gameObject;
            var dialogueManager = hudCamera.transform.Find("DialogueManager").gameObject;
            dreamMsg = dialogueManager.transform.Find("Dream Msg").gameObject;
        }
        public void HideHUD()
        {
            foreach (var f in GameCameras.instance.hudCanvas.GetComponents<PlayMakerFSM>())
            {
                f.SendEvent("OUT");
            }
        }
        public void ShowTitle()
        {
            title.GetComponent<FadeGroup>().FadeUp();
            titleBackground.SetActive(true);
            foreach (var s in dreamMsg.GetComponentsInChildren<SpriteRenderer>())
            {
                s.enabled = false;
            }
            foreach (var s in dreamMsg.GetComponentsInChildren<MeshRenderer>())
            {
                s.enabled = false;
            }
        }
        public void HideTitle()
        {
            title.GetComponent<FadeGroup>().FadeDown();
            titleBackground.SetActive(false);
            foreach (var f in GameCameras.instance.hudCanvas.GetComponents<PlayMakerFSM>())
            {
                f.SendEvent("IN");
            }
            foreach (var s in dreamMsg.GetComponentsInChildren<SpriteRenderer>())
            {
                s.enabled = true;
            }
            foreach (var s in dreamMsg.GetComponentsInChildren<MeshRenderer>())
            {
                s.enabled = true;
            }
        }
    }
}