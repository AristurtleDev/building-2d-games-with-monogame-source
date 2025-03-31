using DungeonSlime.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.UI;

namespace DungeonSlime.UI;

public class OptionsMenu : UIElement
{
    private float _previousSongVolume;
    private float _previousSoundEffectVolume;

    private UISprite _musicPanel;
    private UISprite _soundEffectPanel;
    private UISlider<float> _musicVolumeSlider;
    private UISlider<float> _soundEffectVolumeSlider;
    private UIButton _acceptButton;
    private UIButton _cancelButton;

    // The sound effect to play when a UI action is performed.
    private SoundEffect _uiSoundEffect;

    public OptionsMenu()
    {
        _previousSongVolume = Core.Audio.SongVolume;
        _previousSoundEffectVolume = Core.Audio.SoundEffectVolume;
        CreateChildren();
    }

    private void CreateChildren()
    {
        // Load the ui texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/ui-atlas-definition.xml");

        // Create the options label as a child of this menu.
        UISprite optionsLabel = AddChild<UISprite>();
        optionsLabel.Sprite = atlas.CreateSprite("options-label");
        optionsLabel.Position = new Vector2(112, 20);

        // Create the enter label as a child of this menu.
        UISprite enterLabel = AddChild<UISprite>();
        enterLabel.Sprite = atlas.CreateSprite("enter-label");
        enterLabel.Position = new Vector2(640, 52);

        // Create the escape label as a child of this menu.
        UISprite escapeLabel = AddChild<UISprite>();
        escapeLabel.Sprite = atlas.CreateSprite("escape-label");
        escapeLabel.Position = new Vector2(804, 52);

        // Create the music panel as a child of this menu.
        _musicPanel = AddChild<UISprite>();
        _musicPanel.Sprite = atlas.CreateSprite("panel");
        _musicPanel.Position = new Vector2(198, 139);

        // Create the sound effect panel as a child of this menu.
        _soundEffectPanel = AddChild<UISprite>();
        _soundEffectPanel.Sprite = atlas.CreateSprite("panel");
        _soundEffectPanel.Position = new Vector2(198, 406);

        // Create the accept button as a child of this menu
        _acceptButton = AddChild<UIButton>();
        _acceptButton.NotSelectedSprite = atlas.CreateSprite("accept-button");
        _acceptButton.NotSelectedSprite.CenterOrigin();
        _acceptButton.SelectedSprite = atlas.CreateAnimatedSprite("accept-button-selected");
        _acceptButton.SelectedSprite.CenterOrigin();
        _acceptButton.Position = new Vector2(432, 670);

        // Create the cancel button as a child of this menu
        _cancelButton = AddChild<UIButton>();
        _cancelButton.NotSelectedSprite = atlas.CreateSprite("cancel-button");
        _cancelButton.NotSelectedSprite.CenterOrigin();
        _cancelButton.SelectedSprite = atlas.CreateAnimatedSprite("cancel-button-selected");
        _cancelButton.SelectedSprite.CenterOrigin();
        _cancelButton.Position = new Vector2(848, 670);

        // Create the music volume panel label as a child of the music panel.
        UISprite musicLabel = _musicPanel.AddChild<UISprite>();
        musicLabel.Sprite = atlas.CreateSprite("music-label");
        musicLabel.Position = new Vector2(42, 42);

        // Create the music volume slider as a child of the music panel.
        _musicVolumeSlider = _musicPanel.AddChild<UISlider<float>>();
        _musicVolumeSlider.SliderSprite = atlas.CreateSprite("slider");
        _musicVolumeSlider.FillSprite = atlas.CreateSprite("slider-fill");
        _musicVolumeSlider.FillBounds = new Rectangle(108, 4, 566, 36);
        _musicVolumeSlider.Value = Core.Audio.SongVolume;
        _musicVolumeSlider.MinValue = 0.0f;
        _musicVolumeSlider.MaxValue = 1.0f;
        _musicVolumeSlider.Step = 0.1f;
        _musicVolumeSlider.Position = new Vector2(27, 117);

        // Create the sound effect volume panel label as a child of the music panel.
        UISprite soundLabel = _soundEffectPanel.AddChild<UISprite>();
        soundLabel.Sprite = atlas.CreateSprite("sound-label");
        soundLabel.Position = new Vector2(42, 42);

        // Create the sound effect volume slider as a child of the sound effect panel.
        _soundEffectVolumeSlider = _soundEffectPanel.AddChild<UISlider<float>>();
        _soundEffectVolumeSlider.SliderSprite = atlas.CreateSprite("slider");
        _soundEffectVolumeSlider.FillSprite = atlas.CreateSprite("slider-fill");
        _soundEffectVolumeSlider.FillBounds = new Rectangle(108, 4, 566, 36);
        _soundEffectVolumeSlider.Value = Core.Audio.SoundEffectVolume;
        _soundEffectVolumeSlider.MinValue = 0.0f;
        _soundEffectVolumeSlider.MaxValue = 1.0f;
        _soundEffectVolumeSlider.Step = 0.1f;
        _soundEffectVolumeSlider.Position = new Vector2(27, 117);


        // Music panel is default selected
        _musicPanel.IsEnabled = true;
        _soundEffectPanel.IsEnabled = false;
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
        if (_musicPanel.IsEnabled)
        {
            UpdateMusicPanel();
        }
        else if (_soundEffectPanel.IsEnabled)
        {
            UpdateSFXPanel();
        }
        else if (_acceptButton.IsSelected)
        {
            UpdateAcceptButton(gameTime);
        }
        else if (_cancelButton.IsSelected)
        {
            UpdateCancelButton(gameTime);
        }

        base.Update(gameTime);
    }

