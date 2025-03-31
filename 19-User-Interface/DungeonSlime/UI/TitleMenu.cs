using DungeonSlime.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.UI;

namespace DungeonSlime.UI;

public class TitleMenu : UIElement
{
    // The UI Sprite used to display the game title.
    private UISprite _titleSprite;

    // The UI button used to start gameplay
    private UIButton _startButton;

    // The UI button used to open the options menu.
    private UIButton _optionsButton;

    // The sound effect to play when a UI action is performed.
    private SoundEffect _uiSoundEffect;

    public TitleMenu()
    {
        Position = Vector2.Zero;
    }

    public void Initialize()
    {
        // Get the bounds of the screen for positioning.
        Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        // Set the position of the title sprite
        _titleSprite.Position = new Vector2(
            screenBounds.Center.X,
            _titleSprite.Sprite.Height * 0.5f + 100.0f
        );

        // Set the position of the start button
        _startButton.Position = new Vector2(
            screenBounds.Center.X - (int)_startButton.Sprite.Width,
            screenBounds.Bottom - 100.0f
        );

        // Set the position of the options button
        _optionsButton.Position = new Vector2(
            screenBounds.Center.X + _optionsButton.Sprite.Width,
            screenBounds.Bottom - 100.0f
        );

        // Set the start button to selected by default
        _startButton.IsSelected = true;
        _optionsButton.IsSelected = false;

        // Add all ui elements as children of this menu
        AddChild(_titleSprite);
        AddChild(_startButton);
        AddChild(_optionsButton);
    }

    public void LoadContent()
    {
        // Load the ui texture atlas from the XML configuration file
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "ui-atlas-definition.xml");

        // Create the sprite for the game title
        Sprite titleSprite = atlas.CreateSprite("title");
        _titleSprite = new UISprite(titleSprite);
        _titleSprite.CenterOrigin();

        // Create the start button
        Sprite startButtonSprite = atlas.CreateSprite("start-button");
        AnimatedSprite startButtonSelectedSprite = atlas.CreateAnimatedSprite("start-button-selected");
        _startButton = new UIButton(startButtonSprite, startButtonSelectedSprite);
        _startButton.CenterOrigin();

        // Create the options button
        Sprite optionsButtonSprite = atlas.CreateSprite("options-button");
        AnimatedSprite optionsButtonSelectedSprite = atlas.CreateAnimatedSprite("options-button-selected");
        _optionsButton = new UIButton(optionsButtonSprite, optionsButtonSelectedSprite);
        _optionsButton.CenterOrigin();

        // Load the ui sound effect.
        _uiSoundEffect = Core.Content.Load<SoundEffect>("audio/menu");
    }

    public override void Update(GameTime gameTime)
    {
        if(_startButton.IsSelected)
        {
            if(InputProfile.MenuRight())
            {
                Core.Audio.PlaySoundEffect(_uiSoundEffect);
                _startButton.IsSelected = false;
                _optionsButton.IsSelected = true;
            }
            else if(InputProfile.MenuAccept())
            {
                Core.Audio.PlaySoundEffect(_uiSoundEffect);
                // Core.ChangeScene(new GameSelectScene());
            }
        }
        else if(_optionsButton.IsSelected)
        {
            if(InputProfile.MenuLeft())
            {
                Core.Audio.PlaySoundEffect(_uiSoundEffect);
                _startButton.IsSelected = true;
                _optionsButton.IsSelected = false;
            }
            else if(InputProfile.MenuAccept())
            {
                Core.Audio.PlaySoundEffect(_uiSoundEffect);
                Core.ChangeScene(new OptionsScene());
            }
        }

        base.Update(gameTime);
    }
}
