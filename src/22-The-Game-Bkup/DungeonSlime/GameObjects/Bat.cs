using System;
using System.IO;
using System.Security.Authentication;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime.GameObjects;

public class Bat
{
    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    public AnimatedSprite Sprite { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }

    public void RandomizePosition(SlimeSegment slimeHead, Rectangle bounds)
    {
        // Get the center of the bounds.
        Vector2 centerBounds = bounds.Center.ToVector2();

        // Calculate the vector from the center of the bounds to the slime head.
        Vector2 centerToHead = slimeHead.At - centerBounds;

        // Determine the wall furthest from the slime head by finding which
        // component (x or y) is larger and in which direction
        if(Math.Abs(centerToHead.X) > Math.Abs(centerToHead.Y))
        {
            // The slime head is either closer to the left or right side of the
            // bounds
            if(centerToHead.X > 0)
            {
                // The slime head is on the right side of the bounds, so place
                // the bat on the left side
                Position = new Vector2(bounds.Left + Sprite.Width, centerBounds.Y);
            }
            else
            {
                // The slime head is on the left side of the bounds, so place
                // the bat on the right side.
                Position = new Vector2(bounds.Right - Sprite.Width * 2.0f, centerBounds.Y);
            }
        }
        else
        {
            // The slime head is either closer to the top or the bottom of the
            // bounds.
            if(centerToHead.Y > 0)
            {
                // The slime head is at the bottom of the bounds, so place the
                // bat at the top
                Position = new Vector2(centerBounds.X, bounds.Top + Sprite.Height);
            }
            else
            {
                // The slime head is at the top of the bounds, so place the
                // bat at the bottom
                Position = new Vector2(centerBounds.X, bounds.Bottom - Sprite.Height * 2.0f);
            }
        }
    }

    public void RandomizeVelocity()
    {
        // Generate a random angle
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2.0f);

        // Convert angle to a direction vector
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        // Multiply the direction vector by the movement speed
        Velocity = direction * MOVEMENT_SPEED;
    }

    public Circle GetBounds()
    {
        // Create the bounds
        Circle bounds = new Circle(
            (int)(Position.X + Sprite.Width * 0.5f),
            (int)(Position.Y + Sprite.Height * 0.5f),
            (int)(Sprite.Width * 0.25f)
        );

        return bounds;
    }
}
