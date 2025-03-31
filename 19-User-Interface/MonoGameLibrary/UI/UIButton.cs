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
    public bool IsSelected { get; set; }

    /// <summary>
    /// Creates a new ui button.
    /// </summary>
    public UIButton() : base() { }

    public override void Update(GameTime gameTime)
    {
        if (NotSelectedSprite is AnimatedSprite notSelectedSprite)
        {
            notSelectedSprite.Update(gameTime);
        }

        if (SelectedSprite is AnimatedSprite selectedSprite)
        {
            selectedSprite.Update(gameTime);
        }

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // If not visual, short circuit and return back early.
        if (!IsVisible)
        {
            return;
        }

        // Draw the selected or not selected sprite based on the selected property.
        if (IsSelected)
        {
            // Set the color of the selected sprite based on the enabled property.
            SelectedSprite.Color = IsEnabled ? EnabledColor : DisabledColor;

            // Draw the selected sprite.
            SelectedSprite.Draw(spriteBatch, AbsolutePosition);
        }
        else
        {
            // Set the color of the not selected sprite based on the enabled property.
            NotSelectedSprite.Color = IsEnabled ? EnabledColor : DisabledColor;

            // Draw the not selected sprite.
            NotSelectedSprite.Draw(spriteBatch, AbsolutePosition);
        }

        // Call base draw so that child elements are drawn.
        base.Draw(spriteBatch);
    }
}
