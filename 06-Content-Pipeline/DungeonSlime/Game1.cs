using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private FramesPerSecondCounter _fpsCounter;
    private Texture2D _logo;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Create and add the component to the game's components collection
        _fpsCounter = new FramesPerSecondCounter(this);
        Components.Add(_fpsCounter);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _logo = Content.Load<Texture2D>("images/logo");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Rectangle iconSourceRect = new Rectangle(0, 0, 128, 128);
        Rectangle wordmarkSourceRect = new Rectangle(150, 34, 458, 58);

        _spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack);
        _spriteBatch.Draw(
            _logo,              // texture
            new Vector2(        // position
              Window.ClientBounds.Width,
              Window.ClientBounds.Height) * 0.5f,
            iconSourceRect,     // sourceRectangle
            Color.White,        // color
            0.0f,               // rotation
            new Vector2(        // origin
              iconSourceRect.Width,
              iconSourceRect.Height) * 0.5f,
            1.0f,               // scale
            SpriteEffects.None, // effects
            1.0f);              // layerDepth


        _spriteBatch.Draw(
            _logo,              // texture
            new Vector2(        // position
              Window.ClientBounds.Width,
              Window.ClientBounds.Height) * 0.5f,
            wordmarkSourceRect, // sourceRectangle
            Color.White,        // color
            0.0f,               // rotation
            new Vector2(        // origin
              wordmarkSourceRect.Width,
              wordmarkSourceRect.Height) * 0.5f,
            1.0f,               // scale
            SpriteEffects.None, // effects
            0.0f);              // layerDepth

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
