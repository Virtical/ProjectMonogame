using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TankMonogame.Model;
using TankMonogame.Shared.Enums;

namespace TankMonogame.View
{
    public static class DrawMethods
    {
        public static void Draw(this SpriteBatch spriteBatch, Map map)
        {
            foreach (Cell cell in map.Cells)
            {
                spriteBatch.Draw(Map.TextureCell[cell.Type], cell.LTPoint.ToVector2(), Color.White);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, TankHull tankHull)
        {
            spriteBatch.Draw(TankHull.Texture, tankHull.Pos, null, Color.White, tankHull.Angle, tankHull.Anchor, 1f, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Turret turret)
        {
            spriteBatch.Draw(Turret.Texture, turret.Pos, null, Color.White, turret.Angle, turret.Anchor, 1f, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, List<Bullet> bullets)
        {
            foreach (var bullet in bullets)
            {
                spriteBatch.Draw(Bullet.Texture, bullet.Pos, null, Color.White, bullet.Angle, bullet.Anchor, 1f, SpriteEffects.None, 0f);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, List<Explosion> explosions)
        {
            foreach (var explosion in explosions)
            {
                Rectangle sourceRectangle = new Rectangle(0, 64 * (explosion.AnimationFrame / 3), 120, 64);

                spriteBatch.Draw(Explosion.Texture, explosion.Pos, sourceRectangle, Color.White, explosion.Angle, explosion.Anchor, 1.0f, SpriteEffects.None, 1);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, HashSet<Burrel> burrels)
        {
            foreach (var burrel in burrels)
            {
                spriteBatch.Draw(Burrel.Texture, burrel.LTPoint.ToVector2(), Color.White);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, UndergroundLauncher undergroundLauncher)
        {
            Rectangle sourceRectangle = new Rectangle(59 * undergroundLauncher.AnimationFrame, 0, 59, 78);
            spriteBatch.Draw(UndergroundLauncher.Texture, undergroundLauncher.Pos, sourceRectangle, Color.White, undergroundLauncher.Angle, undergroundLauncher.Anchor, 1.0f, SpriteEffects.None, 1);
        }

        public static void Draw(this SpriteBatch spriteBatch, List<Lava> burnPoints)
        {
            foreach (var burnPoint in burnPoints)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        spriteBatch.Draw(Lava.Texture, (burnPoint.Position + new Point(x * 64, y * 64)).ToVector2(), Color.White);
                    }
                }
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, List<Rocket> rockets)
        {
            foreach (var rocket in rockets)
            {
                spriteBatch.Draw(Rocket.Texture, rocket.Pos, null, Color.White, rocket.Angle + 1.570796f, rocket.Anchor, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
