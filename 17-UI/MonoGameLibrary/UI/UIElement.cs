using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.UI;

public class UIElement
{
    private List<UIElement> _children;

    /// <summary>
    /// Get or Sets the ui element that is the parent of this element.
    /// </summary>
    public UIElement Parent { get; set; }

    /// <summary>
    /// Gets or Sets the position of this ui element relative to its parent
    /// element.
    /// </summary>
    public Point LocalPosition { get; set; }

    /// <summary>
    /// Gets the absolute position in screen coordinates of this ui element.
    /// </summary>
    public Point AbsolutePosition
    {
        get
        {
            if (Parent == null)
            {
                return LocalPosition;
            }

            return Parent.AbsolutePosition + LocalPosition;
        }
    }

    public Rectangle Bounds => new Rectangle(AbsolutePosition, Size);

    /// <summary>
    /// Gets or Sets the size, in pixels, of this element.
    /// </summary>
    public Point Size { get; set; }

    /// <summary>
    /// Creates a new ui element.
    /// </summary>
    /// <param name="parent">The parent element, if any.</param>
    /// <param name="width">The width, in pixels, of this ui element..</param>
    /// <param name="height">The height, in pixels, of this ui element.</param>
    public UIElement(UIElement parent, int width, int height)
    {
        _children = new List<UIElement>();

        if (parent != null)
        {
            parent.AddChild(this);
        }

        Size = new Point(width, height);
    }

    /// <summary>
    /// Adds a new child ui element to this ui element.
    /// </summary>
    /// <param name="child">The child to add.</param>
    public void AddChild(UIElement child)
    {
        _children.Add(child);
        child.Parent = this;
    }

    /// <summary>
    /// Removes the specified child ui element from this ui element.
    /// </summary>
    /// <param name="child">The child to remove.</param>
    public void RemoveChild(UIElement child)
    {
        if (_children.Remove(child))
        {
            child.Parent = null;
        }
    }

    /// <summary>
    /// Removes all child ui elements from this ui element.
    /// </summary>
    public void ClearChildren()
    {
        foreach (UIElement child in _children)
        {
            child.Parent = null;
        }

        _children.Clear();
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (UIElement child in _children)
        {
            child.Update(gameTime);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (UIElement child in _children)
        {
            child.Draw(spriteBatch);
        }
    }
}
