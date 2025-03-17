using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.UI;

public class Panel : UIElement
{
    public Color BackgroundColor { get; set; }
    public Color BorderColor { get; set; }
    public int BorderThickness { get; set; }
    public string Title { get; set; }
    public Color TitleColor { get; set; }
    public List<UIElement> Children { get; private set; }

    public Panel(Vector2 position, Vector2 size) : base(position, size)
    {
        Children = new List<UIElement>();
        BackgroundColor = new Color(0, 0, 0, 200);
        BorderColor = Color.White;
        BorderThickness = 2;
        TitleColor = Color.White;
    }

    public void AddChild(UIElement child)
    {
        Children.Add(child);
    }

    public override void Update(GameTime gameTime)
    {
        foreach(UIElement child in Children)
        {
            child.Update(gameTime);
        }
    }
}
