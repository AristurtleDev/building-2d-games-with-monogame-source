using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UISprite : UIElement
{
    public Sprite Sprite { get; protected set; }

    public Color Color
    {
        get => Sprite.Color;
        set => Sprite.Color = value;
    }

    public UISprite(string name, Sprite sprite, bool centerOrigin = false)
        : base(name)
    {
        Sprite = sprite;

        if (centerOrigin)
        {
            CenterOrigin();
        }
    }

    public UISprite(string name, Sprite sprite, UIElement parent, bool centerOrigin = false)
        : base(name, parent)
    {
        Sprite = sprite;

        if (centerOrigin)
        {
            CenterOrigin();
        }
    }

    public virtual void CenterOrigin()
    {
        Sprite.CenterOrigin();
    }

    public override void Update(GameTime gameTime)
    {
        if (!Enabled)
        {
            return;
        }

        if (Sprite is AnimatedSprite animatedSprite)
        {
            animatedSprite.Update(gameTime);
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!Visible)
        {
            return;
        }

        Sprite.Draw(spriteBatch, Position.ToVector2());

        base.Draw(spriteBatch);
    }
}
