using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime.GameObjects;

public class Bat
{
    private const float MOVEMENT_SPEED = 5.0f;

    // Tracks the position of the bat.
    private Vector2 _position;

    // Tracks the velocity of the bat.
    private Vector2 _velocity;

    /// <summary>
    /// Gets or Sets the AnimatedSprite used when drawing the bat.
    /// </summary>
    public AnimatedSprite Sprite { get; set; }

    /// <summary>
    /// Gets or Sets the sound effect to play when the bat bounces off the
    /// edge of the room.
    /// </summary>
    public SoundEffect BounceSoundEffect { get; set; }

    /// <summary>
    /// Gets or Sets the bounds of the room the bat is confined to.
    /// </summary>
    public Rectangle RoomBounds { get; set; }

    /// <summary>
    /// Updates the bat.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current update cycle.</param>
    public void Update(GameTime gameTime)
    {
        Sprite.Update(gameTime);

        // Calculate a new position for the bat
        Vector2 newPosition = _position + _velocity; ;

        // Get the bounds for the bat
        Circle bounds = GetBounds();

        // Use distance based checks to determine if the bat is within the
        // bounds of the room, and if it is outside, reflect it about the normal
        // of the edge it went outside of
        Vector2 normal = Vector2.Zero;
        if (bounds.Left < RoomBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newPosition.X = RoomBounds.Left;
        }
        else if (bounds.Right > RoomBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newPosition.X = RoomBounds.Right - Sprite.Width;
        }

        if (bounds.Top < RoomBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newPosition.Y = RoomBounds.Top;
        }
        else if (bounds.Bottom > RoomBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newPosition.Y = RoomBounds.Bottom - Sprite.Height;
        }

        // If the normal is anything but Vector2.Zero, this means the bat
        // went outside the bounds and we should reflect it about the normal.
        if (normal != Vector2.Zero)
        {
            _velocity = Vector2.Reflect(_velocity, normal);
            Core.Audio.PlaySoundEffect(BounceSoundEffect);
        }

        // Assign the new position
        _position = newPosition;
    }

    /// <summary>
    /// Returns a Circle value that represents collision bounds of the bat.
    /// </summary>
    /// <returns>A Circle value.</returns>
    public Circle GetBounds()
    {
        int x = (int)(_position.X + Sprite.Width * 0.5f);
        int y = (int)(_position.Y + Sprite.Height * 0.5f);
        int radius = (int)(Sprite.Width * 0.25f);

        return new Circle(x, y, radius);
    }

    /// <summary>
    /// Assigns a new random velocity to the bat.
    /// </summary>
    public void AssignRandomVelocity()
    {
        // Generate a random angle
        float angle = (float)(Random.Shared.NextDouble() * MathHelper.TwoPi);

        // Convert the angle to a direction vector
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        // Multiply the direction vector by the movement speed to get the
        // final velocity
        _velocity = direction * MOVEMENT_SPEED;
    }

    /// <summary>
    /// Positions the bat so that it is within the room bounds away from the
    /// slime.
    /// </summary>
    /// <param name="slime">The slime to position the bat away from</param>
    public void PositionAwayFromSlime(Slime slime)
    {
        // Get center of room bounds
        float centerX = RoomBounds.X + RoomBounds.Width * 0.5f;
        float centerY = RoomBounds.Y + RoomBounds.Height * 0.5f;
        Vector2 center = new Vector2(centerX, centerY);

        // Get the center of the slime bounds
        Circle slimeBounds = slime.GetBounds();
        Vector2 slimePos = new Vector2(slimeBounds.X, slimeBounds.Y);

        // Calculate the vector from the center of the room to the slime
        Vector2 centerToSlime = slimePos - center;

        // Determine the furthest wall by finding which component (x or y) is
        // larger and in which direction.
        if (Math.Abs(centerToSlime.X) > Math.Abs(centerToSlime.Y))
        {
            // Slime is closer to either the left or right wall so the
            // Y position will be the center Y position
            _position.Y = center.Y;

            // Now determine the x position based on which wall the slime
            // is closest to.
            if (centerToSlime.X > 0)
            {
                // Slime is on the right side, place bat at the left wall
                _position.X = RoomBounds.Left + Sprite.Width;
            }
            else
            {
                // Slime is on the left side, place bat at the right wall
                _position.X = RoomBounds.Right - Sprite.Width * 2.0f;
            }
        }
        else
        {
            // Slime is closer to either the top or bottom wall, so the
            // X position will be the center X position
            _position.X = center.X;

            // Now determine the Y position based on which wall the slime is
            // closest to.
            if (centerToSlime.Y > 0)
            {
                // Slime is closer to the bottom, place bat at the top wall.
                _position.Y = RoomBounds.Top - Sprite.Height;
            }
            else
            {
                // Slime is closer to the top, place bat at bottom wall
                _position.Y = RoomBounds.Bottom - Sprite.Height * 2.0f;
            }
        }

    }

    /// <summary>
    /// Draws the bat.
    /// </summary>
    public void Draw()
    {
        Sprite.Draw(Core.SpriteBatch, _position);
    }

}
