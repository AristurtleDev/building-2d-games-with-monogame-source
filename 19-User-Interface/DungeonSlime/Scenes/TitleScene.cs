using DungeonSlime.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.UI;

namespace DungeonSlime.Scenes;

public class TitleScene : Scene
{
    private UIElement _titleMenu;
    private UIElement _optionsMenu;

    public TitleScene() : base() { }

    public override void LoadContent()
    {
        // Load the ui texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/ui-atlas-definition.xml");

        // Load the sound effect to play when ui actions occur.
        SoundEffect uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");

        // Create the UI controller
        UIElementController controller = new UIElementController();

        // Create the title menu
        CreateTileMenu(atlas, uiSoundEffect, controller);

        // Create the options menu
        CreateOptionsMenu(atlas, uiSoundEffect, controller);
    }

    private void CreateTileMenu(TextureAtlas atlas, SoundEffect soundEffect, UIElementController controller)
    {
        // Create the title menu
        _titleMenu = new UISprite();
        _titleMenu.Position = Vector2.Zero;
        _titleMenu.Controller = controller;
        _titleMenu.IsSelected = true;

        UISprite titleSprite = _titleMenu.CreateChild<UISprite>();
        titleSprite.Sprite = atlas.CreateSprite("title");
        titleSprite.Sprite.CenterOrigin();
        titleSprite.Position = new Vector2(640, 220);

        UIButton startButton = _titleMenu.CreateChild<UIButton>();
        startButton.NotSelectedSprite = atlas.CreateSprite("start-button");
        startButton.NotSelectedSprite.CenterOrigin();
        startButton.SelectedSprite = atlas.CreateAnimatedSprite("start-button-selected");
        startButton.SelectedSprite.CenterOrigin();
        startButton.Position = new Vector2(432, 670);
        startButton.IsSelected = true;
        startButton.Controller = controller;

        UIButton optionsButton = _titleMenu.CreateChild<UIButton>();
        optionsButton.NotSelectedSprite = atlas.CreateSprite("options-button");
        optionsButton.NotSelectedSprite.CenterOrigin();
        optionsButton.SelectedSprite = atlas.CreateAnimatedSprite("options-button-selected");
        optionsButton.SelectedSprite.CenterOrigin();
        optionsButton.Position = new Vector2(848, 670);
        optionsButton.Controller = controller;

        _titleMenu.RightAction = () =>
        {
            Core.Audio.PlaySoundEffect(soundEffect);
            startButton.IsSelected = false;
            optionsButton.IsSelected = true;
        };

        _titleMenu.LeftAction = () =>
        {
            Core.Audio.PlaySoundEffect(soundEffect);
            startButton.IsSelected = true;
            optionsButton.IsSelected = false;
        };

        _titleMenu.ConfirmAction = () =>
        {
            if (startButton.IsSelected)
            {
                Core.Audio.PlaySoundEffect(soundEffect);
                Core.ChangeScene(new GameSelectScene());
            }
            else if (optionsButton.IsSelected)
            {
                Core.Audio.PlaySoundEffect(soundEffect);
                _titleMenu.IsEnabled = false;
                _titleMenu.IsVisible = false;
                _optionsMenu.IsEnabled = true;
                _optionsMenu.IsVisible = true;
            }
        };
    }

