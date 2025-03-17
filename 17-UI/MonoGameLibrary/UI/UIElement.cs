using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.UI;

public abstract class UIElement
{
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public Rectangle Bounds => new Rectangle(Position.ToPoint(), Size.ToPoint());

    public UIElement(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;
    }

    public virtual void Update(GameTime gameTime) { }
    public virtual void Draw(GameTime gameTime) { }
    public bool Contains(Point point) => Bounds.Contains(point);
    public bool Contains(Vector2 position) => Contains(position.ToPoint());

}
