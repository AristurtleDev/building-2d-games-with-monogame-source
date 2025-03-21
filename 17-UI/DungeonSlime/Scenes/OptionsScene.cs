using System.Collections;
using System.Security.AccessControl;
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
    private enum Panel
    {
        Speed,
        Mode
    }

    private enum Speed
    {
        Slow,
        Normal,
        Fast
    }

    private enum Mode
    {
        Normal,
        Dark
    }

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

    // Repeatable background
    private Texture2D _backgroundTexture;
    private Rectangle _backgroundDestination;
    private Vector2 _backgroundOffset;
    private float _scrollSpeed = 50.0f;

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

    private AnimatedSprite _enterAnimatedSprite;
    private AnimatedSprite _escapeAnimatedSprite;


    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = false;

        _backgroundOffset = Vector2.Zero;
        _backgroundDestination = new Rectangle(
            0,
            0,
            Core.GraphicsDevice.PresentationParameters.BackBufferWidth,
            Core.GraphicsDevice.PresentationParameters.BackBufferHeight
        );

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
        TextureAtlas uiAtlas = TextureAtlas.FromFile(Content, "images/ui-atlas-definition.xml");

        // Load the pattern used for the repeatable moving background
        _backgroundTexture = Content.Load<Texture2D>("images/background");

        _gameOptionsSprite = uiAtlas.CreateSprite("game-options");

        // Create a sprite for both the speed panel and mode panel based on the same region
        _speedPanel = uiAtlas.CreateSprite("panel");
        _modePanel = uiAtlas.CreateSprite("panel");

        // Create the sprite for the Speed panel label text
        _speedSprite = uiAtlas.CreateSprite("speed");

        // Create the sprite for the Mode panel label text
        _modeSprite = uiAtlas.CreateSprite("mode");

        // Create the sprite and animated sprite for the speed slow button
        _speedSlowButtonSprite = uiAtlas.CreateSprite("slow");
        _speedSlowButtonSprite.CenterOrigin();

        _speedSlowButtonAnimatedSprite = uiAtlas.CreateAnimatedSprite("slow-selected");
        _speedSlowButtonAnimatedSprite.CenterOrigin();

        // Create the sprite and the animated sprite for the speed normal button;
        _speedNormalButtonSprite = uiAtlas.CreateSprite("normal");
        _speedNormalButtonSprite.CenterOrigin();

        _speedNormalButtonAnimatedSprite = uiAtlas.CreateAnimatedSprite("normal-selected");
        _speedNormalButtonAnimatedSprite.CenterOrigin();

        // Create the sprite and the animated sprite for the speed fast button;
        _speedFastButtonSprite = uiAtlas.CreateSprite("fast");
        _speedFastButtonSprite.CenterOrigin();

        _speedFastButtonAnimatedSprite = uiAtlas.CreateAnimatedSprite("fast-selected");
        _speedFastButtonAnimatedSprite.CenterOrigin();

        // Create the sprite and the animated sprite for the mode normal button;
        _modeNormalButtonSprite = uiAtlas.CreateSprite("normal");
        _modeNormalButtonSprite.CenterOrigin();

        _modeNormalButtonAnimatedSprite = uiAtlas.CreateAnimatedSprite("normal-selected");
        _modeNormalButtonAnimatedSprite.CenterOrigin();

        // Create the sprite and the animated sprite for the mode dark button;
        _modeDarkButtonSprite = uiAtlas.CreateSprite("dark");
        _modeDarkButtonSprite.CenterOrigin();

        _modeDarkButtonAnimatedSprite = uiAtlas.CreateAnimatedSprite("dark-selected");
        _modeDarkButtonAnimatedSprite.CenterOrigin();

        // Create the enter and exit animated sprites
        _enterAnimatedSprite = uiAtlas.CreateAnimatedSprite("enter");
        // _enterAnimatedSprite.Origin = new Vector2(0, _enterAnimatedSprite.Height);
        // _enterAnimatedSprite.CenterOrigin();

        _escapeAnimatedSprite = uiAtlas.CreateAnimatedSprite("escape");
        // _escapeAnimatedSprite.CenterOrigin();
    }

    public override void Update(GameTime gameTime)
    {
        // Update the offsets for the background pattern wrapping
        _backgroundOffset.X += _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _backgroundOffset.Y -= _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Ensure that the offset doesn't go beyond the texture bounds so it's a seamless wrap
        _backgroundOffset.X %= _backgroundTexture.Width;
        _backgroundOffset.Y %= _backgroundTexture.Height;

        // Update the enter and escape animated sprites
        _enterAnimatedSprite.Update(gameTime);
        _escapeAnimatedSprite.Update(gameTime);

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
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointWrap, blendState: BlendState.AlphaBlend);
        Core.SpriteBatch.Draw(_backgroundTexture, _backgroundDestination, new Rectangle(_backgroundOffset.ToPoint(), _backgroundDestination.Size), Color.White);
        Core.SpriteBatch.End();

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _gameOptionsSprite.Draw(Core.SpriteBatch, new Vector2(112, 20));

        _enterAnimatedSprite.Draw(Core.SpriteBatch,new Vector2(750, 24));
        _escapeAnimatedSprite.Draw(Core.SpriteBatch,new Vector2(950, 24));


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
