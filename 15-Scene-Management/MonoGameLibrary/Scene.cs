using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary;

/// <summary>
/// Abstract base class for game scenes that provides core functionality and lifecycle management.
/// Each scene manages its own content, rendering, and state.
/// </summary>
public abstract class Scene
{
    /// <summary>
    /// Gets a reference to the Game instance this scene belongs to.
    /// </summary>
    public Game Game { get; }

    /// <summary>
    /// Gets the ContentManager used for loading scene-specific assets.
    /// Content loaded through this manager will be automatically unloaded when the scene ends.
    /// </summary>
    public ContentManager Content { get; }

    /// <summary>
    /// Gets the GraphicsDevice used for rendering this scene.
    /// </summary>
    public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;

    /// <summary>
    /// Gets or Sets the color used to clear the back buffer when before drawing the scene.
    /// Defaults to CornflowerBlue.
    /// </summary>
    public Color ClearColor { get; set; } = Color.CornflowerBlue;

    /// <summary>
    /// Gets or Sets the sampler state used when rendering textures in this scene.
    /// Defaults to PointClamp.
    /// </summary>
    public SamplerState SamplerState { get; set; } = SamplerState.PointClamp;

    /// <summary>
    /// Gets the render target used by this scene for off-screen rendering.
    /// </summary>
    public RenderTarget2D RenderTarget { get; protected set; }

    /// <summary>
    /// Creates a new Scene instance.
    /// </summary>
    /// <param name="game">The Game instance this scene belongs to.</param>
    public Scene(Game game)
    {
        Game = game;
        Content = new ContentManager(game.Services);
        Content.RootDirectory = game.Content.RootDirectory;
    }

    /// <summary>
    /// Initializes this scene.
    /// </summary>
    public virtual void Initialize()
    {
        LoadContent();
    }

    /// <summary>
    /// Loads content specific to this scene.
    /// </summary>
    public virtual void LoadContent()
    {
        GenerateRenderTarget();
    }

    /// <summary>
    /// Unloads all content that was loaded by this scene.
    /// </summary>
    public virtual void UnloadContent()
    {
        Content.Unload();
        Content.Dispose();
    }

    /// <summary>
    /// Creates or recreates the render target used by this scene.
    /// Called automatically during LoadContent.
    /// </summary>
    public virtual void GenerateRenderTarget()
    {
        int width = Game.GraphicsDevice.PresentationParameters.BackBufferWidth;
        int height = Game.GraphicsDevice.PresentationParameters.BackBufferHeight;

        if (RenderTarget != null && !RenderTarget.IsDisposed)
        {
            RenderTarget.Dispose();
        }

        RenderTarget = new RenderTarget2D(GraphicsDevice, width, height);
    }

    /// <summary>
    /// Updates this scene's logic.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public virtual void Update(GameTime gameTime) { }

    /// <summary>
    /// Prepares the scene for rendering.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch instance used for rendering.</param>
    public virtual void BeforeDraw(SpriteBatch spriteBatch)
    {
        GraphicsDevice.SetRenderTarget(RenderTarget);
        Game.GraphicsDevice.Clear(ClearColor);
        spriteBatch.Begin(samplerState: SamplerState);
    }

    /// <summary>
    /// Draws this scene.
    /// Override this method to implement scene-specific rendering.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch instance used for rendering.</param>
    public virtual void Draw(SpriteBatch spriteBatch) { }

    /// <summary>
    /// Completes the rendering process for this scene.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch instance used for rendering.</param>
    public virtual void AfterDraw(SpriteBatch spriteBatch)
    {
        spriteBatch.End();
        GraphicsDevice.SetRenderTarget(null);
    }
}
