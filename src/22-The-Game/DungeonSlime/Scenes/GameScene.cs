using System;
using DungeonSlime.GameObjects;
using DungeonSlime.UI;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;

namespace DungeonSlime.Scenes;

public class GameScene : Scene
{
    // Reference to the slime.
    private Slime _slime;

    // Reference to the bat.
    private Bat _bat;

    // Defines the tilemap to draw.
    private Tilemap _tilemap;

    // Defines the bounds of the room that the slime and bat are contained within.
    private Rectangle _roomBounds;

    // The sound effect to play when the slime eats a bat.
    private SoundEffect _collectSoundEffect;

    // The SpriteFont Description used to draw text
    private SpriteFont _font;

    // Tracks the players score.
    private int _score;

    // Defines the position to draw the score text at.
    private Vector2 _scoreTextPosition;

    // Defines the origin used when drawing the score text.
    private Vector2 _scoreTextOrigin;

    // A reference to the pause panel UI element so we can set its visibility
    // when the game is paused.
    private Panel _pausePanel;

    // A reference to the resume button UI element so we can focus it
    // when the game is paused.
    private AnimatedButton _resumeButton;

    // The UI sound effect to play when a UI event is triggered.
    private SoundEffect _uiSoundEffect;

    // Reference to the texture atlas that we can pass to UI elements when they
    // are created.
    private TextureAtlas _atlas;

    public override void Initialize()
    {
        // Create the slime
        _slime = new Slime();

        // Register with the slime's collision events
        _slime.BodyCollision += GameOver;
        _slime.WallCollision += GameOver;

        // Create the bat.
        _bat = new Bat();

        // LoadContent is called during base.Initialize().
        base.Initialize();

        // During the game scene, we want to disable exit on escape. Instead,
        // the escape key will be used to return back to the title screen
        Core.ExitOnEscape = false;

        Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        _roomBounds = new Rectangle(
            (int)_tilemap.TileWidth,
            (int)_tilemap.TileHeight,
            screenBounds.Width - (int)_tilemap.TileWidth * 2,
            screenBounds.Height - (int)_tilemap.TileHeight * 2
        );

        // Both the slime and the bat need to be made aware of the bounds of
        // the room
        _slime.RoomBounds = _roomBounds;
        _bat.RoomBounds = _roomBounds;

        // The slime needs to be made aware of the size of a tile so it can
        // calculate its movements
        _slime.TileSize = (int)_tilemap.TileWidth;

        // Set the position of the score text to align to the left edge of the
        // room bounds, and to vertically be at the center of the first tile.
        _scoreTextPosition = new Vector2(_roomBounds.Left, _tilemap.TileHeight * 0.5f);

        // Set the origin of the text so it's left-centered.
        float scoreTextYOrigin = _font.MeasureString("Score").Y * 0.5f;
        _scoreTextOrigin = new Vector2(0, scoreTextYOrigin);

        InitializeUI();

        InitializeNewGame();
    }

    private void InitializeUI()
    {
        GumService.Default.Root.Children.Clear();

        CreatePausePanel();
    }

    private void CreatePausePanel()
    {
        _pausePanel = new Panel();
        _pausePanel.Visual.Anchor(Anchor.Center);
        _pausePanel.Visual.WidthUnits = DimensionUnitType.Absolute;
        _pausePanel.Visual.HeightUnits = DimensionUnitType.Absolute;
        _pausePanel.Visual.Height = 70;
        _pausePanel.Visual.Width = 264;
        _pausePanel.IsVisible = false;
        _pausePanel.AddToRoot();

        TextureRegion backgroundRegion = _atlas.GetRegion("panel-background");

        NineSliceRuntime background = new NineSliceRuntime();
        background.Dock(Dock.Fill);
        background.Texture = backgroundRegion.Texture;
        background.TextureAddress = TextureAddress.Custom;
        background.TextureHeight = backgroundRegion.Height;
        background.TextureLeft = backgroundRegion.SourceRectangle.Left;
        background.TextureTop = backgroundRegion.SourceRectangle.Top;
        background.TextureWidth = backgroundRegion.Width;
        _pausePanel.AddChild(background);

        TextRuntime textInstance = new TextRuntime();
        textInstance.Text = "PAUSED";
        textInstance.CustomFontFile = @"fonts/04b_30.fnt";
        textInstance.UseCustomFont = true;
        textInstance.FontScale = 0.5f;
        textInstance.X = 10f;
        textInstance.Y = 10f;
        _pausePanel.AddChild(textInstance);

        _resumeButton = new AnimatedButton(_atlas);
        _resumeButton.Text = "RESUME";
        _resumeButton.Visual.Anchor(Anchor.BottomLeft);
        _resumeButton.Visual.X = 9f;
        _resumeButton.Visual.Y = -9f;
        _resumeButton.Click += HandleResumeButtonClicked;
        _pausePanel.AddChild(_resumeButton);

        AnimatedButton quitButton = new AnimatedButton(_atlas);
        quitButton.Text = "QUIT";
        quitButton.Visual.Anchor(Anchor.BottomRight);
        quitButton.Visual.X = -9f;
        quitButton.Visual.Y = -9f;
        quitButton.Click += HandleQuitButtonClicked;

        _pausePanel.AddChild(quitButton);
    }

