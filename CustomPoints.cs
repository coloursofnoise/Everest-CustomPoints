using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Celeste;
using Monocle;

namespace Celeste.Mod.CustomPoints
{
    public class CustomPoints : Entity
    {
        //Contains one sprite for each character, all sprites have to be updated
        private Sprite[] sprites;

        //Celeste graphics stuff?
        private VertexLight light;
        private BloomPoint bloom;
        private DisplacementRenderer.Burst burst;

        private static VoiceClip sound = new VoiceClip("event:/char/dialogue/madeline");
        private bool narrated;
        private string narrator;
        private int portrait;

        private bool ghostberry;
        private bool moonberry;

        private string text;
        //Combined width in pixels of the sprites
        private float textWidth;

        //Convenience constructor
        public CustomPoints(Vector2 position, string text, bool narrated = false, string narrator = "madeline", int portrait = 0) : this(position, false, text, false, narrated, narrator, portrait) { }
        public CustomPoints(Vector2 position, bool ghostberry, string text, bool moonberry, bool narrated = false, string narrator = "madeline", int portrait = 0) : base(position)
        {
            sprites = new Sprite[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                base.Add(this.sprites[i] = GFX.SpriteBank.Create("customPoints"));
            }
            textWidth = sprites.Sum(sprite => sprite.Width);

            //Celeste graphics stuff??
            base.Add(this.light = new VertexLight(Color.White, 1f, 16, 24));
            base.Add(this.bloom = new BloomPoint(1f, 12f));
            base.Depth = -2000100;
            //I should learn about these sometime
            base.Tag = (Tags.Persistent | Tags.TransitionUpdate | Tags.FrozenUpdate);

            this.ghostberry = ghostberry;
            this.moonberry = moonberry;
            this.text = text;
            this.narrated = narrated;
            this.narrator = narrator;
            this.portrait = portrait;
        }

        public override void Added(Scene scene)
        {
            base.Added(scene);

            //Put each character where it should be, then play their animations.
            for (int i = 0; i < text.Length; i++)
            {
                sprites[i].CenterOrigin();
                sprites[i].X = (sprites[i].X - (textWidth / 2) + ((sprites[i].Width * i) - 1));

                sprites[i].Play("text" + text[i]);
            }

            foreach (Sprite spr in sprites)
            {
                spr.OnFinish = delegate (string a)
                {
                    base.RemoveSelf();
                };
            }

            sound.Path = "event:/char/dialogue/"+narrator;



            if (narrated)
            {
                base.Add(sound);
            }
            if (narrator != "secret_character")
            {
                sound.Param("dialogue_portrait", portrait);
                //sound.Param("dialogue_end", 1);
            }

            //Not entirely sure what this is for, maybe garbage collection if something gets missed
            foreach (Entity entity in base.Scene.Tracker.GetEntities<StrawberryPoints>())
            {
                if (entity != this && Vector2.DistanceSquared(entity.Position, this.Position) <= 256f)
                {
                    entity.RemoveSelf();
                }
            }
            //Graphics?????
            this.burst = (scene as Level).Displacement.AddBurst(this.Position, 0.3f, 16f, 24f, 0.3f, null, null);
        }

        public override void Update()
        {
            //This is all copied almost verbatum from the StrawberryPoints class
            Level level = base.Scene as Level;
            if (level.Frozen)
            {
                if (this.burst != null)
                {
                    this.burst.AlphaFrom = (this.burst.AlphaTo = 0f);
                    this.burst.Percent = this.burst.Duration;
                }
                return;
            }
            base.Update();
            Camera camera = level.Camera;
            base.Y -= 8f * Engine.DeltaTime;
            this.light.Alpha = Calc.Approach(this.light.Alpha, 0f, Engine.DeltaTime * 4f);
            this.bloom.Alpha = this.light.Alpha;
            ParticleType particleType = this.ghostberry ? Strawberry.P_GhostGlow : this.moonberry? Strawberry.P_MoonGlow : Strawberry.P_Glow;
            foreach (Sprite spr in sprites)
            {
                if (base.Scene.OnInterval(0.05f))
                {
                    if (spr.Color == particleType.Color2)
                    {
                        spr.Color = particleType.Color;
                    }
                    else
                    {
                        spr.Color = particleType.Color2;
                    }
                }
                if (base.Scene.OnInterval(0.06f) && spr.CurrentAnimationFrame > 11)
                {
                    level.ParticlesFG.Emit(particleType, 1, this.Position + Vector2.UnitY * -2f, new Vector2(8f, 4f));
                }
            }
        }
    }
}
