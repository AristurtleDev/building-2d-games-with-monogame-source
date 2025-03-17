// using Microsoft.Xna.Framework;
// using MonoGameLibrary.Graphics;

// namespace MonoGameLibrary.UI;

// public class StackPanel : Panel
// {
//     /// <summary>
//     /// Gets or sets the spacing between child elements.
//     /// </summary>
//     public int Spacing { get; set; } = 5;

//     /// <summary>
//     /// Gets or sets the orientation of the stack.
//     /// </summary>
//     public Orientation Orientation { get; set; } = Orientation.Vertical;

//     /// <summary>
//     /// Creates a new StackPanel with the specified dimensions, nine-slice border, and orientation.
//     /// </summary>
//     /// <param name="width">The width of the panel.</param>
//     /// <param name="height">The height of the panel.</param>
//     /// <param name="borderNineSlice">The nine-slice to use for borders.</param>
//     /// <param name="orientation">The orientation of the stack.</param>
//     public StackPanel(int width, int height, NineSlice borderNineSlice, Orientation orientation = Orientation.Vertical)
//         : base(width, height, borderNineSlice)
//     {
//         Orientation = orientation;
//     }

//     /// <summary>
//     /// Adds a child element to this container and positions it in the stack.
//     /// </summary>
//     /// <param name="child">The child element to add.</param>
//     public override void AddChild(IUIElement child)
//     {
//         base.AddChild(child);
//         LayoutChildren();
//     }

//     /// <summary>
//     /// Removes a child element from this container and updates the stack layout.
//     /// </summary>
//     /// <param name="child">The child element to remove.</param>
//     public override void RemoveChild(IUIElement child)
//     {
//         base.RemoveChild(child);
//         LayoutChildren();
//     }

//     /// <summary>
//     /// Updates the position of all children based on the stack orientation.
//     /// </summary>
//     private void LayoutChildren()
//     {
//         int currentX = Bounds.X;
//         int currentY = Bounds.Y;

//         foreach (var child in Children)
//         {
//             // Position the child
//             child.Position = new Vector2(currentX, currentY);

//             // Update the next position based on orientation
//             if (Orientation == Orientation.Vertical)
//             {
//                 currentY += (int)child.Size.Y + Spacing;
//             }
//             else // Horizontal
//             {
//                 currentX += (int)child.Size.X + Spacing;
//             }
//         }
//     }

//     protected override void OnPositionChanged()
//     {
//         base.OnPositionChanged();
//         LayoutChildren();
//     }
// }
