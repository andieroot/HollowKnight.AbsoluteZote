namespace AnyZote;

public partial class Control : Module
{
    private void LoadPrefabsCycloneSlash(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        var greyPrince = preloadedObjects["GG_Grey_Prince_Zote"]["Grey Prince"];
        var fsm = greyPrince.LocateMyFSM("Control");
        prefabs["cycloneSlashJumpAudio1"] = (fsm.GetState("Jump").Actions[0] as AudioPlaySimple).oneShotClip;
        prefabs["cycloneSlashJumpAudioPlayer"] = (fsm.GetState("Jump").Actions[1] as AudioPlayerOneShot).audioPlayer;
        prefabs["cycloneSlashJumpAudio2"] = (fsm.GetState("Jump").Actions[1] as AudioPlayerOneShot).audioClips;
        prefabs["cycloneSlashJumpLandCamera"] = (fsm.GetState("Land Normal").Actions[1] as SendEventByName).eventTarget;
        prefabs["cycloneSlashChargeAudioPlayer"] = (fsm.GetState("FT Through").Actions[2] as AudioPlayerOneShot).audioPlayer;
        prefabs["cycloneSlashChargeAudio"] = (fsm.GetState("FT Through").Actions[2] as AudioPlayerOneShot).audioClips;
        var sly = preloadedObjects["GG_Sly"]["Battle Scene"].transform.Find("Sly Boss").gameObject;
        fsm = sly.LocateMyFSM("Control");
        prefabs["cyclonePlayer"] = (fsm.GetState("Cycloning").Actions[3] as AudioPlayerOneShotSingle).audioPlayer;
        prefabs["cycloneAudio"] = (fsm.GetState("Cycloning").Actions[3] as AudioPlayerOneShotSingle).audioClip;
        var cycloneTink = sly.transform.Find("S1").gameObject;
        prefabs["cycloneTink"] = cycloneTink;
        var brothers = preloadedObjects["GG_Nailmasters"]["Brothers"];
        var oro = brothers.transform.Find("Oro").gameObject;
        fsm = oro.LocateMyFSM("nailmaster");
        var cycloneSlashChargeChargeEffect = oro.transform.Find("Charge Effect").gameObject;
        cycloneSlashChargeChargeEffect.transform.localPosition = new Vector3(0, -3f, 0.001f);
        cycloneSlashChargeChargeEffect.transform.localScale = new Vector3(2, 1, 1);
        prefabs["cycloneSlashChargeChargeEffect"] = cycloneSlashChargeChargeEffect;
        var cycloneSlashChargeNACharge = oro.transform.Find("NA Charge").gameObject;
        cycloneSlashChargeNACharge.transform.localPosition = new Vector3(0, -2f, -0.001f);
        prefabs["cycloneSlashChargeNACharge"] = cycloneSlashChargeNACharge;
        var cycloneSlashChargeNACharged = oro.transform.Find("NA Charged").gameObject;
        cycloneSlashChargeNACharged.transform.localPosition = new Vector3(0, -2f, -0.001f);
        prefabs["cycloneSlashChargeNACharged"] = cycloneSlashChargeNACharged;
        var cycloneSlashChargePtDash = oro.transform.Find("Pt Dash").gameObject;
        cycloneSlashChargePtDash.transform.localPosition = new Vector3(-0.63f, -4.69f, 0.001f);
        prefabs["cycloneSlashChargePtDash"] = cycloneSlashChargePtDash;
        prefabs["cycloneSlashSlashAudioPlayer"] = (fsm.GetState("D Slash").Actions[1] as AudioPlayerOneShotSingle).audioPlayer;
        prefabs["cycloneSlashSlashAudio"] = (fsm.GetState("D Slash").Actions[1] as AudioPlayerOneShotSingle).audioClip;
        var cycloneSlashSlashFlash1 = oro.transform.Find("Dash Slash").gameObject;
        cycloneSlashSlashFlash1.transform.localPosition = new Vector3(2.69f, -1.37f, 0);
        var localScale = cycloneSlashSlashFlash1.transform.localScale;
        cycloneSlashSlashFlash1.transform.localScale = localScale;
        prefabs["cycloneSlashSlashFlash1"] = cycloneSlashSlashFlash1;
        var cycloneSlashSlashFlash2 = oro.transform.Find("Sharp Flash").gameObject;
        cycloneSlashSlashFlash2.transform.localPosition = new Vector3(7, -1.5f, 0);
        localScale = cycloneSlashSlashFlash2.transform.localScale;
        cycloneSlashSlashFlash2.transform.localScale = localScale;
        prefabs["cycloneSlashSlashFlash2"] = cycloneSlashSlashFlash2;
        var battleScene = preloadedObjects["GG_Nosk_Hornet"]["Battle Scene"];
        var hornetNosk = battleScene.transform.Find("Hornet Nosk").gameObject;
        fsm = hornetNosk.LocateMyFSM("Hornet Nosk");
        var cycloneSlashChargeBlob = (fsm.GetState("Spit 1").Actions[1] as FlingObjectsFromGlobalPool).gameObject;
        prefabs["cycloneSlashChargeBlob"] = cycloneSlashChargeBlob;
    }
    private void UpdateFSMCycloneSlash(PlayMakerFSM fsm)
    {
        var greyPrince = GameObject.Find("Grey Prince");
        var cycloneSlashChargeChargeEffect = UnityEngine.Object.Instantiate(prefabs["cycloneSlashChargeChargeEffect"] as GameObject, greyPrince.transform);
        cycloneSlashChargeChargeEffect.name = "cycloneSlashChargeChargeEffect";
        var cycloneTink = UnityEngine.Object.Instantiate(prefabs["cycloneTink"] as GameObject, greyPrince.transform);
        cycloneTink.name = "cycloneTink";
        cycloneTink.SetActive(false);
        cycloneTink.transform.localPosition = new Vector3(-8, -2.5f, 0);
        cycloneTink.transform.localScale = new Vector3(3, 1.15f, 1);
        cycloneTink.transform.localRotation = Quaternion.Euler(0, 0, 335);
        cycloneTink = UnityEngine.Object.Instantiate(prefabs["cycloneTink"] as GameObject, greyPrince.transform);
        cycloneTink.name = "cycloneTink2";
        cycloneTink.SetActive(false);
        cycloneTink.transform.localPosition = new Vector3(8, -2.5f, 0);
        cycloneTink.transform.localScale = new Vector3(-3, 1.15f, 1);
        cycloneTink.transform.localRotation = Quaternion.Euler(0, 0, 25);
        var cycloneEffect = HeroController.instance.gameObject.transform.Find("Attacks").gameObject.transform.Find("Cyclone Slash").gameObject;
        cycloneEffect = UnityEngine.Object.Instantiate(cycloneEffect as GameObject, greyPrince.transform);
        cycloneEffect.name = "cycloneEffect";
        cycloneEffect.SetActive(false);
        cycloneEffect.transform.localPosition = new Vector3(0, -1.7f, -0.0013f);
        cycloneEffect.transform.localScale = new Vector3(2.5f, 3, 1.3863f);
        var hits = cycloneEffect.transform.Find("Hits").gameObject;
        hits.transform.Find("Hit L").gameObject.RemoveComponent<PolygonCollider2D>();
        hits.transform.Find("Hit R").gameObject.RemoveComponent<PolygonCollider2D>();
        var cycloneSlashChargeNACharge = UnityEngine.Object.Instantiate(prefabs["cycloneSlashChargeNACharge"] as GameObject, greyPrince.transform);
        cycloneSlashChargeNACharge.name = "cycloneSlashChargeNACharge";
        var cycloneSlashChargeNACharged = UnityEngine.Object.Instantiate(prefabs["cycloneSlashChargeNACharged"] as GameObject, greyPrince.transform);
        cycloneSlashChargeNACharged.name = "cycloneSlashChargeNACharged";
        var cycloneSlashChargePtDash = UnityEngine.Object.Instantiate(prefabs["cycloneSlashChargePtDash"] as GameObject, greyPrince.transform);
        cycloneSlashChargePtDash.name = "cycloneSlashChargePtDash";
        var cycloneSlashSlashFlash1 = UnityEngine.Object.Instantiate(prefabs["cycloneSlashSlashFlash1"] as GameObject, greyPrince.transform);
        cycloneSlashSlashFlash1.name = "cycloneSlashSlashFlash1";
        var cycloneSlashSlashFlash2 = UnityEngine.Object.Instantiate(prefabs["cycloneSlashSlashFlash2"] as GameObject, greyPrince.transform);
        cycloneSlashSlashFlash2.name = "cycloneSlashSlashFlash2";
        fsm.AddState("Cyclone Slash Jump Antic");
        fsm.AddState("Cyclone Slash Jump");
        fsm.AddState("Cyclone Slash Charge");
        fsm.AddState("Cyclone Slash Charged");
        fsm.AddState("Cyclone Slash Dash");
        fsm.AddState("Cyclone Slash Slash");
        UpdateStateCycloneSlashJumpAntic(fsm);
        UpdateStateCycloneSlashJump(fsm);
        UpdateStateCycloneSlashCharge(fsm);
        UpdateStateCycloneSlashCharged(fsm);
        UpdateStateCycloneSlashDash(fsm);
        UpdateStateCycloneSlashSlash(fsm);
    }
    private void UpdateStateCycloneSlashJumpAntic(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Cyclone Slash Jump Antic", fsm.CreateSetVelocity2d(0, 0));
        fsm.AccessFloatVariable("cycloneSlashTargetLeft").Value = 8.19f;
        fsm.AccessFloatVariable("cycloneSlashTargetRight").Value = 44.61f;
        fsm.AddCustomAction("Cyclone Slash Jump Antic", () =>
        {
            var x = HeroController.instance.transform.position.x;
            float destination;
            float destinationNext;
            var myX = fsm.gameObject.transform.position.x;
            float dis = 3;
            if (x < myX)
            {
                destination = myX + 8;
                if (destination > 44.61f)
                    destination = 44.61f;
            }
            else
            {
                destination = myX - 8;
                if (destination < 8.19f)
                    destination = 8.19f;
            }
            destinationNext = x;
            fsm.AccessFloatVariable("cycloneSlashDestination").Value = destination;
            fsm.AccessFloatVariable("cycloneSlashDestinationNext").Value = destinationNext;
            fsm.AccessBoolVariable("cycloneSlashCancel").Value = false;
        });
        fsm.AddCustomAction("Cyclone Slash Jump Antic", fsm.CreateFacePosition("cycloneSlashDestinationNext", true));
        fsm.AddAction("Cyclone Slash Jump Antic", fsm.CreateTk2dPlayAnimationWithEvents(
            fsm.gameObject, "Antic", fsm.GetFSMEvent("FINISHED")));
        fsm.AddTransition("Cyclone Slash Jump Antic", "FINISHED", "Cyclone Slash Jump");
    }
    private void UpdateStateCycloneSlashJump(PlayMakerFSM fsm)
    {
        fsm.AddAction("Cyclone Slash Jump", fsm.CreateAudioPlaySimple(
            1, prefabs["cycloneSlashJumpAudio1"] as FsmObject));
        fsm.AddAction("Cyclone Slash Jump", fsm.CreateAudioPlayerOneShot(
            prefabs["cycloneSlashJumpAudioPlayer"] as FsmGameObject, fsm.gameObject,
            prefabs["cycloneSlashJumpAudio2"] as AudioClip[], new float[3] { 1, 1, 1 }, 1, 1, 1, 0));
        fsm.AddCustomAction("Cyclone Slash Jump", () =>
        {
            var destination = fsm.AccessFloatVariable("cycloneSlashDestination").Value;
            float velocityX = 2 * (destination - fsm.gameObject.transform.position.x);
            float velocityY = 60;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 6;
            rigidbody2D.velocity = new Vector2(velocityX, velocityY);
        });
        fsm.AddAction("Cyclone Slash Jump", fsm.CreateCheckCollisionSide(null, null, fsm.GetFSMEvent("LAND")));
        fsm.AddAction("Cyclone Slash Jump", fsm.CreateCheckCollisionSideEnter(null, null, fsm.GetFSMEvent("LAND")));
        fsm.AddTransition("Cyclone Slash Jump", "LAND", "Cyclone Slash Charge");
    }
    private void UpdateStateCycloneSlashCharge(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Cyclone Slash Charge", () =>
        {
            var gameObjectPrefab = (prefabs["cycloneSlashChargeBlob"] as FsmGameObject).Value;
            Vector3 a = fsm.gameObject.transform.position;
            int num = UnityEngine.Random.Range(8, 17);
            for (int i = 1; i <= 0; i++)
            {
                GameObject gameObject = gameObjectPrefab.Spawn(a, Quaternion.Euler(Vector3.zero));
                float x = gameObject.transform.position.x;
                float y = gameObject.transform.position.y;
                float z = gameObject.transform.position.z;
                float num2 = 45 * (float)i / num;
                float num3 = UnityEngine.Random.Range(50, 71);
                if (fsm.gameObject.transform.localScale.x < 0)
                {
                    num3 = 180 - num3;
                }
                var vectorX = num2 * Mathf.Cos(num3 * 0.017453292f);
                var vectorY = num2 * Mathf.Sin(num3 * 0.017453292f);
                Vector2 velocity;
                velocity.x = vectorX;
                velocity.y = vectorY;
                gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
            }
        });
        fsm.AddAction("Cyclone Slash Charge", fsm.CreateSendEventByName(
            prefabs["cycloneSlashJumpLandCamera"] as FsmEventTarget, "AverageShake", 0));
        fsm.AddCustomAction("Cyclone Slash Charge", () =>
        {
            var tk2dSpriteAnimator_ = fsm.gameObject.GetComponent<tk2dSpriteAnimator>();
            var oldClip = tk2dSpriteAnimator_.GetClipByName("Spit");
            var newClip = new tk2dSpriteAnimationClip();
            newClip.CopyFrom(oldClip);
            newClip.frames = new tk2dSpriteAnimationFrame[2];
            newClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            for (int i = 0; i < newClip.frames.Length; i++)
            {
                newClip.frames[i] = oldClip.frames[oldClip.frames.Length - i - 1];
            }
            tk2dSpriteAnimator_.Play(newClip);
            var x = fsm.gameObject.transform.position.x;
            float destination;
            int direction;
            var slack = 10;
            var cycloneSlashTargetLeft = fsm.AccessFloatVariable("cycloneSlashTargetLeft").Value;
            var cycloneSlashTargetRight = fsm.AccessFloatVariable("cycloneSlashTargetRight").Value;
            if (x > HeroController.instance.transform.position.x)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            fsm.AccessFloatVariable("cycloneSlashDestination").Value = HeroController.instance.transform.position.x;
            fsm.AccessIntVariable("cycloneSlashDirection").Value = direction;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 3;
            rigidbody2D.velocity = Vector2.zero;
            fsm.gameObject.transform.Find("cycloneSlashChargeChargeEffect").gameObject.SetActive(true);
            fsm.gameObject.transform.Find("cycloneSlashChargeNACharge").gameObject.SetActive(true);
        });
        fsm.AddCustomAction("Cyclone Slash Charge", fsm.CreateFacePosition("cycloneSlashDestination", true));
        fsm.AddAction("Cyclone Slash Charge", fsm.CreateWait(0.4f, fsm.GetFSMEvent("1")));
        fsm.AddTransition("Cyclone Slash Charge", "1", "Cyclone Slash Charged");
    }
    private void UpdateStateCycloneSlashCharged(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Cyclone Slash Charged", () =>
        {
            fsm.gameObject.transform.Find("cycloneSlashChargeNACharge").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneSlashChargeNACharged").gameObject.SetActive(true);
        });
        fsm.AddAction("Cyclone Slash Charged", fsm.CreateWait(0.25f, fsm.GetFSMEvent("FINISHED")));
        fsm.AddTransition("Cyclone Slash Charged", "FINISHED", "Cyclone Slash Dash");
    }
    private void UpdateStateCycloneSlashDash(PlayMakerFSM fsm)
    {
        fsm.AddAction("Cyclone Slash Dash", fsm.CreatePlayParticleEmitterInState(
            fsm.gameObject.transform.Find("cycloneSlashChargePtDash").gameObject));
        fsm.AddCustomAction("Cyclone Slash Dash", () =>
        {
            var tk2dSpriteAnimator_ = fsm.gameObject.GetComponent<tk2dSpriteAnimator>();
            var oldClip = tk2dSpriteAnimator_.GetClipByName("Stomp Slash End");
            var oldClip2 = tk2dSpriteAnimator_.GetClipByName("Stomp Shift");
            var newClip = new tk2dSpriteAnimationClip();
            newClip.CopyFrom(oldClip);
            newClip.frames = new tk2dSpriteAnimationFrame[1 + 2];
            newClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.PingPong;
            for (int i = 0; i < newClip.frames.Length; i++)
            {
                newClip.frames[i] = oldClip.frames[oldClip.frames.Length - i - 1];
            }
            for (int i = 0; i < 2; ++i)
            {
                newClip.frames[1 + i] = oldClip2.frames[i];
            }
            tk2dSpriteAnimator_.Play(newClip);
            fsm.gameObject.transform.Find("cycloneSlashChargeChargeEffect").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneSlashChargeNACharged").gameObject.SetActive(false);
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            var v = fsm.AccessIntVariable("cycloneSlashDirection").Value * 32;
            rigidbody2D.gravityScale = 1;
            rigidbody2D.velocity = new Vector2(v, 32);
            var position = rigidbody2D.position;
            position.y += 0.1f;
            rigidbody2D.position = position;
            fsm.gameObject.transform.Find("cycloneTink").gameObject.SetActive(true);
            fsm.gameObject.transform.Find("cycloneTink2").gameObject.SetActive(true);
            fsm.gameObject.transform.Find("cycloneEffect").gameObject.SetActive(true);
        });
        fsm.AddAction("Cyclone Slash Dash", fsm.CreateGeneralAction(() =>
        {
            var x = fsm.gameObject.transform.position.x;
            var heroX = HeroController.instance.gameObject.transform.position.x;
            if (Math.Abs(x - heroX) < 1)
            {
                var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
                var v = rigidbody2D.velocity;
                v.y = -32;
                rigidbody2D.velocity = v;
                if (!fsm.AccessBoolVariable("cycloneSlashCancel").Value)
                {
                    fsm.AccessBoolVariable("cycloneSlashCancel").Value = true;
                    if (random.Next(2) == 0)
                    {
                        rigidbody2D.velocity = new Vector2(0, 0);
                        rigidbody2D.gravityScale = 0;
                        fsm.gameObject.transform.Find("cycloneTink").gameObject.SetActive(false);
                        fsm.gameObject.transform.Find("cycloneTink2").gameObject.SetActive(false);
                        fsm.gameObject.transform.Find("cycloneEffect").gameObject.SetActive(false);
                        fsm.SetState("Stomp");
                    }
                }
            }
        }));
        fsm.AddAction("Cyclone Slash Dash", fsm.CreateCheckCollisionSide(null, null, fsm.GetFSMEvent("LAND")));
        fsm.AddAction("Cyclone Slash Dash", fsm.CreateCheckCollisionSideEnter(null, null, fsm.GetFSMEvent("LAND")));
        fsm.AddAction("Cyclone Slash Dash", fsm.CreateAudioPlayerOneShot(
            prefabs["cycloneSlashJumpAudioPlayer"] as FsmGameObject, fsm.gameObject,
            prefabs["cycloneSlashJumpAudio2"] as AudioClip[], new float[3] { 1, 1, 1 }, 1, 1, 1, 0));
        fsm.AddTransition("Cyclone Slash Dash", "LAND", "Cyclone Slash Slash");
    }
    private void UpdateStateCycloneSlashSlash(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Cyclone Slash Slash", () =>
        {
            var tk2dSpriteAnimator_ = fsm.gameObject.GetComponent<tk2dSpriteAnimator>();
            var oldClip = tk2dSpriteAnimator_.GetClipByName("Stomp Slash End");
            var newClip = new tk2dSpriteAnimationClip();
            newClip.CopyFrom(oldClip);
            newClip.frames = new tk2dSpriteAnimationFrame[3];
            newClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            for (int i = 0; i < newClip.frames.Length; i++)
            {
                newClip.frames[i] = oldClip.frames[oldClip.frames.Length - i - 1];
            }
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.velocity = new Vector2(0, 0);
            rigidbody2D.gravityScale = 3;
            fsm.gameObject.transform.Find("cycloneTink").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneTink2").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("cycloneEffect").gameObject.SetActive(false);
        });
        fsm.AddTransition("Cyclone Slash Slash", "FINISHED", "Move Choice 3");
    }
}