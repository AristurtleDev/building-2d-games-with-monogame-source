using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Scenes;

public abstract class SceneTransition : IDisposable
{
    public bool IsDisposed { get; private set; }
    public bool IsTransitioning { get; private set; }
    public SceneTransitionKind Kind { get; private set; }
    public TimeSpan TransitionTime { get; private set; }
    public TimeSpan TransitionTimeRemaining { get; private set; }
    public RenderTarget2D SourceTexture { get; private set; }
    public RenderTarget2D RenderTarget { get; private set; }
    public event EventHandler TransitionCompleted;

    public SceneTransition(TimeSpan transitionTime, SceneTransitionKind kind)
    {
        TransitionTimeRemaining = TransitionTime = transitionTime;
        Kind = kind;
        GenerateRenderTarget();
    }

    ~SceneTransition() => Dispose(false);

    public virtual void Start(RenderTarget2D sourceTexture)
    {
        SourceTexture = sourceTexture;
        IsTransitioning = true;
    }

    public virtual void Update(GameTime gameTime)
    {
        TransitionTimeRemaining -= gameTime.ElapsedGameTime;

        if (TransitionTimeRemaining <= TimeSpan.Zero)
        {
            IsTransitioning = false;

            if (TransitionCompleted != null)
            {
                TransitionCompleted(this, EventArgs.Empty);
            }
        }
    }

    public void Draw(Color clearColor)
    {
        BeginRender(clearColor);
        Render();
        EndRender();
    }

    private void BeginRender(Color clearColor)
    {
        //  Prepare the graphics device.
        Core.GraphicsDevice.SetRenderTarget(RenderTarget);
        Core.GraphicsDevice.Viewport = new Viewport(RenderTarget.Bounds);
        Core.GraphicsDevice.Clear(clearColor);

        //  Begin the sprite batch.
        Core.SpriteBatch.Begin(blendState: BlendState.AlphaBlend,
                               samplerState: SamplerState.PointClamp);
    }

    protected virtual void Render() { }

    private void EndRender()
    {
        //  End the sprite batch.
        Core.SpriteBatch.End();

        // Unbind the render target.
        Core.GraphicsDevice.SetRenderTarget(null);
    }

    protected virtual void GenerateRenderTarget()
    {
        // Determine the width of the render target based on the width of the back buffer.
        int width = Core.GraphicsDevice.PresentationParameters.BackBufferWidth;

        // Determine the height of the render target based on the height of the back buffer.
        int height = Core.GraphicsDevice.PresentationParameters.BackBufferHeight;

        //  If the RenderTarget instance has already been created previously but has yet
        //  to be disposed of properly, dispose of the instance before setting a new one.
        if (RenderTarget != null && !RenderTarget.IsDisposed)
        {
            RenderTarget.Dispose();
        }

        // Generate the RenderTarget
        RenderTarget = new RenderTarget2D(Core.GraphicsDevice, width, height);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isDisposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (isDisposing)
        {
            if (RenderTarget != null)
            {
                RenderTarget.Dispose();
                RenderTarget = null;
            }
        }

        IsDisposed = true;
    }
}
