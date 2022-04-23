namespace AbsoluteZote;
public class Control : Module
{
    private class Variables
    {
        public float dashSlashDestination;
    }
    Variables variables = new Variables();
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
            fsm.AddState("Dash Slash Land");
            UpdateStateDashSlashAntic(fsm);
            UpdateStateDashSlashJump(fsm);
            UpdateStateDashSlashLand(fsm);
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
        fsm.AddCustomAction("Dash Slash Jump Antic", () =>
        {
            float destination, targetLeft = 8.19f, targetRight = 44.61f;
            if (HeroController.instance.transform.position.x - targetLeft > targetRight - HeroController.instance.transform.position.x)
            {
                destination = targetRight;
            }
            else
            {
                destination = targetLeft;
            }
            var positon = fsm.gameObject.transform.position;
            var localScale = fsm.gameObject.transform.localScale;
            if (positon.x < destination)
            {
                if (localScale.x < 0)
                {

                    localScale.x = -localScale.x;
                }
            }
            else
            {
                if (localScale.x > 0)
                {
                    localScale.x = -localScale.x;
                }
            }
            fsm.transform.localScale = localScale;
            variables.dashSlashDestination = destination;
        });
        fsm.AddAction("Dash Slash Jump Antic", fsm.CreateTk2dPlayAnimationWithEvents("Antic", FsmEvent.Finished));
        fsm.AddTransition("Dash Slash Jump Antic", "FINISHED", "Dash Slash Jump");
    }
    private void UpdateStateDashSlashJump(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Dash Slash Jump", () =>
        {
            var destination = variables.dashSlashDestination;
            float velocityX = 2 * (destination - fsm.gameObject.transform.position.x);
            float velocityY = 90;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 6;
            rigidbody2D.velocity = new Vector2(velocityX, velocityY);
        });
        fsm.AddAction("Dash Slash Jump", fsm.CreateCheckCollisionSide(fsm.GetFSMEvent("LAND")));
        fsm.AddAction("Dash Slash Jump", fsm.CreateCheckCollisionSideEnter(fsm.GetFSMEvent("LAND")));
        fsm.AddTransition("Dash Slash Jump", "LAND", "Dash Slash Land");
    }
    private void UpdateStateDashSlashLand(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Dash Slash Land", () =>
        {
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 3;
            rigidbody2D.velocity = Vector2.zero;
        });
        fsm.AddCustomAction("Dash Slash Land", () => fsm.SendEvent("FINISHED"));
        fsm.AddTransition("Dash Slash Land", "FINISHED", "Idle Start");
    }
}
