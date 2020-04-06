using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using Celeste;
using Celeste.Mod;
using Monocle;

namespace Celeste.Mod.CustomPoints
{
    public class CustomPointsManager : EverestModule
    {
        public static CustomPointsManager Instance;
        public CustomPointsManager()
        {
            Instance = this;
        }
        public override void Load()
        {
        }
        public override void Unload()
        {
        }
    }
}
