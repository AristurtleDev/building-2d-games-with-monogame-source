using System;
using DungeonSlime.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;

namespace DungeonSlime.Scenes;

public class OptionsScene : Scene
{
    private OptionsMenu _optionsMenu;


    public override void Initialize()
    {
        _optionsMenu = new OptionsMenu();

        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the options screen, we disable exit on escape so that we can
        // use the escape key for other actions.
        Core.ExitOnEscape = false;

        _optionsMenu.Initialize();
        _optionsMenu.Accepted += OnOptionsAccepted;
        _optionsMenu.Cancelled += OnOptionsCancelled;
    }

    private void OnOptionsAccepted(object sender, EventArgs e)
    {
        Core.ChangeScene(new TitleScene());
    }

    private void OnOptionsCancelled(object sender, EventArgs e)
    {
        Core.ChangeScene(new TitleScene());
    }

    public override void LoadContent()
    {
        _optionsMenu.LoadContent(Core.Content);
    }

    public override void Update(GameTime gameTime)
    {
        _optionsMenu.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        SpriteBatch spriteBatch = Core.SpriteBatch;

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _optionsMenu.Draw(spriteBatch);
        spriteBatch.End();
    }
}
