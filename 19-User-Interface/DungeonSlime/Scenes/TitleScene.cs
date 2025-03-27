using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.UI;

namespace DungeonSlime.Scenes;

public class TitleScene : Scene
{
    private UISprite _titleUISprite;
    private UIButton _startUIButton;
    private UIButton _optionsUIButton;

    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = true;

        // Get the bounds of the screen for position calculations
        Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        // Precalculate the position of the title ui sprite so we don't have to
        // calculate it each frame.
        _titleUISprite.Position = new Point(screenBounds.Center.X, 80 + (int)(_titleUISprite.Sprite.Height * 0.5f));

        // Precalculate the position of the start button
        _startUIButton.Position = new Point(screenBounds.Center.X - (int)_startUIButton.Sprite.Width, screenBounds.Bottom - 100);

        // Precalculate the position of the options button
        _optionsUIButton.Position = new Point(screenBounds.Center.X + (int)_optionsUIButton.Sprite.Width, screenBounds.Bottom - 100);

        _startUIButton.CenterOrigin();
        _optionsUIButton.CenterOrigin();

        // Set the start button as the initial selected button
        _startUIButton.IsSelected = true;
    }

    public override void LoadContent()
    {
        // Create a texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        // Get the sprite for the title ui sprite.
        Sprite titleSprite = atlas.CreateSprite("title-card");

        // Center the origin of sprite for the title ui sprite.
        titleSprite.CenterOrigin();

        // Create the title ui sprite.
        _titleUISprite = new UISprite(titleSprite);

        // Get the sprites for the start ui button
        Sprite startButtonNormalSprite = atlas.CreateSprite("start-button");
        AnimatedSprite startButtonSelectedSprite = atlas.CreateAnimatedSprite("start-button-animation");

        // Create the start ui button
        _startUIButton = new UIButton(startButtonNormalSprite, startButtonSelectedSprite);

        // Get the sprites for the options ui button
        Sprite optionsButtonNormalSprite = atlas.CreateSprite("options-button");
        AnimatedSprite optionsButtonSelectedSprite = atlas.CreateAnimatedSprite("options-button-animation");

        // Create the options button
        _optionsUIButton = new UIButton(optionsButtonNormalSprite, optionsButtonSelectedSprite);
    }

    public override void Update(GameTime gameTime)
    {
        _startUIButton.Update(gameTime);
        _optionsUIButton.Update(gameTime);

        if (_startUIButton.IsSelected && Core.Input.Keyboard.WasKeyJustPressed(Keys.Right))
        {
            _startUIButton.IsSelected = false;
            _optionsUIButton.IsSelected = true;
        }
        else if (_optionsUIButton.IsSelected && Core.Input.Keyboard.WasKeyJustPressed(Keys.Left))
        {
            _startUIButton.IsSelected = true;
            _optionsUIButton.IsSelected = false;
        }

        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            if (_startUIButton.IsSelected)
            {
                Core.ChangeScene(new GameScene());
            }
            else if (_optionsUIButton.IsSelected)
            {
                Core.ChangeScene(new OptionsScene());
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the title sprite.
        _titleUISprite.Draw(Core.SpriteBatch);

        // Draw the start button
        _startUIButton.Draw(Core.SpriteBatch);

        // Draw the options button
        _optionsUIButton.Draw(Core.SpriteBatch);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
}
