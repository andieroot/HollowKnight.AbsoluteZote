namespace AbsoluteZote;
public class AbsoluteZote : Mod, ITogglableMod
{
    public static AbsoluteZote absoluteZote;
    private readonly Statue statue;
    private readonly Arena arena;
    private readonly Skin skin;
    public readonly Title title;
    private readonly DreamNail dreamNail;
    private readonly Control control;
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
        ModHooks.HeroUpdateHook += HeroUpdateHook;
        ModHooks.HitInstanceHook += HitInstanceHook;
        ModHooks.LanguageGetHook += LanguageGetHook;
        On.EnemyDreamnailReaction.RecieveDreamImpact += RecieveDreamImpact;
        On.PlayMakerFSM.OnEnable += PlayMakerFSMOnEnable;
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChanged;
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
        ModHooks.HeroUpdateHook -= HeroUpdateHook;
        ModHooks.HitInstanceHook += HitInstanceHook;
        ModHooks.LanguageGetHook -= LanguageGetHook;
        On.EnemyDreamnailReaction.RecieveDreamImpact -= RecieveDreamImpact;
        On.PlayMakerFSM.OnEnable -= PlayMakerFSMOnEnable;
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= ActiveSceneChanged;
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
    private HitInstance HitInstanceHook(Fsm fsm, HitInstance hitInstance)
    {
        try
        {
            foreach (var module in modules)
            {
                hitInstance = module.UpdateHit(fsm, hitInstance);
            }
        }
        catch (Exception exception)
        {
            LogError(exception.Message);
        }
        return hitInstance;
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
}
