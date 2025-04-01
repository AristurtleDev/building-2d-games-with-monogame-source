using DungeonSlime.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.UI;

namespace DungeonSlime.Scenes;

public class GameSelectScene : Scene
{
    private UIElement _menu;
    private GameOptions _options;

    public GameSelectScene() : base() { }

    public override void Initialize()
    {
        _options = new GameOptions();

        base.Initialize();
    }

    public override void LoadContent()
    {
        // Load the ui texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/ui-atlas-definition.xml");

        // Load the sound effect to play when ui actions occur.
        SoundEffect uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");

        // Create the UI controller
        UIElementController controller = new UIElementController();

        // Create the game select menu.
        CreateGameSelectMenu(atlas, uiSoundEffect, controller);
    }

    private void CreateGameSelectMenu(TextureAtlas atlas, SoundEffect soundEffect, UIElementController controller)
    {
        // Create the root UI element for the menu.
        _menu = new UIElement();
        _menu.Controller = controller;
        _menu.IsSelected = true;

        // Create the select label as a child of the menu
        UISprite selectLabel = _menu.CreateChild<UISprite>();
        selectLabel.Sprite = atlas.CreateSprite("select-label");
        selectLabel.Position = new Vector2(112, 20);

        // Create the enter label as a child of the menu.
        UISprite enterLabel = _menu.CreateChild<UISprite>();
        enterLabel.Sprite = atlas.CreateSprite("enter-label");
        enterLabel.Position = new Vector2(640, 52);

        // Create the escape label as a child of the menu.
        UISprite escapeLabel = _menu.CreateChild<UISprite>();
        escapeLabel.Sprite = atlas.CreateSprite("escape-label");
        escapeLabel.Position = new Vector2(804, 52);

        // Create the speed panel as a child of the menu.
        UISprite speedPanel = _menu.CreateChild<UISprite>();
        speedPanel.Sprite = atlas.CreateSprite("panel");
        speedPanel.Position = new Vector2(198, 139);
        speedPanel.IsSelected = true;

        // Create the mode panel as a child of the menu.
        UISprite modePanel = _menu.CreateChild<UISprite>();
        modePanel.Sprite = atlas.CreateSprite("panel");
        modePanel.Position = new Vector2(198, 406);
        modePanel.IsEnabled = false;

        // Create the accept button as a child of the menu
        UIButton acceptButton = _menu.CreateChild<UIButton>();
        acceptButton.NotSelectedSprite = atlas.CreateSprite("accept-button");
        acceptButton.NotSelectedSprite.CenterOrigin();
        acceptButton.SelectedSprite = atlas.CreateAnimatedSprite("accept-button-selected");
        acceptButton.SelectedSprite.CenterOrigin();
        acceptButton.Position = new Vector2(432, 670);

        // Create the cancel button as a child of the menu
        UIButton cancelButton = _menu.CreateChild<UIButton>();
        cancelButton.NotSelectedSprite = atlas.CreateSprite("cancel-button");
        cancelButton.NotSelectedSprite.CenterOrigin();
        cancelButton.SelectedSprite = atlas.CreateAnimatedSprite("cancel-button-selected");
        cancelButton.SelectedSprite.CenterOrigin();
        cancelButton.Position = new Vector2(848, 670);

        // Create the speed panel label as a child of the speed panel.
        UISprite speedLabel = speedPanel.CreateChild<UISprite>();
        speedLabel.Sprite = atlas.CreateSprite("speed-label");
        speedLabel.Position = new Vector2(42, 42);

        // Create the slow speed button as a child of the speed panel.
        UIButton slowSpeedButton = speedPanel.CreateChild<UIButton>();
        slowSpeedButton.NotSelectedSprite = atlas.CreateSprite("slow-button");
        slowSpeedButton.NotSelectedSprite.CenterOrigin();
        slowSpeedButton.SelectedSprite = atlas.CreateAnimatedSprite("slow-button-selected");
        slowSpeedButton.SelectedSprite.CenterOrigin();
        slowSpeedButton.Position = new Vector2(148, 148);

        // Create the normal speed button as a child of the speed panel.
        UIButton normalSpeedButton = speedPanel.CreateChild<UIButton>();
        normalSpeedButton.NotSelectedSprite = atlas.CreateSprite("normal-button");
        normalSpeedButton.NotSelectedSprite.CenterOrigin();
        normalSpeedButton.SelectedSprite = atlas.CreateAnimatedSprite("normal-button-selected");
        normalSpeedButton.SelectedSprite.CenterOrigin();
        normalSpeedButton.Position = new Vector2(420, 148);
        normalSpeedButton.IsSelected = true;

        // Create the fast speed button as a child of the speed panel.
        UIButton fastSpeedButton = speedPanel.CreateChild<UIButton>();
        fastSpeedButton.NotSelectedSprite = atlas.CreateSprite("fast-button");
        fastSpeedButton.NotSelectedSprite.CenterOrigin();
        fastSpeedButton.SelectedSprite = atlas.CreateAnimatedSprite("fast-button-selected");
        fastSpeedButton.SelectedSprite.CenterOrigin();
        fastSpeedButton.Position = new Vector2(691, 148);

        // Create the mode panel label as a child of the mode panel.
        UISprite modeLabel = modePanel.CreateChild<UISprite>();
        modeLabel.Sprite = atlas.CreateSprite("mode-label");
        modeLabel.Position = new Vector2(42, 42);

        // Create the normal mode button as a child of the mode panel.
        UIButton normalModeButton = modePanel.CreateChild<UIButton>();
        normalModeButton.NotSelectedSprite = atlas.CreateSprite("normal-button");
        normalModeButton.NotSelectedSprite.CenterOrigin();
        normalModeButton.SelectedSprite = atlas.CreateAnimatedSprite("normal-button-selected");
        normalModeButton.SelectedSprite.CenterOrigin();
        normalModeButton.Position = new Vector2(148, 148);
        normalModeButton.IsSelected = true;

        // Create the dark mode button as a child of the mode panel.
        UIButton darkModeButton = modePanel.CreateChild<UIButton>();
        darkModeButton.NotSelectedSprite = atlas.CreateSprite("dark-button");
        darkModeButton.NotSelectedSprite.CenterOrigin();
        darkModeButton.SelectedSprite = atlas.CreateAnimatedSprite("dark-button-selected");
        darkModeButton.SelectedSprite.CenterOrigin();
        darkModeButton.Position = new Vector2(691, 148);

        _menu.DisabledColor = new Color(70, 86, 130, 255);

        _menu.UpAction = () =>
        {
            if (modePanel.IsSelected)
            {
                modePanel.IsSelected = false;
                modePanel.IsEnabled = false;
                speedPanel.IsSelected = true;
                speedPanel.IsEnabled = true;
            }
            else if (acceptButton.IsSelected)
            {
                acceptButton.IsSelected = false;
                modePanel.IsSelected = true;
                modePanel.IsEnabled = true;
            }
            else if (cancelButton.IsSelected)
            {
                cancelButton.IsSelected = false;
                modePanel.IsSelected = true;
                modePanel.IsEnabled = true;
            }
        };

        _menu.DownAction = () =>
        {
            if (speedPanel.IsSelected)
            {
                modePanel.IsSelected = true;
                modePanel.IsEnabled = true;
                speedPanel.IsSelected = false;
                speedPanel.IsEnabled = false;
            }
            else if (modePanel.IsSelected)
            {
                modePanel.IsSelected = false;
                modePanel.IsEnabled = false;
                acceptButton.IsSelected = true;
            }
        };

        _menu.LeftAction = () =>
        {
            if (speedPanel.IsSelected)
            {
                if (_options.Speed == GameOptions.SlimeSpeed.Normal)
                {
                    normalSpeedButton.IsSelected = false;
                    slowSpeedButton.IsSelected = true;
                    _options.Speed = GameOptions.SlimeSpeed.Slow;
                }
                else if (_options.Speed == GameOptions.SlimeSpeed.Fast)
                {
                    fastSpeedButton.IsSelected = false;
                    normalSpeedButton.IsSelected = true;
                    _options.Speed = GameOptions.SlimeSpeed.Normal;
                }
            }
            else if (modePanel.IsSelected)
            {
                if (_options.Mode == GameOptions.GameMode.Dark)
                {
                    darkModeButton.IsSelected = false;
                    normalModeButton.IsSelected = true;
                    _options.Mode = GameOptions.GameMode.Normal;
                }
            }
            else if (cancelButton.IsSelected)
            {
                cancelButton.IsSelected = false;
                acceptButton.IsSelected = true;
            }
        };

        _menu.RightAction = () =>
        {
            if (speedPanel.IsSelected)
            {
                if (_options.Speed == GameOptions.SlimeSpeed.Slow)
                {
                    slowSpeedButton.IsSelected = false;
                    normalSpeedButton.IsSelected = true;
                    _options.Speed = GameOptions.SlimeSpeed.Normal;
                }
                else if (_options.Speed == GameOptions.SlimeSpeed.Normal)
                {
                    normalSpeedButton.IsSelected = false;
                    fastSpeedButton.IsSelected = true;
                    _options.Speed = GameOptions.SlimeSpeed.Fast;
                }
            }
            else if (modePanel.IsSelected)
            {
                if (_options.Mode == GameOptions.GameMode.Normal)
                {
                    normalModeButton.IsSelected = false;
                    darkModeButton.IsSelected = true;
                    _options.Mode = GameOptions.GameMode.Dark;
                }
            }
            else if (acceptButton.IsSelected)
            {
                acceptButton.IsSelected = false;
                cancelButton.IsSelected = true;
            }
        };

        _menu.ConfirmAction = () =>
        {
            if (speedPanel.IsSelected)
            {
                speedPanel.IsSelected = false;
                speedPanel.IsEnabled = false;
                modePanel.IsSelected = true;
                modePanel.IsEnabled = true;
            }
            else if (modePanel.IsSelected)
            {
                modePanel.IsSelected = false;
                modePanel.IsEnabled = false;
                acceptButton.IsSelected = true;
            }
            if (acceptButton.IsSelected)
            {
                Core.ChangeScene(new GameScene(_options));
            }
            else if (cancelButton.IsSelected)
            {
                Core.ChangeScene(new TitleScene());
            }
        };

        _menu.CancelAction = () =>
        {
            if (modePanel.IsSelected)
            {
                modePanel.IsSelected = false;
                modePanel.IsEnabled = false;
                speedPanel.IsSelected = true;
                speedPanel.IsEnabled = true;
            }
            else if (acceptButton.IsSelected || cancelButton.IsSelected)
            {
                acceptButton.IsSelected = false;
                modePanel.IsSelected = true;
                modePanel.IsEnabled = true;
            }
        };
    }

    public override void Update(GameTime gameTime)
    {
        _menu.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _menu.Draw(Core.SpriteBatch);

        Core.SpriteBatch.End();
    }
}
