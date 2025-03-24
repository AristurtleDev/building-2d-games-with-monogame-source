using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UISprite : UIElement
{
    public required Sprite Sprite { get; init; }

    public override Point Size
    {
        get => new Point((int)Sprite.Width, (int)Sprite.Height);
        set
        {
            Point currentSize = Size;

            // Calculate the scale factors needed for each axis
            float scaleX = value.X / (float)currentSize.X;
            float scaleY = value.Y / (float)currentSize.Y;

            Sprite.Scale = new Vector2(scaleX, scaleY);
        }
    }

    public UISprite(UIElement parent, Sprite sprite)
        : base(parent, (int)sprite.Width, (int)sprite.Height)
    {
        Sprite = sprite;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Sprite.Draw(spriteBatch, AbsolutePosition.ToVector2());
        base.Draw(spriteBatch);
    }
}
