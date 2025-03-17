using System.Security.AccessControl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.UI;

namespace DungeonSlime.Scenes;

public class OptionsScene : Scene
{

    private NineSlice _panel;
    private Button _slowButton;
    private Button _normalButton;
    private Button _fastButton;


    public override void Initialize()
    {
        // LoadContent is called during base.Initialize().
        base.Initialize();

        // While on the title screen, we can enable exit on escape so the player
        // can close the game by pressing the escape key.
        Core.ExitOnEscape = false;
    }

    public override void LoadContent()
    {
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/atlas-definition.xml");

        SpriteFont font = Core.Content.Load<SpriteFont>("fonts/gameFont");

        // Get the panel texture region
        TextureRegion panelEnabledRegion = atlas.GetRegion("panel");

        TextureRegion buttonRegion = atlas.GetRegion("button");
        TextureRegion buttonFocusedRegion = atlas.GetRegion("button-focused");
        TextureRegion buttonClickedRegion = atlas.GetRegion("button-clicked");

        NineSlice buttonBorder = new NineSlice(buttonRegion, 3);
        NineSlice buttonFocusedBorder = new NineSlice(buttonFocusedRegion, 3);
        NineSlice buttonClickedBorder = new NineSlice(buttonClickedRegion, 3);

        _slowButton = new Button(font, "Slow", 150, 20, 25, 10, buttonBorder, buttonFocusedBorder, buttonClickedBorder);
        _slowButton.Position = new Vector2(100, 100);
        _slowButton.NormalBorder = buttonBorder;
        _slowButton.HoveredBorder = buttonFocusedBorder;
    }

    public override void Update(GameTime gameTime)
    {
        _slowButton.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        // Begin the sprite batch to prepare for rendering.
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _slowButton.Draw(Core.SpriteBatch);

        // Always end the sprite batch when finished.
        Core.SpriteBatch.End();
    }
}
