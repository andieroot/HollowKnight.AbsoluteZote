using System.Reflection;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using System.Collections.Generic;
using Vasi;
using SFCore;

namespace AbsoluteZote
{
    public class Arena : Module
    {
        public Arena(AbsoluteZote absoluteZote) : base(absoluteZote)
        {
        }
        public override void Initialize(UnityEngine.SceneManagement.Scene scene)
        {
            if (scene.name == "GG_Grey_Prince_Zote")
            {
                var ggArenaPrefab = GameObject.Find("GG_Arena_Prefab").gameObject;
                ggArenaPrefab.transform.Find("Crowd").gameObject.SetActive(false);
                ggArenaPrefab.transform.Find("Godseeker Crowd").gameObject.SetActive(false);
                var bg = ggArenaPrefab.transform.Find("BG").gameObject;
                bg.transform.Find("bg_pillar").gameObject.SetActive(false);
                bg.transform.Find("bg_pillar (1)").gameObject.SetActive(false);
                bg.transform.Find("throne").gameObject.SetActive(false);
                bg.transform.Find("GG_step (1)").gameObject.SetActive(false);
                bg.transform.Find("GG_step (2)").gameObject.SetActive(false);
                bg.transform.Find("GG_step (3)").gameObject.SetActive(false);
                bg.transform.Find("GG_step (5)").gameObject.SetActive(false);
                bg.transform.Find("GG_step (6)").gameObject.SetActive(false);
                bg.transform.Find("GG_step (8)").gameObject.SetActive(false);
                bg.transform.Find("GG_step (9)").gameObject.SetActive(false);
                bg.transform.Find("GG_step (10)").gameObject.SetActive(false);
                bg.transform.Find("GG_step (11)").gameObject.SetActive(false);
                bg.transform.Find("GG_step (12)").gameObject.SetActive(false);
                bg.transform.Find("GG_step (13)").gameObject.SetActive(false);
                bg.transform.Find("GG_step (14)").gameObject.SetActive(false);
                bg.transform.Find("GG_scene_arena_extra_0000_3").gameObject.SetActive(false);
                bg.transform.Find("GG_scene_arena_extra_0000_3 (3)").gameObject.SetActive(false);
                bg.transform.Find("GG_scene_arena_extra_0000_3 (4)").gameObject.SetActive(false);
                bg.transform.Find("GG_scene_arena_extra_0000_3 (5)").gameObject.SetActive(false);
                bg.transform.Find("black_fader_GG").gameObject.SetActive(false);
                bg.transform.Find("black_fader_GG (1)").gameObject.SetActive(false);
                bg.transform.Find("black_fader_GG (3)").gameObject.SetActive(false);
                bg.transform.Find("black_fader_GG (4)").gameObject.SetActive(false);
                bg.transform.Find("black_fader_GG (5)").gameObject.SetActive(false);
                bg.transform.Find("black_fader_GG (6)").gameObject.SetActive(false);
                bg.transform.Find("black_fader_GG (7)").gameObject.SetActive(false);
                bg.transform.Find("black_fader_GG (8)").gameObject.SetActive(false);
                bg.transform.Find("GG_scenery_0005_16 (3)").gameObject.SetActive(false);
                foreach (var i in new List<int> { 2, 4, 5, 7, 8 })
                {
                    var haze2 = bg.transform.Find("haze2 (" + i.ToString() + ")").gameObject;
                    var spriteRender = haze2.GetComponent<SpriteRenderer>();
                    spriteRender.color = new Color(1, 1, 1, spriteRender.color.a / 1.5f);
                }
                bg.transform.Find("haze2 (3)").gameObject.SetActive(false);
                GameObject.Find("Mighty_Zote_0002_20 (1)").gameObject.SetActive(false);
                GameObject.Find("Mighty_Zote_0004_18 (7)").gameObject.SetActive(false);
                GameObject.Find("Mighty_Zote_0004_18 (8)").gameObject.SetActive(false);
                GameObject.Find("Mighty_Zote_0004_18 (9)").gameObject.SetActive(false);
                GameObject.Find("Mighty_Zote_0002_20").gameObject.SetActive(false);
                GameObject.Find("Mighty_Zote_0004_18 (6)").gameObject.SetActive(false);
                GameObject.Find("Mighty_Zote_0004_18 (5)").gameObject.SetActive(false);
            }
        }
    }
}