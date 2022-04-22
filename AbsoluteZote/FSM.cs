namespace AbsoluteZote;
public static class FSM
{
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
}
