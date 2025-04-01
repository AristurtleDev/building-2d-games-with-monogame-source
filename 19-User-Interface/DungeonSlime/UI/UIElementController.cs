using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Input;
using MonoGameLibrary.UI;

namespace DungeonSlime.UI;

public class UIElementController : IUIElementController
{
    private KeyboardInfo _keyboard;
    private GamePadInfo _gamePad;

    public UIElementController()
    {
        _keyboard = Core.Input.Keyboard;
        _gamePad = Core.Input.GamePads[0];
    }

    public bool NavigateUp()
    {
        return _keyboard.WasKeyJustPressed(Keys.Up) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadUp) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickUp);
    }

    public bool NavigateDown()
    {
        return _keyboard.WasKeyJustPressed(Keys.Down) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadDown) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickDown);
    }

    public bool NavigateLeft()
    {
        return _keyboard.WasKeyJustPressed(Keys.Left) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadLeft) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickLeft);
    }

    public bool NavigateRight()
    {
        return _keyboard.WasKeyJustPressed(Keys.Right) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadRight) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickRight);
    }

    public bool Confirm()
    {
        return _keyboard.WasKeyJustPressed(Keys.Enter) ||
               _gamePad.WasButtonJustPressed(Buttons.A);
    }

    public bool Cancel()
    {
        return _keyboard.WasKeyJustPressed(Keys.Escape) ||
               _gamePad.WasButtonJustPressed(Buttons.B);
    }
}
