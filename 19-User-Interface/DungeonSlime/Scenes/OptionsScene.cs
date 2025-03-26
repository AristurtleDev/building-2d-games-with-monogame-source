using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.UI;

namespace DungeonSlime.Scenes;

public class OptionsScene : Scene
{
    private GameOptions _options;

    private enum Panel
    {
        Speed,
        Mode
    }

    private UISprite _speedUIPanel;
    private UISprite _modeUIPanel;
    private UISprite _speedUILabel;
    private UISprite _modeUILabel;
    private UISprite _gameOptionsUILabel;
    private UISprite _enterUILabel;
    private UISprite _escapeUILabel;
    private UIButton _speedSlowUIButton;
    private UIButton _speedNormalUIButton;
    private UIButton _speedFastUIButton;
    private UIButton _modeNormalUIButton;
    private UIButton _modeDarkUIButton;


    // Tracks which panel is the current active (selected) panel.
    private Panel _selectedPanel;

    // Tracks which speed has been selected.
    private Speed _selectedSpeed;

    // Tracks which mode has been selected.
    private Mode _selectedMode;

    // The color tint to apply to the panel and its elements that are active.
    private Color _activeColor = Color.White;

    // The color tint to apply to the panel and its elements that are no active.
    private Color _disabledColor = new Color(70, 86, 130, 255);

    // The sprite that displays the text Game Options inside a stylized container.
    private Sprite _gameOptionsSprite;



    private Sprite _speedPanel;
    private Sprite _speedSprite;
    private Sprite _speedSlowButtonSprite;
    private AnimatedSprite _speedSlowButtonAnimatedSprite;
    private Sprite _speedNormalButtonSprite;
    private AnimatedSprite _speedNormalButtonAnimatedSprite;
    private Sprite _speedFastButtonSprite;
    private AnimatedSprite _speedFastButtonAnimatedSprite;


    private Sprite _modePanel;
    private Sprite _modeSprite;
    private Sprite _modeNormalButtonSprite;
    private AnimatedSprite _modeNormalButtonAnimatedSprite;
    private Sprite _modeDarkButtonSprite;
    private AnimatedSprite _modeDarkButtonAnimatedSprite;

    private Sprite _enterLabelSprite;
    private Sprite _escapeLabelSprite;


    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = false;

        // Select the Speed panel by default
        _selectedPanel = Panel.Speed;

        // Select normal speed by default
        _selectedSpeed = Speed.Normal;

        // Select normal game mode by default
        _selectedMode = Mode.Normal;

