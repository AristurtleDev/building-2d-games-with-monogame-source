using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Scenes;

public abstract class Scene : IDisposable
{
    /// <summary>
    /// Gets a reference to the current game instance.
    /// </summary>
    protected Game Game { get; }

    /// <summary>
    /// Gets a reference to the scene manager this scene belongs to.
    /// </summary>
    protected ISceneManager SceneManager { get; }

    /// <summary>
    /// Gets the ContentManager used for loading scene-specific assets.
    /// </summary>
    /// <remarks>
    /// Assets loaded through this ContentManager will be automatically unloaded when this scene ends.
    /// </remarks>
    protected ContentManager Content { get; }

    /// <summary>
    /// Gets the graphics device manager used to manage the presentation of graphics.
    /// </summary>
    protected GraphicsDeviceManager GraphicsDeviceManager { get; }

    /// <summary>
    /// Gets a reference to the GraphicsDevice used for rendering.
    /// </summary>
    protected GraphicsDevice GraphicsDevice { get; }

    /// <summary>
    /// Gets a value that indicates if the scene has been disposed of.
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Creates a new scene instance.
    /// </summary>
    /// <param name="game">The game instance</param>
    public Scene(Game game)
    {
        Game = game;

        // Get the GraphicsDeviceManager from the game's services container
        GraphicsDeviceManager = (GraphicsDeviceManager)Game.Services.GetService<IGraphicsDeviceManager>();

        // Get the GraphicsDevice from the game's services container
        GraphicsDevice = (GraphicsDevice)Game.Services.GetService<IGraphicsDeviceService>();

        // Get the ISceneManager service from the game's service container
        SceneManager = Game.Services.GetService<ISceneManager>();

        // Create a content manager for the scene
        Content = new ContentManager(game.Services);

        // Set the root directory for content to the same as the root directory
        // for the game's content.
        Content.RootDirectory = game.Content.RootDirectory;
    }

    // Finalizer, called when object is cleaned up by garbage collector.
    ~Scene() => Dispose(false);

    /// <summary>
    /// Initializes the scene.
    /// </summary>
    /// <remarks>
    /// When overriding this in a derived class, ensure that base.Initialize()
    /// still called as this is when LoadContent is called.
    /// </remarks>
    public virtual void Initialize()
    {
        LoadContent();
    }

    /// <summary>
    /// Override to provide logic to load content for the scene.
    /// </summary>
    public virtual void LoadContent() { }

    /// <summary>
    /// Unloads scene-specific content.
    /// </summary>
    public virtual void UnloadContent()
    {
        Content.Unload();
    }

    /// <summary>
    /// Updates this scene.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current frame.</param>
    public virtual void Update(GameTime gameTime) { }

    /// <summary>
    /// Draws this scene.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used for rendering.</param>
    /// <param name="gameTime">A snapshot of the timing values for the current frame.</param>
    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }

    /// <summary>
    /// Disposes of this scene.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of this scene.
    /// </summary>
    /// <param name="disposing">'
    /// Indicates whether managed resources should be disposed.  This value is only true when called from the main
    /// Dispose method.  When called from the finalizer, this will be false.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disposing)
        {
            UnloadContent();
            Content.Dispose();
        }
    }
}
