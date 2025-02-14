using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGameLibrary;
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

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        InputManager.Initialize();

        base.Initialize();

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
    }

    protected override void Update(GameTime gameTime)
    {
        InputManager.Update(gameTime);

        //  Update the slime and bat animated sprites
        _slime.Update(gameTime);
        _bat.Update(gameTime);

        UpdateBatMovement();

        // Store the previous position before moving
        Vector2 _previousSlimePosition = _slimePosition;

        HandleKeyboardInput();
        HandleMouseInput();
        HandleGamepadInput();
        HandleTouchInput();

        if (CollisionCheck())
        {
            // Divide the width and height of the screen into equal columns and
            // rows based on the width and height of the bat.
            int totalColumns = GraphicsDevice.PresentationParameters.BackBufferWidth / (int)_bat.Width;
            int totalRows = GraphicsDevice.PresentationParameters.BackBufferHeight / (int)_bat.Height;

            // Choose a random row and column.
            int column = Random.Shared.Next(0, totalColumns);
            int row = Random.Shared.Next(0, totalRows);

            // Change the bat position.
            _batPosition = new Vector2(column * _bat.Width, row * _bat.Height);

            // Assign a new random velocity.
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

    private bool CollisionCheck()
    {
        Circle slimeBounds = new Circle
        (
            (int)(_slimePosition.X + (_slime.Width * 0.5f)),
            (int)(_slimePosition.Y + (_slime.Height * 0.5f)),
            (int)(_slime.Width * 0.5f)
        );

        Circle batBounds = new Circle
        (
            (int)(_batPosition.X + (_bat.Width * 0.5f)),
            (int)(_batPosition.Y + (_bat.Height * 0.5f)),
            (int)(_bat.Width * 0.5f)
        );


        return slimeBounds.Intersects(batBounds);
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

    private void UpdateBatMovement()
    {
        // Calculate the new position of the bat based on the velocity
        Vector2 newPosition = _batPosition + _batVelocity;

        // Get the bounds of the screen as a rectangle
        Rectangle screenBounds = new Rectangle(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );

        // Get the bounds of the bat as a rectangle
        Rectangle batBounds = new Rectangle(
            (int)newPosition.X,
            (int)newPosition.Y,
            (int)_bat.Width,
            (int)_bat.Height
        );

        // if the bat is not contained within the bounds of the screen, then we
        // perform our collision response and bounce (reflect) it off the screen
        // edge that it is closest too
        if (!screenBounds.Contains(batBounds))
        {
            // First find the distance from the edges of the bat to each edge of the screen
            float distanceLeft = Math.Abs(screenBounds.Left - batBounds.Left);
            float distanceRight = Math.Abs(screenBounds.Right - batBounds.Right);
            float distanceTop = Math.Abs(screenBounds.Top - batBounds.Top);
            float distanceBottom = Math.Abs(screenBounds.Bottom - batBounds.Bottom);

            // Determine which edge is the closest edge
            float minDistance = Math.Min(
                Math.Min(distanceLeft, distanceRight),
                Math.Min(distanceTop, distanceBottom)
            );

            Vector2 normal;

            if (minDistance == distanceLeft)
            {
                // The bat is closest to the left edge, so get the left edge normal
                // and move the new position so the left edge of the bat will be
                // flush with the left edge of the screen.
                normal = Vector2.UnitX;
                newPosition.X = 0;
            }
            else if (minDistance == distanceRight)
            {
                // The bat is closest to the right edge, so get the right edge normal
                // and move the new position so that the right edge of the bat will
                // be flush with the right edge of the screen.            
                normal = -Vector2.UnitX;
                newPosition.X = screenBounds.Right - _bat.Width;
            }
            else if (minDistance == distanceTop)
            {
                // The bat is closest to the top edge, so get the top edge normal
                // and move the new position so that the top edge of the bat will
                // be flush with the top edge of the screen.
                normal = Vector2.UnitY;
                newPosition.Y = 0;
            }
            else
            {
                // the bat is closest to the bottom edge, so get the bottom edge normal
                // and move the new position so that the bottom edge of the bat will
                // be flush with the bottom edge of the screen.
                normal = -Vector2.UnitY;
                newPosition.Y = screenBounds.Bottom - _bat.Height;
            }

            // Reflect the velocity about the normal
            _batVelocity = Vector2.Reflect(_batVelocity, normal);
        }

        // Set the new position of the bat
        _batPosition = newPosition;
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
