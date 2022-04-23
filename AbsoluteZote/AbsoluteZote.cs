namespace AbsoluteZote;
public class AbsoluteZote : Mod, ITogglableMod
{
    static public AbsoluteZote absoluteZote;
    private Statue statue;
    private Arena arena;
    private Skin skin;
    public Title title;
    private DreamNail dreamNail;
    private Control control;
    public List<Module> modules = new();
    public AbsoluteZote() : base("AbsoluteZote")
    {
        absoluteZote = this;
        statue = new(this);
        arena = new(this);
        skin = new(this);
        title = new(this);
        dreamNail = new(this);
        control = new(this);
    }
    public override string GetVersion() => "1.0.0.0";
    public override List<(string, string)> GetPreloadNames()
    {
        List<(string, string)> preloadNames = new();
        foreach (var module in modules)
        {
            foreach (var name in module.GetPreloadNames())
            {
                preloadNames.Add(name);
            }
        }
        return preloadNames;
    }
    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        On.PlayMakerFSM.OnEnable += PlayMakerFSMOnEnable;
        ModHooks.LanguageGetHook += LanguageGetHook;
        ModHooks.HeroUpdateHook += HeroUpdateHook;
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChanged;
        On.EnemyDreamnailReaction.RecieveDreamImpact += RecieveDreamImpact;
        if (preloadedObjects != null)
        {
            foreach (var module in modules)
            {
                module.LoadPrefabs(preloadedObjects);
            }
        }
    }
    public void Unload()
    {
        On.PlayMakerFSM.OnEnable -= PlayMakerFSMOnEnable;
        ModHooks.LanguageGetHook -= LanguageGetHook;
        ModHooks.HeroUpdateHook -= HeroUpdateHook;
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= ActiveSceneChanged;
    }
    private void PlayMakerFSMOnEnable(On.PlayMakerFSM.orig_OnEnable onEnable, PlayMakerFSM fsm)
    {
        try
        {
            foreach (var module in modules)
            {
                module.UpdateFSM(fsm);
            }
            onEnable(fsm);
        }
        catch (Exception exception)
        {
            LogError(exception.Message);
        }
    }
    private string LanguageGetHook(string key, string sheet, string text)
    {
        try
        {
            foreach (var module in modules)
            {
                text = module.UpdateText(key, sheet, text);
            }
        }
        catch (Exception exception)
        {
            LogError(exception.Message);
        }
        return text;
    }
    private void HeroUpdateHook()
    {
        try
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GG_Grey_Prince_Zote");
            }
        }
        catch (Exception exception)
        {
            LogError(exception.Message);
        }
    }
    private void ActiveSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
    {
        try
        {
            foreach (var module in modules)
                module.Initialize(to);
        }
        catch (Exception exception)
        {
            LogError(exception.Message);
        }
    }
    private void RecieveDreamImpact(On.EnemyDreamnailReaction.orig_RecieveDreamImpact receiveDreamImpact, EnemyDreamnailReaction enemyDreamnailReaction)
    {
        try
        {
            foreach (var module in modules)
            {
                if (module.UpdateDreamnailReaction(enemyDreamnailReaction))
                {
                    return;
                }
            }
            receiveDreamImpact(enemyDreamnailReaction);
        }
        catch (Exception exception)
        {
            LogError(exception.Message);
        }
    }
}
