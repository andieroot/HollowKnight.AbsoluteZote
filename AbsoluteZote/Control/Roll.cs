namespace AbsoluteZote;

public partial class Control : Module
{
    private void LoadPrefabsRoll(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        var greyPrince = preloadedObjects["GG_Grey_Prince_Zote"]["Grey Prince"];
        var fsm = greyPrince.LocateMyFSM("Control");
        prefabs["rollJumpAudio1"] = (fsm.GetState("Jump").Actions[0] as AudioPlaySimple).oneShotClip;
        prefabs["rollJumpAudioPlayer"] = (fsm.GetState("Jump").Actions[1] as AudioPlayerOneShot).audioPlayer;
        prefabs["rollJumpAudio2"] = (fsm.GetState("Jump").Actions[1] as AudioPlayerOneShot).audioClips;
        prefabs["rollRollingLandCamera"] = (fsm.GetState("Land Normal").Actions[1] as SendEventByName).eventTarget;
    }
    private void UpdateHitInstanceRoll(HealthManager healthManager, HitInstance hitInstance)
    {
        var gameObject = healthManager.gameObject;
        var fsm = gameObject.GetComponent<PlayMakerFSM>();
        if (fsm.ActiveStateName == "Roll Rolling")
        {
            if (hitInstance.Direction == 90)
            {
                var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
                var velocity = rigidbody2D.velocity;
                var random = new System.Random();
                var velocityX = (float)((1 - 2 * random.NextDouble()) * 20);
                var velocityY = -velocity.y;
                rigidbody2D.velocity = new Vector2(velocityX, velocityY);
            }
            fsm.SetState("Roll Rolling Hard");
        }
    }
    private void UpdateFSMRoll(PlayMakerFSM fsm)
    {
        fsm.AddState("Roll Jump Antic");
        fsm.AddState("Roll Jump");
        fsm.AddState("Roll Antic");
        fsm.AddState("Roll Rolling");
        fsm.AddState("Roll Rolling Hard");
        fsm.AddState("Roll Rolling Start");
        fsm.AddState("Roll Rolling Left Wall");
        fsm.AddState("Roll Rolling Right Wall");
        fsm.AddState("Roll Rolling Land");
        UpdateStateRollJumpAntic(fsm);
        UpdateStateRollJump(fsm);
        UpdateStateRollAntic(fsm);
        UpdateStateRollRolling(fsm);
        UpdateStateRollRollingHard(fsm);
        UpdateStateRollRollingStart(fsm);
        UpdateStateRollRollingLeftWall(fsm);
        UpdateStateRollRollingRightWall(fsm);
        UpdateStateRollRollingLand(fsm);
    }
    private void UpdateStateRollJumpAntic(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Roll Jump Antic", fsm.CreateSetVelocity2d(0, 0));
        fsm.AddAction("Roll Jump Antic", fsm.CreateFaceObject(HeroController.instance.gameObject, true));
        fsm.AddAction("Roll Jump Antic", fsm.CreateTk2dPlayAnimationWithEvents(
            fsm.gameObject, "Antic", fsm.GetFSMEvent("FINISHED")));
        fsm.AddTransition("Roll Jump Antic", "FINISHED", "Roll Jump");
    }
    private void UpdateStateRollJump(PlayMakerFSM fsm)
    {
        fsm.AddAction("Roll Jump", fsm.CreateAudioPlaySimple(
            1, prefabs["rollJumpAudio1"] as FsmObject));
        fsm.AddAction("Roll Jump", fsm.CreateAudioPlayerOneShot(
            prefabs["rollJumpAudioPlayer"] as FsmGameObject, fsm.gameObject,
            prefabs["rollJumpAudio2"] as AudioClip[], new float[3] { 1, 1, 1 }, 1, 1, 1, 0));
        var velocity = 60;
        fsm.AddCustomAction("Roll Jump", () =>
        {
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.velocity = new Vector2(0, velocity);
        });
        fsm.AddAction("Roll Jump", fsm.CreateWait(velocity / 180.0f, fsm.GetFSMEvent("FINISHED")));
        fsm.AddTransition("Roll Jump", "FINISHED", "Roll Antic");
    }
    private void UpdateStateRollAntic(PlayMakerFSM fsm)
    {
        var tk2dSpriteAnimator_ = fsm.gameObject.GetComponent<tk2dSpriteAnimator>();
        var oldClip = tk2dSpriteAnimator_.GetClipByName("Stomp Antic");
        var newClip = new tk2dSpriteAnimationClip();
        newClip.CopyFrom(oldClip);
        newClip.frames = new tk2dSpriteAnimationFrame[3];
        newClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
        for (int i = 0; i < newClip.frames.Length; i++)
        {
            newClip.frames[i] = oldClip.frames[i];
        }
        fsm.AddCustomAction("Roll Antic", () =>
        {
            tk2dSpriteAnimator_.Play(newClip);
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.gravityScale = 0;
        });
        fsm.AddTransition("Roll Antic", "FINISHED", "Roll Rolling Start");
    }
    private void UpdateStateRollRollingStart(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Roll Rolling Start", () =>
        {
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            var random = new System.Random();
            var velocityX = (float)((1 - 2 * random.NextDouble()) * 20);
            rigidbody2D.velocity = new Vector2(velocityX, 0);
            rigidbody2D.gravityScale = 2;
            fsm.AccessIntVariable("rollCount").Value = 0;
            fsm.SendEvent("FINISHED");
        });
        fsm.AddTransition("Roll Rolling Start", "FINISHED", "Roll Rolling");
    }
    private void UpdateStateRollRolling(PlayMakerFSM fsm)
    {
        var tk2dSpriteAnimator_ = fsm.gameObject.GetComponent<tk2dSpriteAnimator>();
        var oldClip = tk2dSpriteAnimator_.GetClipByName("Stomp Antic");
        var newClip = new tk2dSpriteAnimationClip();
        newClip.CopyFrom(oldClip);
        newClip.frames = new tk2dSpriteAnimationFrame[2];
        newClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
        newClip.fps /= 3;
        for (int i = 0; i < newClip.frames.Length; i++)
        {
            newClip.frames[i] = oldClip.frames[i + 1];
        }
        fsm.AddCustomAction("Roll Rolling", () => tk2dSpriteAnimator_.Play(newClip));
        fsm.AddAction("Roll Rolling", fsm.CreateCheckCollisionSide(
            fsm.GetFSMEvent("L"), fsm.GetFSMEvent("R"), fsm.GetFSMEvent("LAND")));
        fsm.AddAction("Roll Rolling", fsm.CreateCheckCollisionSideEnter(
            fsm.GetFSMEvent("L"), fsm.GetFSMEvent("R"), fsm.GetFSMEvent("LAND")));
        fsm.AddTransition("Roll Rolling", "L", "Roll Rolling Left Wall");
        fsm.AddTransition("Roll Rolling", "R", "Roll Rolling Right Wall");
        fsm.AddTransition("Roll Rolling", "LAND", "Roll Rolling Land");
    }
    private void UpdateStateRollRollingHard(PlayMakerFSM fsm)
    {
        var tk2dSpriteAnimator_ = fsm.gameObject.GetComponent<tk2dSpriteAnimator>();
        var oldClip = tk2dSpriteAnimator_.GetClipByName("Stomp Antic");
        var newClip = new tk2dSpriteAnimationClip();
        newClip.CopyFrom(oldClip);
        newClip.frames = new tk2dSpriteAnimationFrame[2];
        newClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
        newClip.fps /= 3;
        for (int i = 0; i < newClip.frames.Length; i++)
        {
            newClip.frames[i] = oldClip.frames[i + 1];
        }
        fsm.AddCustomAction("Roll Rolling Hard", () => tk2dSpriteAnimator_.Play(newClip));
        fsm.AddAction("Roll Rolling Hard", fsm.CreateCheckCollisionSide(
            fsm.GetFSMEvent("L"), fsm.GetFSMEvent("R"), fsm.GetFSMEvent("LAND")));
        fsm.AddAction("Roll Rolling Hard", fsm.CreateCheckCollisionSideEnter(
            fsm.GetFSMEvent("L"), fsm.GetFSMEvent("R"), fsm.GetFSMEvent("LAND")));
        fsm.AddAction("Roll Rolling Hard", fsm.CreateWait(0.1f, fsm.GetFSMEvent("1")));
        fsm.AddTransition("Roll Rolling Hard", "L", "Roll Rolling Left Wall");
        fsm.AddTransition("Roll Rolling Hard", "R", "Roll Rolling Right Wall");
        fsm.AddTransition("Roll Rolling Hard", "LAND", "Roll Rolling Land");
        fsm.AddTransition("Roll Rolling Hard", "1", "Roll Rolling");
    }
    private void UpdateStateRollRollingLeftWall(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Roll Rolling Left Wall", () =>
        {
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            var random = new System.Random();
            var velocityX = (float)((1 - 2 * random.NextDouble()) * 20);
            if (velocityX < 0)
            {
                velocityX *= -1;
            }
            var velocityY = rigidbody2D.velocity.y;
            rigidbody2D.velocity = new Vector2(velocityX, velocityY);
            var positon = rigidbody2D.position;
            positon.x += 1e-2f;
            rigidbody2D.position = positon;
            fsm.SendEvent("FINISHED");
        });
        fsm.AddTransition("Roll Rolling Left Wall", "FINISHED", "Roll Rolling");
    }
    private void UpdateStateRollRollingRightWall(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Roll Rolling Right Wall", () =>
        {
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            var random = new System.Random();
            var velocityX = (float)((1 - 2 * random.NextDouble()) * 20);
            if (velocityX > 0)
            {
                velocityX *= -1;
            }
            var velocityY = rigidbody2D.velocity.y;
            rigidbody2D.velocity = new Vector2(velocityX, velocityY);
            var positon = rigidbody2D.position;
            positon.x -= 1e-2f;
            rigidbody2D.position = positon;
            fsm.SendEvent("FINISHED");
        });
        fsm.AddTransition("Roll Rolling Right Wall", "FINISHED", "Roll Rolling");
    }
    private void UpdateStateRollRollingLand(PlayMakerFSM fsm)
    {
        var tk2dSpriteAnimator_ = fsm.gameObject.GetComponent<tk2dSpriteAnimator>();
        var oldClip = tk2dSpriteAnimator_.GetClipByName("Stomp Antic");
        var newClip = new tk2dSpriteAnimationClip();
        newClip.CopyFrom(oldClip);
        newClip.frames = new tk2dSpriteAnimationFrame[3];
        newClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
        for (int i = 0; i < newClip.frames.Length; i++)
        {
            newClip.frames[i] = oldClip.frames[newClip.frames.Length - i - 1];
        }
        fsm.AddCustomAction("Roll Rolling Land", () =>
        {
            fsm.AccessIntVariable("rollCount").Value += 1;
            if (fsm.AccessIntVariable("rollCount").Value == 16)
            {
                var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
                rigidbody2D.velocity = Vector2.zero;
                tk2dSpriteAnimator_.Play(newClip);
            }
            else
            {
                var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
                var random = new System.Random();
                var velocityX = (float)((1 - 2 * random.NextDouble()) * 20);
                var velocityY = 50;
                rigidbody2D.velocity = new Vector2(velocityX, velocityY);
                var positon = rigidbody2D.position;
                positon.y += 1e-2f;
                rigidbody2D.position = positon;
                fsm.SendEvent("1");
            }
        });
        fsm.AddTransition("Roll Rolling Land", "FINISHED", "Idle Start");
        fsm.AddTransition("Roll Rolling Land", "1", "Roll Rolling");
    }
}