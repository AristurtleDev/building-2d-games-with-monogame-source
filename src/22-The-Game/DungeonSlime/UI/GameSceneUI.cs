using System;
using Gum.DataTypes;
using Gum.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using MonoGameGum;
using MonoGameGum.Forms.Controls;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime.UI;

public class GameSceneUI : ContainerRuntime
{
    // The string format to use when updating the text for the score display.
    private static readonly string s_scoreFormat = "SCORE: {0:D6}";

    // The sound effect to play for auditory feedback of the user interface.
    private SoundEffect _uiSoundEffect;

    // A colored rectangle used to overlay the game scene when the pause or
    // game over panels are visible.
    private ColoredRectangleRuntime _overlay;

    // The pause panel
    private Panel _pausePanel;

    // The resume button on the pause panel. Field is used to track reference so
    // focus can be set when the pause panel is shown.
    private AnimatedButton _resumeButton;

    // The game over panel.
    private Panel _gameOverPanel;

    // The retry button on the game over panel. Field is used to track reference
    // so focus can be set when the game over panel is shown.
    private AnimatedButton _retryButton;

    // The text runtime used to display the players score on the game screen.
    private TextRuntime _scoreText;

    /// <summary>
    /// Event invoked when the Resume button on the Pause panel is clicked.
    /// </summary>
    public event EventHandler ResumeButtonClick;

    /// <summary>
    /// Event invoked when the Quit button on either the Pause panel or the
    /// Game Over panel is clicked.
    /// </summary>
    public event EventHandler QuitButtonClick;

    /// <summary>
    /// Event invoked when the Retry button on the Game Over panel is clicked.
    /// </summary>
    public event EventHandler RetryButtonClick;

    /// <summary>
    /// Creates a new GameSceneUI instance.
    /// </summary>
    public GameSceneUI()
    {
        // The game scene UI inherits from ContainerRuntime, so we set its
        // doc to fill so it fills the entire screen.
        Dock(Gum.Wireframe.Dock.Fill);

        // Add it to the root element.
        this.AddToRoot();

        // Get a reference to the content manager that was registered with the
        // GumService when it was original initialized.
        ContentManager content = GumService.Default.ContentLoader.XnaContentManager;

        // Use that content manager to load the sound effect and atlas for the
        // user interface elements
        _uiSoundEffect = content.Load<SoundEffect>("audio/ui");
        TextureAtlas atlas = TextureAtlas.FromFile(content, "images/atlas-definition.xml");

        // Create the text that will display the players score and add it as
        // a child to this container.
        CreateScoreText();
        AddChild(_scoreText);

        // Create the background overlay that fills the container with a
        // transparent blue background when the pause panel or game over
        // panel is shown to visually take focus away from the active game and
        // add it as a child to this container.
        CreateOverlay();
        AddChild(_overlay);

        // Create the Pause panel that is displayed when the game is paused and
        // add it as a child to this container
        CreatePausePanel(atlas);
        AddChild(_pausePanel.Visual);

        // Create the Game Over panel that is displayed when a game over occurs
        // and add it as a child to this container
        CreateGameOverPanel(atlas);
        AddChild(_gameOverPanel.Visual);
    }

    private void CreateScoreText()
    {
        _scoreText = new TextRuntime();
        _scoreText.Anchor(Gum.Wireframe.Anchor.TopLeft);
        _scoreText.WidthUnits = DimensionUnitType.RelativeToChildren;
        _scoreText.X = 20.0f;
        _scoreText.Y = 5.0f;
        _scoreText.UseCustomFont = true;
        _scoreText.CustomFontFile = @"fonts/04b_30.fnt";
        _scoreText.FontScale = 0.25f;
        _scoreText.Text = string.Format(s_scoreFormat, 0);
    }

    private void CreateOverlay()
    {
        _overlay = new ColoredRectangleRuntime();
        _overlay = new ColoredRectangleRuntime();
        _overlay.Dock(Gum.Wireframe.Dock.Fill);
        _overlay.Red = 20;
        _overlay.Green = 23;
        _overlay.Blue = 47;
        _overlay.Alpha = 175;
        _overlay.Visible = false;
    }

