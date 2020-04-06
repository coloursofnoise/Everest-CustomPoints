using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Celeste;
using Celeste.Mod.Entities;
using MonoMod;

namespace Celeste.Mod.CustomPoints
{
    [RegisterStrawberry(true, false)]
    [CustomEntity("CustomPointsStrawberry")]
    class StrawberryTest : Strawberry
    {
        public StrawberryTest(EntityData data, Vector2 position) : base(data, position, new EntityID(data.Level.Name, data.ID))
        {
            Logger.Log("debugname", "id: " + data.ID);
            Logger.Log("debugname", "levelname: " + data.Level.Name);
        }
    }
}
