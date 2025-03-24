using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UIContainer : UIElement
{
    public NineSlice Border { get; set; }

    public UIContainer(UIElement parent, int width, int height, NineSlice border)
        : base(parent, width, height)
    {
        Border = border;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Border.Draw(spriteBatch, Bounds, Color.White);

        base.Draw(spriteBatch);
    }
}
