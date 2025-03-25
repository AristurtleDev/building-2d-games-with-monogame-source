// using System;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using MonoGameLibrary.Graphics;
// using MonoGameLibrary.Input;

// namespace MonoGameLibrary.UI;

// public abstract class UIButton : UIElement
// {
//     public UIButtonState State { get; private set; }

//     public Action Hovered { get; set; }
//     public Action Pressed { get; set; }
//     public Action Clicked { get; set; }

//     public UIButton(UIElement parent, int width, int height, string label, SpriteFont font)
//         : base(parent, width, height)
//     {
//         _label = label;
//         _font = font;
//     }

//     public void ConfigureState(UIButtonState state, NineSlice border, Color labelColor)
//     {
//         _borders[(int)state] = border;
//         _labelColors[(int)state] = labelColor;
//     }

//     public override void Update(GameTime gameTime)
//     {
//         // Get the mouse info.
//         MouseInfo mouse = Core.Input.Mouse;

//         UIButtonState previousState = State;

//         // Check if mouse is over the button
//         bool isMouseOver = Bounds.Contains(mouse.Position);

//         // Determine the button state based on mouse interaction
//         if (isMouseOver)
//         {
//             if (mouse.IsButtonDown(MouseButton.Left))
//             {
//                 State = UIButtonState.Pressed;
//                 Pressed?.Invoke();
//             }
//             else if (mouse.WasButtonJustReleased(MouseButton.Left))
//             {
//                 State = UIButtonState.Clicked;
//                 Clicked?.Invoke();
//             }
//             else
//             {
//                 State = UIButtonState.Hovered;
//                 Hovered?.Invoke();
//             }
//         }
//         else
//         {
//             State = UIButtonState.Default;
//         }

//         // Update child elements
//         base.Update(gameTime);

//         // Reset clicked state after one frame
//         if (State == UIButtonState.Clicked && previousState != UIButtonState.Clicked)
//         {
//             // Schedule state to return to hovered on next update if mouse still over button
//             State = isMouseOver ? UIButtonState.Hovered : UIButtonState.Default;
//         }
//     }

//     public override void Draw(SpriteBatch spriteBatch)
//     {
//         NineSlice border = _borders[(int)State];

//         if (border != null)
//         {
//             border.Draw(spriteBatch, Bounds, Color.White);
//         }

//         base.Draw(spriteBatch);
//     }


// }
