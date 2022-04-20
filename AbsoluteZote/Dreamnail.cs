namespace AbsoluteZote;
public class DreamNail : Module
{
    public DreamNail(AbsoluteZote absoluteZote) : base(absoluteZote)
    {
    }
    public override bool UpdateDreamnailReaction(EnemyDreamnailReaction enemyDreamnailReaction)
    {
        if (IsGreyPrince(enemyDreamnailReaction.gameObject))
        {
            int amount = GameManager.instance.playerData.GetBool("equippedCharm_30") ? -66 : -33;
            HeroController.instance.AddMPCharge(amount);
            PlayMakerFSM fsm = PlayMakerFSM.FindFsmOnGameObject(FsmVariables.GlobalVariables.GetFsmGameObject("Enemy Dream Msg").Value, "Display");
            fsm.FsmVariables.GetFsmInt("Convo Amount").Value = 5;
            fsm.FsmVariables.GetFsmString("Convo Title").Value = "GREY_PRINCE";
            fsm.SendEvent("DISPLAY ENEMY DREAM");
            var dreamImpactPrefab = typeof(EnemyDreamnailReaction).GetField("dreamImpactPrefab", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(enemyDreamnailReaction) as GameObject;
            dreamImpactPrefab.Spawn().transform.position = enemyDreamnailReaction.transform.position;
            enemyDreamnailReaction.gameObject.GetComponent<SpriteFlash>().flashDreamImpact();
            return true;
        }
        else
        {
            return false;
        }
    }
}
