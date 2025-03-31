using DungeonSlime.Scenes;
using DungeonSlime.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Core
{
    // The background theme song
    private Song _themeSong;

    // The texture used for the background pattern.
    private Texture2D _backgroundPattern;

    // The destination rectangle for the background pattern to fill.
    private Rectangle _backgroundDestination;

    // The offset to apply when drawing the background pattern so it appears to
    // be scrolling.
    private Vector2 _backgroundOffset;

    // The speed that the background pattern scrolls.
    private float _scrollSpeed = 50.0f;

    public Game1() : base("Dungeon Slime", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();

        // Initialize the offset of the background pattern at zero
        _backgroundOffset = Vector2.Zero;

        // Set the background pattern destination rectangle to fill the entire
        // screen background
        _backgroundDestination = GraphicsDevice.PresentationParameters.Bounds;

        // Start playing the background music
        Audio.PlaySong(_themeSong);

        // Start the game with the title scene.
        ChangeScene(new MenuScene<TitleMenu>());
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        // Load the background theme music
        _themeSong = Content.Load<Song>("audio/theme");

        // Load the background pattern texture.
        _backgroundPattern = Content.Load<Texture2D>("images/background-pattern");
    }

    protected override void Update(GameTime gameTime)
    {
        // Update the offsets for the background pattern wrapping so that it
        // scrolls down and to the right.
        float offset = _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _backgroundOffset.X -= offset;
        _backgroundOffset.Y -= offset;

        // Ensure that the offsets do not go beyond the texture bounds so it is
        // a seamless wrap
        _backgroundOffset.X %= _backgroundPattern.Width;
        _backgroundOffset.Y %= _backgroundPattern.Height;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        // Draw the background pattern first using the PointWrap sampler state.
        SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
        SpriteBatch.Draw(_backgroundPattern, _backgroundDestination, new Rectangle(_backgroundOffset.ToPoint(), _backgroundDestination.Size), Color.White * 0.5f);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