    private void HandleResumeButtonClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Make the pause panel invisible to resume the game.
        _pausePanel.IsVisible = false;
    }

    private void HandleQuitButtonClicked(object sender, EventArgs e)
    {
        // A UI interaction occurred, play the sound effect
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Go back to the title scene.
        Core.ChangeScene(new TitleScene());
    }

    private void InitializeNewGame()
    {
        // Calculate the position for the slime, which will be at the center
        // tile of the tile map.
        Vector2 slimePos = new Vector2();
        slimePos.X = (_tilemap.Columns / 2) * _tilemap.TileWidth;
        slimePos.Y = (_tilemap.Rows / 2) * _tilemap.TileHeight;

        // Initialize the slime with the calculated starting position
        _slime.Initialize(slimePos);

        // Initialize the bat by placing it somewhere away from the slime
        // and assigning it a random velocity
        _bat.PositionAwayFromSlime(_slime);
        _bat.AssignRandomVelocity();

        // Reset the score
        _score = 0;
    }

    public override void LoadContent()
    {
        // Create the texture atlas from the XML configuration file
        _atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        // Create the animated sprite for the slime from the atlas.
        _slime.Sprite = _atlas.CreateAnimatedSprite("slime-animation");
        _slime.Sprite.Scale = new Vector2(4.0f, 4.0f);

        // Create the animated sprite for the bat from the atlas.
        _bat.Sprite = _atlas.CreateAnimatedSprite("bat-animation");
        _bat.Sprite.Scale = new Vector2(4.0f, 4.0f);

        // Create the tilemap from the XML configuration file.
        _tilemap = Tilemap.FromFile(Content, "images/tilemap-definition.xml");
        _tilemap.Scale = new Vector2(4.0f, 4.0f);

        // Load the bounce sound effect for the bat
        _bat.BounceSoundEffect = Content.Load<SoundEffect>("audio/bounce");

        // Load the collect sound effect
        _collectSoundEffect = Content.Load<SoundEffect>("audio/collect");

        // Load the font
        _font = Core.Content.Load<SpriteFont>("fonts/04B_30");

        // Load the sound effect to play when ui actions occur.
        _uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");
    }

    public override void Update(GameTime gameTime)
    {
        // Ensure the UI is always updated
        GumService.Default.Update(gameTime);

        // Check if the game should be paused
        if (GameController.Pause())
        {
            PauseGame();
        }

        // If the game is paused, do not continue
        if (_pausePanel.IsVisible)
        {
            return;
        }

        // Update the slime and the bat
        _slime.Update(gameTime);
        _bat.Update(gameTime);

        // Determine if the slime is colliding with the bat, meaning it
        // eats the bat
        Circle slimeBounds = _slime.GetBounds();
        Circle batBounds = _bat.GetBounds();

        if (slimeBounds.Intersects(batBounds))
        {
            // The slime ate the bat, so move the bat to a new position with
            // a new velocity
            _bat.PositionAwayFromSlime(_slime);
            _bat.AssignRandomVelocity();

            // Grow the slime
            _slime.Grow();

            // Increment the score
            _score += 100;

            // Play the collect sound effect
            Core.Audio.PlaySoundEffect(_collectSoundEffect);
        }
    }

    private void PauseGame()
    {
        // Make the pause panel UI element visible.
        _pausePanel.IsVisible = true;

        // Set the resume button to have focus
        _resumeButton.IsFocused = true;
    }

    private void GameOver(object sender, EventArgs args)
    {

    }

    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the tilemap
        _tilemap.Draw(Core.SpriteBatch);

        // Draw the slime.
        _slime.Draw();

        // Draw the bat.
        _bat.Draw();

        // Draw the score
        Core.SpriteBatch.DrawString(
            _font,              // spriteFont
            $"Score: {_score}", // text
            _scoreTextPosition, // position
            Color.White,        // color
            0.0f,               // rotation
            _scoreTextOrigin,   // origin
            1.0f,               // scale
            SpriteEffects.None, // effects
            0.0f                // layerDepth
        );

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();

        // Draw the Gum UI
        GumService.Default.Draw();
    }
}
