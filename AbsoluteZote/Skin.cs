namespace AbsoluteZote;
public class Skin : Module
{
    Texture2D texture2D;
    public Skin(AbsoluteZote absoluteZote) : base(absoluteZote)
    {
        var stream = typeof(AbsoluteZote).Assembly.GetManifestResourceStream("AbsoluteZote.Resources.Skin.Texture2D.png");
        MemoryStream memoryStream = new((int)stream.Length);
        stream.CopyTo(memoryStream);
        stream.Close();
        var bytes = memoryStream.ToArray();
        memoryStream.Close();
        texture2D = new(0, 0);
        texture2D.LoadImage(bytes, true);
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
            tk2dSprite.CurrentSprite.material.mainTexture = texture2D;
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
