using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Tracks the FramesPerSecondCounter instance.
    private FramesPerSecondCounter _fpsCounter;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Create a new FramesPerSecondCounter.
        _fpsCounter = new FramesPerSecondCounter(this);

        // Add the FramesPerSecondCounter ot the game's component collection
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
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Update the counter
        _fpsCounter.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Update the frame counter.
        _fpsCounter.UpdateCounter();

        // Update the window title to show the frames per second.
        Window.Title = $" FPS: {_fpsCounter.FramesPerSecond}";

        base.Draw(gameTime);
    }
}
