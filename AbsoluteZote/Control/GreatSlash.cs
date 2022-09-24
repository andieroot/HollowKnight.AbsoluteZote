namespace AbsoluteZote;

public partial class Control : Module
{
    private void LoadPrefabsGreatSlash(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        var greyPrince = preloadedObjects["GG_Grey_Prince_Zote"]["Grey Prince"];
        var fsm = greyPrince.LocateMyFSM("Control");
        prefabs["greatSlashJumpAudio1"] = (fsm.GetState("Jump").Actions[0] as AudioPlaySimple).oneShotClip;
        prefabs["greatSlashJumpAudioPlayer"] = (fsm.GetState("Jump").Actions[1] as AudioPlayerOneShot).audioPlayer;
        prefabs["greatSlashJumpAudio2"] = (fsm.GetState("Jump").Actions[1] as AudioPlayerOneShot).audioClips;
        prefabs["greatSlashJumpLandCamera"] = (fsm.GetState("Land Normal").Actions[1] as SendEventByName).eventTarget;
        prefabs["greatSlashChargeAudioPlayer"] = (fsm.GetState("FT Through").Actions[2] as AudioPlayerOneShot).audioPlayer;
        prefabs["greatSlashChargeAudio"] = (fsm.GetState("FT Through").Actions[2] as AudioPlayerOneShot).audioClips;
        var brothers = preloadedObjects["GG_Nailmasters"]["Brothers"];
        var oro = brothers.transform.Find("Oro").gameObject;
        fsm = oro.LocateMyFSM("nailmaster");
        var greatSlashChargeChargeEffect = oro.transform.Find("Charge Effect").gameObject;
        greatSlashChargeChargeEffect.transform.localPosition = new Vector3(0, -3f, 0.001f);
        greatSlashChargeChargeEffect.transform.localScale = new Vector3(2, 1, 1);
        prefabs["greatSlashChargeChargeEffect"] = greatSlashChargeChargeEffect;
        var greatSlashChargeNACharge = oro.transform.Find("NA Charge").gameObject;
        greatSlashChargeNACharge.transform.localPosition = new Vector3(0, -2f, -0.001f);
        prefabs["greatSlashChargeNACharge"] = greatSlashChargeNACharge;
        var greatSlashChargeNACharged = oro.transform.Find("NA Charged").gameObject;
        greatSlashChargeNACharged.transform.localPosition = new Vector3(0, -2f, -0.001f);
        prefabs["greatSlashChargeNACharged"] = greatSlashChargeNACharged;
        var greatSlashChargePtDash = oro.transform.Find("Pt Dash").gameObject;
        greatSlashChargePtDash.transform.localPosition = new Vector3(-0.63f, -4.69f, 0.001f);
        prefabs["greatSlashChargePtDash"] = greatSlashChargePtDash;
        prefabs["greatSlashSlashAudioPlayer"] = (fsm.GetState("D Slash").Actions[1] as AudioPlayerOneShotSingle).audioPlayer;
        prefabs["greatSlashSlashAudio"] = (fsm.GetState("D Slash").Actions[1] as AudioPlayerOneShotSingle).audioClip;
        var greatSlashSlashFlash1 = oro.transform.Find("Dash Slash").gameObject;
        greatSlashSlashFlash1.transform.localPosition = new Vector3(2.69f, -1.37f, 0);
        var localScale = greatSlashSlashFlash1.transform.localScale;
        localScale.x *= -1;
        greatSlashSlashFlash1.transform.localScale = localScale;
        prefabs["greatSlashSlashFlash1"] = greatSlashSlashFlash1;
        var greatSlashSlashFlash2 = oro.transform.Find("Sharp Flash").gameObject;
        greatSlashSlashFlash2.transform.localPosition = new Vector3(7, -1.5f, 0);
        localScale = greatSlashSlashFlash2.transform.localScale;
        localScale.x *= -1;
        greatSlashSlashFlash2.transform.localScale = localScale;
        prefabs["greatSlashSlashFlash2"] = greatSlashSlashFlash2;
        var battleScene = preloadedObjects["GG_Nosk_Hornet"]["Battle Scene"];
        var hornetNosk = battleScene.transform.Find("Hornet Nosk").gameObject;
        fsm = hornetNosk.LocateMyFSM("Hornet Nosk");
        var greatSlashChargeBlob = (fsm.GetState("Spit 1").Actions[1] as FlingObjectsFromGlobalPool).gameObject;
        prefabs["greatSlashChargeBlob"] = greatSlashChargeBlob;
    }
    private void UpdateFSMGreatSlash(PlayMakerFSM fsm)
    {
        var greyPrince = GameObject.Find("Grey Prince");
        var greatSlashChargeChargeEffect = UnityEngine.Object.Instantiate(prefabs["greatSlashChargeChargeEffect"] as GameObject, greyPrince.transform);
        greatSlashChargeChargeEffect.name = "greatSlashChargeChargeEffect";
        var greatSlashChargeNACharge = UnityEngine.Object.Instantiate(prefabs["greatSlashChargeNACharge"] as GameObject, greyPrince.transform);
        greatSlashChargeNACharge.name = "greatSlashChargeNACharge";
        var greatSlashChargeNACharged = UnityEngine.Object.Instantiate(prefabs["greatSlashChargeNACharged"] as GameObject, greyPrince.transform);
        greatSlashChargeNACharged.name = "greatSlashChargeNACharged";
        var greatSlashChargePtDash = UnityEngine.Object.Instantiate(prefabs["greatSlashChargePtDash"] as GameObject, greyPrince.transform);
        greatSlashChargePtDash.name = "greatSlashChargePtDash";
        var greatSlashSlashFlash1 = UnityEngine.Object.Instantiate(prefabs["greatSlashSlashFlash1"] as GameObject, greyPrince.transform);
        greatSlashSlashFlash1.name = "greatSlashSlashFlash1";
        var greatSlashSlashFlash2 = UnityEngine.Object.Instantiate(prefabs["greatSlashSlashFlash2"] as GameObject, greyPrince.transform);
        greatSlashSlashFlash2.name = "greatSlashSlashFlash2";
        fsm.AddState("Great Slash Jump Antic");
        fsm.AddState("Great Slash Jump");
        fsm.AddState("Great Slash Charge");
        fsm.AddState("Great Slash Charged");
        fsm.AddState("Great Slash Dash");
        fsm.AddState("Great Slash Slash");
        UpdateStateGreatSlashJumpAntic(fsm);
        UpdateStateGreatSlashJump(fsm);
        UpdateStateGreatSlashCharge(fsm);
        UpdateStateGreatSlashCharged(fsm);
        UpdateStateGreatSlashDash(fsm);
        UpdateStateGreatSlashSlash(fsm);
    }
    private void UpdateStateGreatSlashJumpAntic(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Great Slash Jump Antic", fsm.CreateSetVelocity2d(0, 0));
        fsm.AccessFloatVariable("greatSlashTargetLeft").Value = 8.19f;
        fsm.AccessFloatVariable("greatSlashTargetRight").Value = 44.61f;
        fsm.AddCustomAction("Great Slash Jump Antic", () =>
        {
            var x = HeroController.instance.transform.position.x;
            float destination;
            float destinationNext;
            var myX=fsm.gameObject.transform.position.x;
            float dis = 5;
            if (x < myX)
            {
                if (Math.Abs(x - myX) < dis)
                {
                    destination = myX - 1e-3f;
                }
                else
                {
                    destination = x + dis;
                }
            }
            else
            {
                if (Math.Abs(x - myX) < dis)
                {
                    destination = myX + 1e-3f;
                }
                else
                {
                    destination = x - dis;
                }
            }
            destinationNext = destination;
            fsm.AccessFloatVariable("greatSlashDestination").Value = destination;
            fsm.AccessFloatVariable("greatSlashDestinationNext").Value = destinationNext;
        });
        fsm.AddCustomAction("Great Slash Jump Antic", fsm.CreateFacePosition("greatSlashDestinationNext", true));
        fsm.AddAction("Great Slash Jump Antic", fsm.CreateTk2dPlayAnimationWithEvents(
            fsm.gameObject, "Antic", fsm.GetFSMEvent("FINISHED")));
        fsm.AddTransition("Great Slash Jump Antic", "FINISHED", "Great Slash Jump");
    }
    private void UpdateStateGreatSlashJump(PlayMakerFSM fsm)
    {
        fsm.AddAction("Great Slash Jump", fsm.CreateAudioPlaySimple(
            1, prefabs["greatSlashJumpAudio1"] as FsmObject));
        fsm.AddAction("Great Slash Jump", fsm.CreateAudioPlayerOneShot(
            prefabs["greatSlashJumpAudioPlayer"] as FsmGameObject, fsm.gameObject,
            prefabs["greatSlashJumpAudio2"] as AudioClip[], new float[3] { 1, 1, 1 }, 1, 1, 1, 0));
        fsm.AddCustomAction("Great Slash Jump", () =>
        {
            var destination = fsm.AccessFloatVariable("greatSlashDestination").Value;
            float velocityX = 2 * (destination - fsm.gameObject.transform.position.x);
            float velocityY = 90;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 6;
            rigidbody2D.velocity = new Vector2(velocityX, velocityY);
        });
        fsm.AddAction("Great Slash Jump", fsm.CreateCheckCollisionSide(null, null, fsm.GetFSMEvent("LAND")));
        fsm.AddAction("Great Slash Jump", fsm.CreateCheckCollisionSideEnter(null, null, fsm.GetFSMEvent("LAND")));
        fsm.AddTransition("Great Slash Jump", "LAND", "Great Slash Charge");
    }
    private void UpdateStateGreatSlashCharge(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Great Slash Charge", () =>
        {
            var gameObjectPrefab = (prefabs["greatSlashChargeBlob"] as FsmGameObject).Value;
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
        fsm.AddAction("Great Slash Charge", fsm.CreateSendEventByName(
            prefabs["greatSlashJumpLandCamera"] as FsmEventTarget, "AverageShake", 0));
        fsm.AddAction("Great Slash Charge", fsm.CreateAudioPlayerOneShot(
            prefabs["greatSlashChargeAudioPlayer"] as FsmGameObject, fsm.gameObject,
            prefabs["greatSlashChargeAudio"] as AudioClip[], new float[2] { 1, 1 }, 1, 1, 1, 0));
        fsm.AddCustomAction("Great Slash Charge", () =>
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
            var greatSlashTargetLeft = fsm.AccessFloatVariable("greatSlashTargetLeft").Value;
            var greatSlashTargetRight = fsm.AccessFloatVariable("greatSlashTargetRight").Value;
            if (x - greatSlashTargetLeft > greatSlashTargetRight - x)
            {
                destination = greatSlashTargetLeft + slack;
                direction = -1;
            }
            else
            {
                destination = greatSlashTargetRight - slack;
                direction = 1;
            }
            fsm.AccessFloatVariable("greatSlashDestination").Value = destination;
            fsm.AccessIntVariable("greatSlashDirection").Value = direction;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 3;
            rigidbody2D.velocity = Vector2.zero;
            fsm.gameObject.transform.Find("greatSlashChargeChargeEffect").gameObject.SetActive(true);
            fsm.gameObject.transform.Find("greatSlashChargeNACharge").gameObject.SetActive(true);
        });
        fsm.AddCustomAction("Great Slash Charge", fsm.CreateFacePosition("greatSlashDestination", true));
        fsm.AddAction("Great Slash Charge", fsm.CreateWait(1, fsm.GetFSMEvent("1")));
        fsm.AddTransition("Great Slash Charge", "1", "Great Slash Charged");
    }
    private void UpdateStateGreatSlashCharged(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Great Slash Charged", () =>
        {
            fsm.gameObject.transform.Find("greatSlashChargeNACharge").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("greatSlashChargeNACharged").gameObject.SetActive(true);
        });
        fsm.AddAction("Great Slash Charged", fsm.CreateWait(0.25f, fsm.GetFSMEvent("FINISHED")));
        fsm.AddTransition("Great Slash Charged", "FINISHED", "Great Slash Dash");
    }
    private void UpdateStateGreatSlashDash(PlayMakerFSM fsm)
    {
        fsm.AddAction("Great Slash Dash", fsm.CreatePlayParticleEmitterInState(
            fsm.gameObject.transform.Find("greatSlashChargePtDash").gameObject));
        fsm.AddCustomAction("Great Slash Dash", () =>
        {
            var tk2dSpriteAnimator_ = fsm.gameObject.GetComponent<tk2dSpriteAnimator>();
            var oldClip = tk2dSpriteAnimator_.GetClipByName("Stomp Slash End");
            var newClip = new tk2dSpriteAnimationClip();
            newClip.CopyFrom(oldClip);
            newClip.frames = new tk2dSpriteAnimationFrame[1];
            newClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
            for (int i = 0; i < newClip.frames.Length; i++)
            {
                newClip.frames[i] = oldClip.frames[oldClip.frames.Length - i - 1];
            }
            tk2dSpriteAnimator_.Play(newClip);
            fsm.gameObject.transform.Find("greatSlashChargeChargeEffect").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("greatSlashChargeNACharged").gameObject.SetActive(false);
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            var v = fsm.AccessIntVariable("greatSlashDirection").Value * 64;
            rigidbody2D.velocity = new Vector2(v, 0);
        });
        fsm.AddAction("Great Slash Dash", fsm.CreateGeneralAction(() =>
        {
            var destination = fsm.AccessFloatVariable("greatSlashDestination");
            var direction = fsm.AccessIntVariable("greatSlashDirection");
            var x = fsm.gameObject.transform.position.x;
            if ((x - destination.Value) * direction.Value >= 0)
            {
                fsm.SendEvent("1");
            }
            if ((x - HeroController.instance.transform.position.x) * direction.Value >= 0)
            {
                fsm.SendEvent("1");
            }
        }));
        fsm.AddTransition("Great Slash Dash", "1", "Great Slash Slash");
    }
    private void UpdateStateGreatSlashSlash(PlayMakerFSM fsm)
    {
        fsm.AddAction("Great Slash Slash", fsm.CreateAudioPlayerOneShotSingle(
            prefabs["greatSlashSlashAudioPlayer"] as FsmGameObject, fsm.gameObject,
            prefabs["greatSlashSlashAudio"] as FsmObject, 1, 1, 1, 0));
        fsm.AddCustomAction("Great Slash Slash", () =>
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
            tk2dSpriteAnimator_.Play(newClip);
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.velocity = new Vector2(0, 0);
            fsm.gameObject.transform.Find("greatSlashSlashFlash1").gameObject.SetActive(true);
            fsm.gameObject.transform.Find("greatSlashSlashFlash2").gameObject.SetActive(true);
        });
        fsm.AddAction("Great Slash Slash", fsm.CreateWait(0.25f, fsm.GetFSMEvent("1")));
        fsm.AddTransition("Great Slash Slash", "1", "Move Choice 3");
    }
}