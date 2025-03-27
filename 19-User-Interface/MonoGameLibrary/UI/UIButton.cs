using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UIButton : UISprite
{
    private Sprite _normalSprite;
    private Sprite _selectedSprite;

    /// <summary>
    /// Gets or Sets a value that indicates whether this ui button is selected.
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// Creates a new ui button.
    /// </summary>
    /// <param name="normalSprite">The sprite to use when the button is not selected.</param>
    /// <param name="selectedSprite">The sprite to use when the button is selected.</param>
    public UIButton(Sprite normalSprite, Sprite selectedSprite)
        : base(normalSprite)
    {
        _normalSprite = normalSprite;
        _selectedSprite = selectedSprite;
    }

    /// <summary>
    /// Creates a new ui button as a child of the given ui element.
    /// </summary>
    /// <param name="parent">The ui element to set as the parent of this ui button.</param>
    /// <param name="normalSprite">The sprite to use when the button is not selected.</param>
    /// <param name="selectedSprite">The sprite to use when the button is selected.</param>
    public UIButton(UIElement parent, Sprite normalSprite, Sprite selectedSprite)
        : base(parent, normalSprite)
    {
        _normalSprite = normalSprite;
        _selectedSprite = selectedSprite;
    }

    /// <summary>
    /// Centers the origin of the normal and selected sprites for this ui button.
    /// </summary>
    public override void CenterOrigin()
    {
        _normalSprite.CenterOrigin();
        _selectedSprite.CenterOrigin();
    }

    /// <summary>
    /// Updates this ui button.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current update cycle.</param>
    public override void Update(GameTime gameTime)
    {
        // Determine which sprite to use based on if this button is selected or not.
        Sprite = IsSelected ? _selectedSprite : _normalSprite;

        // Call base update to then draw this as a normal ui sprite.
        base.Update(gameTime);
    }
}
