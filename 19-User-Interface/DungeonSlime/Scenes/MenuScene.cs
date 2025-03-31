using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.UI;

namespace DungeonSlime.Scenes;

public class MenuScene<T> : Scene where T : UIElement, new()
{
    private T _menu;

    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on a menu screen, we disable exit on escape so that we can use
        // the escape key for other actions.
        Core.ExitOnEscape = false;
    }

    public override void LoadContent()
    {
        // Create the menu
        _menu = new T();
    }

    public override void Update(GameTime gameTime)
    {
        // Update the menu
        _menu.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        // Draw the menu.
        _menu.Draw(Core.SpriteBatch);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
}
