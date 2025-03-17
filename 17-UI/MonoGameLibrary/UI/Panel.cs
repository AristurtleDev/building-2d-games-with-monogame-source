// using System.Collections.Generic;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using MonoGameLibrary.Graphics;

// namespace MonoGameLibrary.UI
// {
//     /// <summary>
//     /// A container with a background that uses NineSlice rendering for borders.
//     /// </summary>
//     public class Panel : UIElement
//     {
//         /// <summary>
//         /// Gets or sets the nine-slice used for rendering the panel's border.
//         /// </summary>
//         public NineSlice NineSlice { get; set; }

//         /// <summary>
//         /// Gets or sets the color to tint the panel's nine-slice.
//         /// </summary>
//         public Color Color { get; set; }

//         /// <summary>
//         /// Creates a new Panel with the specified dimensions and nine-slice border.
//         /// </summary>
//         /// <param name="width">The width of the panel.</param>
//         /// <param name="height">The height of the panel.</param>
//         /// <param name="borderNineSlice">The nine-slice to use for borders.</param>
//         public Panel(int width, int height, NineSlice borderNineSlice)
//         {
//             Size = new Vector2(width, height);
//             Color = Color.White;
//             Enabled = true;
//             Visible = true;
//             NineSlice = borderNineSlice;
//         }

//         /// <summary>
//         /// Draws the panel and its children.
//         /// </summary>
//         /// <param name="spriteBatch">The sprite batch to use for drawing.</param>
//         public override void Draw(SpriteBatch spriteBatch)
//         {
//             if (!Visible)
//             {
//                 return;
//             }

//             if (NineSlice != null)
//             {
//                 NineSlice.Draw(spriteBatch, AbsoluteBounds, Color);
//             }

//             // Draw all children
//             base.Draw(spriteBatch);
//         }
//     }
// }
