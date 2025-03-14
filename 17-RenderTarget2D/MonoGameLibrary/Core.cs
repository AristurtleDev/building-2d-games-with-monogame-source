using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;

namespace MonoGameLibrary;

public class Core : Game
{
    internal static Core s_instance;

    /// <summary>
    /// Gets a reference to the Core instance.
    /// </summary>
    public static Core Instance => s_instance;

    // The scene that is currently active.
    private static Scene s_activeScene;

    // The next scene to switch to, if there is one.
    private static Scene s_nextScene;

    private static SceneTransition s_transitionOut;
    private static SceneTransition s_transitionIn;
    private static SceneTransition s_activeTransition;

    /// <summary>
    /// Gets the graphics device manager to control the presentation of graphics.
    /// </summary>
    public static GraphicsDeviceManager Graphics { get; private set; }

    /// <summary>
    /// Gets the sprite batch used for all 2D rendering.
    /// </summary>
    public static SpriteBatch SpriteBatch { get; private set; }

    /// <summary>
    /// Gets a reference to to the input management system.
    /// </summary>
    public static InputManager Input { get; private set; }

    /// <summary>
    /// Gets or Sets a value that indicates if the game should exit when the esc key on the keyboard is pressed.
    /// </summary>
    public static bool ExitOnEscape { get; set; }

    /// <summary>
    /// Gets a reference to the audio control system.
    /// </summary>
    public static AudioController Audio { get; private set; }

    /// <summary>
    /// Creates a new Core instance.
    /// </summary>
    /// <param name="title">The title to display in the title bar of the game window.</param>
    /// <param name="width">The initial width, in pixels, of the game window.</param>
    /// <param name="height">The initial height, in pixels, of the game window.</param>
    /// <param name="fullScreen">Indicates if the game should start in fullscreen mode.</param>
    public Core(string title, int width, int height, bool fullScreen)
    {
        // Ensure that multiple cores are not created.
        if (s_instance != null)
        {
            throw new InvalidOperationException($"Only a single Core instance can be created");
        }

        // Store reference to engine for global member access.
        s_instance = this;

        // Create a new graphics device manager.
        Graphics = new GraphicsDeviceManager(this);

        // Set the graphics defaults
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = fullScreen;

        // Apply the graphic presentation changes
        Graphics.ApplyChanges();

        // Set the window title
        Window.Title = title;

        // Set the root directory for content
        Content.RootDirectory = "Content";

        // Mouse is visible by default
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Create a new input manager
        Input = new InputManager();

        // Create a new audio controller.
        Audio = new AudioController();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // Create the sprite batch instance.
        SpriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void UnloadContent()
    {
        // Dispose of the audio controller.
        Audio.Dispose();

        base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // Update the input manager.
        Input.Update(gameTime);

        // Update the audio controller.
        Audio.Update();

        if (ExitOnEscape && Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Exit();
        }

        //  If there is a current transition happening, then we need to update
        //  that transition. Otherwise, if there is no current transition, but there
        //  is a next scene to switch to, switch to that scene instead.
        if (s_activeTransition != null && s_activeTransition.IsTransitioning)
        {
            s_activeTransition.Update(gameTime);
        }
        else if (s_activeTransition == null && s_nextScene != null)
        {
            TransitionScene();
        }

        // If there is an active scene, update it.
        if (s_activeScene != null)
        {
            s_activeScene.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // If there is an active scene, draw it.
        if (s_activeScene != null)
        {
            s_activeScene.Draw(gameTime);

            if (s_activeTransition != null && s_activeTransition.IsTransitioning)
            {
                s_activeTransition.Draw(Color.Black);
            }

            //  Prepare the graphics device for the final render.
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();

            //  If we are transitioning, then we render the transition effect; otherwise, we'll render
            //  the current scene.
            if (s_activeTransition != null && s_activeTransition.IsTransitioning)
            {
                SpriteBatch.Draw(texture: s_activeTransition.RenderTarget,
                                 destinationRectangle: s_activeTransition.RenderTarget.Bounds,
                                 sourceRectangle: s_activeTransition.RenderTarget.Bounds,
                                 color: Color.White);
            }
            else if (s_activeScene != null)
            {
                SpriteBatch.Draw(texture: s_activeScene.RenderTarget,
                                 destinationRectangle: s_activeScene.RenderTarget.Bounds,
                                 sourceRectangle: s_activeScene.RenderTarget.Bounds,
                                 color: Color.White);
            }

            SpriteBatch.End();
        }

        base.Draw(gameTime);
    }

    public static void ChangeScene(Scene next)
    {
        // Only set the next scene value if it is not the same
        // instance as the currently active scene.
        if (s_activeScene != next)
        {
            s_nextScene = next;
        }
    }

    public static void ChangeScene(Scene to, SceneTransition transitionOut, SceneTransition transitionIn)
    {
        if (s_activeTransition == null || !s_activeTransition.IsTransitioning)
        {
            if (s_activeScene != to)
            {
                s_nextScene = to;
                s_transitionOut = transitionOut;
                s_transitionIn = transitionIn;

                s_transitionOut.TransitionCompleted += TransitionOutCompleted;
                s_transitionIn.TransitionCompleted += TransitionInCompleted;

                s_activeTransition = s_transitionOut;

                s_activeTransition.Start(s_activeScene.RenderTarget);
            }
        }
    }

    private static void TransitionOutCompleted(object sender, EventArgs e)
    {
        //  Unsubscribe from the event so we don't leave any references.
        s_transitionOut.TransitionCompleted -= TransitionOutCompleted;

        //  Dispose of the instance.
        s_transitionOut.Dispose();
        s_transitionOut = null;

        //  Change the scene.
        TransitionScene();

        //  Set the current transition to the in transition and start it.
        s_activeTransition = s_transitionIn;
        s_activeTransition.Start(s_activeScene.RenderTarget);
    }

    private static void TransitionInCompleted(object sender, EventArgs e)
    {
        //  Unsubscribe from the event so we don't leave any references.
        s_transitionIn.TransitionCompleted -= TransitionInCompleted;

        //  Dispose of the instance.
        s_transitionIn.Dispose();
        s_transitionIn = null;
        s_activeTransition = null;
    }

    private static void TransitionScene()
    {
        // If there is an active scene, dispose of it
        if (s_activeScene != null)
        {
            s_activeScene.Dispose();
        }

        // Force the garbage collector to collect to ensure memory is cleared
        GC.Collect();

        // Change the currently active scene to the new scene
        s_activeScene = s_nextScene;

        // Null out the next scene value so it doesn't trigger a change over and over.
        s_nextScene = null;

        // If the active scene now is not null, initialize it.
        // Remember, just like with Game, the Initialize call also calls the
        // Scene.LoadContent
        if (s_activeScene != null)
        {
            s_activeScene.Initialize();
        }
    }
}
