using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.UI;

public class UIElement
{
    private Dictionary<string, UIElement> _children;

    public string Name { get; }
    public UIElement Parent { get; private set; }
    public Point Position { get; set; }
    public Point AbsolutePosition
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

    public bool Enabled { get; set; }
    public bool Visible { get; set; }

    public UIElement(string name)
    {
        _children = new Dictionary<string, UIElement>();
        Name = name;
        Enabled = true;
        Visible = true;
    }

    public UIElement(string name, UIElement parent)
        : this(name)
    {
        Parent = parent;
    }

    public void AddChild(UIElement child)
    {
        if (child.Parent != null)
        {
            child.Parent.RemoveChild(child);
        }

        _children.Add(child.Name, child);
        child.Parent = this;
    }

    public void RemoveChild(UIElement child)
    {
        if (_children.Remove(child.Name))
        {
            child.Parent = null;
        }
    }

    public UIElement GetChild(string name)
    {
        return _children[name];
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (UIElement child in _children.Values)
        {
            child.Update(gameTime);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {

        foreach (UIElement child in _children.Values)
        {
            child.Draw(spriteBatch);
        }

    }
}
