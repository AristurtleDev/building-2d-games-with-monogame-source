using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.UI;

public abstract class UIElement : IEnumerable<UIElement>
{
    private List<UIElement> _children;
    private bool _isEnabled;
    private bool _isVisible;
    private Color _enabledColor;
    private Color _disabledColor;

    /// <summary>
    /// Gets the ui element that is the parent of this ui element, or null of there is no parent.
    /// </summary>
    public UIElement Parent { get; private set; }

    /// <summary>
    /// Gets or Sets the position of this element.
    /// </summary>
    /// <remarks>
    /// If this element is a child element, this position is relative to the position of the parent.
    /// </remarks>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Gets the position of this element relative to the game screen.
    /// </summary>
    public Vector2 AbsolutePosition
    {
        get
        {
            if (Parent == null)
            {
                return Position;
            }

            return Parent.AbsolutePosition + Position;
        }
    }

    /// <summary>
    /// Gets or Sets a value that indicates whether this UI ELement is enabled.
    /// </summary>
    /// <remarks>
    /// If this UI element is a child of another UI element, and that parent is
    /// not enabled, this this will return false.
    /// </remarks>
    public bool IsEnabled
    {
        get
        {
            // If there is a parent element, and if that parent element is not
            // enabled, then return false automatically since all children of
            // a disabled parent are also disabled.
            if (Parent is UIElement parent && !parent.IsEnabled)
            {
                return false;
            }

            // Otherwise, there is either no parent element or the parent element
            // is enabled, in which case we just return back the enabled state
            // of this element
            return _isEnabled;

        }
        set => _isEnabled = value;
    }

    /// <summary>
    /// Gets or Sets a value that indicates whether this UI element is visible.
    /// </summary>
    /// <remarks>
    /// If this UI element is a child of another UI element, and that parent is
    /// not visible, then this will return false.
    /// </remarks>
    public bool IsVisible
    {
        get
        {
            // If there is a parent element, and if that parent element is not
            // visible, then return false automatically since all children of
            // a non-visible parent are also not visible.
            if (Parent is UIElement parent && !parent.IsVisible)
            {
                return false;
            }

            // Otherwise, there is either no parent element or the parent element
            // is visible, in which case, we just return back the visual state
            // of this element
            return _isVisible;
        }
        set => _isVisible = true;
    }

    /// <summary>
    /// Gets or Sets the color mask to apply to this element and all its children
    //  when it is enabled.
    /// </summary>
    /// <remarks>
    /// Default value is Color.White.
    /// </remarks>
    public Color EnabledColor
    {
        get => _enabledColor;
        set
        {
            // If the color being set is the same color that is already set,
            // short circuit and return early so we don't waste time cycling
            // through all children.
            if (_enabledColor == value)
            {
                return;
            }

            _enabledColor = value;

            // Update enabled color of each child ui element to match
            // for visual consistency.
            foreach (UIElement child in this)
            {
                child.EnabledColor = value;
            }
        }
    }

    /// <summary>
    /// Gets or Sets the color mask to apply to this element and all its children
    //  when it is disabled.
    /// </summary>
    /// <remarks>
    /// Default value is Color.White.
    /// </remarks>
    public Color DisabledColor
    {
        get => _disabledColor;
        set
        {
            // If the color being set is the same color that is already set,
            // short circuit and return early so we don't waste time cycling
            // through all children.
            if (_disabledColor == value)
            {
                return;
            }

            _disabledColor = value;

            // Update the disabled color of each child ui element to match
            // for visual consistency.
            foreach (UIElement child in this)
            {
                child.DisabledColor = value;
            }
        }
    }

    /// <summary>
    /// Creates a new ui element with an optional parent.
    /// </summary>
    public UIElement()
    {
        _children = new List<UIElement>();
        IsEnabled = true;
        IsVisible = true;
        EnabledColor = Color.White;
        DisabledColor = Color.White;
    }

    /// <summary>
    /// Adds the given ui element as a child of this ui element.
    /// </summary>
    /// <param name="child">The ui element to add as a child of this ui element.</param>
    public T AddChild<T>() where T : UIElement, new()
    {
        T child = new T();
        _children.Add(child);
        child.Parent = this;
        return child;
    }

    /// <summary>
    /// Removes the given ui element from the children of this ui element.
    /// </summary>
    /// <param name="child">The child element to remove.</param>
    public void RemoveChild(UIElement child)
    {
        // Remove the child from this element's child collection, and if it
        // successful, orphan the child.
        if (_children.Remove(child))
        {
            child.Parent = null;
        }
    }

    /// <summary>
    /// Updates this ui element.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current update cycle.</param>
    public virtual void Update(GameTime gameTime)
    {
        foreach (UIElement child in _children)
        {
            child.Update(gameTime);
        }
    }

    /// <summary>
    /// Draws this ui element.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used to draw.</param>
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        // Draw each child element of this element that is also a visual element.
        foreach (UIElement child in _children)
        {
            child.Draw(spriteBatch);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through each child element in this ui element.
    /// </summary>
    /// <returns>An enumerator that iterates through each child element in this ui element.</returns>
    public IEnumerator<UIElement> GetEnumerator() => _children.GetEnumerator();

    /// <summary>
    /// Returns an enumerator that iterates through each child element in this ui element.
    /// </summary>
    /// <returns>An enumerator that iterates through each child element in this ui element.</returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
