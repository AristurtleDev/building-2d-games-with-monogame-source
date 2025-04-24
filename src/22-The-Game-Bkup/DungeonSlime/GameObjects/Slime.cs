using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime.GameObjects;

public class Slime
{
    private Vector2 _nextDirection;
    private List<SlimeSegment> _segments;
    private TimeSpan _movementTimer;
    private float _movementLerpAmount;
    private static readonly TimeSpan s_movementTime = TimeSpan.FromMilliseconds(500);

    public AnimatedSprite Sprite { get; set; }
    public Vector2 Position { get; set; }
    public GameController Controller { get; set; }
    public float MovementAmount { get; set; }

    public void Update(GameTime gameTime)
    {
        Sprite.Update(gameTime);

        CheckInput();

        // Increment the movement timer.
        _movementTimer += gameTime.ElapsedGameTime * 2.5f;

        // If the movement timer has exceeded the time to move, move the slime chain.
        if (_movementTimer >= s_movementTime)
        {
            _movementTimer -= s_movementTime;
            UpdateSlimeMovement();
        }

        // Update the movement lerp amount.
        _movementLerpAmount = (float)(_movementTimer.TotalSeconds / s_movementTime.TotalSeconds);
    }

    private void CheckInput()
    {
        // Store the potential direction change.
        Vector2 potentialNextDirection = _nextDirection;

        // Check Movement Actions
        if (Controller.MoveUp())
        {
            potentialNextDirection = -Vector2.UnitY;
        }
        else if (Controller.MoveDown())
        {
            potentialNextDirection = Vector2.UnitY;
        }
        else if (Controller.MoveLeft())
        {
            potentialNextDirection = -Vector2.UnitX;
        }
        else if (Controller.MoveRight())
        {
            potentialNextDirection = Vector2.UnitX;
        }

        // Only allow direction change if it is not reversing the current direction.
        // This prevents the head segment of the slime chain from backing into itself.
        if (Vector2.Dot(potentialNextDirection, _segments[0].Direction) >= 0)
        {
            _nextDirection = potentialNextDirection;
        }
    }

    private void UpdateSlimeMovement()
    {
        // Get the head segment.
        SlimeSegment currentHead = _segments[0];

        // Create a new segment that will be placed at the position the current
        // head segment is moving to.
        SlimeSegment newHead = new SlimeSegment();
        newHead.Direction = _nextDirection;
        newHead.At = currentHead.To;
        newHead.To = newHead.At + newHead.Direction * MovementAmount;

        // Add the new head to the font of the chain
        _segments.Insert(0, newHead);

        // Remove the tail from the chain
        _segments.RemoveAt(_segments.Count - 1);
    }

    public Circle GetBounds()
    {
        // Get the head segment.
        SlimeSegment head = _segments[0];

        // Create the bounds
        Circle bounds = new Circle(
            (int)(head.At.X + Sprite.Width * 0.5f),
            (int)(head.At.Y + Sprite.Height * 0.5f),
            (int)(Sprite.Width * 0.5f)
        );

        return bounds;
    }

    public void Draw()
    {
        // Iterate each segment and calculate the position to draw it at visually
        // based on the movement lerp amount
        foreach (SlimeSegment segment in _segments)
        {
            Vector2 pos = Vector2.Lerp(segment.At, segment.To, _movementLerpAmount);
            Sprite.Draw(Core.SpriteBatch, pos);
        }
    }
}
