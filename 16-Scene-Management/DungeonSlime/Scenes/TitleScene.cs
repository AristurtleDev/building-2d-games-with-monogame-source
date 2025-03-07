using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;

namespace DungeonSlime.Scenes;

public class TitleScene : Scene
{
    private const string TITLE = "Dungeon Slime";

    private IInputManager _input;
    private IAudioManager _audio;
    private SpriteFont _titleFont;
    private Vector2 _titlePos;
    private Vector2 _titleOrigin;

    public TitleScene(Game game) : base(game)
    {

    }

    public override void Initialize()
    {
        _input = Game.Services.GetService<IInputManager>();
        _audio = Game.Services.GetService<IAudioManager>();

        base.Initialize();

        // Pre calculate the position of the title text so we're not doing
        // it every draw frame
        _titlePos = new Vector2(
            GraphicsDevice.PresentationParameters.BackBufferWidth * 0.5f,
            100);

        // Precalculate the center origin of the title tet so we're not doing
        // it every draw frame
        Vector2 titleSize = _titleFont.MeasureString(TITLE);
        _titleOrigin = titleSize * 0.5f;
    }

    public override void LoadContent()
    {
        _titleFont = Content.Load<SpriteFont>("fonts/title-font");
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.DrawString(_titleFont, TITLE, _titlePos, Color.White, 0.0f, _titleOrigin, 1.0f, SpriteEffects.None, 0.0f);
        spriteBatch.End();
    }
}
