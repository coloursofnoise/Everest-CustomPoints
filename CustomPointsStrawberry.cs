using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Celeste;
using Celeste.Mod;
using Celeste.Mod.Entities;
using Monocle;

namespace Celeste.Mod.CustomPoints
{
    [RegisterStrawberry(true, false)]
    [CustomEntity("CustomPoints/CustomPointsStrawberry", "CustomPoints/CustomPointsGolden", "CustomPoints/CustomPointsGoldenNoDash")]
    class CustomPointsStrawberry : Strawberry, IStrawberry
    {
        public string pointsText { get; protected set; }
        private bool isGhostBerry;

        public CustomPointsStrawberry(EntityData data, Vector2 offset) : base(getBaseStrawberry(data), offset, new EntityID(data.Level.Name, data.ID))
        {
            data.Name = getCustomStrawberryName(data);
            pointsText = data.Attr("text", "").ToUpper();
            isGhostBerry = SaveData.Instance.CheckStrawberry(this.ID);
        }
        public static EntityData getBaseStrawberry(EntityData data)
        {
            if (data.Name == "CustomPoints/CustomPointsGolden")
                data.Name = "goldenBerry";
            else if (data.Name == "CustomPoints/CustomPointsGoldenNoDash")
                data.Name = "memorialTextController";
            else data.Name = "strawberry";
            return data;
        }
        private string getCustomStrawberryName(EntityData data)
        {
            string name = "CustomPoints/CustomPointsStrawberry";
            if (data.Name == "goldenBerry")
                name = "CustomPoints/CustomPointsGolden";
            else if (data.Name == "memorialTextController")
                name = "CustomPoints/CustomPointsGoldenNoDash";
            return name;
        }
        public override void Added(Scene scene)
        {
            base.Added(scene);
            On.Celeste.Strawberry.CollectRoutine += Strawberry_CollectRoutine;
        }

        public override void Removed(Scene scene)
        {
            On.Celeste.Strawberry.CollectRoutine -= Strawberry_CollectRoutine;
            base.Removed(scene);
        }
        private IEnumerator Strawberry_CollectRoutine(On.Celeste.Strawberry.orig_CollectRoutine orig, Strawberry self, int collectIndex)
        {
            IEnumerator output = orig(self, collectIndex);
            while (output.MoveNext())
            {
                yield return null;
            }
            if (Object.ReferenceEquals(self, this))
            {
                this.Scene.Add(new CustomPoints(this.Position, this.isGhostBerry, pointsText, this.Moon));
                this.RemoveSelf();
            }
            yield break;
        }
    }
}
