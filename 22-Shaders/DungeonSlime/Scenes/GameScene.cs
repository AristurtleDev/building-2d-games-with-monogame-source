using System;
using System.Collections.Generic;
using System.Linq;
using DungeonSlime.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.UI;

namespace DungeonSlime.Scenes;

public class GameScene : Scene
{
    private UISprite _pauseMenu;
    private UISprite _gameOverMenu;

    // Defines the slime animated sprite.
    private AnimatedSprite _slime;

    // Defines the bat animated sprite.
    private AnimatedSprite _bat;

    // Tracks the position of the slime.
    private Vector2 _slimePosition;

    // Speed multiplier when moving.
    private const float MOVEMENT_SPEED = 5.0f;

    // Tracks the position of the bat.
    private Vector2 _batPosition;

    // Tracks the velocity of the bat.
    private Vector2 _batVelocity;

    // Defines the tilemap to draw.
    private Tilemap _tilemap;

    // Defines the bounds of the room that the slime and bat are contained within.
    private Rectangle _roomBounds;

    // The sound effect to play when the bat bounces off the edge of the screen.
    private SoundEffect _bounceSoundEffect;

    // The sound effect to play when the slime eats a bat.
    private SoundEffect _collectSoundEffect;

    // The SpriteFont Description used to draw text
    private SpriteFont _font;

    // Tracks the players score.
    private int _score;

    // Defines the position to draw the score text at.
    private Vector2 _scoreTextPosition;

    // Defines the origin used when drawing the score text.
    private Vector2 _scoreTextOrigin;

    // Tracks the segments of the slime chain.
    private List<SlimeSegment> _slimes;

    // The next direction to apply to the head of the slime chain
    // during the next movement update.
    private Vector2 _nextDirection;

    // The amount of time that has elapsed since the last movement update.
    private TimeSpan _tickTimer;

    // A constant value that provides the amount of time to wait between
    // movement updates.
    private readonly static TimeSpan s_tickTime = TimeSpan.FromMilliseconds(500);

    // The control input profile for the game.
    private GameController _controller;

    private Queue<Vector2> _inputBuffer;
    private const int MAX_BUFFER_SIZE = 2;

    private Effect _grayscaleEffect;
    private float _saturation = 1.0f;
    private bool _transitioningToGrayscale = false;
    private bool _isGameOver;


    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // During the game scene, we want to disable exit on escape. Instead,
        // the escape key will be used to return back to the title screen
        Core.ExitOnEscape = false;

        Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        _roomBounds = new Rectangle(
            (int)_tilemap.TileWidth,
            (int)_tilemap.TileHeight,
            screenBounds.Width - (int)_tilemap.TileWidth * 2,
            screenBounds.Height - (int)_tilemap.TileHeight * 2
        );

        // Set the position of the score text to align to the left edge of the
        // room bounds.
        _scoreTextPosition = new Vector2(_roomBounds.Left, _tilemap.TileHeight * 0.5f);

        // Set the origin of the text so it is centered horizontally
        float scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
        _scoreTextOrigin = new Vector2(0, scoreTextYOrigin);

        // Initialize the game controller
        _controller = new GameController();

