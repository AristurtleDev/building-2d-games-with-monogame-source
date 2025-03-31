using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UISprite : UIElement
{
    /// <summary>
    /// Gets or Sets the sprite represented by this ui element.
    /// </summary>
    public Sprite Sprite { get; set; }

    /// <summary>
    /// Creates a new ui sprite.
    /// </summary>
    public UISprite() : base() { }

    /// <summary>
    /// Updates this ui sprite.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current update cycle.</param>
    public override void Update(GameTime gameTime)
    {
        // If disabled, short circuit and return back early
        if (!IsEnabled)
        {
            return;
        }

        // If the sprite provided is actually an animated sprite, update it.
        if (Sprite is AnimatedSprite animatedSprite)
        {
            animatedSprite.Update(gameTime);
        }

        // Call base update so that child elements are updated.
        base.Update(gameTime);
    }

    /// <summary>
    /// Draws this ui sprite.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used for rendering.</param>
    public override void Draw(SpriteBatch spriteBatch)
    {
        // If not visible, short circuit and return back early.
        if (!IsVisible)
        {
            return;
        }

        // Only draw the sprite if there is a sprite to draw
        if (Sprite != null)
        {
            // Set the color of the sprite based on the enabled property
            Sprite.Color = IsEnabled ? EnabledColor : DisabledColor;

            // Draw the sprite
            Sprite.Draw(spriteBatch, AbsolutePosition);
        }

        // Call base draw so that child elements are drawn.
        base.Draw(spriteBatch);
    }
}