    private void CreatePausePanel(TextureAtlas atlas)
    {
        _pausePanel = new Panel();
        _pausePanel.Anchor(Gum.Wireframe.Anchor.Center);
        _pausePanel.Visual.WidthUnits = DimensionUnitType.Absolute;
        _pausePanel.Visual.HeightUnits = DimensionUnitType.Absolute;
        _pausePanel.Visual.Width = 264.0f;
        _pausePanel.Visual.Height = 70.0f;
        _pausePanel.IsVisible = false;

        TextureRegion backgroundRegion = atlas.GetRegion("panel-background");

        NineSliceRuntime background = new NineSliceRuntime();
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Texture = backgroundRegion.Texture;
        background.TextureAddress = TextureAddress.Custom;
        background.TextureHeight = backgroundRegion.Height;
        background.TextureWidth = backgroundRegion.Width;
        background.TextureTop = backgroundRegion.SourceRectangle.Top;
        background.TextureLeft = backgroundRegion.SourceRectangle.Left;
        _pausePanel.AddChild(background);

        TextRuntime text = new TextRuntime();
        text.Text = "PAUSED";
        text.UseCustomFont = true;
        text.CustomFontFile = "fonts/04b_30.fnt";
        text.FontScale = 0.5f;
        text.X = 10.0f;
        text.Y = 10.0f;
        _pausePanel.AddChild(text);

        _resumeButton = new AnimatedButton(atlas);
        _resumeButton.Text = "RESUME";
        _resumeButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        _resumeButton.Visual.X = 9.0f;
        _resumeButton.Visual.Y = -9.0f;

        _resumeButton.Click += OnResumeButtonClicked;
        _resumeButton.GotFocus += OnElementGotFocus;

        _pausePanel.AddChild(_resumeButton);

        AnimatedButton quitButton = new AnimatedButton(atlas);
        quitButton.Text = "QUIT";
        quitButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        quitButton.Visual.X = -9.0f;
        quitButton.Visual.Y = -9.0f;

        quitButton.Click += OnQuitButtonClicked;
        quitButton.GotFocus += OnElementGotFocus;

        _pausePanel.AddChild(quitButton);
    }

    private void CreateGameOverPanel(TextureAtlas atlas)
    {
        _gameOverPanel = new Panel();
        _gameOverPanel.Anchor(Gum.Wireframe.Anchor.Center);
        _gameOverPanel.Visual.WidthUnits = DimensionUnitType.Absolute;
        _gameOverPanel.Visual.HeightUnits = DimensionUnitType.Absolute;
        _gameOverPanel.Visual.Width = 264.0f;
        _gameOverPanel.Visual.Height = 70.0f;
        _gameOverPanel.IsVisible = false;

        TextureRegion backgroundRegion = atlas.GetRegion("panel-background");

        NineSliceRuntime background = new NineSliceRuntime();
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Texture = backgroundRegion.Texture;
        background.TextureAddress = TextureAddress.Custom;
        background.TextureHeight = backgroundRegion.Height;
        background.TextureWidth = backgroundRegion.Width;
        background.TextureTop = backgroundRegion.SourceRectangle.Top;
        background.TextureLeft = backgroundRegion.SourceRectangle.Left;
        _gameOverPanel.AddChild(background);

        TextRuntime text = new TextRuntime();
        text.Text = "GAME OVER";
        text.UseCustomFont = true;
        text.CustomFontFile = "fonts/04b_30.fnt";
        text.FontScale = 0.5f;
        text.X = 10.0f;
        text.Y = 10.0f;
        _gameOverPanel.AddChild(text);

        _retryButton = new AnimatedButton(atlas);
        _retryButton.Text = "RETRY";
        _retryButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        _retryButton.Visual.X = 9.0f;
        _retryButton.Visual.Y = -9.0f;

        _retryButton.Click += OnRetryButtonClicked;
        _retryButton.GotFocus += OnElementGotFocus;

        _gameOverPanel.AddChild(_retryButton);

        AnimatedButton quitButton = new AnimatedButton(atlas);
        quitButton.Text = "QUIT";
        quitButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        quitButton.Visual.X = -9.0f;
        quitButton.Visual.Y = -9.0f;

        quitButton.Click += OnQuitButtonClicked;
        quitButton.GotFocus += OnElementGotFocus;

        _gameOverPanel.AddChild(quitButton);
    }

