using DungeonSlime.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Core
{
    // The background theme song
    private Song _themeSong;

    // Repeatable background
    private Texture2D _backgroundPattern;
    private Rectangle _backgroundDestination;
    private Vector2 _backgroundOffset;
    private float _scrollSpeed = 50.0f;

    public Game1() : base("Dungeon Slime", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();

        _backgroundOffset = Vector2.Zero;
        _backgroundDestination = GraphicsDevice.PresentationParameters.Bounds;

        // Start playing the background music
        Audio.PlaySong(_themeSong);

        // Start the game with the title scene.
        ChangeScene(new TitleScene());
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        // Load the background theme music
        _themeSong = Content.Load<Song>("audio/theme");

        // Load the background pattern
        _backgroundPattern = Content.Load<Texture2D>("images/background");
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Update the offsets for the background pattern wrapping
        _backgroundOffset.X += _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _backgroundOffset.Y -= _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Ensure that the offset doesn't go beyond the texture bounds so it's a seamless wrap
        _backgroundOffset.X %= _backgroundPattern.Width;
        _backgroundOffset.Y %= _backgroundPattern.Height;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        SpriteBatch.Begin(samplerState: SamplerState.PointWrap, blendState: BlendState.AlphaBlend);
        SpriteBatch.Draw(_backgroundPattern, _backgroundDestination, new Rectangle(_backgroundOffset.ToPoint(), _backgroundDestination.Size), Color.White);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
