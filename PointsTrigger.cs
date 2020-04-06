using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Celeste;
using Celeste.Mod.Entities;
using Monocle;

namespace Celeste.Mod.CustomPoints
{
    [CustomEntity("CustomPoints/PointsTrigger")]
    class PointsTrigger : Trigger
    {
        private string text;
        private bool oneUse;
        private bool collected = false;

        private bool narrated = false;
        private string narrator;
        private int portrait;

        public PointsTrigger(EntityData data, Vector2 offset) : base(data, offset)
        {
            this.text = data.Attr("text", "InsertTextHere");
            this.oneUse = data.Bool("oneUse", true);
            this.narrated = data.Bool("narrated", false);
            this.narrator = data.Attr("narrator", "madeline");
            this.portrait = data.Int("portrait", 1);
        }
        public override void OnEnter(Player player)
        {
            base.OnEnter(player);
            if (!collected)
            {
                //Logger.Log("Points", "Hit trigger area");
                base.Add(new Coroutine(this.CollectRoutine(text, player), true));
                Audio.Play("event:/game/general/strawberry_pulse", this.Position);
                if (oneUse)
                    collected = true;
            }
        }
        private IEnumerator CollectRoutine(string text, Player player)
        {
            this.Tag = Tags.TransitionUpdate;
            this.Scene.Add(new CustomPoints(player.Position + new Vector2(0,-5f) + (player.Speed/50), text, narrated, narrator, portrait));
            yield break;
        }
    }
}