        InitializeNewGame();
    }

    private void InitializeNewGame()
    {
        // Create the list of slime segments
        _slimes = new List<SlimeSegment>();

        // Initial slime position will be the center tile of the tile map.
        int centerRow = _tilemap.Rows / 2;
        int centerColumn = _tilemap.Columns / 2;

        // Create the initial head segment.
        SlimeSegment segment = new SlimeSegment();
        segment.At = new Vector2(centerColumn * _tilemap.TileWidth, centerRow * _tilemap.TileHeight);
        segment.To = segment.At + new Vector2(_tilemap.TileWidth, 0);
        segment.Direction = Vector2.UnitX;
        _slimes.Add(segment);

        // Set the initial direction to match the head
        _nextDirection = segment.Direction;

        // Initialize the bat position to a random position
        AssignRandomBatPosition();

        // Assign the initial random position and velocity to the bat
        AssignRandomBatVelocity();

        // Reset the timer and score
        _tickTimer = s_tickTime;
        _score = 0;

        _inputBuffer = new Queue<Vector2>(MAX_BUFFER_SIZE);

        _saturation = 1.0f;
        _transitioningToGrayscale = false;
        _isGameOver = false;
        _grayscaleEffect.Parameters["Saturation"].SetValue(_saturation);
    }

    public override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        // Create the slime animated sprite from the atlas.
        _slime = atlas.CreateAnimatedSprite("slime-animation");

        // Create the bat animated sprite from the atlas.
        _bat = atlas.CreateAnimatedSprite("bat-animation");

        // Load the tilemap from the XML configuration file.
        _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");

        // Load the bounce sound effect
        _bounceSoundEffect = Content.Load<SoundEffect>("audio/bounce");

        // Load the collect sound effect
        _collectSoundEffect = Content.Load<SoundEffect>("audio/collect");

        // Load the font
        _font = Core.Content.Load<SpriteFont>("fonts/gameFont");

        // Load the sound effect to play when ui actions occur.
        SoundEffect uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");

        // Create the UI Controller
        UIElementController controller = new UIElementController();

        // Create the pause menu.
        CreatePauseMenu(atlas, uiSoundEffect, controller);

        // Create the game over menu.
        CreateGameOverMenu(atlas, uiSoundEffect, controller);

        _grayscaleEffect = Content.Load<Effect>("effects/gray-scale-effect");
    }

    private void CreatePauseMenu(TextureAtlas atlas, SoundEffect soundEffect, UIElementController controller)
    {
        // Create the root container for the paused menu.
        _pauseMenu = new UISprite();
        _pauseMenu.Sprite = atlas.CreateSprite("overlay-pixel");
        _pauseMenu.Sprite.Scale = Core.GraphicsDevice.PresentationParameters.Bounds.Size.ToVector2();
        _pauseMenu.Controller = controller;
        _pauseMenu.IsSelected = true;
        _pauseMenu.IsEnabled = _pauseMenu.IsVisible = false;

        // Create the paused panel as a child of the paused menu.
        UISprite pausePanel = _pauseMenu.CreateChild<UISprite>();
        pausePanel.Sprite = atlas.CreateSprite("panel");
        pausePanel.Position = new Vector2(215, 249);

        // Create the paused text as a child of the paused panel.
        UISprite pausedText = pausePanel.CreateChild<UISprite>();
        pausedText.Sprite = atlas.CreateSprite("paused-label");
        pausedText.Position = new Vector2(42, 42);

        // Create the resume button as a child of the paused panel.
        UIButton resumeButton = pausePanel.CreateChild<UIButton>();
        resumeButton.NotSelectedSprite = atlas.CreateSprite("resume-button");
        resumeButton.NotSelectedSprite.CenterOrigin();
        resumeButton.SelectedSprite = atlas.CreateAnimatedSprite("resume-button-selected");
        resumeButton.SelectedSprite.CenterOrigin();
        resumeButton.Position = new Vector2(148, 148);
        resumeButton.IsSelected = true;

        // Create the quite button as a child of the paused panel.
        UIButton quitButton = pausePanel.CreateChild<UIButton>();
        quitButton.NotSelectedSprite = atlas.CreateSprite("quit-button");
        quitButton.NotSelectedSprite.CenterOrigin();
        quitButton.SelectedSprite = atlas.CreateAnimatedSprite("quit-button-selected");
        quitButton.SelectedSprite.CenterOrigin();
        quitButton.Position = new Vector2(691, 148);

        // Wire up the actions to perform when the Right action is triggered
        // for the menu.
        _pauseMenu.LeftAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (quitButton.IsSelected)
            {
                // The quit button is selected and the left action was
                // performed, so deselect the quit button and select the resume
                // button.
                quitButton.IsSelected = false;
                resumeButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Right action is triggered
        // for the menu.
        _pauseMenu.RightAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (resumeButton.IsSelected)
            {
                // The resume button is selected and the right action was
                // performed, so deselect the resume button and select the quit
                // button
                resumeButton.IsSelected = false;
                quitButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Confirm action is triggered
        // for the menu.
        _pauseMenu.ConfirmAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (resumeButton.IsSelected)
            {
                // The resume button is selected and the confirm action was
                // performed, so unpause the game by disabling the pause menu.
                _pauseMenu.IsEnabled = _pauseMenu.IsVisible = _pauseMenu.IsSelected = false;
            }
            else if (quitButton.IsSelected)
            {
                // The quit button is selected and the confirm action was
                // performed, so quit the game by changing the scene back to the
                // title scene.
                Core.ChangeScene(new TitleScene());
            }
        };

        // Wire up the actions to perform when the Cancel action is triggered
        // for the menu.
        _pauseMenu.CancelAction = () =>
        {
            // Play the sound effect.
            Core.Audio.PlaySoundEffect(soundEffect);

            // Unpause the game by disabling the paused menu.
            _pauseMenu.IsEnabled = _pauseMenu.IsVisible = _pauseMenu.IsSelected = false;
        };
    }

    private void CreateGameOverMenu(TextureAtlas atlas, SoundEffect soundEffect, UIElementController controller)
    {
        // Create the root container for the game over menu.
        _gameOverMenu = new UISprite();
        _gameOverMenu.Sprite = atlas.CreateSprite("overlay-pixel");
        _gameOverMenu.Sprite.Scale = Core.GraphicsDevice.PresentationParameters.Bounds.Size.ToVector2();
        _gameOverMenu.Controller = controller;
        _gameOverMenu.IsSelected = true;
        _gameOverMenu.IsEnabled = _gameOverMenu.IsVisible = false;

        // Create the game over panel as a child of the game over menu.
        UISprite gameOverPanel = _gameOverMenu.CreateChild<UISprite>();
        gameOverPanel.Sprite = atlas.CreateSprite("panel");
        gameOverPanel.Position = new Vector2(215, 249);

        // Create the game over text as a child of the game over panel.
        UISprite gameOverText = gameOverPanel.CreateChild<UISprite>();
        gameOverText.Sprite = atlas.CreateSprite("game-over-label");
        gameOverText.Position = new Vector2(42, 42);

        // Create the retry button as a child of the game over panel.
        UIButton retryButton = gameOverPanel.CreateChild<UIButton>();
        retryButton.NotSelectedSprite = atlas.CreateSprite("retry-button");
        retryButton.NotSelectedSprite.CenterOrigin();
        retryButton.SelectedSprite = atlas.CreateAnimatedSprite("retry-button-selected");
        retryButton.SelectedSprite.CenterOrigin();
        retryButton.Position = new Vector2(148, 148);
        retryButton.IsSelected = true;

        // Create the quit button as a child of the game over panel.
        UIButton quitButton = gameOverPanel.CreateChild<UIButton>();
        quitButton.NotSelectedSprite = atlas.CreateSprite("quit-button");
        quitButton.NotSelectedSprite.CenterOrigin();
        quitButton.SelectedSprite = atlas.CreateAnimatedSprite("quit-button-selected");
        quitButton.SelectedSprite.CenterOrigin();
        quitButton.Position = new Vector2(691, 148);

        // Wire up the actions to perform when the Right action is triggered
        // for the menu.
        _gameOverMenu.LeftAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (quitButton.IsSelected)
            {
                // The quit button is selected and the left action was
                // performed, so deselect the quit button and select the retry
                // button.
                quitButton.IsSelected = false;
                retryButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Right action is triggered
        // for the menu.
        _gameOverMenu.RightAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (retryButton.IsSelected)
            {
                // The retry button is selected and the right action was
                // performed, so deselect the retry button and select the quit
                // button.
                retryButton.IsSelected = false;
                quitButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Confirm action is triggered
        // for the menu.
        _gameOverMenu.ConfirmAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (retryButton.IsSelected)
            {
                // The retry button is selected and the confirm action was
                // performed, so initialize a new game and hide the game over menu.
                InitializeNewGame();
                _gameOverMenu.IsEnabled = _gameOverMenu.IsVisible = _gameOverMenu.IsSelected = false;
            }
            else if (quitButton.IsSelected)
            {
                // The quite button is selected and the confirm action was
                // performed, so change the scene back to the title scene.
                Core.ChangeScene(new TitleScene());
            }
        };
    }

    public override void Update(GameTime gameTime)
    {
        _pauseMenu.Update(gameTime);
        _gameOverMenu.Update(gameTime);

        if (_transitioningToGrayscale)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _saturation = MathHelper.Max(0.0f, _saturation - 1.5f * deltaTime);


            if (_saturation <= 0.0f)
            {
                _transitioningToGrayscale = false;
            }
        }

        if (!_pauseMenu.IsEnabled && !_gameOverMenu.IsEnabled)
        {
            UpdateGame(gameTime);
        }
    }

    private void UpdateGame(GameTime gameTime)
    {
        if (_isGameOver)
        {
            return;
        }

        // Update the slime animated sprite.
        _slime.Update(gameTime);

        // Update the bat animated sprite.
        _bat.Update(gameTime);

        CheckInput();

        // Check if there is a collision between the slime and bat
        CheckSlimeAndBatCollision();

        // Increment the tick timer
        _tickTimer += gameTime.ElapsedGameTime * 2.5f;

        // If the tick timer has exceeded the time to tick, move the slime chain
        if (_tickTimer >= s_tickTime)
        {
            _tickTimer -= s_tickTime;
            UpdateSlimeMovement();
        }

        // Move the bat
        MoveBat();
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

    private void AssignRandomBatPosition()
    {
        // Get a reference to the snake's head
        SlimeSegment head = _slimes[0];

        // Calculate the center of the room
        Vector2 roomCenter = new Vector2(
            _roomBounds.X + _roomBounds.Width * 0.5f,
            _roomBounds.Y + _roomBounds.Height * 0.5f
        );

        // Calculate the vector from the room center to the head
        Vector2 centerToHead = head.At - roomCenter;

        // Determine the furthest wall by finding which component (x or y) is
        // larger and in which direction
        if (Math.Abs(centerToHead.X) > Math.Abs(centerToHead.Y))
        {
            // Head is closer to either the left or right wall
            if (centerToHead.X > 0)
            {
                // Head is on the right side, place bat on the left wall
                _batPosition = new Vector2(
                    _roomBounds.Left + _bat.Width,
                    roomCenter.Y
                );
            }
            else
            {
                // Head is on left side, place bat on the right wall
                _batPosition = new Vector2(
                    _roomBounds.Right - _bat.Width * 2,
                    roomCenter.Y
                );
            }
        }
        else
        {
            // Head is closer to top or bottom wall
            if (centerToHead.Y > 0)
            {
                // Head is closer to bottom, place bat on top wall
                _batPosition = new Vector2(
                    roomCenter.X,
                    _roomBounds.Top + _bat.Height
                );
            }
            else
            {
                // Head is closer to top, place bat on bottom wall.
                _batPosition = new Vector2(
                    roomCenter.X,
                    _roomBounds.Bottom - _bat.Height * 2
                );
            }
        }
    }

    private void CheckInput()
    {
        // Check for pause action
        if (_controller.Pause())
        {
            _pauseMenu.IsEnabled = _pauseMenu.IsVisible = _pauseMenu.IsSelected = true;
            return;
        }

        // Store the potential direction change
        Vector2? potentialNextDirection = null;

        // Check movement actions
        if (_controller.MoveUp())
        {
            potentialNextDirection = -Vector2.UnitY;
        }
        else if (_controller.MoveDown())
        {
            potentialNextDirection = Vector2.UnitY;
        }
        else if (_controller.MoveLeft())
        {
            potentialNextDirection = -Vector2.UnitX;
        }
        else if (_controller.MoveRight())
        {
            potentialNextDirection = Vector2.UnitX;
        }

        // If a new direction was input, consider adding it to the buffer.
        if (potentialNextDirection.HasValue && _inputBuffer.Count < MAX_BUFFER_SIZE)
        {
            // If the buffer is empty, validate against the current direction;
            // otherwise, validate against the last buffered direction
            Vector2 validateAgainst = _inputBuffer.Count > 0 ?
                                      _inputBuffer.Last() :
                                      _slimes[0].Direction;

            // Check if this is a valid direction change (not reversed).
            if (Vector2.Dot(potentialNextDirection.Value, validateAgainst) >= 0)
            {
                // Only add if it is different from the las buffered direction.
                if (_inputBuffer.Count == 0 || _inputBuffer.Last() != potentialNextDirection.Value)
                {
                    _inputBuffer.Enqueue(potentialNextDirection.Value);
                }
            }
        }
    }

    private void UpdateSlimeMovement()
    {

        // Get the next direction from the input buffer if one is available
        if (_inputBuffer.Count > 0)
        {
            _nextDirection = _inputBuffer.Dequeue();
        }

        // Get the head of the chain
        SlimeSegment previousHead = _slimes[0];

        // Calculate the new head position
        SlimeSegment newHead = new SlimeSegment();
        newHead.Direction = _nextDirection;
        newHead.At = previousHead.To;
        newHead.To = newHead.At + newHead.Direction * _tilemap.TileWidth;

        // Add the new head and remove the last segment.
        // This effectively move the entire chain forward
        _slimes.Insert(0, newHead);
        _slimes.RemoveAt(_slimes.Count - 1);

        // Now that the slime has moved, ensure it is within the room bounds
        if (!_roomBounds.Contains(newHead.To))
        {
            // It has moved outside the room bounds (collided with a wall) so
            // it is game over
            _isGameOver = true;
            _transitioningToGrayscale = true;
            _gameOverMenu.IsEnabled = _gameOverMenu.IsVisible = _gameOverMenu.IsSelected = true;
        }

        // Next check if the slime is colliding with its own body.
        Circle headBounds = new Circle(
            (int)(newHead.At.X + _slime.Width * 0.5f),
            (int)(newHead.At.Y + _slime.Height * 0.5f),
            (int)(_slime.Width * 0.5f)
        );

        foreach (SlimeSegment child in _slimes[1..])
        {
            Circle childBounds = new Circle(
                (int)(child.At.X + _slime.Width * 0.5f),
                (int)(child.At.Y + _slime.Height * 0.5f),
                (int)(_slime.Width * 0.5f)
            );

            if (headBounds.Intersects(childBounds))
            {
                // The head is colliding with a body segment, so it is game over.
                _isGameOver = true;
                _transitioningToGrayscale = true;
                _gameOverMenu.IsEnabled = _gameOverMenu.IsVisible = _gameOverMenu.IsSelected = true;
            }
        }
    }

    private void CheckSlimeAndBatCollision()
    {
        // Get a reference to the head of the slime
        SlimeSegment head = _slimes[0];

        // Create the bounds for the head of the slime
        Circle slimeBounds = new Circle(
            (int)(head.At.X + _slime.Width * 0.5f),
            (int)(head.At.Y + _slime.Height * 0.5f),
            (int)(_slime.Width * 0.5f)
        );

        // Create the bounds for the bat
        Circle batBounds = new Circle(
            (int)(_batPosition.X + _bat.Width * 0.5f),
            (int)(_batPosition.Y + _bat.Height * 0.5f),
            (int)(_bat.Width * 0.25f)
        );

        // If the slime and bat are intersecting, then eat the bat and create
        // a new slime segment
        if (slimeBounds.Intersects(batBounds))
        {
            // Get the tail segment
            SlimeSegment tail = _slimes[_slimes.Count - 1];

            // Create a new tail segment behind the current tail
            SlimeSegment newTail = new SlimeSegment();
            newTail.At = tail.To + tail.ReverseDirection * _tilemap.TileWidth;
            newTail.To = tail.At;
            newTail.Direction = Vector2.Normalize(tail.At - newTail.At);

            // Add the new segment to the end of the chain
            _slimes.Add(newTail);

            // Assign the bat position to a random position
            AssignRandomBatPosition();

            // Assign a new random velocity to the bat.
            AssignRandomBatVelocity();

            // Play the collect sound effect
            Core.Audio.PlaySoundEffect(_collectSoundEffect);

            // Increase the player's score
            _score += 100;
        }
    }

    private void MoveBat()
    {
        // Calculate a new position for the bat
        Vector2 newPosition = _batPosition + _batVelocity;

        // Create the bounds for the bat
        Circle batBounds = new Circle(
            (int)(newPosition.X + _bat.Width * 0.5f),
            (int)(newPosition.Y + _bat.Height * 0.5f),
            (int)(_bat.Width * 0.25f)
        );

        // Use distance based checks to determine of the bat is within the
        // bounds of the game screen, and if it is outside that screen edge,
        // reflect it about the screen edge normal.
        Vector2 normal = Vector2.Zero;

        if (batBounds.Left < _roomBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newPosition.X = _roomBounds.Left;
        }
        else if (batBounds.Right > _roomBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newPosition.X = _roomBounds.Right - _bat.Width;
        }

        if (batBounds.Top < _roomBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newPosition.Y = _roomBounds.Top;
        }
        else if (batBounds.Bottom > _roomBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newPosition.Y = _roomBounds.Bottom - _bat.Height;
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




    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        _grayscaleEffect.Parameters["Saturation"].SetValue(_saturation);

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _grayscaleEffect);

        // Draw the tilemap
        _tilemap.Draw(Core.SpriteBatch);

        // Draw each slime
        float lerpAmount = (float)(_tickTimer.TotalSeconds / s_tickTime.TotalSeconds);
        foreach (SlimeSegment segment in _slimes)
        {
            Vector2 pos = Vector2.Lerp(segment.At, segment.To, lerpAmount);
            _slime.Draw(Core.SpriteBatch, pos);
        }


        // Draw the bat sprite.
        _bat.Draw(Core.SpriteBatch, _batPosition);

        // Draw the score
        Core.SpriteBatch.DrawString(
            _font,              // spriteFont
            $"Score: {_score}", // text
            _scoreTextPosition, // position
            Color.White,        // color
            0.0f,               // rotation
            _scoreTextOrigin,   // origin
            1.0f,               // scale
            SpriteEffects.None, // effects
            0.0f                // layerDepth
        );

        _pauseMenu.Draw(Core.SpriteBatch);
        _gameOverMenu.Draw(Core.SpriteBatch);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
}
