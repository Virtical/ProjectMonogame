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
        public static void Draw(this SpriteBatch spriteBatch, Map map, Dictionary<TypeCell, Texture2D> textureCell)
        {
            foreach (Cell cell in map.Cells)
            {
                spriteBatch.Draw(textureCell[cell.Type], cell.LTPoint.ToVector2(), Color.White);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, TankHull tankHull, Dictionary<int, Texture2D> textures)
        {
            spriteBatch.Draw(textures[tankHull.ImageId], tankHull.Pos, null, Color.White, tankHull.Angle, tankHull.Anchor, 1f, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Turret barrelAndTower, Dictionary<int, Texture2D> textures)
        {
            spriteBatch.Draw(textures[barrelAndTower.ImageId], barrelAndTower.Pos, null, Color.White, barrelAndTower.Angle, barrelAndTower.Anchor, 1f, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, List<Bullet> bullets, Dictionary<int, Texture2D> textures)
        {
            foreach (var bullet in bullets)
            {
                spriteBatch.Draw(textures[bullet.ImageId], bullet.Pos, null, Color.White, bullet.Angle, bullet.Anchor, 1f, SpriteEffects.None, 0f);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, List<Explosion> explosions, Dictionary<int, Texture2D> textures)
        {
            foreach (var explosion in explosions)
            {
                Rectangle sourceRectangle = new Rectangle(0, 64 * (explosion.AnimationFrame / 3), 120, 64);

                spriteBatch.Draw(textures[explosion.ImageId], explosion.Pos, sourceRectangle, Color.White, explosion.Angle, explosion.Anchor, 1.0f, SpriteEffects.None, 1);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, HashSet<Burrel> burrels, Dictionary<int, Texture2D> textures)
        {
            foreach (var burrel in burrels)
            {
                spriteBatch.Draw(textures[burrel.ImageId], burrel.LTPoint.ToVector2(), Color.White);
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, UndergroundLauncher undergroundLauncher, Dictionary<int, Texture2D> textures)
        {
            Rectangle sourceRectangle = new Rectangle(59 * undergroundLauncher.AnimationFrame, 0, 59, 78);
            spriteBatch.Draw(textures[undergroundLauncher.ImageId], undergroundLauncher.Pos, sourceRectangle, Color.White, undergroundLauncher.Angle, undergroundLauncher.Anchor, 1.0f, SpriteEffects.None, 1);
        }

        public static void Draw(this SpriteBatch spriteBatch, List<Point> burnPoint, Dictionary<int, Texture2D> textures)
        {
            foreach (var p in burnPoint)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        spriteBatch.Draw(textures[7], (p + new Point(x * 64, y * 64)).ToVector2(), Color.White);
                    }
                }
            }
        }

        public static void Draw(this SpriteBatch spriteBatch, List<Rocket> rockets, Dictionary<int, Texture2D> textures)
        {
            foreach (var rocket in rockets)
            {
                spriteBatch.Draw(textures[rocket.ImageId], rocket.Pos, null, Color.White, rocket.Angle + 1.570796f, rocket.Anchor, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
