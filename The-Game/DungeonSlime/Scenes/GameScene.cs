using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;

namespace DungeonSlime.Scenes;

public class GameScene : Scene
{
    // Defines the slime animated sprite.
    private AnimatedSprite _slimeAnimatedSprite;

    // Defines the bat animated sprite.
    private AnimatedSprite _batAnimatedSprite;

    // Tracks the position of the slime.
    // private Vector2 _slimePosition;

    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    // Tracks the position of the bat.
    private Vector2 _batPosition;

    // Tracks the velocity of the bat.
    private Vector2 _batVelocity;

    // The sound effect to play when the bat bounces off the edge of the screen.
    private SoundEffect _bounceSoundEffect;

    // The sound effect to play when the slime eats a bat.
    private SoundEffect _collectSoundEffect;

    // The SpriteFont Description used to draw text
    private SpriteFont _font;

    // Tracks the players score.
    private int _score;

    private TileMap _tileMap;

    private Rectangle _roomBounds;

    private TimeSpan _tickTimer;

    private List<Slime> _slimes;

    private Vector2 _nextDirection;

    private readonly static TimeSpan s_tickTime = TimeSpan.FromMilliseconds(500);

    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // During the game scene, we want to disable exit on escape. Instead,
        // the escape key will be used to return back to the title screen
        Core.ExitOnEscape = false;

        // Initial slime position will be at the center tile of the tile map
        int centerRow = _tileMap.Rows / 2;
        int centerColumn = _tileMap.Columns / 2;

        _slimes = new List<Slime>();
        Slime slime = new Slime();
        slime.At = new Vector2(centerColumn, centerRow) * _tileMap.CellSize;
        slime.To = slime.At + new Vector2(_tileMap.CellSize);
        slime.Direction = Vector2.UnitX;
        _slimes.Add(slime);

        _nextDirection = slime.Direction;

        _roomBounds = new Rectangle();
        _roomBounds.X = _tileMap.CellSize;
        _roomBounds.Y = _tileMap.CellSize;
        _roomBounds.Width = Core.GraphicsDevice.PresentationParameters.BackBufferWidth - _tileMap.CellSize * 2;
        _roomBounds.Height = Core.GraphicsDevice.PresentationParameters.BackBufferHeight - _tileMap.CellSize * 2;

        // Determine the height of the font
        float textHeight = _font.MeasureString("A").Y;

        // Set the initial position of the bat to be 10px to the right of the slime.
        _batPosition = new Vector2(_roomBounds.Left, _roomBounds.Top);

        AssignRandomBatPosition();
        // Assign the initial random velocity to the bat.
        AssignRandomBatVelocity();

