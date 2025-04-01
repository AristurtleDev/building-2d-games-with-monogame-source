using DungeonSlime.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.UI;

namespace DungeonSlime.UI;

public class GameOverMenu : UIElement
{
    // The UI button used to resume gameplay.
    private UIButton _resumeButton;

    // The UI button used to quit gameplay.
    private UIButton _quitButton;

    // The sound effect to play when a UI action is performed.
    private SoundEffect _uiSoundEffect;

    public GameOverMenu()
    {
        CreateChildren();
    }

    private void CreateChildren()
    {
        // Load the ui texture atlas from the XML configuration file.
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/ui-atlas-definition.xml");

        // Create the paused label as a child of this menu.
        UISprite optionsLabel = CreateChild<UISprite>();
        optionsLabel.Sprite = atlas.CreateSprite("paused-label");
        optionsLabel.Position = new Vector2(112, 20);

        // Create the paused panel as a child of this menu
        UISprite pausedPanel = CreateChild<UISprite>();
        pausedPanel.Sprite = atlas.CreateSprite("panel");
        pausedPanel.Position = new Vector2(198, 139);

        // Create the resume button as a child of the paused panel.
        _resumeButton = pausedPanel.CreateChild<UIButton>();
        _resumeButton.NotSelectedSprite = atlas.CreateSprite("resume-button");
        _resumeButton.NotSelectedSprite.CenterOrigin();
        _resumeButton.SelectedSprite = atlas.CreateAnimatedSprite("resume-button-selected");
        _resumeButton.SelectedSprite.CenterOrigin();
        _resumeButton.Position = new Vector2(148, 148);

        // Create the quit button as a child of the paused panel.
        _quitButton = pausedPanel.CreateChild<UIButton>();
        _quitButton.NotSelectedSprite = atlas.CreateSprite("quit-button");
        _quitButton.NotSelectedSprite.CenterOrigin();
        _quitButton.SelectedSprite = atlas.CreateAnimatedSprite("quit-button-selected");
        _quitButton.SelectedSprite.CenterOrigin();
        _quitButton.Position = new Vector2(691, 148);

        // Resume button is enabled by default.
        _resumeButton.IsSelected = true;
        _quitButton.IsSelected = false;

        // Load the sound effect to play when ui actions occur.
        _uiSoundEffect = Core.Content.Load<SoundEffect>("audio/ui");
    }


    public override void Update(GameTime gameTime)
    {
        if (_resumeButton.IsSelected)
        {
            UpdateResumeButton();
        }
        else if (_quitButton.IsSelected)
        {
            UpdateQuitButton();
        }

        base.Update(gameTime);
    }

    private void UpdateResumeButton()
    {
        if (InputProfile.MenuRight())
        {
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
            _resumeButton.IsSelected = false;
            _quitButton.IsSelected = true;
        }
        else if (InputProfile.MenuAccept())
        {
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
            Core.ReturnToCachedScene();
        }
    }

    private void UpdateQuitButton()
    {
        if (InputProfile.MenuLeft())
        {
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
            _resumeButton.IsSelected = true;
            _quitButton.IsSelected = false;
        }
        else if (InputProfile.MenuAccept())
        {
            Core.Audio.PlaySoundEffect(_uiSoundEffect);
            Core.ChangeScene(new MenuScene<TitleMenu>());
        }
    }
}
