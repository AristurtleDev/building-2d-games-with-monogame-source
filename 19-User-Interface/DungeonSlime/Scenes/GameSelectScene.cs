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
    // Tracks the ui element that represent the game select menu.
    private UIElement _menu;

    public GameSelectScene() : base() { }

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
        // Tracks the game options chosen
        GameOptions options;
        options.Speed = GameOptions.SlimeSpeed.Normal;
        options.Mode = GameOptions.GameMode.Normal;

        // Create the root container for the game select menu.
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
        UISprite speedText = speedPanel.CreateChild<UISprite>();
        speedText.Sprite = atlas.CreateSprite("speed-text");
        speedText.Position = new Vector2(42, 42);

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
        UISprite modeText = modePanel.CreateChild<UISprite>();
        modeText.Sprite = atlas.CreateSprite("mode-text");
        modeText.Position = new Vector2(42, 42);

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

        // By setting the disabled color of the root game select menu, it
        // will propagate the value to all child elements
        _menu.DisabledColor = new Color(70, 86, 130, 255);

        // Wire up the actions to perform when the Up Action is triggered
        // for the menu.
        _menu.UpAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (modePanel.IsSelected)
            {
                // The mode panel is selected and the up action was performed,
                // so deselect the mode panel and move the navigation up to the
                // speed panel.
                modePanel.IsEnabled = modePanel.IsSelected = false;
                speedPanel.IsEnabled = speedPanel.IsSelected = true;
            }
            else if (acceptButton.IsSelected)
            {
                // The accept button is selected and the up action was
                // performed, so deselect the accept button and move the
                // navigation up to the mode panel.
                acceptButton.IsSelected = false;
                modePanel.IsEnabled = modePanel.IsSelected = true;
            }
            else if (cancelButton.IsSelected)
            {
                // The cancel button is selected and the up action was
                // performed, so deselect the cancel button and move the
                // navigation up to the mode panel.
                cancelButton.IsSelected = false;
                modePanel.IsEnabled = modePanel.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Down action is triggered
        // for the menu.
        _menu.DownAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (speedPanel.IsSelected)
            {
                // The speed panel is selected and the down action was
                // performed, so deselect the speed panel and move the
                // navigation down to the mode panel.
                speedPanel.IsEnabled = speedPanel.IsSelected = false;
                modePanel.IsEnabled = modePanel.IsSelected = true;
            }
            else if (modePanel.IsSelected)
            {
                // The mode panel is selected and the down action was
                // performed, so deselect the mode panel and move the
                // navigation down to the accept button.
                modePanel.IsEnabled = modePanel.IsSelected = false;
                acceptButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Left action is triggered
        // for the menu.
        _menu.LeftAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (speedPanel.IsSelected)
            {
                // The speed panel is selected and the left action was
                // performed, so change the speed options so it reduces
                // the speed based on the current speed chosen.
                if (options.Speed == GameOptions.SlimeSpeed.Normal)
                {
                    normalSpeedButton.IsSelected = false;
                    slowSpeedButton.IsSelected = true;
                    options.Speed = GameOptions.SlimeSpeed.Slow;
                }
                else if (options.Speed == GameOptions.SlimeSpeed.Fast)
                {
                    fastSpeedButton.IsSelected = false;
                    normalSpeedButton.IsSelected = true;
                    options.Speed = GameOptions.SlimeSpeed.Normal;
                }
            }
            else if (modePanel.IsSelected)
            {
                // The mode panel is select and the left action was performed,
                // so change the mode option from dark to normal.
                if (options.Mode == GameOptions.GameMode.Dark)
                {
                    darkModeButton.IsSelected = false;
                    normalModeButton.IsSelected = true;
                    options.Mode = GameOptions.GameMode.Normal;
                }
            }
            else if (cancelButton.IsSelected)
            {
                // The cancel button is selected and the left action was
                // performed, so deselect the cancel button and select the
                // accept button.
                cancelButton.IsSelected = false;
                acceptButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Right action is triggered
        // for the menu.
        _menu.RightAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (speedPanel.IsSelected)
            {
                // The speed panel is selected and the right action was
                // performed, so change the speed options so it increases
                // the speed based on the current speed chosen.
                if (options.Speed == GameOptions.SlimeSpeed.Slow)
                {
                    slowSpeedButton.IsSelected = false;
                    normalSpeedButton.IsSelected = true;
                    options.Speed = GameOptions.SlimeSpeed.Normal;
                }
                else if (options.Speed == GameOptions.SlimeSpeed.Normal)
                {
                    normalSpeedButton.IsSelected = false;
                    fastSpeedButton.IsSelected = true;
                    options.Speed = GameOptions.SlimeSpeed.Fast;
                }
            }
            else if (modePanel.IsSelected)
            {
                // The mode panel is select and the right action was performed,
                // so change the mode option from normal to dark.
                if (options.Mode == GameOptions.GameMode.Normal)
                {
                    normalModeButton.IsSelected = false;
                    darkModeButton.IsSelected = true;
                    options.Mode = GameOptions.GameMode.Dark;
                }
            }
            else if (acceptButton.IsSelected)
            {
                // The accept button is selected and the right action was
                // performed, so deselect the accept button and select the
                // cancel button.
                acceptButton.IsSelected = false;
                cancelButton.IsSelected = true;
            }
        };

        // Wire up the actions to perform when the Confirm action is triggered
        // for the menu.
        _menu.ConfirmAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (speedPanel.IsSelected)
            {
                // The speed panel is selected and the confirm action was
                // performed, so deselect the speed panel and move the
                // navigation to the mode panel.
                speedPanel.IsEnabled = speedPanel.IsSelected = false;
                modePanel.IsEnabled = modePanel.IsSelected = true;
            }
            else if (modePanel.IsSelected)
            {
                // The mode panel is selected and the confirm action was
                // performed, so deselect the mode panel and move the
                // navigation to the accept button.
                modePanel.IsEnabled = modePanel.IsSelected = false;
                acceptButton.IsSelected = true;
            }
            if (acceptButton.IsSelected)
            {
                // The accept button is selected and the confirm action was
                // performed, so change the scene to the game scene using
                // the current options configured.
                Core.ChangeScene(new GameScene(options));
            }
            else if (cancelButton.IsSelected)
            {
                // The cancel button is selected and the confirm action was
                // performed, so change the scene back to the title scene.
                Core.ChangeScene(new TitleScene());
            }
        };

        // Wire up the actions to perform when the Cancel action is triggered
        // for the menu.
        _menu.CancelAction = () =>
        {
            // Play the sound effect
            Core.Audio.PlaySoundEffect(soundEffect);

            if (modePanel.IsSelected)
            {
                // The mode panel is selected and the cancel action was
                // performed, so deselect the mode panel and move the
                // navigation to the speed panel.
                modePanel.IsEnabled = modePanel.IsSelected = false;
                speedPanel.IsEnabled = speedPanel.IsSelected = true;
            }
            else if (acceptButton.IsSelected)
            {
                // The accept button is selected and the cancel action was
                // performed, so deselect the accept button and move the
                // navigation to the mode panel.
                acceptButton.IsSelected = false;
                modePanel.IsEnabled = modePanel.IsSelected = true;
            }
            else if (cancelButton.IsSelected)
            {
                // The cancel button is selected and the cancel action was
                // performed, so deselect the cancel button and move the
                // navigation to the mode panel.
                acceptButton.IsSelected = false;
                modePanel.IsEnabled = modePanel.IsSelected = true;
            }
        };
    }

    public override void Update(GameTime gameTime)
    {
        // Update the menu.
        _menu.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _menu.Draw(Core.SpriteBatch);
        Core.SpriteBatch.End();
    }
}