        // Everything in the mode panel starts dark like it's deactivated
        _modePanel.Color =
        _modeSprite.Color =
        _modeNormalButtonSprite.Color =
        _modeNormalButtonAnimatedSprite.Color =
        _modeDarkButtonSprite.Color =
        _modeDarkButtonAnimatedSprite.Color = _disabledColor;
    }

    public override void LoadContent()
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");

        // Get the sprites for the ui elements
        Sprite panelSprite = atlas.CreateSprite("panel");
        Sprite gameOptionsSprite = atlas.CreateSprite("game-options-label");
        Sprite enterLabelSprite = atlas.CreateSprite("enter-label");
        Sprite escapeLabelSprite = atlas.CreateSprite("escape-label");
        Sprite speedLabelSprite = atlas.CreateSprite("speed-label");
        Sprite modeLabelSprite = atlas.CreateSprite("mode-label");
        Sprite slowSprite = atlas.CreateSprite("slow-button-not-selected");
        Sprite normalSprite = atlas.CreateSprite("normal-button-not-selected");
        Sprite fastSprite = atlas.CreateSprite("fast-button-not-selected");
        Sprite darkSprite = atlas.CreateSprite("dark-button-not-selected");
        AnimatedSprite slowSelectedSprite = atlas.CreateAnimatedSprite("slow-button-selected-animation");
        AnimatedSprite normalSelectedSprite = atlas.CreateAnimatedSprite("normal-button-selected-animation");
        AnimatedSprite fastSelectedSprite = atlas.CreateAnimatedSprite("fast-button-selected-animation");
        AnimatedSprite darkSelectedSprite = atlas.CreateAnimatedSprite("dark-button-selected-animation");

        // create the ui buttons
        _speedSlowUIButton = new UIButton("Slow Speed", slowSprite, slowSelectedSprite, true);
        _speedNormalUIButton = new UIButton("Normal Speed", normalSprite, normalSelectedSprite, true);
        _speedFastUIButton = new UIButton("Fast Speed", fastSprite, fastSelectedSprite, true);
        _modeNormalUIButton = new UIButton("Normal Mode", normalSprite, normalSelectedSprite, true);
        _modeDarkUIButton = new UIButton("Dark Mode", darkSprite, darkSelectedSprite, true);

        // Create the ui labels
        _gameOptionsUILabel = new UISprite("Game Options", gameOptionsSprite);
        _enterUILabel = new UISprite("Enter Ok", enterLabelSprite);
        _escapeUILabel = new UISprite("Escape Cancel", escapeLabelSprite);
        _modeUILabel = new UISprite("Mode", modeLabelSprite);
        _speedUILabel = new UISprite("Speed", speedLabelSprite);

        // Create the panels
        _speedUIPanel = new UISprite("Speed Panel", panelSprite);
        _modeUIPanel = new UISprite("Mode Panel", panelSprite);

        // Add the children of the speed ui panel
        _speedUIPanel.AddChild(_speedUILabel);
        _speedUIPanel.AddChild(_speedSlowUIButton);
        _speedUIPanel.AddChild(_speedNormalUIButton);
        _speedUIPanel.AddChild(_speedFastUIButton);

        // Add the children of the mode ui panel
        _modeUIPanel.AddChild(_modeUILabel);
        _modeUIPanel.AddChild(_modeNormalUIButton);
        _modeUIPanel.AddChild(_modeDarkUIButton);
    }

    public override void Update(GameTime gameTime)
    {
        // If escape is pressed, go back to the title scene
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            Core.ChangeScene(new TitleScene());
        }

        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Down) || Core.Input.Keyboard.WasKeyJustPressed(Keys.Up))
        {
            if (_selectedPanel == Panel.Speed)
            {
                _speedPanel.Color =
                _speedSprite.Color =
                _speedSlowButtonSprite.Color =
                _speedSlowButtonAnimatedSprite.Color =
                _speedNormalButtonSprite.Color =
                _speedNormalButtonAnimatedSprite.Color =
                _speedFastButtonSprite.Color =
                _speedFastButtonAnimatedSprite.Color = _disabledColor;

                _modePanel.Color =
                _modeSprite.Color =
                _modeNormalButtonSprite.Color =
                _modeNormalButtonAnimatedSprite.Color =
                _modeDarkButtonSprite.Color =
                _modeDarkButtonAnimatedSprite.Color = _activeColor;

                _selectedPanel = Panel.Mode;
            }
            else
            {
                _modePanel.Color =
                _modeSprite.Color =
                _modeNormalButtonSprite.Color =
                _modeNormalButtonAnimatedSprite.Color =
                _modeDarkButtonSprite.Color =
                _modeDarkButtonAnimatedSprite.Color = _disabledColor;

                _speedPanel.Color =
                _speedSprite.Color =
                _speedSlowButtonSprite.Color =
                _speedSlowButtonAnimatedSprite.Color =
                _speedNormalButtonSprite.Color =
                _speedNormalButtonAnimatedSprite.Color =
                _speedFastButtonSprite.Color =
                _speedFastButtonAnimatedSprite.Color = _activeColor;

                _selectedPanel = Panel.Speed;
            }
        }

        if (_selectedPanel == Panel.Speed)
        {
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Left))
            {
                switch (_selectedSpeed)
                {
                    case Speed.Slow:
                        _selectedSpeed = Speed.Fast;
                        break;
                    case Speed.Normal:
                        _selectedSpeed = Speed.Slow;
                        break;
                    case Speed.Fast:
                        _selectedSpeed = Speed.Normal;
                        break;
                }
            }
            else if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Right))
            {
                switch (_selectedSpeed)
                {
                    case Speed.Slow:
                        _selectedSpeed = Speed.Normal;
                        break;
                    case Speed.Normal:
                        _selectedSpeed = Speed.Fast;
                        break;
                    case Speed.Fast:
                        _selectedSpeed = Speed.Slow;
                        break;
                }
            }
        }
        else
        {
            if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Left))
            {
                switch (_selectedMode)
                {
                    case Mode.Normal:
                        _selectedMode = Mode.Dark;
                        break;
                    case Mode.Dark:
                        _selectedMode = Mode.Normal;
                        break;
                }
            }
            else if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Right))
            {
                switch (_selectedMode)
                {
                    case Mode.Normal:
                        _selectedMode = Mode.Dark;
                        break;
                    case Mode.Dark:
                        _selectedMode = Mode.Normal;
                        break;
                }
            }
        }

        // Only update the animated button sprites for the selected panel
        if (_selectedPanel == Panel.Speed)
        {
            switch (_selectedSpeed)
            {
                case Speed.Slow:
                    _speedSlowButtonAnimatedSprite.Update(gameTime);
                    break;
                case Speed.Normal:
                    _speedNormalButtonAnimatedSprite.Update(gameTime);
                    break;
                case Speed.Fast:
                    _speedFastButtonAnimatedSprite.Update(gameTime);
                    break;
            }
        }
        else
        {
            switch (_selectedMode)
            {
                case Mode.Normal:
                    _modeNormalButtonAnimatedSprite.Update(gameTime);
                    break;
                case Mode.Dark:
                    _modeDarkButtonAnimatedSprite.Update(gameTime);
                    break;
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _gameOptionsSprite.Draw(Core.SpriteBatch, new Vector2(112, 20));

        _enterLabelSprite.Draw(Core.SpriteBatch, new Vector2(750, 24));
        _escapeLabelSprite.Draw(Core.SpriteBatch, new Vector2(950, 24));


        _speedPanel.Draw(Core.SpriteBatch, new Vector2(198, 139));
        _speedSprite.Draw(Core.SpriteBatch, new Vector2(240, 181));

        _modePanel.Draw(Core.SpriteBatch, new Vector2(198, 406));
        _modeSprite.Draw(Core.SpriteBatch, new Vector2(240, 448));

        switch (_selectedSpeed)
        {
            case Speed.Slow:
                _speedSlowButtonAnimatedSprite.Draw(Core.SpriteBatch, new Vector2(350, 273));
                _speedNormalButtonSprite.Draw(Core.SpriteBatch, new Vector2(615, 273));
                _speedFastButtonSprite.Draw(Core.SpriteBatch, new Vector2(883, 273));
                break;
            case Speed.Normal:
                _speedSlowButtonSprite.Draw(Core.SpriteBatch, new Vector2(350, 273));
                _speedNormalButtonAnimatedSprite.Draw(Core.SpriteBatch, new Vector2(615, 273));
                _speedFastButtonSprite.Draw(Core.SpriteBatch, new Vector2(883, 273));
                break;
            case Speed.Fast:
                _speedSlowButtonSprite.Draw(Core.SpriteBatch, new Vector2(350, 273));
                _speedNormalButtonSprite.Draw(Core.SpriteBatch, new Vector2(615, 273));
                _speedFastButtonAnimatedSprite.Draw(Core.SpriteBatch, new Vector2(883, 273));
                break;
        }

        switch (_selectedMode)
        {
            case Mode.Normal:
                _modeNormalButtonAnimatedSprite.Draw(Core.SpriteBatch, new Vector2(479, 531));
                _modeDarkButtonSprite.Draw(Core.SpriteBatch, new Vector2(784, 531));
                break;

            case Mode.Dark:
                _modeNormalButtonSprite.Draw(Core.SpriteBatch, new Vector2(479, 531));
                _modeDarkButtonAnimatedSprite.Draw(Core.SpriteBatch, new Vector2(784, 531));
                break;
        }

        Core.SpriteBatch.End();

    }

}
