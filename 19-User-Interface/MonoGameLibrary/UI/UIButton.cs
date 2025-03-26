using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UIButton : UISprite
{
    private Sprite _normalSprite;
    private Sprite _selectedSprite;

    public bool IsSelected { get; set; }

    public UIButton(string name, Sprite normalSprite, Sprite selectedSprite, bool centerOrigin)
        : base(name, normalSprite, centerOrigin)
    {
        _normalSprite = normalSprite;
        _selectedSprite = selectedSprite;

        if (centerOrigin)
        {
            CenterOrigin();
        }
    }

    public UIButton(string name, UIElement parent, Sprite normalSprite, Sprite selectedSprite, bool centerOrigin)
        : base(name, normalSprite, parent, centerOrigin)
    {
        _normalSprite = normalSprite;
        _selectedSprite = selectedSprite;

        if (centerOrigin)
        {
            CenterOrigin();
        }
    }

    public override void CenterOrigin()
    {
        _normalSprite.CenterOrigin();
        _selectedSprite.CenterOrigin();
    }

    public override void Update(GameTime gameTime)
    {
        Sprite = IsSelected ? _selectedSprite : _normalSprite;

        base.Update(gameTime);
    }
}
