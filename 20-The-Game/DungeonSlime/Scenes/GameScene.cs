using System;
using System.Collections.Generic;
using System.Linq;
using DungeonSlime.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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

    private TimeSpan _tickTimer;
    private List<Slime> _slimes;
    private Vector2 _nextDirection;
    private readonly static TimeSpan s_tickTime = TimeSpan.FromMilliseconds(500);

    private Song _themeSong;
    private Song _gameOverSong;

    private GameController _controller;
    private Queue<Vector2> _inputBuffer = new Queue<Vector2>();
    private const int MAX_BUFFER_SIZE = 2;


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
        // room bounds, and to vertically be at the center of the first tile.
        _scoreTextPosition = new Vector2(_roomBounds.Left, _tilemap.TileHeight * 0.5f);

        // Set the origin of the text so it's left-centered.
        float scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
        _scoreTextOrigin = new Vector2(0, scoreTextYOrigin);

        _controller = new GameController();

        InitializeNewGame();
    }

    private void InitializeNewGame()
    {
        _slimes = new List<Slime>();

        // Initial slime position will be the center tile of the tile map.
        int centerRow = _tilemap.Rows / 2;
        int centerColumn = _tilemap.Columns / 2;
        _slimes = new List<Slime>();
        Slime slime = new Slime();
        slime.At = new Vector2(centerColumn * _tilemap.TileWidth, centerRow * _tilemap.TileHeight);
        slime.To = slime.At + new Vector2(_tilemap.TileWidth);
        slime.Direction = Vector2.UnitX;
        _slimes.Add(slime);

        _nextDirection = slime.Direction;

        // Assign the initial random velocity to the bat.
        AssignRandomBatPosition();
        AssignRandomBatVelocity();

        _tickTimer = s_tickTime;
        _score = 0;
    }

    public override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        // Create the slime animated sprite from the atlas.
        _slime = atlas.CreateAnimatedSprite("slime-animation");
        // _slime.Scale = new Vector2(4.0f, 4.0f);

        // Create the bat animated sprite from the atlas.
        _bat = atlas.CreateAnimatedSprite("bat-animation");
        // _bat.Scale = new Vector2(4.0f, 4.0f);

        // Load the tilemap from the XML configuration file.
        _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");
        // _tilemap.Scale = new Vector2(4.0f, 4.0f);

        // Load the bounce sound effect
        _bounceSoundEffect = Content.Load<SoundEffect>("audio/bounce");

        // Load the collect sound effect
        _collectSoundEffect = Content.Load<SoundEffect>("audio/collect");

        // Load the font
        _font = Core.Content.Load<SpriteFont>("fonts/04B_30");

        // Load the sound effect to play when ui actions occur.
        SoundEffect uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");

        // Load the theme song
        _themeSong = Core.Content.Load<Song>("audio/theme");

        // Load the game over song
        _gameOverSong = Content.Load<Song>("audio/game_over");

        // Create the UI Controller
        UIElementController controller = new UIElementController();

        // Create the pause menu.
        CreatePauseMenu(atlas, uiSoundEffect, controller);

        // Create the game over menu.
        CreateGameOverMenu(atlas, uiSoundEffect, controller);
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
            Core.Audio.PlaySong(_themeSong);

            if (retryButton.IsSelected)
            {
                // The retry button is selected and the confirm action was
                // performed, so deselect the game over menu.
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
        if (!_pauseMenu.IsEnabled && !_gameOverMenu.IsEnabled)
        {
            UpdateGame(gameTime);
        }
    }

    private void UpdateGame(GameTime gameTime)
    {
        // Update the slime animated sprite.
        _slime.Update(gameTime);

        // Update the bat animated sprite.
        _bat.Update(gameTime);

        CheckInput();

        // Check for keyboard input and handle it.
        // CheckKeyboardInput();

        // Check for gamepad input and handle it.
        // CheckGamePadInput();

        // Get a reference to the head of the slime.
        Slime head = _slimes[0];

        // Create the bounds for the head of the slime.
        Circle slimeBounds = new Circle(
            (int)(head.At.X + (_slime.Width * 0.5f)),
            (int)(head.At.Y + (_slime.Height * 0.5f)),
            (int)(_slime.Width * 0.5f)
        );

        // Create the bounds for the bat
        Circle batBounds = new Circle(
            (int)(_batPosition.X + _bat.Width * 0.5f),
            (int)(_batPosition.Y + _bat.Height * 0.5f),
            (int)(_bat.Height * 0.25f)
        );

        // If the slime and bat are intersecting, then eat the bat and create
        // a new slime
        if (slimeBounds.Intersects(batBounds))
        {
            Slime tail = _slimes[_slimes.Count - 1];
            Slime newTail = new Slime();
            newTail.At = tail.At + tail.ReverseDirection * _tilemap.TileWidth;
            newTail.To = tail.At;
            newTail.Direction = Vector2.Normalize(tail.At - newTail.At);
            _slimes.Add(newTail);

            // Assign a new position and velocity to the bat.
            AssignRandomBatPosition();
            AssignRandomBatVelocity();

            // Play the collect sound effect.
            Core.Audio.PlaySoundEffect(_collectSoundEffect);

            // Increase the player's score.
            _score += 100;
        }

        // Increase the tick timer
        _tickTimer = _tickTimer + gameTime.ElapsedGameTime * 2.5f;

        // If the tick timer has exceeded the time to tick
        if (_tickTimer >= s_tickTime)
        {
            _tickTimer = TimeSpan.Zero;

            // Process the input buffer
            if(_inputBuffer.Count > 0)
            {
                // Take the next direction from the buffer
                _nextDirection = _inputBuffer.Dequeue();
            }

            // Get the first slime in the chain
            Slime previousHead = _slimes[0];

            // Calculate the slime that will be the new head of the chain
            Slime newHead = new Slime();
            newHead.Direction = _nextDirection;
            newHead.At = previousHead.To;
            newHead.To = newHead.At + newHead.Direction * _tilemap.TileWidth;

            _slimes.Insert(0, newHead);
            _slimes.RemoveAt(_slimes.Count - 1);

            if (!_roomBounds.Contains(newHead.To))
            {
                Core.Audio.PlaySong(_gameOverSong);
                _gameOverMenu.IsEnabled = _gameOverMenu.IsSelected = _gameOverMenu.IsVisible = true;
            }

            foreach (Slime child in _slimes[1..])
            {
                Circle headBounds = new Circle(
                    (int)(newHead.At.X + (_slime.Width * 0.5f)),
                    (int)(newHead.At.Y + (_slime.Height * 0.5f)),
                    (int)(_slime.Width * 0.5f)
                );

                Circle childBounds = new Circle(
                    (int)(child.At.X + (_slime.Width * 0.5f)),
                    (int)(child.At.Y + (_slime.Height * 0.5f)),
                    (int)(_slime.Width * 0.5f)
                );

                if (headBounds.Intersects(childBounds))
                {
                    Core.Audio.PlaySong(_gameOverSong);
                    _gameOverMenu.IsEnabled = _gameOverMenu.IsSelected = _gameOverMenu.IsVisible = true;
                }
            }
        }

        MoveBat();
    }

    private void MoveBat()
    {
        Vector2 newPosition = _batPosition + _batVelocity;

        Rectangle batBounds = new Rectangle(
            (int)newPosition.X,
            (int)newPosition.Y,
            (int)_bat.Width,
            (int)_bat.Height
        );

        // Use distance based checks to determine fi the bat is within the
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

    private void AssignRandomBatPosition()
    {
        // Get a reference to the snake's head
        Slime head = _slimes[0];

        // Calculate the center of the room
        Vector2 roomCenter = new Vector2(
            _roomBounds.X + _roomBounds.Width * 0.5f,
            _roomBounds.Y + _roomBounds.Height * 0.5f
        );

        // Calculate the vector from the room center to the head
        Vector2 centerToHead = head.At - roomCenter;

        // Determine the furthest wall by finding which component (x or y) is
        // larger and in which direction
        Vector2 newPosition;
        if(Math.Abs(centerToHead.X) > Math.Abs(centerToHead.Y))
        {
            // Head is closer to either the left or right wall
            if(centerToHead.X > 0)
            {
                // Head is on the right side, place bat on the left wall
                newPosition = new Vector2(
                    _roomBounds.Left + _bat.Width,
                    roomCenter.Y
                );
            }
            else
            {
                // Head is on left side, place bat on the right wall
                newPosition = new Vector2(
                    _roomBounds.Right - _bat.Width * 2,
                    roomCenter.Y
                );
            }
        }
        else
        {
            // Head is closer to top or bottom wall
            if(centerToHead.Y > 0)
            {
                // Head is closer to bottom, place bat on top wall
                newPosition = new Vector2(
                    roomCenter.X,
                    _roomBounds.Top + _bat.Height
                );
            }
            else
            {
                // Head is closer to top, place bat on bottom wall.
                newPosition = new Vector2(
                    roomCenter.X,
                    _roomBounds.Bottom - _bat.Height * 2
                );
            }
        }

        // Add a small random variation to make it less predictable
        // Vary position by up to 20% of room width/height
        float xVariation = (float)Random.Shared.NextDouble() * 0.2f * _roomBounds.Width - 0.1f * _roomBounds.Width;
        float yVariation = (float)Random.Shared.NextDouble() * 0.2f * _roomBounds.Height - 0.1f * _roomBounds.Height;

        newPosition.X += xVariation;
        newPosition.Y += yVariation;

        // Ensure the bat stays within room bounds after adding variation
        newPosition.X = Math.Clamp(newPosition.X,
            _roomBounds.Left + _bat.Width,
            _roomBounds.Right - _bat.Width * 2);

        newPosition.Y = Math.Clamp(
            newPosition.Y,
            _roomBounds.Top + _bat.Height,
            _roomBounds.Bottom - _bat.Height * 2
        );

        _batPosition = newPosition;
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

    private void CheckInput()
    {
        if(_controller.Pause())
        {
            _pauseMenu.IsEnabled = _pauseMenu.IsVisible = _pauseMenu.IsSelected = true;
        }

        // Check for direction changes
        Vector2? newDirection = null;

        if(_controller.MoveUp())
        {
            newDirection = -Vector2.UnitY;
        }
        else if(_controller.MoveDown())
        {
            newDirection = Vector2.UnitY;
        }
        else if(_controller.MoveLeft())
        {
            newDirection = -Vector2.UnitX;
        }
        else if(_controller.MoveRight())
        {
            newDirection = Vector2.UnitX;
        }

        // If a new direction was input, consider adding it to the buffer
        if(newDirection.HasValue && _inputBuffer.Count < MAX_BUFFER_SIZE)
        {
            // If buffer is empty, validate against the current direction.
            // Otherwise, validate against the last buffered direction
            Vector2 validateAgainst = _inputBuffer.Count > 0 ?
                                      _inputBuffer.Last() :
                                      _slimes[0].Direction;

            // Check if this is a valid direction change (not a reversed)
            if(Vector2.Dot(newDirection.Value, validateAgainst) >= 0)
            {
                // Only add if it is different from the last buffered direction
                if(_inputBuffer.Count == 0 || _inputBuffer.Last() != newDirection.Value)
                {
                    _inputBuffer.Enqueue(newDirection.Value);
                }
            }
        }
    }

    // private void CheckKeyboardInput()
    // {
    //     // Get a reference to the keyboard info
    //     KeyboardInfo keyboard = Core.Input.Keyboard;

    //     // If the escape key is pressed, return to the title screen
    //     if (keyboard.WasKeyJustPressed(Keys.Escape))
    //     {
    //         _pauseMenu.IsEnabled = _pauseMenu.IsVisible = _pauseMenu.IsSelected = true;
    //     }

    //     // Check for direction changes
    //     Vector2? newDirection = null;

    //     // If the W or Up keys are down, move the slime up on the screen.
    //     if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
    //     {
    //         newDirection = -Vector2.UnitY;
    //     }
    //     // if the S or Down keys are down, move the slime down on the screen.
    //     else if
    //     (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
    //     {
    //         newDirection = Vector2.UnitY;
    //     }
    //     // If the A or Left keys are down, move the slime left on the screen.
    //     else if
    //     (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
    //     {
    //         newDirection = -Vector2.UnitX;
    //     }
    //     // If the D or Right keys are down, move the slime right on the screen.
    //     else if
    //     (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
    //     {
    //         newDirection = Vector2.UnitX;
    //     }

    //     // If a new direction was input and the buffer isn't full, add it
    //     if(newDirection.HasValue && _inputBuffer.Count < MAX_BUFFER_SIZE)
    //     {
    //         // Only buffer directions that are different from the last buffered direction
    //         if(_inputBuffer.Count == 0 || _inputBuffer.Last() != newDirection.Value)
    //         {
    //             _inputBuffer.Enqueue(newDirection.Value);
    //         }
    //     }

    //     // Only apply the direction change if it's not a reverse direction
    //     // This prevents the snake from backing into itself
    //     if (Vector2.Dot(newDirection, _slimes[0].Direction) >= 0)
    //     {
    //         _nextDirection = newDirection;
    //     }
    // }

    // private void CheckGamePadInput()
    // {
    //     // Get the gamepad info for gamepad one.
    //     GamePadInfo gamePadOne = Core.Input.GamePads[(int)PlayerIndex.One];

    //     if (gamePadOne.WasButtonJustPressed(Buttons.Start))
    //     {
    //         _pauseMenu.IsEnabled = _pauseMenu.IsVisible = _pauseMenu.IsSelected = true;
    //     }

    //     // Stores the potential new direction
    //     Vector2 potentialDirection = _nextDirection;

    //     if (gamePadOne.WasButtonJustPressed(Buttons.DPadUp) || gamePadOne.WasButtonJustPressed(Buttons.LeftThumbstickUp))
    //     {
    //         potentialDirection = -Vector2.UnitY;
    //     }
    //     else if
    //     (gamePadOne.WasButtonJustPressed(Buttons.DPadDown) || gamePadOne.WasButtonJustPressed(Buttons.LeftThumbstickDown))
    //     {
    //         potentialDirection = Vector2.UnitY;
    //     }
    //     // If the A or Left keys are down, move the slime left on the screen.
    //     else if
    //     (gamePadOne.WasButtonJustPressed(Buttons.DPadLeft) || gamePadOne.WasButtonJustPressed(Buttons.LeftThumbstickLeft))
    //     {
    //         potentialDirection = -Vector2.UnitX;
    //     }
    //     // If the D or Right keys are down, move the slime right on the screen.
    //     else if
    //     (gamePadOne.WasButtonJustPressed(Buttons.DPadRight) || gamePadOne.WasButtonJustPressed(Buttons.LeftThumbstickRight))
    //     {
    //         potentialDirection = Vector2.UnitX;
    //     }

    //     // Only apply the direction change if it's not a reverse direction
    //     // This prevents the snake from backing into itself
    //     if (Vector2.Dot(potentialDirection, _slimes[0].Direction) >= 0)
    //     {
    //         _nextDirection = potentialDirection;
    //     }
    // }

    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the tilemap
        _tilemap.Draw(Core.SpriteBatch);

        // Draw each slime
        foreach (Slime slime in _slimes)
        {
            Vector2 pos = Vector2.Lerp(slime.At, slime.To, (float)_tickTimer.TotalSeconds / (float)s_tickTime.TotalSeconds);
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
