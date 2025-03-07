using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;

namespace DungeonSlime.Scenes;

public class TitleScene : Scene
{
    private const string TITLE = "Dungeon Slime";
    private const string PRESS_ENTER = "Press Enter TO Start";

    // Reference to the input manager implementation.
    private IInputManager _input;

    // Reference to the audio manager implementation.
    private IAudioManager _audio;

    // The font to use to render the title text
    private SpriteFont _titleFont;

    // The font to use to render normal text.
    private SpriteFont _standardFont;

    // The position to draw the title text at.
    private Vector2 _titlePos;

    // The origin to set for the title text when drawing it.
    private Vector2 _titleOrigin;

    // The position to draw the press enter text at.
    private Vector2 _pressEnterPos;

    // The origin to set for the press enter text when drawing it.
    private Vector2 _pressEnterOrigin;

    // The slime animation to give the title screen some life.
    private AnimatedSprite _slime;

    //  The position to draw the slime animation at.
    private Vector2 _slimePos;

    public TitleScene(Game game) : base(game)
    {

    }

    public override void Initialize()
    {
        // Retrieve the services used by this scene from the games' service container.
        _input = Game.Services.GetService<IInputManager>();
        _audio = Game.Services.GetService<IAudioManager>();

        // LoadContent is called during base.Initialize().
        base.Initialize();

        // begin playing the background music
        _audio.PlaySong("audio/theme");

        // Precalculate the positions and origins for texts and the slime sprite
        // so we're not calculating it every draw frame.
        _titlePos = new Vector2(
            GraphicsDevice.PresentationParameters.BackBufferWidth * 0.5f,
            100);

        Vector2 titleSize = _titleFont.MeasureString(TITLE);
        _titleOrigin = titleSize * 0.5f;

        _pressEnterPos = new Vector2(
            GraphicsDevice.PresentationParameters.BackBufferWidth * 0.5f,
            GraphicsDevice.PresentationParameters.BackBufferHeight - 100
        );

        Vector2 pressEnterSize = _standardFont.MeasureString(PRESS_ENTER);
        _pressEnterOrigin = pressEnterSize * 0.5f;

        _slimePos = new Vector2(
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        ) * 0.5f;

        _slime.CenterOrigin();
        _slime.Scale = new Vector2(3.0f, 3.0f);

    }

    public override void LoadContent()
    {
        // Load the font for the title text.
        _titleFont = Content.Load<SpriteFont>("fonts/title-font");

        // Load the font for the standard txt.
        _standardFont = Game.Content.Load<SpriteFont>("fonts/standard-font");

        // Create a texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Game.Content, "images/atlas-definition.xml");

        // Create the slime animated sprite from the atlas.
        _slime = atlas.CreateAnimatedSprite("slime-animation");

        // Load the background music
        _audio.AddSong("audio/theme");
    }

    public override void Update(GameTime gameTime)
    {
        // Update the sprite
        _slime.Update(gameTime);

        // If the user presses enter, switch to the game scene.
        if(_input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            SceneManager.ChangeScene(new GameScene(Game));
        }
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        // Begin the sprite batch to prepare for rendering.
        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw that title text
        spriteBatch.DrawString(_titleFont, TITLE, _titlePos, Color.White, 0.0f, _titleOrigin, 1.0f, SpriteEffects.None, 0.0f);

        // Draw the press enter text
        spriteBatch.DrawString(_standardFont, PRESS_ENTER, _pressEnterPos, Color.White, 0.0f, _pressEnterOrigin, 1.0f, SpriteEffects.None, 0.0f);

        // Draw the animated slime
        _slime.Draw(spriteBatch, _slimePos);

        // Always end the sprite batch when finished.
        spriteBatch.End();
    }
}
