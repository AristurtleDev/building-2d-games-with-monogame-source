using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UILabel : UIElement
{
    private string _text;
    private SpriteFont _font;
    private Vector2 _textSize;
    private TextAlignment _alignment;

    public UILabel(UIElement parent, string text, SpriteFont font, TextAlignment alignment, NineSlice border)
        : base(parent, 0, 0)
    {
        _font = font;
        _text = text;

        _textSize = _font.MeasureString(_text);
        Size = _textSize.ToPoint();

        if (border != null)
        {
            // Add double the height and double the width of a character in the
            // font as padding around the text
            Vector2 glyphSize = _font.MeasureString("A") * 2.0f;
            Size = new Point(Size.X + (int)glyphSize.X * 2, Size.Y + (int)glyphSize.Y * 2);
        }
    }


}
