using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UISprite : UIElement
{
    private Color _enabledColor;
    private Color _disabledColor;

    /// <summary>
    /// Gets or Sets the sprite represented by this ui element.
    /// </summary>
    public Sprite Sprite { get; set; }

    /// <summary>
    /// Gets or Sets the color mask to apply to this ui sprite when it is enabled.
    /// </summary>
    /// <remarks>
    /// Default value is Color.White.
    /// </remarks>
    public Color EnabledColor
    {
        get => _enabledColor;
        set
        {
            // If the color being set is the same color that it already is, just
            // return back early.
            if (_enabledColor == value)
            {
                return;
            }

            _enabledColor = value;

            // Update the enabled color of each child ui sprite element.
            foreach (UISprite sprite in this)
            {
                sprite.EnabledColor = value;
            }
        }
    }

    /// <summary>
    /// Gets or Sets the color mask to apply to this ui sprite when it is disabled.
    /// </summary>
    /// <remarks>
    /// Default value is Color.White.
    /// </remarks>
    public Color DisabledColor
    {
        get => _disabledColor;
        set
        {
            // If the color being set is the same color that it already is, just
            // return back early.
            if (_disabledColor == value)
            {
                return;
            }

            _disabledColor = value;

            // Update the disabled color of each child ui sprite element.
            foreach (UISprite sprite in this)
            {
                sprite.DisabledColor = value;
            }
        }
    }

    /// <summary>
    /// Creates a new ui sprite.
    /// </summary>
    public UISprite() : base()
    {
        EnabledColor = Color.White;
        DisabledColor = Color.White;
    }

    /// <summary>
    /// Updates this ui sprite.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current update cycle.</param>
    public override void Update(GameTime gameTime)
    {
        // If disabled, return early.
        if (!Enabled)
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
        // If not visible, return early.
        if (!Visible)
        {
            return;
        }

        // Only draw the sprite if there is a sprite to draw
        if (Sprite != null)
        {
            // Set the color of the sprite based on the enabled property
            Sprite.Color = Enabled ? EnabledColor : DisabledColor;

            // Draw the sprite
            Sprite.Draw(spriteBatch, AbsolutePosition);
        }

        // Call base draw so that child elements are drawn.
        base.Draw(spriteBatch);
    }
}
