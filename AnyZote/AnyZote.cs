namespace AnyZote;
[Serializable]
public class Settings
{
    public int status = 0;
    public int skipIntro = 0;
}
public class AnyZote : Mod, IGlobalSettings<Settings>, IMenuMod
{
    public static AnyZote anyZote;
    private readonly Statue statue;
    private readonly Arena arena;
    private readonly Skin skin;
    public readonly Title title;
    private readonly DreamNail dreamNail;
    private readonly Control control;
    private readonly Afterimage afterimage;
    public List<Module> modules = new();
    public Settings settings_ = new();
    public bool ToggleButtonInsideMenu => true;
    public AnyZote() : base("AnyZote")
    {
        anyZote = this;
        statue = new(this);
        arena = new(this);
        skin = new(this);
        title = new(this);
        dreamNail = new(this);
        control = new(this);
        afterimage = new(this);
    }
    public override string GetVersion() => "2.1.0.0";
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
        ModHooks.LanguageGetHook += LanguageGetHook;
        On.EnemyDreamnailReaction.RecieveDreamImpact += RecieveDreamImpact;
        On.HealthManager.TakeDamage += HealthManagerTakeDamage;
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
    private List<Module> GetActiveModules()
    {
        if (settings_.status == 0)
        {
            return modules;
        }
        else if (settings_.status == 1)
        {
            return new List<Module>()
            {
                skin,
                arena,
            };
        }
        else
        {
            return new List<Module>() { };
        }
    }
    private void HeroUpdateHook()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GG_Grey_Prince_Zote");
        }
    }
    private string LanguageGetHook(string key, string sheet, string text)
    {
        foreach (var module in GetActiveModules())
        {
            text = module.UpdateText(key, sheet, text);
        }
        return text;
    }
    private void RecieveDreamImpact(On.EnemyDreamnailReaction.orig_RecieveDreamImpact receiveDreamImpact, EnemyDreamnailReaction enemyDreamnailReaction)
    {

        foreach (var module in GetActiveModules())
        {
            if (module.UpdateDreamnailReaction(enemyDreamnailReaction))
            {
                return;
            }
        }
        receiveDreamImpact(enemyDreamnailReaction);
    }
    private void HealthManagerTakeDamage(On.HealthManager.orig_TakeDamage takeDamage, HealthManager healthManager, HitInstance hitInstance)
    {
        foreach (var module in GetActiveModules())
        {
            module.UpdateHitInstance(healthManager, hitInstance);
        }
        takeDamage(healthManager, hitInstance);
    }
    private void PlayMakerFSMOnEnable(On.PlayMakerFSM.orig_OnEnable onEnable, PlayMakerFSM fsm)
    {
        foreach (var module in GetActiveModules())
        {
            module.UpdateFSM(fsm);
        }
        onEnable(fsm);
    }

    private void ActiveSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
    {
        foreach (var module in GetActiveModules())
        {
            module.Initialize(to);
        }
    }
    public void OnLoadGlobal(Settings settings) => settings_ = settings;
    public Settings OnSaveGlobal() => settings_;
    public List<IMenuMod.MenuEntry> GetMenuData(IMenuMod.MenuEntry? menu)
    {
        List<IMenuMod.MenuEntry> menus = new();
        menus.Add(
            new()
            {
                Values = new string[]
                {
                    Language.Language.Get("MOH_ON", "MainMenu"),
                    "SKIN-ONLY",
                    Language.Language.Get("MOH_OFF", "MainMenu"),
                },
                Saver = i => settings_.status = i,
                Loader = () => settings_.status
            }
        );
        menus.Add(
            new()
            {
                Values = new string[]
                {
                    "KEEP INTRO",
                    "SKIP INTRO"
                },
                Saver = i => settings_.skipIntro = i,
                Loader = () => settings_.skipIntro
            }
        );
        return menus;
    }
}
