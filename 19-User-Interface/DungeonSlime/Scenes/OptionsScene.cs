using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.UI;

namespace DungeonSlime.Scenes;

public class OptionsScene : Scene
{
    private GameOptions _options;

    private UISprite _speedUIPanel;
    private UISprite _speedUILabel;
    private UISprite _modeUIPanel;
    private UISprite _modeUILabel;
    private UISprite _gameOptionsUILabel;
    private UISprite _enterUILabel;
    private UISprite _escapeUILabel;
    private UIButton _speedSlowUIButton;
    private UIButton _speedNormalUIButton;
    private UIButton _speedFastUIButton;
    private UIButton _modeNormalUIButton;
    private UIButton _modeDarkUIButton;

    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the options screen, we disable exit on escape so that we can
        // use the escape key for other actions.
        Core.ExitOnEscape = false;

        // Set the position of the UI Elements
        _speedUIPanel.Position = new Point(198, 139);
        _modeUIPanel.Position = new Point(198, 406);

        _gameOptionsUILabel.Position = new Point(112, 20);
        _enterUILabel.Position = new Point(640, 40);
        _escapeUILabel.Position = new Point(804, 40);

        _speedUILabel.Position = new Point(43, 42);
        _modeUILabel.Position = new Point(43, 42);

        _speedSlowUIButton.Position = new Point(148, 134);
        _speedNormalUIButton.Position = new Point(424, 134);
        _speedFastUIButton.Position = new Point(689, 134);

        _modeNormalUIButton.Position = new Point(242, 134);
        _modeDarkUIButton.Position = new Point(590, 134);

        // Center the origin of the buttons
        _speedSlowUIButton.CenterOrigin();
        _speedNormalUIButton.CenterOrigin();
        _speedFastUIButton.CenterOrigin();
        _modeNormalUIButton.CenterOrigin();
        _modeDarkUIButton.CenterOrigin();