    private void UpdateMusicPanel()
    {
        if (InputProfile.MenuDown() || InputProfile.MenuAccept())
        {
            _musicPanel.IsEnabled = false;
            _soundEffectPanel.IsEnabled = true;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuLeft())
        {
            _musicVolumeSlider.StepDown();
            Core.Audio.SongVolume = _musicVolumeSlider.Value;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuRight())
        {
            _musicVolumeSlider.StepUp();
            Core.Audio.SongVolume = _musicVolumeSlider.Value;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
    }

    private void UpdateSFXPanel()
    {
        if (InputProfile.MenuUp() || InputProfile.MenuCancel())
        {
            _musicPanel.IsEnabled = true;
            _soundEffectPanel.IsEnabled = false;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuDown() || InputProfile.MenuAccept())
        {
            _soundEffectPanel.IsEnabled = false;
            _acceptButton.IsSelected = true;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuLeft())
        {
            _soundEffectVolumeSlider.StepDown();
            Core.Audio.SoundEffectVolume = _soundEffectVolumeSlider.Value;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
        else if (InputProfile.MenuRight())
        {
            _soundEffectVolumeSlider.StepUp();
            Core.Audio.SoundEffectVolume = _soundEffectVolumeSlider.Value;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
        }
    }

    private void UpdateAcceptButton(GameTime gameTime)
    {
        if (InputProfile.MenuUp() || InputProfile.MenuCancel())
        {
            _acceptButton.IsSelected = false;
            _soundEffectPanel.IsEnabled = true;
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
            Core.ChangeScene(new MenuScene<TitleMenu>());
        }
    }

    private void UpdateCancelButton(GameTime gameTime)
    {
        if (InputProfile.MenuUp() || InputProfile.MenuCancel())
        {
            _cancelButton.IsSelected = false;
            _soundEffectPanel.IsEnabled = true;
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
            Core.Audio.SongVolume = _previousSongVolume;
            Core.Audio.SoundEffectVolume = _previousSoundEffectVolume;
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
            Core.ChangeScene(new MenuScene<TitleMenu>());
        }
    }
}
