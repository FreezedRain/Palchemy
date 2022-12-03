using System;
using UnityEngine;

namespace Potions.Gameplay
{
    [Serializable]
    public class SpriteSet
    {
        public Sprite Up;
        public Sprite Right;
        public Sprite Down;
        public Sprite DownRight;
        public Sprite UpRight;

        public (Sprite, bool) GetSprite(Vector2 dir)
        {
            float angle = Mathf.Round(Vector2.SignedAngle(dir, Vector2.up) / 45f) * 45;
            if (angle < 0) angle += 360;
            switch (angle)
            {
                case 0:
                    return (Up, false);
                case 45:
                    return (UpRight, false);
                case 90:
                    return (Right, false);
                case 135:
                    return (DownRight, false);
                case 180:
                    return (Down, false);
                case 225:
                    return (DownRight, true);
                case 270:
                    return (Right, true);
                case 315:
                    return (UpRight, true);
                default:
                    return (Up, false);
            }
        }
    }
}