        _tickTimer = s_tickTime;
    }

    public override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        // Create the slime animated sprite from the atlas.
        _slimeAnimatedSprite = atlas.CreateAnimatedSprite("slime-animation");
        _slimeAnimatedSprite.Scale = new Vector2(2.0f, 2.0f);

        // Create the bat animated sprite from the atlas.
        _batAnimatedSprite = atlas.CreateAnimatedSprite("bat-animation");
        _batAnimatedSprite.Scale = new Vector2(2.0f, 2.0f);

        // Load the bounce sound effect
        _bounceSoundEffect = Content.Load<SoundEffect>("audio/bounce");

        // Load the collect sound effect
        _collectSoundEffect = Content.Load<SoundEffect>("audio/collect");

        // Create the tilemap from the XML configuration file
        _tileMap = TileMap.FromFile(Core.Content, "tileMap.xml");

        // Load the font
        _font = Core.Content.Load<SpriteFont>("fonts/gameFont");
    }

    public override void Update(GameTime gameTime)
    {
        // gameTime.ElapsedGameTime = gameTime.ElapsedGameTime * 0.25f;
        // Update the slime animated sprite.
        _slimeAnimatedSprite.Update(gameTime);

        // Update the bat animated sprite.
        _batAnimatedSprite.Update(gameTime);

        // Check for keyboard input and handle it.
        CheckKeyboardInput();

        // Create the bounds for the current head of the slime
        Slime head = _slimes[0];

        Circle slimeBounds = new Circle(
            (int)(head.At.X + _slimeAnimatedSprite.Width * 0.5f),
            (int)(head.At.Y + _slimeAnimatedSprite.Height * 0.5f),
            (int)(_slimeAnimatedSprite.Height * 0.5f)
        );

        // Create the bounds for the bat
        Circle batBounds = new Circle(
            (int)(_batPosition.X + _batAnimatedSprite.Width * 0.5f),
            (int)(_batPosition.Y + _batAnimatedSprite.Width * 0.5f),
            (int)(_batAnimatedSprite.Width * 0.25f)
        );

        // If the slime and bat are intersecting, then eat the bat and
        // poop out a new slime
        if (slimeBounds.Intersects(batBounds))
        {
            Slime tail = _slimes[_slimes.Count - 1];
            Slime newTail = new Slime();
            newTail.At = tail.At + tail.ReverseDirection * _tileMap.CellSize;
            newTail.To = tail.At;
            newTail.Direction = Vector2.Normalize(tail.At - newTail.At);
            _slimes.Add(newTail);
            AssignRandomBatPosition();
            AssignRandomBatVelocity();
        }


        _tickTimer = _tickTimer + gameTime.ElapsedGameTime * 2.5f;

        if (_tickTimer >= s_tickTime)
        {
            _tickTimer = TimeSpan.Zero;

            // Get the first slime in the chain.
            Slime previousHead = _slimes[0];

            // Calculate the slime that will be the new head of the chain
            Slime newHead = new Slime();
            newHead.Direction = _nextDirection;
            newHead.At = previousHead.To;
            newHead.To = newHead.At + newHead.Direction * _tileMap.CellSize;

            if (_roomBounds.Contains(newHead.To))
            {
                _slimes.Insert(0, newHead);
                _slimes.RemoveAt(_slimes.Count - 1);
            }
        }

        MoveBat();
    }

    private void MoveBat()
    {
        Vector2 newPosition = _batPosition + _batVelocity;

        Rectangle batBounds = new Rectangle();
        batBounds.X = (int)newPosition.X;
        batBounds.Y = (int)newPosition.Y;
        batBounds.Width = (int)_batAnimatedSprite.Width;
        batBounds.Height = (int)_batAnimatedSprite.Height;

        // Use distance based checks to determine if the bat is within the
        // bounds of the game screen, and if it is outside that screen edge,
        // reflect it about the screen edge normal
        Vector2 normal = Vector2.Zero;

        if (batBounds.Left < _roomBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newPosition.X = _roomBounds.Left;
        }
        else if (batBounds.Right > _roomBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newPosition.X = _roomBounds.Right - _batAnimatedSprite.Width;
        }

        if (batBounds.Top < _roomBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newPosition.Y = _roomBounds.Top;
        }
        else if (batBounds.Bottom > _roomBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newPosition.Y = _roomBounds.Bottom - _batAnimatedSprite.Height;
        }

        // If the normal is anything but Vector2.Zero, this means the bat had
        // moved outside the screen edge so we should reflect it about the
        // normal.
        if (normal != Vector2.Zero)
        {
            _batVelocity = Vector2.Reflect(_batVelocity, normal);

            // Play the bounce sound effect
            Core.Audio.PlaySoundEffect(_bounceSoundEffect);
        }

        _batPosition = newPosition;
    }

    private void AssignRandomBatPosition()
    {
        int x = Random.Shared.Next(0, _tileMap.Columns);
        int y = Random.Shared.Next(0, _tileMap.Rows);
        _batPosition = new Vector2(x, y) * _tileMap.CellSize;
    }

    private void AssignRandomBatVelocity()
    {
        // Generate a random angle
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);

        // Convert angle to a direction vector
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);

        // Multiply the direction vector by the movement speed
        _batVelocity = direction * MOVEMENT_SPEED;
    }

    private void CheckKeyboardInput()
    {
        // If the escape key is pressed, return to the title screen
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Core.ChangeScene(new TitleScene());
        }

        // If the W or Up keys are down, move the slime up on the screen.
        if (Core.Input.Keyboard.IsKeyDown(Keys.W) || Core.Input.Keyboard.IsKeyDown(Keys.Up))
        {
            _nextDirection = -Vector2.UnitY;
        }
        // if the S or Down keys are down, move the slime down on the screen.
        else if
        (Core.Input.Keyboard.IsKeyDown(Keys.S) || Core.Input.Keyboard.IsKeyDown(Keys.Down))
        {
            _nextDirection = Vector2.UnitY;
        }
        // If the A or Left keys are down, move the slime left on the screen.
        else if
        (Core.Input.Keyboard.IsKeyDown(Keys.A) || Core.Input.Keyboard.IsKeyDown(Keys.Left))
        {
            _nextDirection = -Vector2.UnitX;
        }
        // If the D or Right keys are down, move the slime right on the screen.
        else if
        (Core.Input.Keyboard.IsKeyDown(Keys.D) || Core.Input.Keyboard.IsKeyDown(Keys.Right))
        {
            _nextDirection = Vector2.UnitX;
        }

        // If the M key is pressed, toggle mute state for audio.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.M))
        {
            Core.Audio.ToggleMute();
        }

        // If the + button is pressed, increase the volume.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.OemPlus))
        {
            Core.Audio.IncreaseVolume(0.1f);
        }

        // If the - button was pressed, decrease the volume.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.OemMinus))
        {
            Core.Audio.DecreaseVolume(0.1f);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the tilemap
        _tileMap.Draw(Core.SpriteBatch);

        foreach (Slime slime in _slimes)
        {
            Vector2 pos = Vector2.Lerp(slime.At, slime.To, (float)_tickTimer.TotalSeconds / (float)s_tickTime.TotalSeconds);
            _slimeAnimatedSprite.Draw(Core.SpriteBatch, pos);
        }

        // Draw the bat sprite.
        _batAnimatedSprite.Draw(Core.SpriteBatch, _batPosition);

        // Draw the score
        Core.SpriteBatch.DrawString(_font, $"Score: {_score}", Vector2.Zero, Color.White);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
}
