using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UIButton : UIElement
{
    /// <summary>
    /// Gets or Sets the sprite to draw when this ui button is not selected.
    /// </summary>
    public Sprite NotSelectedSprite { get; set; }

    /// <summary>
    /// Gets or Sets the sprite to draw when this ui button is
    /// </summary>
    public Sprite SelectedSprite { get; set; }

    /// <summary>
    /// Gets or Sets a value that indicates whether this ui button is selected.
    /// </summary>
    public bool IsSelected {get; set;}

    /// <summary>
    /// Creates a new ui button.
    /// </summary>
    public UIButton() : base() { }

    public override void Update(GameTime gameTime)
    {
        if(NotSelectedSprite is AnimatedSprite notSelectedSprite)
        {
            notSelectedSprite.Update(gameTime);
        }

        if(SelectedSprite is AnimatedSprite selectedSprite)
        {
            selectedSprite.Update(gameTime);
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if(IsSelected)
        {
            SelectedSprite.Draw(spriteBatch, AbsolutePosition);
        }
        else
        {
            NotSelectedSprite.Draw(spriteBatch, AbsolutePosition);
        }

        base.Draw(spriteBatch);
    }
}
