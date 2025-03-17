using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.UI;

public interface IUIElement
{
    IUIElement Parent { get; set; }
    Point Position { get; set; }
    Point Size {get; set;}
    Rectangle Bounds { get; }
    bool Enabled { get; set; }
    bool Visible { get; set; }

    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}
