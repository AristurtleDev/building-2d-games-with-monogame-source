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
    private RenderTarget2D _backgroundRenderTarget;
    private TextureRegion _backgroundTextureRegion;
    private Rectangle _backgroundDestination;
    private Vector2 _backgroundOffset;
    private float _scrollSpeed = 50.0f;

    private Sprite _gameOptionsSprite;
    private Sprite _modeSprite;
    private Sprite _speedSprite;
    private Sprite _panelSprite;
    private Sprite _disabledSlow;
    private Sprite _disabledNormal;
    private Sprite _disabledFast;
    private Sprite _disabledDark;
    private AnimatedSprite _selectedSlow;
    private AnimatedSprite _selectedNormal;
    private AnimatedSprite _selectedFast;
    private AnimatedSprite _selectedDark;


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
    }

    public override void LoadContent()
    {
        TextureAtlas uiAtlas = TextureAtlas.FromFile(Content, "images/ui-atlas-definition.xml");
        _modeSprite = uiAtlas.CreateSprite("mode");
        _speedSprite = uiAtlas.CreateSprite("speed");
        _gameOptionsSprite = uiAtlas.CreateSprite("game-options");
        _panelSprite = uiAtlas.CreateSprite("panel");
        _disabledSlow = uiAtlas.CreateSprite("slow-disabled");
        _disabledNormal = uiAtlas.CreateSprite("normal-disabled");
        _disabledFast = uiAtlas.CreateSprite("fast-disabled");
        _disabledDark = uiAtlas.CreateSprite("dark-disabled");
        _selectedSlow = uiAtlas.CreateAnimatedSprite("slow-selected");
        _selectedNormal = uiAtlas.CreateAnimatedSprite("normal-selected");
        _selectedFast = uiAtlas.CreateAnimatedSprite("fast-selected");
        _selectedDark = uiAtlas.CreateAnimatedSprite("dark-selected");
        _backgroundTextureRegion = uiAtlas.GetRegion("background");
        _backgroundRenderTarget = new RenderTarget2D(Core.GraphicsDevice, _backgroundTextureRegion.Width, _backgroundTextureRegion.Height);
    }

    public override void Update(GameTime gameTime)
    {
        _backgroundOffset.X += _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _backgroundOffset.Y -= _scrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _backgroundOffset.X %= _backgroundTextureRegion.Width;
        _backgroundOffset.Y %= _backgroundTextureRegion.Height;

        _selectedNormal.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.SetRenderTarget(_backgroundRenderTarget);
        Core.GraphicsDevice.Clear(Color.Black);

        Core.SpriteBatch.Begin();
        _backgroundTextureRegion.Draw(Core.SpriteBatch, Vector2.Zero, Color.White);
        Core.SpriteBatch.End();

        Core.GraphicsDevice.SetRenderTarget(null);


        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointWrap, blendState: BlendState.AlphaBlend);
        Core.SpriteBatch.Draw(_backgroundRenderTarget, _backgroundDestination, new Rectangle(_backgroundOffset.ToPoint(), _backgroundDestination.Size), Color.White);
        Core.SpriteBatch.End();

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _gameOptionsSprite.Draw(Core.SpriteBatch, new Vector2(112, 20));
        _panelSprite.Draw(Core.SpriteBatch, new Vector2(198, 139));
        _panelSprite.Draw(Core.SpriteBatch, new Vector2(198, 406));
        _modeSprite.Draw(Core.SpriteBatch, new Vector2(240, 448));
        _speedSprite.Draw(Core.SpriteBatch, new Vector2(240, 181));
        _disabledSlow.Draw(Core.SpriteBatch, new Vector2(238, 273));
        _selectedNormal.Draw(Core.SpriteBatch, new Vector2(495, 273));
        _disabledFast.Draw(Core.SpriteBatch, new Vector2(790, 273));
        _selectedNormal.Draw(Core.SpriteBatch, new Vector2(379, 531));
        _disabledDark.Draw(Core.SpriteBatch, new Vector2(684, 531));
        Core.SpriteBatch.End();

    }
}
