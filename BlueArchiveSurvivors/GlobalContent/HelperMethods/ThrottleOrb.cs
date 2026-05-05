using RoR2.Orbs;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMod.GlobalContent.HelperMethods
{
    internal static class ThrottleOrb
    {
        public static List<Queue<GenericDamageOrb>> OrbAttacks = new();
        public static void Init()
        {
            RoR2Application.onFixedUpdate += RoR2Application_onFixedUpdate;
            RoR2Application.onUpdate += RoR2Application_onUpdate;
        }

        private static void RoR2Application_onUpdate()
        {
            throw new NotImplementedException();
        }

        private static void RoR2Application_onFixedUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
