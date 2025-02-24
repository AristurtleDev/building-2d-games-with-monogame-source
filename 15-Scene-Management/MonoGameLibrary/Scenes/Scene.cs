using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Input;

namespace MonoGameLibrary.Scenes;

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

    public AudioManager Audio { get; }

    public InputManager Input { get; }

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
    /// Creates a new Scene instance.
    /// </summary>
    /// <param name="game">The Game instance this scene belongs to.</param>
    public Scene(Game game)
    {
        Game = game;
        Content = new ContentManager(game.Services);
        Content.RootDirectory = game.Content.RootDirectory;
        Audio = game.Services.GetService<AudioManager>();
        Input = game.Services.GetService<InputManager>();
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
    public virtual void LoadContent() { }

    /// <summary>
    /// Unloads all content that was loaded by this scene.
    /// </summary>
    public virtual void UnloadContent()
    {
        Content.Unload();
        Content.Dispose();
    }

    /// <summary>
    /// Called before the main Update method.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public virtual void BeforeUpdate(GameTime gameTime) { }

    /// <summary>
    /// Updates this scene's logic.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public virtual void Update(GameTime gameTime) { }

    /// <summary>
    /// Called after the main Update method.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public virtual void AfterUpdate(GameTime gameTime) { }

    /// <summary>
    /// Prepares the scene for rendering.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch instance used for rendering.</param>
    public virtual void BeforeDraw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(ClearColor);
        spriteBatch.Begin(samplerState: SamplerState);
    }

    /// <summary>
    /// Draws this scene.
    /// Override this method to implement scene-specific rendering.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch instance used for rendering.</param>
    public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime) { }

    /// <summary>
    /// Completes the rendering process for this scene.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch instance used for rendering.</param>
    public virtual void AfterDraw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.End();
    }
}
