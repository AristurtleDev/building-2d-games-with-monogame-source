using System;
using DungeonSlime.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.UI;

namespace DungeonSlime.Scenes;

public class TitleScene : Scene
{
    private TitleMenu _titleMenu;

    public override void Initialize()
    {
        _titleMenu = new TitleMenu();

        // LoadContent is called during base.Initialize().
        base.Initialize();

        _titleMenu.Initialize();

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = true;
    }

    public override void LoadContent()
    {
        _titleMenu.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        _titleMenu.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _titleMenu.Draw(Core.SpriteBatch);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
}
