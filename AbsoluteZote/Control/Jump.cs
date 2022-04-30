namespace AbsoluteZote;

public partial class Control : Module
{
    private void LoadPrefabsJump(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
    }
    private void UpdateFSMJump(PlayMakerFSM fsm)
    {
        fsm.InsertCustomAction("Set Jumps", () =>
        {
            fsm.AccessIntVariable("Jumps Min").Value = 1;
            fsm.AccessIntVariable("Jumps Max").Value = 4;
        }, 0);
        fsm.AddAction("In Air", fsm.CreateGeneralAction(() =>
        {
            var rigidbody2D = fsm.gameObject.GetComponent<Rigidbody2D>();
            var y = rigidbody2D.velocity.y;
            if (fsm.AccessFloatVariable("jumpLastVelocityY").Value > 0 && y < 0)
            {
                var heroPositon = HeroController.instance.transform.position;
                var tracking = new Vector2(heroPositon.x - rigidbody2D.position.x, heroPositon.y - rigidbody2D.position.y);
                tracking.x *= 2.5f;
                tracking.y *= 1.5f;
                rigidbody2D.velocity += tracking;
            }
            fsm.AccessFloatVariable("jumpLastVelocityY").Value = y;
        }));
        fsm.RemoveAction("Fall Through?", 1);
        fsm.AddCustomAction("Fall Through?", () => fsm.SendEvent("FINISHED"));
        fsm.InsertCustomAction("Land Waves", () =>
        {
            var shockWave = (fsm.GetState("Land Waves").Actions[0] as SpawnObjectFromGlobalPool).storeObject.Value;
            var localScale = shockWave.transform.localScale;
            localScale.x *= 2;
            shockWave.transform.localScale = localScale;
        }, 6);
        fsm.AddCustomAction("Land Waves", () =>
        {
            var shockWave = (fsm.GetState("Land Waves").Actions[7] as SpawnObjectFromGlobalPool).storeObject.Value;
            var localScale = shockWave.transform.localScale;
            localScale.x *= 2;
            shockWave.transform.localScale = localScale;
        });
    }
}