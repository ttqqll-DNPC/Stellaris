﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stellaris.Entities
{
    public static class EntityManager
    {
        public static List<Character> players;
        public static List<Character> npcs;
        public static List<Bullet> bullets;
        public static void Initialize()
        {
            players = new List<Character>();
            npcs = new List<Character>();
            bullets = new List<Bullet>();
        }
        public static void Update<T>(IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Entity entity = list[i] as Entity;
                if (list[i] == null) break;
                if (!entity.active)
                {
                    list.RemoveAt(i);
                    continue;
                }
                entity.Update();
            }
        }
        public static void Draw<T>(IList<T> list, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < list.Count; i++)
            {
                (list[i] as Drawable).Draw(spriteBatch);
            }
        }
        public static void Update()
        {
            Update(players);
            Update(npcs);
            Update(bullets);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            Draw(players, spriteBatch);
            Draw(npcs, spriteBatch);
            Draw(bullets, spriteBatch);
        }
    }
}
