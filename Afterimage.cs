using System.Reflection;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using System.Collections.Generic;
using Vasi;
using SFCore;

namespace AbsoluteZote {
    public partial class Afterimage : Module
    {
        public class ImageAnimation : MonoBehaviour
        {
            public tk2dSpriteAnimationClip clip;
            public ImagePool pool;
            float time;
            void Update()
            {
                var newAnimator = gameObject.GetComponent<tk2dSpriteAnimator>();
                if (newAnimator == null)
                {
                    newAnimator = gameObject.AddComponent<tk2dSpriteAnimator>();
                }
                newAnimator.Play(clip);
                time += Time.deltaTime;
                newAnimator.Sprite.color = new Color(1, 0.5f, 1, 0.5f * (1 - time / 0.5f));
                if (time > 0.5)
                {
                    time = 0;
                    gameObject.SetActive(false);
                    pool.inactiveKnights.Add(gameObject);
                }
                gameObject.tag = "Untagged";
            }
        }
        public class ImagePool
        {
            public GameObject knightTemplate;
            public List<GameObject> inactiveKnights = new List<GameObject>();
            public GameObject instantiate(Vector3 positon, Quaternion rotation, Vector3 scale)
            {
                GameObject newKnight = null;
                if (inactiveKnights.Count != 0)
                {
                    newKnight = inactiveKnights[0];
                    newKnight.SetActive(true);
                    inactiveKnights.RemoveAt(0);
                }
                else
                {
                    newKnight = UnityEngine.Object.Instantiate(knightTemplate);
                }
                newKnight.transform.position = new Vector3(positon.x, positon.y, positon.z + 1e-3f);
                newKnight.transform.rotation = rotation;
                newKnight.transform.localScale = scale;
                newKnight.name = "newKnight";
                newKnight.tag = "Untagged";
                return newKnight;
            }
        }
        public class ImageGenerator : MonoBehaviour
        {
            public ImagePool pool = new ImagePool();
            float time;
            void Update()
            {
                time += Time.deltaTime;
                if (time > 0.1)
                {
                    var originalKnight = gameObject;
                    var newKnight = pool.instantiate(originalKnight.transform.position, originalKnight.transform.rotation, originalKnight.transform.localScale);
                    try
                    {
                        time = 0;
                        var originalAnimator = originalKnight.GetComponent<tk2dSpriteAnimator>();
                        var newAnimator = newKnight.GetComponent<tk2dSpriteAnimator>();
                        if (newAnimator == null)
                        {
                            newAnimator = newKnight.AddComponent<tk2dSpriteAnimator>();
                        }
                        newAnimator.SetSprite(originalAnimator.Sprite.Collection, originalAnimator.Sprite.spriteId);
                        newAnimator.Library = originalAnimator.Library;
                        var originalClip = originalAnimator.CurrentClip;
                        var newClip = new tk2dSpriteAnimationClip();
                        newClip.CopyFrom(originalClip);
                        newClip.frames = new tk2dSpriteAnimationFrame[1];
                        newClip.frames[0] = originalClip.frames[originalAnimator.CurrentFrame];
                        newClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
                        var imageAnimation = newKnight.GetComponent<ImageAnimation>();
                        if (imageAnimation == null)
                        {
                            imageAnimation = newKnight.AddComponent<ImageAnimation>();
                        }
                        imageAnimation.clip = newClip;
                        imageAnimation.pool = pool;
                        newAnimator.enabled = false;
                    }
                    catch (System.Exception ex)
                    {
                        UnityEngine.Object.Destroy(newKnight);
                    }
                }
            }
        }
        GameObject knightTemplate;
        public Afterimage(AbsoluteZote absoluteZote) : base(absoluteZote)
        {
        }
        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
        {
            ("GG_Mighty_Zote", "Battle Control"),
        };
        }
        public override void LoadPrefabs(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            var battleControl = preloadedObjects["GG_Mighty_Zote"]["Battle Control"];
            knightTemplate = battleControl.transform.Find("Zotelings").gameObject.transform.Find("Ordeal Zoteling").gameObject;
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<PersistentBoolItem>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<ConstrainPosition>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<HealthManager>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<DamageHero>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<UnityEngine.BoxCollider2D>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<PlayMakerFSM>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<PlayMakerFSM>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<PlayMakerFixedUpdate>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<UnityEngine.AudioSource>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<SpriteFlash>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<PersonalObjectPool>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<Recoil>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<EnemyDreamnailReaction>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<EnemyDeathEffectsUninfected>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<EnemyHitEffectsUninfected>());
            UnityEngine.Object.Destroy(knightTemplate.GetComponent<ExtraDamageable>());
            knightTemplate.tag = "Untagged";
        }
        public override void UpdateFSM(PlayMakerFSM fsm)
        {
            if (IsGreyPrince(fsm.gameObject) && fsm.FsmName == "Control")
            {
                if (fsm.gameObject.GetComponent<ImageGenerator>() == null)
                {
                    var imagePool = new ImagePool
                    {
                        knightTemplate = knightTemplate
                    };
                    fsm.gameObject.AddComponent<ImageGenerator>().pool = imagePool;
                }
            }
        }
    }
}