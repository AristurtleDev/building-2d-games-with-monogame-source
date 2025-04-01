using System;
using DungeonSlime.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.UI;

namespace DungeonSlime.UI;

public class TitleMenu : UIElement
{
    // The UI button used to start gameplay
    private UIButton _startButton;

    // The UI button used to open the options menu.
    private UIButton _optionsButton;

    // The sound effect to play when a UI action is performed.
    private SoundEffect _uiSoundEffect;

    /// <summary>
    /// Raised when the start button is clicked.
    /// </summary>
    public event EventHandler<EventArgs> StartClicked;

    /// <summary>
    /// Raised when the options button is clicked.
    /// </summary>
    public event EventHandler<EventArgs> OptionsClicked;

    public TitleMenu() { }

    private void LoadContent(ContentManager content)
    {
        // Load the ui texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(content, "images/ui-atlas-definition.xml");

        // Create the game title sprite as a child of this menu.
        UISprite titleSprite = CreateChild<UISprite>();
        titleSprite.Sprite = atlas.CreateSprite("title");
        titleSprite.Sprite.CenterOrigin();
        titleSprite.Position = new Vector2(640, 220);

        // Create the start button as a child of this menu.
        _startButton = CreateChild<UIButton>();
        _startButton.NotSelectedSprite = atlas.CreateSprite("start-button");
        _startButton.NotSelectedSprite.CenterOrigin();
        _startButton.SelectedSprite = atlas.CreateAnimatedSprite("start-button-selected");
        _startButton.SelectedSprite.CenterOrigin();
        _startButton.Position = new Vector2(432, 670);


        // Create the options button as a child of this menu.
        _optionsButton = CreateChild<UIButton>();
        _optionsButton.NotSelectedSprite = atlas.CreateSprite("options-button");
        _optionsButton.NotSelectedSprite.CenterOrigin();
        _optionsButton.SelectedSprite = atlas.CreateAnimatedSprite("options-button-selected");
        _optionsButton.SelectedSprite.CenterOrigin();
        _optionsButton.Position = new Vector2(848, 670);

        // Start button is enabled by default.
        _startButton.IsSelected = true;
        _optionsButton.IsSelected = false;

        // Set the disabled color for this menu. This will propagate the value
        // down through all children.
        DisabledColor = new Color(70, 86, 130, 255);

        // Load the sound effect to play when ui actions occur.
        _uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");
    }


    public override void Update(GameTime gameTime)
    {
        if (_startButton.IsSelected)
        {
            UpdateStartButton();
        }
        else if (_optionsButton.IsSelected)
        {
            UpdateOptionsButton();
        }

        base.Update(gameTime);
    }

    private void UpdateStartButton()
    {
        if (InputProfile.MenuRight())
        {
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
            _startButton.IsSelected = false;
            _optionsButton.IsSelected = true;
        }
        else if (InputProfile.MenuAccept())
        {
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
            Core.ChangeScene(new MenuScene<GameSelectMenu>());
        }
    }

    private void UpdateOptionsButton()
    {
        if (InputProfile.MenuLeft())
        {
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
            _startButton.IsSelected = true;
            _optionsButton.IsSelected = false;
        }
        else if (InputProfile.MenuAccept())
        {
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
            Core.ChangeScene(new MenuScene<OptionsMenu>());
        }
    }
}