    private void CreateOptionsMenu(TextureAtlas atlas, SoundEffect soundEffect, UIElementController controller)
    {
        _optionsMenu = new UIElement();
        _optionsMenu.Controller = controller;

        UISprite optionsLabel = _optionsMenu.CreateChild<UISprite>();
        optionsLabel.Sprite = atlas.CreateSprite("options-label");
        optionsLabel.Position = new Vector2(112, 20);

        UISprite enterLabel = _optionsMenu.CreateChild<UISprite>();
        enterLabel.Sprite = atlas.CreateSprite("enter-label");
        enterLabel.Position = new Vector2(640, 52);

        UISprite escapeLabel = _optionsMenu.CreateChild<UISprite>();
        escapeLabel.Sprite = atlas.CreateSprite("escape-label");
        escapeLabel.Position = new Vector2(804, 52);

        UISprite musicPanel = _optionsMenu.CreateChild<UISprite>();
        musicPanel.Sprite = atlas.CreateSprite("panel");
        musicPanel.Position = new Vector2(198, 139);

        UISprite soundEffectPanel = _optionsMenu.CreateChild<UISprite>();
        soundEffectPanel.Sprite = atlas.CreateSprite("panel");
        soundEffectPanel.Position = new Vector2(198, 406);
        soundEffectPanel.IsEnabled = false;

        UIButton acceptButton = _optionsMenu.CreateChild<UIButton>();
        acceptButton.NotSelectedSprite = atlas.CreateSprite("accept-button");
        acceptButton.NotSelectedSprite.CenterOrigin();
        acceptButton.SelectedSprite = atlas.CreateAnimatedSprite("accept-button-selected");
        acceptButton.SelectedSprite.CenterOrigin();
        acceptButton.Position = new Vector2(432, 670);
        acceptButton.Controller = controller;

        UIButton cancelButton = _optionsMenu.CreateChild<UIButton>();
        cancelButton.NotSelectedSprite = atlas.CreateSprite("cancel-button");
        cancelButton.NotSelectedSprite.CenterOrigin();
        cancelButton.SelectedSprite = atlas.CreateAnimatedSprite("cancel-button-selected");
        cancelButton.SelectedSprite.CenterOrigin();
        cancelButton.Position = new Vector2(848, 670);
        cancelButton.Controller = controller;

        UISprite musicLabel = musicPanel.CreateChild<UISprite>();
        musicLabel.Sprite = atlas.CreateSprite("music-label");
        musicLabel.Position = new Vector2(42, 42);

        UISlider<float> musicVolumeSlider = musicPanel.CreateChild<UISlider<float>>();
        musicVolumeSlider.SliderSprite = atlas.CreateSprite("slider");
        musicVolumeSlider.FillSprite = atlas.CreateSprite("slider-fill");
        musicVolumeSlider.FillBounds = new Rectangle(108, 4, 566, 36);
        musicVolumeSlider.Value = Core.Audio.SongVolume;
        musicVolumeSlider.MinValue = 0.0f;
        musicVolumeSlider.MaxValue = 1.0f;
        musicVolumeSlider.Step = 0.1f;
        musicVolumeSlider.Position = new Vector2(27, 117);
        musicVolumeSlider.Controller = controller;
        musicVolumeSlider.IsSelected = true;

        UISprite soundLabel = soundEffectPanel.CreateChild<UISprite>();
        soundLabel.Sprite = atlas.CreateSprite("sound-label");
        soundLabel.Position = new Vector2(42, 42);

        UISlider<float> soundEffectVolumeSlider = soundEffectPanel.CreateChild<UISlider<float>>();
        soundEffectVolumeSlider.SliderSprite = atlas.CreateSprite("slider");
        soundEffectVolumeSlider.FillSprite = atlas.CreateSprite("slider-fill");
        soundEffectVolumeSlider.FillBounds = new Rectangle(108, 4, 566, 36);
        soundEffectVolumeSlider.Value = Core.Audio.SoundEffectVolume;
        soundEffectVolumeSlider.MinValue = 0.0f;
        soundEffectVolumeSlider.MaxValue = 1.0f;
        soundEffectVolumeSlider.Step = 0.1f;
        soundEffectVolumeSlider.Position = new Vector2(27, 117);
        soundEffectVolumeSlider.Controller = controller;

        _optionsMenu.UpAction = () =>
        {
            if (soundEffectPanel.IsSelected)
            {
                Core.Audio.PlaySoundEffect(soundEffect);
                soundEffectPanel.IsSelected = false;
                soundEffectPanel.IsEnabled = false;
                musicPanel.IsSelected = true;
                musicPanel.IsEnabled = true;
            }
            else if (acceptButton.IsSelected)
            {
                Core.Audio.PlaySoundEffect(soundEffect);
                acceptButton.IsSelected = false;
                soundEffectPanel.IsSelected = true;
                soundEffectPanel.IsEnabled = true;
            }
            else if (cancelButton.IsSelected)
            {
                Core.Audio.PlaySoundEffect(soundEffect);
                cancelButton.IsSelected = false;
                soundEffectPanel.IsSelected = true;
                soundEffectPanel.IsEnabled = true;
            }
        };

        _optionsMenu.DownAction = () =>
        {
            if (musicPanel.IsSelected)
            {
                Core.Audio.PlaySoundEffect(soundEffect);
                musicPanel.IsSelected = false;
                musicPanel.IsEnabled = false;
                soundEffectPanel.IsSelected = true;
                soundEffectPanel.IsEnabled = true;
            }
            else if (soundEffectPanel.IsSelected)
            {
                Core.Audio.PlaySoundEffect(soundEffect);
                soundEffectPanel.IsSelected = false;
                soundEffectPanel.IsEnabled = false;
                acceptButton.IsSelected = true;
            }
        };

        _optionsMenu.LeftAction = () =>
        {
            if (musicPanel.IsSelected)
            {
                Core.Audio.PlaySoundEffect(soundEffect);
                Core.Audio.SongVolume = musicVolumeSlider.StepDown();
            }
            else if (soundEffectPanel.IsSelected)
            {
                Core.Audio.SoundEffectVolume = soundEffectVolumeSlider.StepDown();
                Core.Audio.PlaySoundEffect(soundEffect);
            }
            else if (cancelButton.IsSelected)
            {
                cancelButton.IsSelected = false;
                acceptButton.IsSelected = true;
            }
        };

        _optionsMenu.RightAction = () =>
        {
            if (musicPanel.IsSelected)
            {
                Core.Audio.PlaySoundEffect(soundEffect);
                Core.Audio.SongVolume = musicVolumeSlider.StepUp(0;)
            }
            else if (soundEffectPanel.IsSelected)
            {
                Core.Audio.SoundEffectVolume = soundEffectVolumeSlider.StepUp();
                Core.Audio.PlaySoundEffect(soundEffect);
            }
            else if (acceptButton.IsSelected)
            {
                acceptButton.IsSelected = false;
                cancelButton.IsSelected = true;
            }
        };

        _optionsMenu.ConfirmAction = () =>
        {
            if (musicPanel.IsSelected)
            {
                musicPanel.IsSelected = false;
                musicPanel.IsEnabled = false;
                soundEffectPanel.IsSelected = true;
                soundEffectPanel.IsEnabled = true;
            }
            else if (soundEffectPanel.IsSelected)
            {
                soundEffectPanel.IsSelected = false;
                soundEffectPanel.IsEnabled = false;
                acceptButton.IsSelected = true;
            }
            else if (acceptButton.IsSelected)
            {
                musicVolumeSlider.IsSelected = true;
                soundEffectVolumeSlider.IsSelected = false;
                acceptButton.IsSelected = false;
                cancelButton.IsSelected = false;
                musicPanel.IsEnabled = true;
                soundEffectPanel.IsEnabled = false;
                _titleMenu.IsEnabled = true;
                _titleMenu.IsVisible = true;
            }
            else if (cancelButton.IsSelected)
            {
                musicVolumeSlider.IsSelected = true;
                soundEffectVolumeSlider.IsSelected = false;
                acceptButton.IsSelected = false;
                cancelButton.IsSelected = false;
                musicPanel.IsEnabled = true;
                soundEffectPanel.IsEnabled = false;
                _titleMenu.IsEnabled = true;
                _titleMenu.IsVisible = true;
            }
        };

        _optionsMenu.CancelAction = () =>
        {
            if (soundEffectPanel.IsSelected)
            {
                soundEffectPanel.IsSelected = false;
                soundEffectPanel.IsEnabled = false;
                musicPanel.IsSelected = true;
                musicPanel.IsEnabled = true;
            }
            else if (acceptButton.IsSelected)
            {
                soundEffectPanel.IsSelected = true;
                soundEffectPanel.IsEnabled = true;
                acceptButton.IsSelected = false;
            }
            else if (cancelButton.IsSelected)
            {
                soundEffectPanel.IsSelected = true;
                soundEffectPanel.IsEnabled = true;
                cancelButton.IsSelected = false;
            }
        };

    }

    public override void Update(GameTime gameTime)
    {
        _titleMenu.Update(gameTime);
        _optionsMenu.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _titleMenu.Draw(Core.SpriteBatch);
        _optionsMenu.Draw(Core.SpriteBatch);

        Core.SpriteBatch.End();
    }
}
