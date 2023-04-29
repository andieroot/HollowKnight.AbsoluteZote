﻿namespace AnyZote;

public partial class Control : Module
{
    private void LoadPrefabsDashSlash(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        var greyPrince = preloadedObjects["GG_Grey_Prince_Zote"]["Grey Prince"];
        var fsm = greyPrince.LocateMyFSM("Control");
        prefabs["dashSlashJumpAudio1"] = (fsm.GetState("Jump").Actions[0] as AudioPlaySimple).oneShotClip;
        prefabs["dashSlashJumpAudioPlayer"] = (fsm.GetState("Jump").Actions[1] as AudioPlayerOneShot).audioPlayer;
        prefabs["dashSlashJumpAudio2"] = (fsm.GetState("Jump").Actions[1] as AudioPlayerOneShot).audioClips;
        prefabs["dashSlashJumpLandCamera"] = (fsm.GetState("Land Normal").Actions[1] as SendEventByName).eventTarget;
        prefabs["dashSlashChargeAudioPlayer"] = (fsm.GetState("FT Through").Actions[2] as AudioPlayerOneShot).audioPlayer;
        prefabs["dashSlashChargeAudio"] = (fsm.GetState("FT Through").Actions[2] as AudioPlayerOneShot).audioClips;
        var brothers = preloadedObjects["GG_Nailmasters"]["Brothers"];
        var oro = brothers.transform.Find("Oro").gameObject;
        fsm = oro.LocateMyFSM("nailmaster");
        var dashSlashChargeChargeEffect = oro.transform.Find("Charge Effect").gameObject;
        dashSlashChargeChargeEffect.transform.localPosition = new Vector3(0, -3f, 0.001f);
        dashSlashChargeChargeEffect.transform.localScale = new Vector3(2, 1, 1);
        prefabs["dashSlashChargeChargeEffect"] = dashSlashChargeChargeEffect;
        var dashSlashChargeNACharge = oro.transform.Find("NA Charge").gameObject;
        dashSlashChargeNACharge.transform.localPosition = new Vector3(0, -2f, -0.001f);
        prefabs["dashSlashChargeNACharge"] = dashSlashChargeNACharge;
        var dashSlashChargeNACharged = oro.transform.Find("NA Charged").gameObject;
        dashSlashChargeNACharged.transform.localPosition = new Vector3(0, -2f, -0.001f);
        prefabs["dashSlashChargeNACharged"] = dashSlashChargeNACharged;
        var dashSlashChargePtDash = oro.transform.Find("Pt Dash").gameObject;
        dashSlashChargePtDash.transform.localPosition = new Vector3(-0.63f, -4.69f, 0.001f);
        prefabs["dashSlashChargePtDash"] = dashSlashChargePtDash;
        prefabs["dashSlashSlashAudioPlayer"] = (fsm.GetState("D Slash").Actions[1] as AudioPlayerOneShotSingle).audioPlayer;
        prefabs["dashSlashSlashAudio"] = (fsm.GetState("D Slash").Actions[1] as AudioPlayerOneShotSingle).audioClip;
        var dashSlashSlashFlash1 = oro.transform.Find("Dash Slash").gameObject;
        dashSlashSlashFlash1.transform.localPosition = new Vector3(2.69f, -1.37f, 0);
        var localScale = dashSlashSlashFlash1.transform.localScale;
        localScale.x *= -1;
        dashSlashSlashFlash1.transform.localScale = localScale;
        prefabs["dashSlashSlashFlash1"] = dashSlashSlashFlash1;
        var dashSlashSlashFlash2 = oro.transform.Find("Sharp Flash").gameObject;
        dashSlashSlashFlash2.transform.localPosition = new Vector3(7, -1.5f, 0);
        localScale = dashSlashSlashFlash2.transform.localScale;
        localScale.x *= -1;
        dashSlashSlashFlash2.transform.localScale = localScale;
        prefabs["dashSlashSlashFlash2"] = dashSlashSlashFlash2;
        var battleScene = preloadedObjects["GG_Nosk_Hornet"]["Battle Scene"];
        var hornetNosk = battleScene.transform.Find("Hornet Nosk").gameObject;
        fsm = hornetNosk.LocateMyFSM("Hornet Nosk");
        var dashSlashChargeBlob = (fsm.GetState("Spit 1").Actions[1] as FlingObjectsFromGlobalPool).gameObject;
        prefabs["dashSlashChargeBlob"] = dashSlashChargeBlob;
    }
    private void UpdateFSMDashSlash(PlayMakerFSM fsm)
    {
        var greyPrince = GameObject.Find("Grey Prince");
        var dashSlashChargeChargeEffect = UnityEngine.Object.Instantiate(prefabs["dashSlashChargeChargeEffect"] as GameObject, greyPrince.transform);
        dashSlashChargeChargeEffect.name = "dashSlashChargeChargeEffect";
        var dashSlashChargeNACharge = UnityEngine.Object.Instantiate(prefabs["dashSlashChargeNACharge"] as GameObject, greyPrince.transform);
        dashSlashChargeNACharge.name = "dashSlashChargeNACharge";
        var dashSlashChargeNACharged = UnityEngine.Object.Instantiate(prefabs["dashSlashChargeNACharged"] as GameObject, greyPrince.transform);
        dashSlashChargeNACharged.name = "dashSlashChargeNACharged";
        var dashSlashChargePtDash = UnityEngine.Object.Instantiate(prefabs["dashSlashChargePtDash"] as GameObject, greyPrince.transform);
        dashSlashChargePtDash.name = "dashSlashChargePtDash";
        var dashSlashSlashFlash1 = UnityEngine.Object.Instantiate(prefabs["dashSlashSlashFlash1"] as GameObject, greyPrince.transform);
        dashSlashSlashFlash1.name = "dashSlashSlashFlash1";
        var dashSlashSlashFlash2 = UnityEngine.Object.Instantiate(prefabs["dashSlashSlashFlash2"] as GameObject, greyPrince.transform);
        dashSlashSlashFlash2.name = "dashSlashSlashFlash2";
        fsm.AddState("Dash Slash Jump Antic");
        fsm.AddState("Dash Slash Jump");
        fsm.AddState("Dash Slash Charge");
        fsm.AddState("Dash Slash Charged");
        fsm.AddState("Dash Slash Dash");
        fsm.AddState("Dash Slash Slash");
        UpdateStateDashSlashJumpAntic(fsm);
        UpdateStateDashSlashJump(fsm);
        UpdateStateDashSlashCharge(fsm);
        UpdateStateDashSlashCharged(fsm);
        UpdateStateDashSlashDash(fsm);
        UpdateStateDashSlashSlash(fsm);
    }
    private void UpdateStateDashSlashJumpAntic(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Dash Slash Jump Antic", fsm.CreateSetVelocity2d(0, 0));
        fsm.AccessFloatVariable("dashSlashTargetLeft").Value = 8.19f;
        fsm.AccessFloatVariable("dashSlashTargetRight").Value = 44.61f;
        fsm.AddCustomAction("Dash Slash Jump Antic", () =>
        {
            var x = HeroController.instance.transform.position.x;
            float destination;
            float destinationNext;
            var dashSlashTargetLeft = fsm.AccessFloatVariable("dashSlashTargetLeft").Value;
            var dashSlashTargetRight = fsm.AccessFloatVariable("dashSlashTargetRight").Value;
            if (random.Next(2) == 1)
            {
                destination = dashSlashTargetRight;
                destinationNext = dashSlashTargetLeft - 128;
            }
            else
            {
                destination = dashSlashTargetLeft;
                destinationNext = dashSlashTargetRight + 128;
            }
            fsm.AccessFloatVariable("dashSlashDestination").Value = destination;
            fsm.AccessFloatVariable("dashSlashDestinationNext").Value = destinationNext;
        });
        fsm.AddCustomAction("Dash Slash Jump Antic", fsm.CreateFacePosition("dashSlashDestinationNext", true));
        fsm.AddAction("Dash Slash Jump Antic", fsm.CreateTk2dPlayAnimationWithEvents(
            fsm.gameObject, "Antic", fsm.GetFSMEvent("FINISHED")));
        fsm.AddTransition("Dash Slash Jump Antic", "FINISHED", "Dash Slash Jump");
    }
    private void UpdateStateDashSlashJump(PlayMakerFSM fsm)
    {
        fsm.AddAction("Dash Slash Jump", fsm.CreateAudioPlaySimple(
            1, prefabs["dashSlashJumpAudio1"] as FsmObject));
        fsm.AddAction("Dash Slash Jump", fsm.CreateAudioPlayerOneShot(
            prefabs["dashSlashJumpAudioPlayer"] as FsmGameObject, fsm.gameObject,
            prefabs["dashSlashJumpAudio2"] as AudioClip[], new float[3] { 1, 1, 1 }, 1, 1, 1, 0));
        fsm.AddCustomAction("Dash Slash Jump", () =>
        {
            var destination = fsm.AccessFloatVariable("dashSlashDestination").Value;
            float velocityX = 2 * (destination - fsm.gameObject.transform.position.x);
            float velocityY = 90;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 6;
            rigidbody2D.velocity = new Vector2(velocityX, velocityY);
        });
        fsm.AddAction("Dash Slash Jump", fsm.CreateCheckCollisionSide(null, null, fsm.GetFSMEvent("LAND")));
        fsm.AddAction("Dash Slash Jump", fsm.CreateCheckCollisionSideEnter(null, null, fsm.GetFSMEvent("LAND")));
        fsm.AddTransition("Dash Slash Jump", "LAND", "Dash Slash Charge");
    }
    private void UpdateStateDashSlashCharge(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Dash Slash Charge", () =>
        {
            var gameObjectPrefab = (prefabs["dashSlashChargeBlob"] as FsmGameObject).Value;
            Vector3 a = fsm.gameObject.transform.position;
            int num = UnityEngine.Random.Range(8, 17);
            for (int i = 1; i <= num; i++)
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
        fsm.AddAction("Dash Slash Charge", fsm.CreateSendEventByName(
            prefabs["dashSlashJumpLandCamera"] as FsmEventTarget, "AverageShake", 0));
        fsm.AddAction("Dash Slash Charge", fsm.CreateAudioPlayerOneShot(
            prefabs["dashSlashChargeAudioPlayer"] as FsmGameObject, fsm.gameObject,
            prefabs["dashSlashChargeAudio"] as AudioClip[], new float[2] { 1, 1 }, 1, 1, 1, 0));
        fsm.AddCustomAction("Dash Slash Charge", () =>
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
            var dashSlashTargetLeft = fsm.AccessFloatVariable("dashSlashTargetLeft").Value;
            var dashSlashTargetRight = fsm.AccessFloatVariable("dashSlashTargetRight").Value;
            if (x - dashSlashTargetLeft > dashSlashTargetRight - x)
            {
                destination = dashSlashTargetLeft + slack;
                direction = -1;
            }
            else
            {
                destination = dashSlashTargetRight - slack;
                direction = 1;
            }
            fsm.AccessFloatVariable("dashSlashDestination").Value = destination;
            fsm.AccessIntVariable("dashSlashDirection").Value = direction;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 3;
            rigidbody2D.velocity = Vector2.zero;
            fsm.gameObject.transform.Find("dashSlashChargeChargeEffect").gameObject.SetActive(true);
            fsm.gameObject.transform.Find("dashSlashChargeNACharge").gameObject.SetActive(true);
        });
        fsm.AddCustomAction("Dash Slash Charge", fsm.CreateFacePosition("dashSlashDestination", true));
        fsm.AddAction("Dash Slash Charge", fsm.CreateWait(1, fsm.GetFSMEvent("1")));
        fsm.AddTransition("Dash Slash Charge", "1", "Dash Slash Charged");
    }
    private void UpdateStateDashSlashCharged(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Dash Slash Charged", () =>
        {
            fsm.gameObject.transform.Find("dashSlashChargeNACharge").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("dashSlashChargeNACharged").gameObject.SetActive(true);
        });
        fsm.AddAction("Dash Slash Charged", fsm.CreateWait(0.25f, fsm.GetFSMEvent("FINISHED")));
        fsm.AddTransition("Dash Slash Charged", "FINISHED", "Dash Slash Dash");
    }
    private void UpdateStateDashSlashDash(PlayMakerFSM fsm)
    {
        fsm.AddAction("Dash Slash Dash", fsm.CreatePlayParticleEmitterInState(
            fsm.gameObject.transform.Find("dashSlashChargePtDash").gameObject));
        fsm.AddCustomAction("Dash Slash Dash", () =>
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
            fsm.gameObject.transform.Find("dashSlashChargeChargeEffect").gameObject.SetActive(false);
            fsm.gameObject.transform.Find("dashSlashChargeNACharged").gameObject.SetActive(false);
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            var v = fsm.AccessIntVariable("dashSlashDirection").Value * 64;
            rigidbody2D.velocity = new Vector2(v, 0);
        });
        fsm.AddAction("Dash Slash Dash", fsm.CreateGeneralAction(() =>
        {
            var destination = fsm.AccessFloatVariable("dashSlashDestination");
            var direction = fsm.AccessIntVariable("dashSlashDirection");
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
        fsm.AddTransition("Dash Slash Dash", "1", "Dash Slash Slash");
    }
    private void UpdateStateDashSlashSlash(PlayMakerFSM fsm)
    {
        fsm.AddAction("Dash Slash Slash", fsm.CreateAudioPlayerOneShotSingle(
            prefabs["dashSlashSlashAudioPlayer"] as FsmGameObject, fsm.gameObject,
            prefabs["dashSlashSlashAudio"] as FsmObject, 1, 1, 1, 0));
        fsm.AddCustomAction("Dash Slash Slash", () =>
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
            fsm.gameObject.transform.Find("dashSlashSlashFlash1").gameObject.SetActive(true);
            fsm.gameObject.transform.Find("dashSlashSlashFlash2").gameObject.SetActive(true);
        });
        fsm.AddAction("Dash Slash Slash", fsm.CreateWait(0.25f, fsm.GetFSMEvent("1")));
        fsm.AddTransition("Dash Slash Slash", "1", "Move Choice 3");
    }
}