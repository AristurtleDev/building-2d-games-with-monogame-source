

// using System;
// using System.Collections.Generic;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;


// namespace MonoGameLibrary.UI;

// public abstract class UIElement : IUIElement
// {
//     private Point _position;
//     private Point _size;
//     private bool _enabled;
//     private bool _visible;

//     public IUIElement Parent { get; set; }
//     public Point Position
//     {
//         get => _position;
//         set
//         {
//             if (_position == value)
//             {
//                 return;
//             }

//             _position = value;
//             OnPositionChanged();
//         }
//     }

//     public Point Size
//     {
//         get => _size;
//         set
//         {
//             if (_size == value)
//             {
//                 return;
//             }

//             _size = value;
//             OnSizeChanged();
//         }
//     }

//     public Rectangle Bounds => new Rectangle(Position, Size);

//     public bool Enabled
//     {
//         get => _enabled;
//         set
//         {
//             if (_enabled == value)
//             {
//                 return;
//             }

//             _enabled = value;
//             OnEnabledChanged();
//         }
//     }

//     public bool Visible
//     {
//         get => _visible;
//         set
//         {
//             if (_visible == value)
//             {
//                 return;
//             }

//             _visible = value;
//             OnVisibleChanged();
//         }
//     }

//     public virtual void Update(GameTime gameTime) { }
//     public virtual void Draw(SpriteBatch spriteBatch)
//     {
//         if(!Visible)
//         {
//             return;
//         }


//     }
// }
