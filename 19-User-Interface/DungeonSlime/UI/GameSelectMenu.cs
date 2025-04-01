using DungeonSlime.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.UI;

namespace DungeonSlime.UI;

public class GameSelectMenu : UIElement
{
    private GameOptions _options;

    private UISprite _speedPanel;
    private UISprite _modePanel;
    private UIButton _slowSpeedButton;
    private UIButton _normalSpeedButton;
    private UIButton _fastSpeedButton;
    private UIButton _normalModeButton;
    private UIButton _darkModeButton;
    private UIButton _acceptButton;
    private UIButton _cancelButton;

    // The sound effect to play when a UI action is performed.
    private SoundEffect _uiSoundEffect;

    public GameSelectMenu()
    {
        _options = new GameOptions();
        CreateChildren();
    }

    private void CreateChildren()
    {
        // Load the ui texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/ui-atlas-definition.xml");

        // Create the select label as a child of this menu.
        UISprite selectLabel = CreateChild<UISprite>();
        selectLabel.Sprite = atlas.CreateSprite("select-label");
        selectLabel.Position = new Vector2(112, 20);

        // Create the enter label as a child of this menu.
        UISprite enterLabel = CreateChild<UISprite>();
        enterLabel.Sprite = atlas.CreateSprite("enter-label");
        enterLabel.Position = new Vector2(640, 52);

        // Create the escape label as a child of this menu.
        UISprite escapeLabel = CreateChild<UISprite>();
        escapeLabel.Sprite = atlas.CreateSprite("escape-label");
        escapeLabel.Position = new Vector2(804, 52);

        // Create the speed panel as a child of this menu.
        _speedPanel = CreateChild<UISprite>();
        _speedPanel.Sprite = atlas.CreateSprite("panel");
        _speedPanel.Position = new Vector2(198, 139);

        // Create the mode panel as a child of this menu.
        _modePanel = CreateChild<UISprite>();
        _modePanel.Sprite = atlas.CreateSprite("panel");
        _modePanel.Position = new Vector2(198, 406);

        // Create the accept button as a child of this menu
        _acceptButton = CreateChild<UIButton>();
        _acceptButton.NotSelectedSprite = atlas.CreateSprite("accept-button");
        _acceptButton.NotSelectedSprite.CenterOrigin();
        _acceptButton.SelectedSprite = atlas.CreateAnimatedSprite("accept-button-selected");
        _acceptButton.SelectedSprite.CenterOrigin();
        _acceptButton.Position = new Vector2(432, 670);

        // Create the cancel button as a child of this menu
        _cancelButton = CreateChild<UIButton>();
        _cancelButton.NotSelectedSprite = atlas.CreateSprite("cancel-button");
        _cancelButton.NotSelectedSprite.CenterOrigin();
        _cancelButton.SelectedSprite = atlas.CreateAnimatedSprite("cancel-button-selected");
        _cancelButton.SelectedSprite.CenterOrigin();
        _cancelButton.Position = new Vector2(848, 670);

        // Create the speed panel label as a child of the speed panel.
        UISprite speedLabel = _speedPanel.CreateChild<UISprite>();
        speedLabel.Sprite = atlas.CreateSprite("speed-label");
        speedLabel.Position = new Vector2(42, 42);

        // Create the slow speed button as a child of the speed panel.
        _slowSpeedButton = _speedPanel.CreateChild<UIButton>();
        _slowSpeedButton.NotSelectedSprite = atlas.CreateSprite("slow-button");
        _slowSpeedButton.NotSelectedSprite.CenterOrigin();
        _slowSpeedButton.SelectedSprite = atlas.CreateAnimatedSprite("slow-button-selected");
        _slowSpeedButton.SelectedSprite.CenterOrigin();
        _slowSpeedButton.Position = new Vector2(148, 148);

        // Create the normal speed button as a child of the speed panel.
        _normalSpeedButton = _speedPanel.CreateChild<UIButton>();
        _normalSpeedButton.NotSelectedSprite = atlas.CreateSprite("normal-button");
        _normalSpeedButton.NotSelectedSprite.CenterOrigin();
        _normalSpeedButton.SelectedSprite = atlas.CreateAnimatedSprite("normal-button-selected");
        _normalSpeedButton.SelectedSprite.CenterOrigin();
        _normalSpeedButton.Position = new Vector2(420, 148);

        // Create the fast speed button as a child of the speed panel.
        _fastSpeedButton = _speedPanel.CreateChild<UIButton>();
        _fastSpeedButton.NotSelectedSprite = atlas.CreateSprite("fast-button");
        _fastSpeedButton.NotSelectedSprite.CenterOrigin();
        _fastSpeedButton.SelectedSprite = atlas.CreateAnimatedSprite("fast-button-selected");
        _fastSpeedButton.SelectedSprite.CenterOrigin();
        _fastSpeedButton.Position = new Vector2(691, 148);


        // Create the mode panel label as a child of the mode panel.
        UISprite modeLabel = _modePanel.CreateChild<UISprite>();
        modeLabel.Sprite = atlas.CreateSprite("mode-label");
        modeLabel.Position = new Vector2(42, 42);

        // Create the normal mode button as a child of the mode panel.
        _normalModeButton = _modePanel.CreateChild<UIButton>();
        _normalModeButton.NotSelectedSprite = atlas.CreateSprite("normal-button");
        _normalModeButton.NotSelectedSprite.CenterOrigin();
        _normalModeButton.SelectedSprite = atlas.CreateAnimatedSprite("normal-button-selected");
        _normalModeButton.SelectedSprite.CenterOrigin();
        _normalModeButton.Position = new Vector2(148, 148);

        // Create the dark mode button as a child of the mode panel.
        _darkModeButton = _modePanel.CreateChild<UIButton>();
        _darkModeButton.NotSelectedSprite = atlas.CreateSprite("dark-button");
        _darkModeButton.NotSelectedSprite.CenterOrigin();
        _darkModeButton.SelectedSprite = atlas.CreateAnimatedSprite("dark-button-selected");
        _darkModeButton.SelectedSprite.CenterOrigin();
        _darkModeButton.Position = new Vector2(691, 148);

        // Speed panel is default selected
        _speedPanel.IsEnabled = true;
        _modePanel.IsEnabled = false;
        _acceptButton.IsSelected = false;
        _cancelButton.IsSelected = false;

        // Set the disabled color for this menu. This will propagate the value
        // down through all children.
        DisabledColor = new Color(70, 86, 130, 255);

        // Load the sound effect to play when ui actions occur.
        _uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");
    }

