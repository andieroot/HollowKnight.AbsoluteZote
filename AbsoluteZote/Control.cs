namespace AbsoluteZote;
public class Control : Module
{
    public Control(AbsoluteZote absoluteZote) : base(absoluteZote)
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
        if (IsGreyPrince(fsm.gameObject) && fsm.FsmName == "Control")
        {
            UpdateStateEnter1(fsm);
            UpdateStateRoar(fsm);
            UpdateStateRoarEnd(fsm);
            UpdateStateStun(fsm);
            UpdateStateMoveChoice3(fsm);
            UpdateStateDashSlashJumpAntic(fsm);
        }
    }
    public override string UpdateText(string key, string sheet, string text)
    {
        return text;
    }
    private void UpdateStateEnter1(PlayMakerFSM fsm)
    {
        fsm.InsertCustomAction("Enter 1", absoluteZote_.title.HideHUD, 0);
    }
    private void UpdateStateRoar(PlayMakerFSM fsm)
    {
        fsm.InsertCustomAction("Roar", absoluteZote_.title.ShowTitle, 0);
    }
    private void UpdateStateRoarEnd(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Roar End", absoluteZote_.title.HideTitle);
    }
    private void UpdateStateStun(PlayMakerFSM fsm)
    {
        fsm.ChangeTransition("Stun", "TOOK DAMAGE", "Move Choice 3");
        fsm.ChangeTransition("Stun", "FINISHED", "Move Choice 3");
    }
    private void UpdateStateMoveChoice3(PlayMakerFSM fsm)
    {
        fsm.InsertCustomAction("Move Choice 3", () =>
        {
            fsm.SetState("Dash Slash Jump Antic");
        }, 0);
    }
    private void UpdateStateDashSlashJumpAntic(PlayMakerFSM fsm)
    {
        var dashSlashJumpAntic = fsm.AddState("Dash Slash Jump Antic");
        var dashSlashJump = fsm.AddState("Dash Slash Jump");
        fsm.AddCustomAction(dashSlashJumpAntic.Name, fsm.CreateSetVelocity2d(0, 0));
        fsm.AddAction(dashSlashJumpAntic.Name, fsm.CreateTk2dPlayAnimationWithEvents("Antic", FsmEvent.Finished));
        fsm.AddTransition(dashSlashJumpAntic.Name, "FINISHED", dashSlashJump.Name);
        fsm.AddCustomAction(dashSlashJump.Name, () =>
        {
            float destination, targetLeft = 8.19f, targetRight = 44.61f;
            if (HeroController.instance.transform.position.x - targetLeft > targetRight - HeroController.instance.transform.position.x)
            {
                destination = targetLeft;
            }
            else
            {
                destination = targetRight;
            }
            float velocityX = 2*(destination - fsm.gameObject.transform.position.x);
            float velocityY = 90;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 6;
            rigidbody2D.velocity = new Vector2(velocityX, velocityY);
        });
    }
}
