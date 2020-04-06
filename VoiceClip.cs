using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Celeste;
using Monocle;

namespace Celeste.Mod.CustomPoints
{
    public class VoiceClip : SoundSource
    {
        private int entityRefs = 0;
        private string _path;
        public string Path {
            get
            {
                return _path;
            }
            set
            {
                if (!value.Equals(_path))
                {
                    pathChanged = true;
                    _path = value;
                }
            } 
        }
        private bool pathChanged = false;

        private float _portrait = 0;
        public float Portrait
        {
            get
            {
                return _portrait;
            }
            set
            {
                if (value != _portrait)
                {
                    portraitChanged = true;
                    _portrait = value;
                }
            }
        }
        private bool portraitChanged = false;

        public VoiceClip(string path) : base()
        {
            this._path = path;
        }
        public new VoiceClip Param(string param, float value)
        {
            if (!this.Playing)
            {
                return this;
            }
            if (param.Equals("dialogue_portrait"))
            {
                Portrait = value;
                if (portraitChanged)
                {
                    base.Play(Path);
                    portraitChanged = false;
                }
            }
            base.Param(param, value);

            return this;
        }

        public override void Added(Entity entity)
        { 
            if (entityRefs < 1)
            {
                base.Play(Path);
            }
            else if (pathChanged)
            {
                base.Play(Path);
                pathChanged = false;
            }
            base.Added(entity);

            entityRefs++;
        }
        public override void EntityRemoved(Scene scene)
        {
            entityRefs--;

            if (entityRefs < 1)
            {
                base.Stop(true);
                base.EntityRemoved(scene);
            }
        }
        public override void Removed(Entity entity)
        {
            entityRefs--;
            base.Removed(entity);
        }
    }
}
