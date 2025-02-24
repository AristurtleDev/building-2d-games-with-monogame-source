using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGameLibrary;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

namespace DungeonSlime;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private AnimatedSprite _slime;
    private AnimatedSprite _bat;
    private Vector2 _slimePosition;
    private const float MOVEMENT_SPEED = 5.0f;
    private Vector2 _batPosition;
    private Vector2 _batVelocity;
    private AudioManager _audioManager;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Create and add the audio manager
        _audioManager = new AudioManager(this);
        Components.Add(_audioManager);

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        InputManager.Initialize();

        base.Initialize();

        // Start playing background music
        _audioManager.PlaySong("audio/theme");

        _batPosition = new Vector2(_slime.Width + 10, 0);
        AssignRandomBatVelocity();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Create the texture atlas from the XML configuration file
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // Create the slime animated sprite
        _slime = atlas.CreateAnimatedSprite("slime-animation");

        // Create the bat animated sprite
        _bat = atlas.CreateAnimatedSprite("bat-animation");

        // Load audio content
        _audioManager.AddSoundEffect("audio/bounce");
        _audioManager.AddSoundEffect("audio/collect");
        _audioManager.AddSong("audio/theme");

    }

    protected override void Update(GameTime gameTime)
    {
        InputManager.Update(gameTime);

        //  Update the slime and bat animated sprites
        _slime.Update(gameTime);
        _bat.Update(gameTime);

        HandleKeyboardInput();
        HandleMouseInput();
        HandleGamepadInput();
        HandleTouchInput();

        // Create a bounding rectangle for the screen
        Rectangle screenBounds = new Rectangle(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        // Creating a bounding circle for the slime
        Circle slimeBounds = new Circle(
            (int)(_slimePosition.X + (_slime.Width * 0.5f)),
            (int)(_slimePosition.Y + (_slime.Height * 0.5f)),
            (int)(_slime.Width * 0.5f)
        );

        // Use distance based checks to determine if the slime is within the
        // bounds of the game screen, and if it's outside that screen edge,
        // move it back inside.
        if (slimeBounds.Left < screenBounds.Left)
        {
            _slimePosition.X = screenBounds.Left;
        }
        else if (slimeBounds.Right > screenBounds.Right)
        {
            _slimePosition.X = screenBounds.Right - _slime.Width;
        }

        if (slimeBounds.Top < screenBounds.Top)
        {
            _slimePosition.Y = screenBounds.Top;
        }
        else if (slimeBounds.Bottom > screenBounds.Bottom)
        {
            _slimePosition.Y = screenBounds.Bottom - _slime.Height;
        }

        // Calculate the new position of the bat based on the velocity
        Vector2 newBatPosition = _batPosition + _batVelocity;

        // Create a bounding circle for the bat
        Circle batBounds = new Circle(
            (int)(newBatPosition.X + (_bat.Width * 0.5f)),
            (int)(newBatPosition.Y + (_bat.Height * 0.5f)),
            (int)(_bat.Width * 0.5f)
        );

        Vector2 normal = Vector2.Zero;

        // Use distance based checks to determine if the bat is within the
        // bounds of the game screen, and if it's outside that screen edge,
        // reflect it about the screen edge normal
        if (batBounds.Left < screenBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newBatPosition.X = screenBounds.Left;
        }
        else if (batBounds.Right > screenBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newBatPosition.X = screenBounds.Right - _bat.Width;
        }

        if (batBounds.Top < screenBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newBatPosition.Y = screenBounds.Top;
        }
        else if (batBounds.Bottom > screenBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newBatPosition.Y = screenBounds.Bottom - _bat.Height;
        }

        // If the normal is anything but Vector2.Zero, this means the bat had
        // moved outside the screen edge so we should reflect it about the
        // normal.
        if (normal != Vector2.Zero)
        {
            // Play bounce sound through the manager
            _audioManager.PlaySoundEffect("audio/bounce");

            _batVelocity = Vector2.Reflect(_batVelocity, normal);
        }

        _batPosition = newBatPosition;

        if (slimeBounds.Intersects(batBounds))
        {
            // Play collect sound through the manager
            _audioManager.PlaySoundEffect("audio/collect");

            // Divide the width  and height of the screen into equal columns and
            // rows based on the width and height of the bat.
            int totalColumns = GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_bat.Width;
            int totalRows = GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_bat.Height;

            // Choose a random row and column based on the total number of each
            int column = Random.Shared.Next(0, totalColumns);
            int row = Random.Shared.Next(0, totalRows);

            // Change the bat position by setting the x and y values equal to
            // the column and row multiplied by the width and height.
            _batPosition = new Vector2(column * _bat.Width, row * _bat.Height);

            // Assign a new random velocity to the bat
            AssignRandomBatVelocity();
        }

        base.Update(gameTime);
    }

    private void HandleKeyboardInput()
    {
        if (InputManager.Keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        if (InputManager.Keyboard.IsKeyDown(Keys.Up))
        {
            _slimePosition.Y -= MOVEMENT_SPEED;
        }
        if (InputManager.Keyboard.IsKeyDown(Keys.Down))
        {
            _slimePosition.Y += MOVEMENT_SPEED;
        }
        if (InputManager.Keyboard.IsKeyDown(Keys.Left))
        {
            _slimePosition.X -= MOVEMENT_SPEED;
        }
        if (InputManager.Keyboard.IsKeyDown(Keys.Right))
        {
            _slimePosition.X += MOVEMENT_SPEED;
        }
        if (InputManager.Keyboard.WasKeyJustPressed(Keys.M))
        {
            _audioManager.ToggleMute();
        }

        if (InputManager.Keyboard.WasKeyJustPressed(Keys.OemPlus))
        {
            _audioManager.IncreaseVolume(0.1f);
        }

        if (InputManager.Keyboard.WasKeyJustPressed(Keys.OemMinus))
        {
            _audioManager.DecreaseVolume(0.1f);
        }

    }

    private void HandleMouseInput()
    {
        if (InputManager.Mouse.WasButtonJustPressed(MouseButton.Left))
        {
            _batPosition = InputManager.Mouse.Position.ToVector2();
        }
    }

    private void HandleGamepadInput()
    {
        GamePadInfo gamePadOne = InputManager.GamePads[(int)PlayerIndex.One];

        if (gamePadOne.IsButtonDown(Buttons.Back))
        {
            Exit();
        }

        if (gamePadOne.IsButtonDown(Buttons.A))
        {
            _slimePosition.X += gamePadOne.LeftThumbStick.X * 1.5f * MOVEMENT_SPEED;
            _slimePosition.Y -= gamePadOne.LeftThumbStick.Y * 1.5f * MOVEMENT_SPEED;
            gamePadOne.SetVibration(1.0f, TimeSpan.FromSeconds(0.5f));
        }
        else
        {
            _slimePosition.X += gamePadOne.LeftThumbStick.X * MOVEMENT_SPEED;
            _slimePosition.Y -= gamePadOne.LeftThumbStick.Y * MOVEMENT_SPEED;
        }
    }

    private void HandleTouchInput()
    {
        TouchCollection touchCollection = TouchPanel.GetState();

        if (touchCollection.Count > 0)
        {
            TouchLocation touchLocation = touchCollection[0];
            _batPosition = touchLocation.Position;
        }
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

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the slime animated sprite
        _slime.Draw(_spriteBatch, _slimePosition);

        // Draw the bat animated sprite
        _bat.Draw(_spriteBatch, _batPosition);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
