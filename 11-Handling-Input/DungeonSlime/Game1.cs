using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGameLibrary.Graphics;

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

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        _batPosition = new Vector2(_slime.Width + 10, 0);
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
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        //  Update the slime and bat animated sprites
        _slime.Update(gameTime);
        _bat.Update(gameTime);

        HandleKeyboardInput();
        HandleMouseInput();
        HandleGamepadInput();
        HandleTouchInput();

        base.Update(gameTime);
    }

    private void HandleKeyboardInput()
    {
        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Up))
        {
            _slimePosition.Y -= MOVEMENT_SPEED;
        }

        if (keyboardState.IsKeyDown(Keys.Down))
        {
            _slimePosition.Y += MOVEMENT_SPEED;
        }

        if (keyboardState.IsKeyDown(Keys.Left))
        {
            _slimePosition.X -= MOVEMENT_SPEED;
        }

        if (keyboardState.IsKeyDown(Keys.Right))
        {
            _slimePosition.X += MOVEMENT_SPEED;
        }
    }

    private void HandleMouseInput()
    {
        MouseState mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            _batPosition = mouseState.Position.ToVector2();
        }
    }

    private void HandleGamepadInput()
    {
        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

        if (gamePadState.Buttons.A == ButtonState.Pressed)
        {
            _slimePosition.X += gamePadState.ThumbSticks.Left.X * 1.5f * MOVEMENT_SPEED;
            _slimePosition.Y -= gamePadState.ThumbSticks.Left.Y * 1.5f * MOVEMENT_SPEED;
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        }
        else
        {
            _slimePosition.X += gamePadState.ThumbSticks.Left.X * MOVEMENT_SPEED;
            _slimePosition.Y -= gamePadState.ThumbSticks.Left.Y * MOVEMENT_SPEED;
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
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

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the slime animated sprite
        _slime.Draw(_spriteBatch, _slimePosition);

        // Draw the bat animated sprite 10px to the right of the slime.
        _bat.Draw(_spriteBatch, _batPosition);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
