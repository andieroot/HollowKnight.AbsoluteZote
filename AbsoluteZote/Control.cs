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
            fsm.AddState("Dash Slash Jump Antic");
            fsm.AddState("Dash Slash Jump");
            fsm.AddState("Dash Slash Charge");
            fsm.AddState("Dash Slash Dash");
            fsm.AddState("Dash Slash Slash");
            UpdateStateDashSlashAntic(fsm);
            UpdateStateDashSlashJump(fsm);
            UpdateStateDashSlashCharge(fsm);
            UpdateStateDashSlashDash(fsm);
            UpdateStateDashSlashSlash(fsm);
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
    private void UpdateStateDashSlashAntic(PlayMakerFSM fsm)
    {

        fsm.AddCustomAction("Dash Slash Jump Antic", fsm.CreateSetVelocity2d(0, 0));
        fsm.AccessFloatVariable("dashSlashTargetLeft").Value = 8.19f;
        fsm.AccessFloatVariable("dashSlashTargetRight").Value = 44.61f;
        fsm.AddCustomAction("Dash Slash Jump Antic", () =>
        {
            var x = HeroController.instance.transform.position.x;
            float destination;
            var dashSlashTargetLeft = fsm.AccessFloatVariable("dashSlashTargetLeft").Value;
            var dashSlashTargetRight = fsm.AccessFloatVariable("dashSlashTargeRight").Value;
            if (x - dashSlashTargetLeft > dashSlashTargetRight - x)
            {
                destination = dashSlashTargetRight;
            }
            else
            {
                destination = dashSlashTargetLeft;
            }
            fsm.AccessFloatVariable("dashSlashDestination").Value = destination;
        });
        fsm.AddCustomAction("Dash Slash Jump Antic", fsm.CreateFacePosition("dashSlashDestination", true));
        fsm.AddAction("Dash Slash Jump Antic", fsm.CreateTk2dPlayAnimationWithEvents("Antic", FsmEvent.Finished));
        fsm.AddTransition("Dash Slash Jump Antic", "FINISHED", "Dash Slash Jump");
    }
    private void UpdateStateDashSlashJump(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Dash Slash Jump", () =>
        {
            var destination = fsm.AccessFloatVariable("dashSlashDestination").Value;
            float velocityX = 2 * (destination - fsm.gameObject.transform.position.x);
            float velocityY = 90;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 6;
            rigidbody2D.velocity = new Vector2(velocityX, velocityY);
        });
        fsm.AddAction("Dash Slash Jump", fsm.CreateCheckCollisionSide(fsm.GetFSMEvent("LAND")));
        fsm.AddAction("Dash Slash Jump", fsm.CreateCheckCollisionSideEnter(fsm.GetFSMEvent("LAND")));
        fsm.AddTransition("Dash Slash Jump", "LAND", "Dash Slash Charge");
    }
    private void UpdateStateDashSlashCharge(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Dash Slash Charge", () =>
        {
            var x = fsm.gameObject.transform.position.x;
            float destination;
            var slack = 5;
            var dashSlashTargetLeft = fsm.AccessFloatVariable("dashSlashTargetLeft").Value;
            var dashSlashTargetRight = fsm.AccessFloatVariable("dashSlashTargeRight").Value;
            if (x - dashSlashTargetLeft > dashSlashTargetRight - x)
            {
                destination = dashSlashTargetLeft + slack;
            }
            else
            {
                destination = dashSlashTargetRight - slack;
            }
            fsm.AccessFloatVariable("dashSlashDestination").Value = destination;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 3;
            rigidbody2D.velocity = Vector2.zero;
        });
        fsm.AddCustomAction("Dash Slash Charge", fsm.CreateFacePosition("dashSlashDestination", true));
        fsm.AddAction("Dash Slash Charge", fsm.CreateWait(2, fsm.GetFSMEvent("FINISHED")));
        fsm.AddTransition("Dash Slash Charge", "FINISHED", "Dash Slash Dash");
    }
    private void UpdateStateDashSlashDash(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Dash Slash Dash", () =>
        {
            var x = fsm.gameObject.transform.position.x;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            var v = 32;
            if (x < fsm.AccessFloatVariable("dashSlashDestination").Value)
                rigidbody2D.velocity = new Vector2(v, 0);
            else
                rigidbody2D.velocity = new Vector2(-v, 0);

        });
        fsm.AddAction("Dash Slash Dash", fsm.CreateReachDestionation("dashSlashDestination", "dashSlashDirecton", fsm.GetFSMEvent("FINISHED")));
        fsm.AddCustomAction("Dash Slash Dash", () => fsm.SendEvent("FINISHED"));
        fsm.AddTransition("Dash Slash Dash", "FINISHED", "Dash Slash Slash");
    }
    private void UpdateStateDashSlashSlash(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Dash Slash Slash", () => fsm.SendEvent("FINISHED"));
        fsm.AddTransition("Dash Slash Slash", "FINISHED", "Idle Start");
    }
}
