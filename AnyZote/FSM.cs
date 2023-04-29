namespace AnyZote;
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
public class GeneralAction : FsmStateAction
{
    public override void OnUpdate()
    {
        action();
    }
    public Action action;
}
public static class FSM
{
    public static void AddAnimationTest(this PlayMakerFSM fsm)
    {
        var i = 0;
        foreach (var clip in fsm.gameObject.GetComponent<tk2dSpriteAnimator>().Library.clips)
        {
            fsm.AddState("Animation Test " + i);
            ++i;
        }
        var n = i;
        i = 0;
        foreach (var clip in fsm.gameObject.GetComponent<tk2dSpriteAnimator>().Library.clips)
        {
            fsm.AddAction("Animation Test " + i, fsm.CreateTk2dPlayAnimationWithEvents(
                fsm.gameObject, clip.name, fsm.GetFSMEvent("FINISHED")));
            fsm.AddCustomAction("Animation Test " + i, () =>
            {
                AnyZote.anyZote.Log(clip.name);
            });
            fsm.AddAction("Animation Test " + i, fsm.CreateWait(2.5f, fsm.GetFSMEvent("FINISHED")));
            if (i + 1 != n)
            {
                fsm.AddTransition("Animation Test " + i, "FINISHED", "Animation Test " + (i + 1));
            }
            else
            {
                fsm.AddTransition("Animation Test " + i, "FINISHED", "Animation Test " + 0);
            }
            ++i;
        }
    }
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
    public static FsmGameObject AccessGameObjectVariable(this PlayMakerFSM fsm, string name)
    {
        FsmGameObject fsmGameObject = fsm.FsmVariables.GameObjectVariables.FirstOrDefault(x => x.Name == name);
        if (fsmGameObject != null)
            return fsmGameObject;
        fsmGameObject = new FsmGameObject(name);
        fsm.FsmVariables.GameObjectVariables = fsm.FsmVariables.GameObjectVariables.Append(fsmGameObject).ToArray();
        return fsmGameObject;
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
    public static FsmBool AccessBoolVariable(this PlayMakerFSM fsm, string name)
    {
        FsmBool fsmBool = fsm.FsmVariables.BoolVariables.FirstOrDefault(x => x.Name == name);
        if (fsmBool != null)
            return fsmBool;
        fsmBool = new FsmBool(name);
        fsm.FsmVariables.BoolVariables = fsm.FsmVariables.BoolVariables.Append(fsmBool).ToArray();
        return fsmBool;
    }
    public static Tk2dPlayAnimation CreateTk2dPlayAnimation(this PlayMakerFSM fsm, GameObject gameObject, string clip)
    {
        var fsmOwnerDefault = new FsmOwnerDefault
        {
            OwnerOption = OwnerDefaultOption.SpecifyGameObject,
            GameObject = gameObject,
        };
        var tk2DPlayAnimation = new Tk2dPlayAnimation()
        {
            gameObject = fsmOwnerDefault,
            animLibName = "",
            clipName = clip,
        };
        return tk2DPlayAnimation;
    }
    public static Tk2dPlayAnimationWithEvents CreateTk2dPlayAnimationWithEvents(
        this PlayMakerFSM fsm, GameObject gameObject, string clip, FsmEvent fsmEvent)
    {
        var fsmOwnerDefault = new FsmOwnerDefault
        {
            OwnerOption = OwnerDefaultOption.SpecifyGameObject,
            GameObject = gameObject,
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
            if (!spriteFaceRight)
            {
                localScale.x *= -1;
            }
            fsm.transform.localScale = localScale;
        };
    }
    public static CheckCollisionSide CreateCheckCollisionSide(
        this PlayMakerFSM fsm, FsmEvent leftHitEvent, FsmEvent rightHitEvent, FsmEvent bottomHitEvent)
    {
        var checkCollisionSide = new CheckCollisionSide()
        {
            leftHit = false,
            rightHit = false,
            bottomHit = false,
            topHit = false,
            leftHitEvent = leftHitEvent,
            rightHitEvent = rightHitEvent,
            bottomHitEvent = bottomHitEvent,
            otherLayer = false,
            otherLayerNumber = 0,
            ignoreTriggers = false,
        };
        return checkCollisionSide;
    }
    public static CheckCollisionSideEnter CreateCheckCollisionSideEnter(
        this PlayMakerFSM fsm, FsmEvent leftHitEvent, FsmEvent rightHitEvent, FsmEvent bottomHitEvent)
    {
        var checkCollisionSideEnter = new CheckCollisionSideEnter()
        {
            leftHit = false,
            rightHit = false,
            bottomHit = false,
            topHit = false,
            leftHitEvent = leftHitEvent,
            rightHitEvent = rightHitEvent,
            bottomHitEvent = bottomHitEvent,
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
    public static AudioPlaySimple CreateAudioPlaySimple(this PlayMakerFSM fsm, float volume, FsmObject oneShotClip)
    {
        var fsmOwnerDefault = new FsmOwnerDefault
        {
            GameObject = fsm.gameObject,
        };
        return new AudioPlaySimple()
        {
            gameObject = fsmOwnerDefault,
            volume = volume,
            oneShotClip = oneShotClip,
        };
    }
    public static AudioPlayerOneShot CreateAudioPlayerOneShot(
        this PlayMakerFSM fsm, FsmGameObject audioPlayer, GameObject spawnPoint, AudioClip[] audioClips,
        float[] weights, float pitchMin, float pitchMax, float volume, float delay)
    {
        var weights_ = new FsmFloat[weights.Length];
        for (int i = 0; i < weights_.Length; i++)
        {
            weights_[i] = weights[i];
        }
        return new AudioPlayerOneShot()
        {
            audioPlayer = audioPlayer,
            spawnPoint = spawnPoint,
            audioClips = audioClips,
            weights = weights_,
            pitchMin = pitchMin,
            pitchMax = pitchMax,
            volume = volume,
            delay = delay,
            storePlayer = new FsmGameObject(),
        };
    }
    public static AudioPlayerOneShotSingle CreateAudioPlayerOneShotSingle(
        this PlayMakerFSM fsm, FsmGameObject audioPlayer, GameObject spawnPoint, FsmObject audioClip,
        float pitchMin, float pitchMax, float volume, float delay)
    {
        return new AudioPlayerOneShotSingle()
        {
            audioPlayer = audioPlayer,
            spawnPoint = spawnPoint,
            audioClip = audioClip,
            pitchMin = pitchMin,
            pitchMax = pitchMax,
            volume = volume,
            delay = delay,
            storePlayer = new FsmGameObject(),
        };
    }
    public static SendEventByName CreateSendEventByName(this PlayMakerFSM fsm, FsmEventTarget eventTarget, string sendEvent, float delay)
    {
        return new SendEventByName()
        {
            eventTarget = eventTarget,
            sendEvent = sendEvent,
            delay = delay,
            everyFrame = false,
        };
    }
    public static PlayParticleEmitterInState CreatePlayParticleEmitterInState(this PlayMakerFSM fsm, GameObject gameObject)
    {
        var fsmOwnerDefault = new FsmOwnerDefault
        {
            OwnerOption = OwnerDefaultOption.SpecifyGameObject,
            GameObject = gameObject,
        };
        return new PlayParticleEmitterInState()
        {
            gameObject = fsmOwnerDefault,
        };
    }
    public static SpawnObjectFromGlobalPool CreateSpawnObjectFromGlobalPool(
        this PlayMakerFSM fsm, GameObject gameObject, GameObject spawnPoint, Vector3 positon, Vector3 rotation)
    {
        return new SpawnObjectFromGlobalPool()
        {
            gameObject = gameObject,
            spawnPoint = spawnPoint,
            position = positon,
            rotation = rotation,
            storeObject = new FsmGameObject(),
        };
    }
    public static GeneralAction CreateGeneralAction(this PlayMakerFSM fsm, Action action)
    {
        return new GeneralAction()
        {
            action = action,
        };
    }
}