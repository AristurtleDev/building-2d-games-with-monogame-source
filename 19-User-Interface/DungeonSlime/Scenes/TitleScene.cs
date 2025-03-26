using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;

namespace DungeonSlime.Scenes;

public class TitleScene : Scene
{
    // The font to use to render normal text.
    private SpriteFont _font;

    // The sprite to draw for the stylized title
    private Sprite _titleSprite;

    // The position to draw the title sprite at.
    private Vector2 _titlePos;

    private Sprite _startButtonSprite;

    private AnimatedSprite _startButtonSelectedAnimatedSprite;
    private bool _startButtonSelected;
    private Vector2 _startButtonPos;

    private Sprite _optionsButtonSprite;
    private AnimatedSprite _optionsButtonAnimatedSprite;
    private Vector2 _optionsButtonPos;


    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = true;

        // Get the bounds of the screen for position calculations
        Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        // Precalculate the positions and origins for texts and the slime sprite
        // so we're not calculating it every draw frame.
        _titlePos = new Vector2(
            screenBounds.Width * 0.5f,
            80 + _titleSprite.Height * 0.5f);

        // Center the origin of the title sprite.
        _titleSprite.CenterOrigin();

        // Precalculate the position of the start button
        _startButtonPos = new Vector2(screenBounds.Center.X - _startButtonSelectedAnimatedSprite.Width, screenBounds.Bottom - 100);

        // Center the origin of the start button
        _startButtonSprite.CenterOrigin();
        _startButtonSelectedAnimatedSprite.CenterOrigin();

        // Precalculate the position of the options button
        _optionsButtonPos = new Vector2(screenBounds.Center.X + _optionsButtonAnimatedSprite.Width, screenBounds.Bottom - 100);

        // Center the origin of the options button.
        _optionsButtonSprite.CenterOrigin();
        _optionsButtonAnimatedSprite.CenterOrigin();
    }

    public override void LoadContent()
    {
        // Load the font for the standard txt.
        _font = Core.Content.Load<SpriteFont>("fonts/gameFont");

        // Create a texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        _titleSprite = atlas.CreateSprite("title-card");

        _startButtonSprite = atlas.CreateSprite("start-button-not-selected");
        _startButtonSelectedAnimatedSprite = atlas.CreateAnimatedSprite("start-button-selected-animation");

        _optionsButtonSprite = atlas.CreateSprite("options-button-not-selected");
        _optionsButtonAnimatedSprite = atlas.CreateAnimatedSprite("options-button-selected-animation");
    }

    public override void Update(GameTime gameTime)
    {
        // If the user presses enter, switch to the game scene.
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            Core.ChangeScene(new OptionsScene());
        }

        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Left) || Core.Input.Keyboard.WasKeyJustPressed(Keys.Right))
        {
            _startButtonSelected = !_startButtonSelected;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _titleSprite.Draw(Core.SpriteBatch, _titlePos);

        // Draw the start button
        if (_startButtonSelected)
        {
            _startButtonSelectedAnimatedSprite.Draw(Core.SpriteBatch, _startButtonPos);
            _optionsButtonSprite.Draw(Core.SpriteBatch, _optionsButtonPos);
        }
        else
        {
            _startButtonSprite.Draw(Core.SpriteBatch, _startButtonPos);
            _optionsButtonAnimatedSprite.Draw(Core.SpriteBatch, _optionsButtonPos);
        }

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
}
