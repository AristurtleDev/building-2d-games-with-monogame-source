using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Core
{
    private Texture2D _rotate;

    // The MonoGame logo texture
    private Texture2D _logo;

    private Texture2D _pixel;

    private float _rotation;
    private float _speed = 2.0f;

    public Game1() : base("Dungeon Slime", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _logo = Content.Load<Texture2D>("images/logo");
        _rotate = Content.Load<Texture2D>("images/rotate");
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData<Color>(new Color[] { Color.White });

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _rotation += (float)gameTime.ElapsedGameTime.TotalSeconds * _speed;

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Clear the back buffer.
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Begin the sprite batch to prepare for rendering.
        SpriteBatch.Begin(SpriteSortMode.FrontToBack);

        // Draw only the icon portion of the texture.
        SpriteBatch.Draw(
            _logo,              // texture
            new Vector2(        // position
                Window.ClientBounds.Width,
                Window.ClientBounds.Height) * 0.5f,
            null,     // sourceRectangle
            Color.White,        // color
            _rotation,               // rotation
            Vector2.Zero,
            1.0f,               // scale
            SpriteEffects.None, // effects
            1.0f                // layerDepth
        );



        Rectangle origin = new Rectangle(0, 0, 10, 10);
        SpriteBatch.Draw(
            _pixel,
            new Vector2(
                Window.ClientBounds.Width,
                Window.ClientBounds.Height
            ) * 0.5f,
            null,
            Color.MonoGameOrange,
            0.0f,
            new Vector2(0.5f, 0.5f),
            10.0f,
            SpriteEffects.None,
            0.0f)
            ;
        SpriteBatch.Draw(
            _rotate,
            new Vector2(
                Window.ClientBounds.Width,
                Window.ClientBounds.Height
            ) * 0.5f,
            null,
            Color.White,
            _rotation,
            new Vector2(_rotate.Width, _rotate.Height) * 0.5f,
            1.0f,
            SpriteEffects.None,
            1.0f

        );

        // Always end the sprite batch when finished.
        SpriteBatch.End();

        base.Draw(gameTime);
    }


}
