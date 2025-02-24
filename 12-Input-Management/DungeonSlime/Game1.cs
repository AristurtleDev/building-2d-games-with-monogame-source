using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    private InputManager _input;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Create and add the input manager component to the game's component collection
        _input = new InputManager(this);
        Components.Add(_input);
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
        base.Update(gameTime);

        //  Update the slime and bat animated sprites
        _slime.Update(gameTime);
        _bat.Update(gameTime);

        HandleKeyboardInput();
        HandleGamepadInput();
    }

    private void HandleKeyboardInput()
    {
        if (_input.Keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        if (_input.Keyboard.IsKeyDown(Keys.Up))
        {
            _slimePosition.Y -= MOVEMENT_SPEED;
        }

        if (_input.Keyboard.IsKeyDown(Keys.Down))
        {
            _slimePosition.Y += MOVEMENT_SPEED;
        }

        if (_input.Keyboard.IsKeyDown(Keys.Left))
        {
            _slimePosition.X -= MOVEMENT_SPEED;
        }

        if (_input.Keyboard.IsKeyDown(Keys.Right))
        {
            _slimePosition.X += MOVEMENT_SPEED;
        }
    }

    private void HandleGamepadInput()
    {
        GamePadInfo gamePadOne = _input.GamePads[(int)PlayerIndex.One];

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
