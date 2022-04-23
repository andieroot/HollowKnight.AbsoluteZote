namespace AbsoluteZote;
public class ReachDestination : FsmStateAction
{
    public override void OnUpdate()
    {
        var x = gameObject.transform.position.x;
        if ((x - destination.Value) * direction.Value >= 0)
        {
            Finish();
            Fsm.Event(this.finishEvent);
        }
    }
    public GameObject gameObject;
    public FsmFloat destination;
    public FsmInt direction;
    public FsmEvent finishEvent;
}
public static class FSM
{
    public static FsmEvent GetFSMEvent(this PlayMakerFSM fsm, string name)
    {
        foreach (var fsmEvent in fsm.FsmEvents)
        {
            if (fsmEvent.Name == name)
            {
                return fsmEvent;
            }
        }
        throw new Exception();
    }
    public static FsmFloat AccessFloatVariable(this PlayMakerFSM fsm, string name)
    {
        FsmFloat fsmFloat = fsm.FsmVariables.FloatVariables.FirstOrDefault(x => x.Name == name);
        if (fsmFloat != null)
            return fsmFloat;
        fsmFloat = new FsmFloat(name);
        fsm.FsmVariables.FloatVariables = fsm.FsmVariables.FloatVariables.Append(fsmFloat).ToArray();
        return fsmFloat;
    }
    public static FsmInt AccessIntVariable(this PlayMakerFSM fsm, string name)
    {
        FsmInt fsmInt = fsm.FsmVariables.IntVariables.FirstOrDefault(x => x.Name == name);
        if (fsmInt != null)
            return fsmInt;
        fsmInt = new FsmInt(name);
        fsm.FsmVariables.IntVariables = fsm.FsmVariables.IntVariables.Append(fsmInt).ToArray();
        return fsmInt;
    }
    public static Tk2dPlayAnimationWithEvents CreateTk2dPlayAnimationWithEvents(this PlayMakerFSM fsm, string clip, FsmEvent fsmEvent)
    {
        var fsmOwnerDefault = new FsmOwnerDefault
        {
            GameObject = fsm.gameObject,
        };
        var tk2DPlayAnimationWithEvents = new Tk2dPlayAnimationWithEvents()
        {
            gameObject = fsmOwnerDefault,
            clipName = clip,
            animationCompleteEvent = fsmEvent,
        };
        return tk2DPlayAnimationWithEvents;
    }
    public static Action CreateSetVelocity2d(this PlayMakerFSM fsm, float x, float y)
    {
        return () =>
        {
            fsm.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y);
        };
    }
    public static FaceObject CreateFaceObject(this PlayMakerFSM fsm, GameObject gameObject, bool spriteFaceRight)
    {
        var faceObject = new FaceObject()
        {
            objectA = fsm.gameObject,
            objectB = gameObject,
            spriteFacesRight = spriteFaceRight,
            playNewAnimation = false,
            newAnimationClip = "",
            resetFrame = false,
            everyFrame = false,
        };
        return faceObject;
    }
    public static Action CreateFacePosition(this PlayMakerFSM fsm, string destination, bool spriteFaceRight)
    {
        return () =>
        {
            var position = fsm.gameObject.transform.position;
            var localScale = fsm.gameObject.transform.localScale;
            if (position.x < fsm.AccessFloatVariable(destination).Value)
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
        };
    }
    public static CheckCollisionSide CreateCheckCollisionSide(this PlayMakerFSM fsm, FsmEvent fsmEvent)
    {
        var checkCollisionSide = new CheckCollisionSide()
        {
            topHit = false,
            rightHit = false,
            bottomHit = false,
            leftHit = false,
            bottomHitEvent = fsmEvent,
            otherLayer = false,
            otherLayerNumber = 0,
            ignoreTriggers = false,
        };
        return checkCollisionSide;
    }
    public static CheckCollisionSideEnter CreateCheckCollisionSideEnter(this PlayMakerFSM fsm, FsmEvent fsmEvent)
    {
        var checkCollisionSideEnter = new CheckCollisionSideEnter()
        {
            topHit = false,
            rightHit = false,
            bottomHit = false,
            leftHit = false,
            bottomHitEvent = fsmEvent,
            otherLayer = false,
            otherLayerNumber = 0,
            ignoreTriggers = false,
        };
        return checkCollisionSideEnter;
    }
    public static Wait CreateWait(this PlayMakerFSM fsm, float time, FsmEvent fsmEvent)
    {
        var wait = new Wait()
        {
            time = time,
            finishEvent = fsmEvent,
            realTime = false,
        };
        return wait;
    }
    public static ReachDestination CreateReachDestionation(this PlayMakerFSM fsm, string destination, string direction, FsmEvent finishEvent)
    {
        return new ReachDestination()
        {
            gameObject = fsm.gameObject,
            destination = fsm.AccessFloatVariable(destination),
            direction = fsm.AccessIntVariable(direction),
            finishEvent = finishEvent,
        };
    }
}