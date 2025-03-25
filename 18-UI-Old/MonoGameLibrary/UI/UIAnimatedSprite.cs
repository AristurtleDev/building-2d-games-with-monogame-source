using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UIAnimatedSprite : UIElement
{
    public AnimatedSprite AnimatedSprite { get; set; }

    public override Point Size
    {
        get => new Point((int)AnimatedSprite.Width, (int)AnimatedSprite.Height);
        set
        {
            Point currentSize = Size;

            // Calculate the scale factors needed for each axis
            float scaleX = value.X / (float)currentSize.X;
            float scaleY = value.Y / (float)currentSize.Y;

            AnimatedSprite.Scale = new Vector2(scaleX, scaleY);
        }
    }

    public UIAnimatedSprite(UIElement parent, AnimatedSprite animatedSprite)
        : base(parent, (int)animatedSprite.Width, (int)animatedSprite.Height)
    {

    }

    public override void Update(GameTime gameTime)
    {
        AnimatedSprite.Update(gameTime);

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        AnimatedSprite.Draw(spriteBatch, AbsolutePosition.ToVector2());
        base.Draw(spriteBatch);
    }
}
