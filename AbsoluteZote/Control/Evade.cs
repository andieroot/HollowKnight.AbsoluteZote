namespace AbsoluteZote;

public partial class Control : Module
{
    private void LoadPrefabsEvade(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        var greyPrince = preloadedObjects["GG_Grey_Prince_Zote"]["Grey Prince"];
        var fsm = greyPrince.LocateMyFSM("Control");
        prefabs["evadeJumpAudio1"] = (fsm.GetState("Jump").Actions[0] as AudioPlaySimple).oneShotClip;
        prefabs["evadeJumpAudioPlayer"] = (fsm.GetState("Jump").Actions[1] as AudioPlayerOneShot).audioPlayer;
        prefabs["evadeJumpAudio2"] = (fsm.GetState("Jump").Actions[1] as AudioPlayerOneShot).audioClips;
    }
    private void UpdateFSMEvade(PlayMakerFSM fsm)
    {
        fsm.AddState("Evade Jump Antic");
        fsm.AddState("Evade Jump In Air");
        fsm.AddState("Evade Jump Land");
        var evade = () =>
        {
            var rootGameObjects = HeroController.instance.gameObject.scene.GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                if (rootGameObject.name == "Fireball2 Spiral(Clone)")
                {
                    var myPosition = fsm.gameObject.transform.position;
                    var fireballPositon = rootGameObject.transform.position;
                    var fireballVelocity = rootGameObject.GetComponent<Rigidbody2D>().velocity;
                    if (fireballPositon.y - myPosition.y < 2)
                    {
                        var xDiff = myPosition.x - fireballPositon.x;
                        if (Math.Abs(xDiff) <= 11 && Math.Sign(fireballVelocity.x) == Math.Sign(xDiff))
                        {
                            fsm.AccessFloatVariable("evadeVelocityX").Value = Math.Sign(fireballVelocity.x) * 5;
                            fsm.AccessFloatVariable("evadeVelocityY").Value = 90;
                            fsm.SetState("Evade Jump Antic");
                        }
                    }
                }
            }
            var spells = HeroController.instance.gameObject.transform.Find("Spells").gameObject;
            if (spells.transform.Find("Scr Heads 2").gameObject.activeSelf)
            {
                var myPosition = fsm.gameObject.transform.position;
                var heroPositon = HeroController.instance.gameObject.transform.position;
                if (Math.Abs(myPosition.x - heroPositon.x) <= 5)
                {
                    fsm.AccessFloatVariable("evadeVelocityX").Value = Math.Sign(myPosition.x - heroPositon.x) * 20;
                    fsm.AccessFloatVariable("evadeVelocityY").Value = 45;
                    fsm.SetState("Evade Jump Antic");
                }
            }
        };
        fsm.AddAction("Idle Start", fsm.CreateGeneralAction(evade));
        fsm.AddAction("Stand", fsm.CreateGeneralAction(evade));
        fsm.AddAction("Run Antic", fsm.CreateGeneralAction(evade));
        fsm.AddAction("Run", fsm.CreateGeneralAction(evade));
        UpdateStateEvadeJumpAntic(fsm);
        UpdateStateEvadeJumpInAir(fsm);
        UpdateStateEvadeJumpLand(fsm);
    }
    private void UpdateStateEvadeJumpAntic(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Evade Jump Antic", fsm.CreateSetVelocity2d(0, 0));
        fsm.AddAction("Evade Jump Antic", fsm.CreateFaceObject(HeroController.instance.gameObject, true));
        var tk2dSpriteAnimator_ = fsm.gameObject.GetComponent<tk2dSpriteAnimator>();
        var oldClip = tk2dSpriteAnimator_.GetClipByName("Antic");
        var newClip = new tk2dSpriteAnimationClip();
        newClip.CopyFrom(oldClip);
        newClip.fps *= 16;
        fsm.AddCustomAction("Evade Jump Antic", () =>
        {
            tk2dSpriteAnimator_.Play(newClip);
        });
        fsm.AddTransition("Evade Jump Antic", "FINISHED", "Evade Jump In Air");
    }
    private void UpdateStateEvadeJumpInAir(PlayMakerFSM fsm)
    {
        fsm.AddAction("Evade Jump In Air", fsm.CreateAudioPlaySimple(
            1, prefabs["evadeJumpAudio1"] as FsmObject));
        fsm.AddAction("Evade Jump In Air", fsm.CreateAudioPlayerOneShot(
            prefabs["evadeJumpAudioPlayer"] as FsmGameObject, fsm.gameObject,
            prefabs["evadeJumpAudio2"] as AudioClip[], new float[3] { 1, 1, 1 }, 1, 1, 1, 0));
        fsm.AddCustomAction("Evade Jump In Air", () =>
        {
            float velocityX = fsm.AccessFloatVariable("evadeVelocityX").Value;
            float velocityY = fsm.AccessFloatVariable("evadeVelocityY").Value;
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 6;
            rigidbody2D.velocity = new Vector2(velocityX, velocityY);
        });
        fsm.AddAction("Evade Jump In Air", fsm.CreateCheckCollisionSide(null, null, fsm.GetFSMEvent("LAND")));
        fsm.AddAction("Evade Jump In Air", fsm.CreateCheckCollisionSideEnter(null, null, fsm.GetFSMEvent("LAND")));
        fsm.AddTransition("Evade Jump In Air", "LAND", "Evade Jump Land");
    }
    private void UpdateStateEvadeJumpLand(PlayMakerFSM fsm)
    {
        fsm.AddCustomAction("Evade Jump Land", () =>
        {
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 3;
            rigidbody2D.velocity = Vector2.zero;
            fsm.SetState("Idle Start");
        });
    }
}