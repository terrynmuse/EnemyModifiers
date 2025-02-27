﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace FargoEnemyModifiers.Modifiers
{
    public class Bulletproof : Modifier
    {
        public Bulletproof()
        {
            name = "Bulletproof";
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback)
        {
            if (item.ranged)
            {
                damage = (int)(damage * .2f);
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback)
        {
            if (projectile.ranged)
            {
                damage = (int)(damage * .2f);
            }
        }
    }
}
