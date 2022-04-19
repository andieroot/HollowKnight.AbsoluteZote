using UnityEngine;
using Satchel;


namespace AbsoluteZote
{
    public class Skin : Module
    {
        public Skin(AbsoluteZote absoluteZote) : base(absoluteZote)
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
                var greyPrince = UnityEngine.GameObject.Find("Grey Prince").gameObject;
                var tk2dSprite = greyPrince.GetComponent<tk2dSprite>();
                var mainTexture = tk2dSprite.CurrentSprite.material.mainTexture;
                // TextureUtils.WriteTextureToFile(mainTexture, "wochao.png");
                //Log(Directory.GetCurrentDirectory());
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