    public override void Update(GameTime gameTime)
    {
        if (_speedPanel.IsEnabled)
        {
            UpdateSpeedPanel();
        }
        else if (_modePanel.IsEnabled)
        {
            UpdateModePanel();
        }
        else if (_acceptButton.IsSelected)
        {
            UpdateAcceptButton(gameTime);
        }
        else if (_cancelButton.IsSelected)
        {
            UpdateCancelButton(gameTime);
        }

        UpdateButtonStates();

        base.Update(gameTime);
    }

    private void UpdateSpeedPanel()
    {
        if (InputProfile.MenuDown() || InputProfile.MenuAccept())
        {
            _speedPanel.IsEnabled = false;
            _modePanel.IsEnabled = true;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuLeft())
        {
            if (_options.Speed == GameOptions.SlimeSpeed.Normal)
            {
                _options.Speed = GameOptions.SlimeSpeed.Slow;
            }
            else if (_options.Speed == GameOptions.SlimeSpeed.Fast)
            {
                _options.Speed = GameOptions.SlimeSpeed.Normal;
            }

            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuRight())
        {
            if (_options.Speed == GameOptions.SlimeSpeed.Slow)
            {
                _options.Speed = GameOptions.SlimeSpeed.Normal;
            }
            else if (_options.Speed == GameOptions.SlimeSpeed.Normal)
            {
                _options.Speed = GameOptions.SlimeSpeed.Fast;
            }
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
    }

    private void UpdateModePanel()
    {
        if (InputProfile.MenuUp() || InputProfile.MenuCancel())
        {
            _speedPanel.IsEnabled = true;
            _modePanel.IsEnabled = false;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuDown() || InputProfile.MenuAccept())
        {
            _modePanel.IsEnabled = false;
            _acceptButton.IsSelected = true;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuLeft())
        {
            if (_options.Mode == GameOptions.GameMode.Dark)
            {
                _options.Mode = GameOptions.GameMode.Normal;
            }

            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuRight())
        {
            if (_options.Mode == GameOptions.GameMode.Normal)
            {
                _options.Mode = GameOptions.GameMode.Dark;
            }

            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
    }

    private void UpdateAcceptButton(GameTime gameTime)
    {
        if (InputProfile.MenuUp() || InputProfile.MenuCancel())
        {
            _acceptButton.IsSelected = false;
            _modePanel.IsEnabled = true;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuRight())
        {
            _acceptButton.IsSelected = false;
            _cancelButton.IsSelected = true;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuAccept())
        {
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
            Core.ChangeScene(new GameScene(_options));
        }
    }

    private void UpdateCancelButton(GameTime gameTime)
    {
        if (InputProfile.MenuUp() || InputProfile.MenuCancel())
        {
            _cancelButton.IsSelected = false;
            _modePanel.IsEnabled = true;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuLeft())
        {
            _cancelButton.IsSelected = false;
            _acceptButton.IsSelected = true;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuAccept())
        {
            Core.ChangeScene(new MenuScene<TitleMenu>());
        }
    }

    private void UpdateButtonStates()
    {
        _slowSpeedButton.IsSelected = _options.Speed == GameOptions.SlimeSpeed.Slow;
        _normalSpeedButton.IsSelected = _options.Speed == GameOptions.SlimeSpeed.Normal;
        _fastSpeedButton.IsSelected = _options.Speed == GameOptions.SlimeSpeed.Fast;
        _normalModeButton.IsSelected = _options.Mode == GameOptions.GameMode.Normal;
        _darkModeButton.IsSelected = _options.Mode == GameOptions.GameMode.Dark;
    }
}
