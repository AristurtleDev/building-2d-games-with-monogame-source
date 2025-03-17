

// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;

// namespace MonoGameLibrary.UI;

// /// <summary>
// /// A UI element that displays text.
// /// </summary>
// public class Label : UIElement
// {
//     private string _text;
//     private SpriteFont _font;


//     /// <summary>
//     /// Gets or sets the text displayed by the label.
//     /// </summary>
//     public string Text
//     {
//         get => _text;
//         set
//         {
//             _text = value;
//             UpdateSize();
//         }
//     }

//     /// <summary>
//     /// Gets or sets the font used to render the text.
//     /// </summary>
//     public SpriteFont Font
//     {
//         get => _font;
//         set
//         {
//             _font = value;
//             UpdateSize();
//         }
//     }

//     /// <summary>
//     /// Gets or sets the color of the text.
//     /// </summary>
//     public Color TextColor { get; set; } = Color.White;

//     /// <summary>
//     /// Gets or sets the origin point for rendering the text.
//     /// </summary>
//     public Vector2 Origin { get; set; } = Vector2.Zero;

//     /// <summary>
//     /// Gets or sets the text alignment.
//     /// </summary>
//     public TextAlignment Alignment { get; set; } = TextAlignment.Left;

//     /// <summary>
//     /// Creates a new Label with the specified text and font.
//     /// </summary>
//     /// <param name="text">The text to display.</param>
//     /// <param name="font">The font to use for rendering.</param>
//     public Label(string text, SpriteFont font)
//     {
//         Text = text;
//         Font = font;

//         // Set dimensions based on text size
//         Size = font.MeasureString(text);
//     }

//     /// <summary>
//     /// Creates a new Label with the specified text, font, and color.
//     /// </summary>
//     /// <param name="text">The text to display.</param>
//     /// <param name="font">The font to use for rendering.</param>
//     /// <param name="textColor">The color of the text.</param>
//     public Label(string text, SpriteFont font, Color textColor)
//         : this(text, font)
//     {
//         TextColor = textColor;
//     }

//     /// <summary>
//     /// Recalculates label dimensions based on current text and font.
//     /// </summary>
//     protected virtual void UpdateSize()
//     {
//         if (_font == null || string.IsNullOrEmpty(_text))
//         {
//             Size = Vector2.Zero;
//             return;
//         }

//         Size = _font.MeasureString(_text);
//     }

//     /// <summary>
//     /// Draws the label.
//     /// </summary>
//     /// <param name="spriteBatch">The sprite batch to use for drawing.</param>
//     public override void Draw(SpriteBatch spriteBatch)
//     {
//         if (!Visible || Font == null || string.IsNullOrEmpty(_text))
//             return;

//         Vector2 position = Position;

//         if(Parent != null)
//         {
//             position += Parent.AbsolutePosition;
//         }

//         // Adjust position based on alignment
//         Vector2 textSize = Font.MeasureString(Text);

//         switch (Alignment)
//         {
//             case TextAlignment.Center:
//                 position.X += (Size.X - textSize.X) / 2;
//                 break;

//             case TextAlignment.Right:
//                 position.X += Size.X - textSize.X;
//                 break;

//                 // Left alignment is the default, so no adjustment needed
//         }

//         // Draw the text
//         spriteBatch.DrawString(Font, Text, position, TextColor, 0f, Origin, 1f, SpriteEffects.None, 0f);

//         // Draw children
//         base.Draw(spriteBatch);
//     }
// }