    private void OnResumeButtonClicked(object sender, EventArgs args)
    {
        // Button was clicked, play the ui sound effect for auditory feedback.
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Since the resume button was clicked, we need to hide the pause panel.
        HidePausePanel();

        // Invoke the ResumeButtonClick event
        if(ResumeButtonClick != null)
        {
            ResumeButtonClick(sender, args);
        }
    }

    private void OnRetryButtonClicked(object sender, EventArgs args)
    {
        // Button was clicked, play the ui sound effect for auditory feedback.
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Since the retry button was clicked, we need to hide the game over panel.
        HideGameOverPanel();

        // Invoke the RetryButtonClick event.
        if(RetryButtonClick != null)
        {
            RetryButtonClick(sender, args);
        }
    }

    private void OnQuitButtonClicked(object sender, EventArgs args)
    {
        // Button was clicked, play the ui sound effect for auditory feedback.
        Core.Audio.PlaySoundEffect(_uiSoundEffect);

        // Both panels have a quit button, so hide both panels
        HidePausePanel();
        HideGameOverPanel();

        // Invoke the QuitButtonClick event.
        if(QuitButtonClick != null)
        {
            QuitButtonClick(sender, args);
        }
    }

    private void OnElementGotFocus(object sender, EventArgs args)
    {
        // A ui element that can receive focus has received focus, play the
        // ui sound effect for auditory feedback.
        Core.Audio.PlaySoundEffect(_uiSoundEffect);
    }

    /// <summary>
    /// Updates the text on the score display.
    /// </summary>
    /// <param name="score">The score to display.</param>
    public void UpdateScoreText(int score)
    {
        _scoreText.Text = string.Format(s_scoreFormat, score);
    }

    /// <summary>
    /// Tells the game scene ui to show the pause panel.
    /// </summary>
    public void ShowPausePanel()
    {
        // Make the overlay and the pause panel visible.
        _overlay.Visible = true;
        _pausePanel.IsVisible = true;

        // Give the resume button focus for keyboard/gamepad input.
        _resumeButton.IsFocused = true;

        // Ensure the game over panel isn't visible.
        _gameOverPanel.IsVisible = false;
    }

    /// <summary>
    /// Tells the game scene ui to hide the pause panel.
    /// </summary>
    public void HidePausePanel()
    {
        // Hide the overlay and the pause panel.
        _overlay.Visible = false;
        _pausePanel.IsVisible = false;
    }

    /// <summary>
    /// Tells the game scene ui to show the game over panel.
    /// </summary>
    public void ShowGameOverPanel()
    {
        // Make the overlay and the game over panel visible.
        _overlay.Visible = true;
        _gameOverPanel.IsVisible = true;

        // Give the retry button focus for keyboard/gamepad input.
        _retryButton.IsFocused =true;

        // Ensure the pause panel isn't visible.
        _pausePanel.IsVisible = false;
    }

    /// <summary>
    /// Tells the game scene ui to hide the game over panel.
    /// </summary>
    public void HideGameOverPanel()
    {
        // Hide the overlay and game over panel.
        _overlay.Visible = false;
        _gameOverPanel.IsVisible = false;
    }

    /// <summary>
    /// Updates the game scene ui.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current update cycle.</param>
    public void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);
    }

    /// <summary>
    /// Draws the game scene ui.
    /// </summary>
    public void Draw()
    {
        GumService.Default.Draw();
    }
}
