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
        prefabs["shockwave"] = (fsm.GetState("Slash Waves R").Actions[0] as SpawnObjectFromGlobalPool).gameObject.Value;
        var brothers = preloadedObjects["GG_Nailmasters"]["Brothers"];
        var oro = brothers.transform.Find("Oro").gameObject;
        var rollRollingLandDust = oro.transform.Find("Pt Dash").gameObject;
        rollRollingLandDust.transform.localPosition = new Vector3(-0.63f, -4.69f, 0.001f);
        prefabs["rollRollingLandDust"] = rollRollingLandDust;
        prefabs["rollRollingLandAudioPlayer"] = (fsm.GetState("Enter 2").Actions[6] as AudioPlayerOneShotSingle).audioPlayer;
        prefabs["rollRollingLandAudio"] = (fsm.GetState("Enter 2").Actions[6] as AudioPlayerOneShotSingle).audioClip.Value;
    }
    private void UpdateHitInstanceRoll(HealthManager healthManager, HitInstance hitInstance)
    {
        var gameObject = healthManager.gameObject;
        var fsm = gameObject.GetComponent<PlayMakerFSM>();
        if (fsm.ActiveStateName == "Roll Rolling")
        {
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            var velocity = rigidbody2D.velocity;
            if (hitInstance.AttackType == AttackTypes.Nail)
            {
                if (hitInstance.Direction == 0)
                {
                    velocity.x = 25;
                }
                else if (hitInstance.Direction == 90)
                {
                    velocity.x = (float)((1 - 2 * random.NextDouble()) * 20);
                    velocity.y = Math.Abs(velocity.y) + 5;
                }
                else if (hitInstance.Direction == 270)
                {
                    velocity.x = (float)((1 - 2 * random.NextDouble()) * 20);
                    velocity.y = -Math.Abs(velocity.y) - 5;
                }
                else
                {
                    velocity.x = -25;
                }
            }
            else if (hitInstance.AttackType == AttackTypes.Spell)
            {
                if (hitInstance.Direction == 0)
                {
                    velocity.x = 25;
                }
                else if (hitInstance.Direction == 90)
                {
                    if (HeroController.instance.transform.position.x < rigidbody2D.transform.position.x)
                    {
                        velocity.x = 25;
                    }
                    else
                    {
                        velocity.x = -25;
                    }
                    velocity.y = Math.Abs(velocity.y) + 5;
                }
                else if (hitInstance.Direction == 270)
                {
                    if (HeroController.instance.transform.position.x < rigidbody2D.transform.position.x)
                    {
                        velocity.x = 25;
                    }
                    else
                    {
                        velocity.x = -25;
                    }
                    velocity.y = -Math.Abs(velocity.y) - 5;
                }
                else
                {
                    velocity.x = -25;
                }
            }
            if (rigidbody2D.position.x < 7.69)
            {
                velocity.x = Math.Abs(velocity.x);
            }
            else if (rigidbody2D.position.x > 45.31)
            {
                velocity.x = -Math.Abs(velocity.x);
            }
            rigidbody2D.velocity = velocity;
        }
    }
    private void UpdateFSMRoll(PlayMakerFSM fsm)
    {
        var greyPrince = GameObject.Find("Grey Prince");
        var rollRollingLandDust = UnityEngine.Object.Instantiate(
            prefabs["rollRollingLandDust"] as GameObject, greyPrince.transform);
        rollRollingLandDust.name = "rollRollingLandDust";
        fsm.AddState("Roll Jump Antic");
        fsm.AddState("Roll Jump");
        fsm.AddState("Roll Antic");
        fsm.AddState("Roll Rolling");
        fsm.AddState("Roll Rolling Start");
        fsm.AddState("Roll Rolling Left Wall");
        fsm.AddState("Roll Rolling Right Wall");
        fsm.AddState("Roll Rolling Land");
        fsm.AddState("Roll Rolling Land Effects");
        UpdateStateRollJumpAntic(fsm);
        UpdateStateRollJump(fsm);
        UpdateStateRollAntic(fsm);
        UpdateStateRollRolling(fsm);
        UpdateStateRollRollingStart(fsm);
        UpdateStateRollRollingLeftWall(fsm);
        UpdateStateRollRollingRightWall(fsm);
        UpdateStateRollRollingLand(fsm);
        UpdateStateRollRollingLandEffects(fsm);
    }
    private void UpdateStateRollJumpAntic(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction(
            "Roll Jump Antic",() => fsm.gameObject.LocateMyFSM("Stun").SendEvent("STUN CONTROL STOP"));
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
        var velocity = (float)(50 + (1 - random.NextDouble() * 2) * 5);
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
    private void UpdateStateRollRollingLeftWall(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Roll Rolling Left Wall", () =>
        {
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            var velocityX = 0;
            var velocityY = rigidbody2D.velocity.y;
            rigidbody2D.velocity = new Vector2(velocityX, velocityY);
            var positon = rigidbody2D.position;
            positon.x += 1e-1f;
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
            var velocityX = 0;
            var velocityY = rigidbody2D.velocity.y;
            rigidbody2D.velocity = new Vector2(velocityX, velocityY);
            var positon = rigidbody2D.position;
            positon.x -= 1e-1f;
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
                fsm.gameObject.LocateMyFSM("Stun").SendEvent("STUN CONTROL START");
            }
            else
            {
                var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
                var velocityX = (float)((1 - 2 * random.NextDouble()) * 20);
                if (rigidbody2D.position.x < 7.69)
                {
                    velocityX = Math.Abs(velocityX);
                }
                else if (rigidbody2D.position.x > 45.31)
                {
                    velocityX = -Math.Abs(velocityX);
                }
                var velocityY = (float)(50 + (1 - random.NextDouble() * 2) * 5);
                var velocity = new Vector2(velocityX, velocityY);
                var heroPositon = HeroController.instance.transform.position;
                var tracking = new Vector2(heroPositon.x - rigidbody2D.position.x, heroPositon.y - rigidbody2D.position.y);
                tracking.y = Math.Max(0, tracking.y);
                tracking *= 1.5f;
                rigidbody2D.velocity = velocity + tracking;
                var positon = rigidbody2D.position;
                positon.y += 1e-1f;
                rigidbody2D.position = positon;
                fsm.SendEvent("1");
            }
        });
        fsm.AddTransition("Roll Rolling Land", "FINISHED", "Idle Start");
        fsm.AddTransition("Roll Rolling Land", "1", "Roll Rolling Land Effects");
    }
    private void UpdateStateRollRollingLandEffects(PlayMakerFSM fsm)
    {
        var spawnObjectFromGlobalPool = fsm.CreateSpawnObjectFromGlobalPool(
            prefabs["shockwave"] as GameObject, fsm.gameObject, new Vector3(4, 0, 0), new Vector3(0, 0, 0));
        fsm.AddAction("Roll Rolling Land Effects", fsm.CreateAudioPlayerOneShotSingle(
          prefabs["rollRollingLandAudioPlayer"] as FsmGameObject, fsm.gameObject,
          prefabs["rollRollingLandAudio"] as AudioClip, 1, 1, 1, 0));
        fsm.AddAction("Roll Rolling Land Effects", fsm.CreatePlayParticleEmitterInState(
            fsm.gameObject.transform.Find("rollRollingLandDust").gameObject));
        fsm.AddAction("Roll Rolling Land Effects", spawnObjectFromGlobalPool);
        fsm.AddCustomAction("Roll Rolling Land Effects", () =>
        {
            var shockwave = spawnObjectFromGlobalPool.storeObject.Value;
            var localScale = shockwave.transform.localScale;
            localScale.x = 1.4f;
            shockwave.transform.localScale = localScale;
            var position = shockwave.transform.position;
            position.y = 4.8f;
            position.z = 0.003f;
            if (fsm.gameObject.transform.position.x < HeroController.instance.transform.position.x)
            {
                position.x -= 2.5f;
                shockwave.LocateMyFSM("shockwave").AccessBoolVariable("Facing Right").Value = true;
            }
            else
            {
                position.x -= 6;
                shockwave.LocateMyFSM("shockwave").AccessBoolVariable("Facing Right").Value = false;
            }
            shockwave.transform.position = position;
            shockwave.LocateMyFSM("Spawn").AccessIntVariable("Damage").Value = 1;
            shockwave.LocateMyFSM("shockwave").AccessFloatVariable("Speed").Value = 26;
            fsm.SendEvent("FINISHED");
        });
        fsm.AddTransition("Roll Rolling Land Effects", "FINISHED", "Roll Rolling");
    }
}