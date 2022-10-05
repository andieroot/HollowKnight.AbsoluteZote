using System.Reflection;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Modding;
using System.Collections.Generic;
using Vasi;
using SFCore;
using System.IO;

namespace AbsoluteZote {
    public class Skin : Module
    {
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }
        Texture2D texture2D;
        public Skin(AbsoluteZote absoluteZote) : base(absoluteZote)
        {
            var stream = typeof(AbsoluteZote).Assembly.GetManifestResourceStream("AbsoluteZote.Resources.Skin.Texture2D.png");
            MemoryStream memoryStream = new MemoryStream((int)stream.Length);
            CopyStream(stream, memoryStream);
            stream.Close();
            var bytes = memoryStream.ToArray();
            memoryStream.Close();
            texture2D = new Texture2D(0, 0);
            texture2D.LoadImage(bytes, true);
        }
        public override void Initialize(UnityEngine.SceneManagement.Scene scene)
        {
            if (scene.name == "GG_Grey_Prince_Zote")
            {
                var greyPrince = UnityEngine.GameObject.Find("Grey Prince").gameObject;
                var tk2dSprite = greyPrince.GetComponent<tk2dSprite>();
                tk2dSprite.CurrentSprite.material.mainTexture = texture2D;
            }
        }
    }
}