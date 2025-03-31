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
    private UISlider<float> _soundVolumeSlider;
    private UIButton _acceptButton;
    private UIButton _cancelButton;

    // The color to set UI elements when they are enabled.
    private Color _enabledColor = Color.White;

    // The color to set UI elements when they are disabled.
    private Color _disabledColor = new Color(70, 86, 130, 255);

    private SoundEffect _soundEffect;

    public OptionsMenu(TextureAtlas atlas, SoundEffect soundEffect)
    {
        _soundEffect = soundEffect;
        _previousSongVolume = Core.Audio.SongVolume;
        _previousSoundEffectVolume = Core.Audio.SoundEffectVolume;
        CreateChildren(atlas);
    }

    private void CreateChildren(TextureAtlas atlas)
    {
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
        UISprite musicPanel = AddChild<UISprite>();
        musicPanel.Sprite = atlas.CreateSprite("panel");
        musicPanel.Position = new Vector2(198, 139);

        // Create the sound effect panel as a child of this menu.
        UISprite soundEffectPanel = AddChild<UISprite>();
        soundEffectPanel.Sprite = atlas.CreateSprite("panel");
        soundEffectPanel.Position = new Vector2(198, 406);

        // Create the accept button as a child of this menu
        UIButton acceptButton = AddChild<UIButton>();
        acceptButton.NotSelectedSprite = atlas.CreateSprite("accept-button");
        acceptButton.SelectedSprite = atlas.CreateAnimatedSprite("accept-button-selected");
        acceptButton.Position = new Vector2(432, 670);

        // Create the cancel button as a child of this menu
        UIButton cancelButton = AddChild<UIButton>();
        cancelButton.NotSelectedSprite = atlas.CreateSprite("cancel-button");
        cancelButton.SelectedSprite = atlas.CreateAnimatedSprite("cancel-button-selected");
        cancelButton.Position = new Vector2(848, 670);

        // Create the music volume panel label as a child of the music panel.
        UISprite musicLabel = musicPanel.AddChild<UISprite>();
        musicLabel.Sprite = atlas.CreateSprite("music-label");
        musicLabel.Position = new Vector2(42, 42);

        // Create the music volume slider as a child of the music panel.
        UISlider<float> musicVolumeSlider = musicPanel.AddChild<UISlider<float>>();
        musicVolumeSlider.SliderSprite = atlas.CreateSprite("slider");
        musicVolumeSlider.FillSprite = atlas.CreateSprite("slider-fill");
        musicVolumeSlider.FillBounds = new Rectangle(108, 4, 566, 36);
        musicVolumeSlider.Value = Core.Audio.SongVolume;
        musicVolumeSlider.MinValue = 0.0f;
        musicVolumeSlider.MaxValue = 1.0f;
        musicVolumeSlider.Step = 0.1f;

        // Create the sound effect volume panel label as a child of the music panel.
        UISprite soundLabel = musicPanel.AddChild<UISprite>();
        soundLabel.Sprite = atlas.CreateSprite("sound-label");
        soundLabel.Position = new Vector2(42, 42);

        // Create the sound effect volume slider as a child of the sound effect panel.
        UISlider<float> soundEffectVolumeSlider = soundEffectPanel.AddChild<UISlider<float>>();
        soundEffectVolumeSlider.SliderSprite = atlas.CreateSprite("slider");
        soundEffectVolumeSlider.FillSprite = atlas.CreateSprite("slider-fill");
        soundEffectVolumeSlider.FillBounds = new Rectangle(108, 4, 566, 36);
        soundEffectVolumeSlider.Value = Core.Audio.SoundEffectVolume;
        soundEffectVolumeSlider.MinValue = 0.0f;
        soundEffectVolumeSlider.MaxValue = 1.0f;
        soundEffectVolumeSlider.Step = 0.1f;

        this._enabledColor // this is borked.

        // Music panel is default selected
        _musicPanel.Enabled = true;
        _soundEffectPanel.Enabled = false;
        _acceptButton.IsSelected = false;
        _cancelButton.IsSelected = false;
    }

    public void Update(GameTime gameTime)
    {
        if (_musicPanel.Enabled)
        {
            UpdateMusicPanel();
        }
        else if (_soundEffectPanel.Enabled)
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
    }

    private void UpdateMusicPanel()
    {
        if (InputProfile.MenuDown() || InputProfile.MenuAccept())
        {
            _musicPanel.Enabled = false;
            _soundEffectPanel.Enabled = true;
            Core.Audio.PlaySoundEffect(_soundEffect);
        }
        else if (InputProfile.MenuLeft())
        {
            _musicVolumeSlider.StepDown();
            Core.Audio.SongVolume = _musicVolumeSlider.Value;
            Core.Audio.PlaySoundEffect(_soundEffect);
        }
        else if (InputProfile.MenuRight())
        {
            _musicVolumeSlider.StepUp();
            Core.Audio.SongVolume = _musicVolumeSlider.Value;
            Core.Audio.PlaySoundEffect(_soundEffect);
        }
    }

    private void UpdateSFXPanel()
    {
        if (InputProfile.MenuUp() || InputProfile.MenuCancel())
        {
            _musicPanel.Enabled = true;
            _soundEffectPanel.Enabled = false;
            Core.Audio.PlaySoundEffect(_soundEffect);
        }
        else if (InputProfile.MenuDown() || InputProfile.MenuAccept())
        {
            _soundEffectPanel.Enabled = false;
            _acceptButton.IsSelected = true;
            Core.Audio.PlaySoundEffect(_soundEffect);
        }
        else if (InputProfile.MenuLeft())
        {
            _soundVolumeSlider.StepDown();
            Core.Audio.SoundEffectVolume = _soundVolumeSlider.Value;
            Core.Audio.PlaySoundEffect(_soundEffect);
        }
        else if (InputProfile.MenuRight())
        {
            _soundVolumeSlider.StepUp();
            Core.Audio.SoundEffectVolume = _soundVolumeSlider.Value;
            Core.Audio.PlaySoundEffect(_soundEffect);
        }
    }

    private void UpdateAcceptButton(GameTime gameTime)
    {
        if (InputProfile.MenuUp() || InputProfile.MenuCancel())
        {
            _acceptButton.IsSelected = false;
            _soundEffectPanel.Enabled = true;
            Core.Audio.PlaySoundEffect(_soundEffect);
        }
        else if (InputProfile.MenuRight())
        {
            _acceptButton.IsSelected = false;
            _cancelButton.IsSelected = true;
            Core.Audio.PlaySoundEffect(_soundEffect);
        }
        else if (InputProfile.MenuAccept())
        {
            Core.Audio.PlaySoundEffect(_soundEffect);
            Core.ChangeScene(new TitleScene());
        }
    }

    private void UpdateCancelButton(GameTime gameTime)
    {
        if (InputProfile.MenuUp() || InputProfile.MenuCancel())
        {
            _cancelButton.IsSelected = false;
            _soundEffectPanel.Enabled = true;
            Core.Audio.PlaySoundEffect(_soundEffect);
        }
        else if (InputProfile.MenuLeft())
        {
            _cancelButton.IsSelected = false;
            _acceptButton.IsSelected = true;
            Core.Audio.PlaySoundEffect(_soundEffect);
        }
        else if (InputProfile.MenuAccept())
        {
            Core.Audio.SongVolume = _previousSongVolume;
            Core.Audio.SoundEffectVolume = _previousSoundEffectVolume;
            Core.Audio.PlaySoundEffect(_soundEffect);
            Core.ChangeScene(new TitleScene());
        }
    }
}
