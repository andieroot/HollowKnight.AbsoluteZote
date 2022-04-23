namespace AbsoluteZote;
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
}