        _speedUIPanel.Enabled = true;
        _modeUIPanel.Enabled = false;
    }

    public override void LoadContent()
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // Get the sprites for the ui elements
        Sprite panelSprite = atlas.CreateSprite("panel");
        Sprite gameOptionsLabelSprite = atlas.CreateSprite("game-options-label");
        Sprite enterLabelSprite = atlas.CreateSprite("enter-label");
        Sprite escapeLabelSprite = atlas.CreateSprite("escape-label");
        Sprite speedLabelSprite = atlas.CreateSprite("speed-label");
        Sprite modeLabelSprite = atlas.CreateSprite("mode-label");
        Sprite slowButtonSprite = atlas.CreateSprite("slow-button");
        Sprite normalButtonSprite = atlas.CreateSprite("normal-button");
        Sprite fastButtonSprite = atlas.CreateSprite("fast-button");
        Sprite darkButtonSprite = atlas.CreateSprite("dark-button");
        AnimatedSprite slowButtonSelectedSprite = atlas.CreateAnimatedSprite("slow-button-animation");
        AnimatedSprite normalSpeedButtonSelectedSPrite = atlas.CreateAnimatedSprite("normal-button-animation");
        AnimatedSprite normalModeButtonSelectedSprite = atlas.CreateAnimatedSprite("normal-button-animation");
        AnimatedSprite fastButtonSelectedSprite = atlas.CreateAnimatedSprite("fast-button-animation");
        AnimatedSprite darkButtonSelectedSprite = atlas.CreateAnimatedSprite("dark-button-animation");

        // create the ui buttons
        _speedSlowUIButton = new UIButton(slowButtonSprite, slowButtonSelectedSprite);
        _speedNormalUIButton = new UIButton(normalButtonSprite, normalSpeedButtonSelectedSPrite);
        _speedFastUIButton = new UIButton(fastButtonSprite, fastButtonSelectedSprite);
        _modeNormalUIButton = new UIButton(normalButtonSprite, normalModeButtonSelectedSprite);
        _modeDarkUIButton = new UIButton(darkButtonSprite, darkButtonSelectedSprite);


        // Create the ui labels
        _gameOptionsUILabel = new UISprite(gameOptionsLabelSprite);
        _enterUILabel = new UISprite(enterLabelSprite);
        _escapeUILabel = new UISprite(escapeLabelSprite);
        _modeUILabel = new UISprite(modeLabelSprite);
        _speedUILabel = new UISprite(speedLabelSprite);

        // Create the panels
        _speedUIPanel = new UISprite(panelSprite);
        _modeUIPanel = new UISprite(panelSprite);

        // Add the children of the speed ui panel
        _speedUIPanel.AddChild(_speedUILabel);
        _speedUIPanel.AddChild(_speedSlowUIButton);
        _speedUIPanel.AddChild(_speedNormalUIButton);
        _speedUIPanel.AddChild(_speedFastUIButton);

        // Add the children of the mode ui panel
        _modeUIPanel.AddChild(_modeUILabel);
        _modeUIPanel.AddChild(_modeNormalUIButton);
        _modeUIPanel.AddChild(_modeDarkUIButton);

        Color disabledColor = new Color(70, 86, 130, 255);
        _speedUIPanel.DisabledColor = disabledColor;
        _modeUIPanel.DisabledColor = disabledColor;
    }

    public override void Update(GameTime gameTime)
    {
        KeyboardInfo keyboard = Core.Input.Keyboard;

        if (_speedUIPanel.Enabled)
        {
            UpdateSpeedPanel(gameTime);
        }
        else if (_modeUIPanel.Enabled)
        {
            UpdateModePanel(gameTime);
        }

        UpdateButtonStates();
    }

    private void UpdateSpeedPanel(GameTime gameTime)
    {
        _speedUIPanel.Update(gameTime);

        KeyboardInfo keyboard = Core.Input.Keyboard;

        if (keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Core.ChangeScene(new TitleScene());
            return;
        }

        if (keyboard.WasKeyJustPressed(Keys.Down) || keyboard.WasKeyJustPressed(Keys.Enter))
        {
            // Disable the speed ui panel
            _speedUIPanel.Enabled = false;

            // Enable the mode ui panel
            _modeUIPanel.Enabled = true;
        }
        else if (keyboard.WasKeyJustPressed(Keys.Left))
        {
            if (_options.Speed == GameOptions.SlimeSpeed.Normal)
            {
                _options.Speed = GameOptions.SlimeSpeed.Slow;
            }
            else if (_options.Speed == GameOptions.SlimeSpeed.Fast)
            {
                _options.Speed = GameOptions.SlimeSpeed.Normal;
            }
        }
        else if (keyboard.WasKeyJustPressed(Keys.Right))
        {
            if (_options.Speed == GameOptions.SlimeSpeed.Slow)
            {
                _options.Speed = GameOptions.SlimeSpeed.Normal;
            }
            else if (_options.Speed == GameOptions.SlimeSpeed.Normal)
            {
                _options.Speed = GameOptions.SlimeSpeed.Fast;
            }
        }
    }

    private void UpdateModePanel(GameTime gameTime)
    {
        _modeUIPanel.Update(gameTime);

        KeyboardInfo keyboard = Core.Input.Keyboard;

        if (keyboard.WasKeyJustPressed(Keys.Escape) || keyboard.WasKeyJustPressed(Keys.Up))
        {
            // Enabled the speed ui panel
            _speedUIPanel.Enabled = true;

            // Disable the mode ui panel
            _modeUIPanel.Enabled = false;
        }
        else if (keyboard.WasKeyJustPressed(Keys.Left) && _options.Mode == GameOptions.GameMode.Dark)
        {
            _options.Mode = GameOptions.GameMode.Normal;
        }
        else if (keyboard.WasKeyJustPressed(Keys.Right) && _options.Mode == GameOptions.GameMode.Normal)
        {
            _options.Mode = GameOptions.GameMode.Dark;
        }
    }

    private void UpdateButtonStates()
    {
        _speedSlowUIButton.IsSelected = _options.Speed == GameOptions.SlimeSpeed.Slow;
        _speedNormalUIButton.IsSelected = _options.Speed == GameOptions.SlimeSpeed.Normal;
        _speedFastUIButton.IsSelected = _options.Speed == GameOptions.SlimeSpeed.Fast;

        _modeNormalUIButton.IsSelected = _options.Mode == GameOptions.GameMode.Normal;
        _modeDarkUIButton.IsSelected = _options.Mode == GameOptions.GameMode.Dark;
    }

    public override void Draw(GameTime gameTime)
    {
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _gameOptionsUILabel.Draw(Core.SpriteBatch);
        _enterUILabel.Draw(Core.SpriteBatch);
        _escapeUILabel.Draw(Core.SpriteBatch);
        _speedUIPanel.Draw(Core.SpriteBatch);
        _modeUIPanel.Draw(Core.SpriteBatch);

        Core.SpriteBatch.End();
    }
}
