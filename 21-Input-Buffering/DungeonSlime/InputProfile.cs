using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Input;

namespace DungeonSlime;

public static class InputProfile
{
    private static KeyboardInfo s_keyboard;
    private static GamePadInfo s_gamepad;

    static InputProfile()
    {
        s_keyboard = Core.Input.Keyboard;
        s_gamepad = Core.Input.GamePads[0];
    }

    public static bool MenuUp()
    {
        return s_keyboard.WasKeyJustPressed(Keys.Up) ||
               s_gamepad.WasButtonJustPressed(Buttons.DPadUp) ||
               s_gamepad.WasButtonJustPressed(Buttons.LeftThumbstickUp);
    }

    public static bool MenuDown()
    {
        return s_keyboard.WasKeyJustPressed(Keys.Down) ||
               s_gamepad.WasButtonJustPressed(Buttons.DPadDown) ||
               s_gamepad.WasButtonJustPressed(Buttons.LeftThumbstickLeft);
    }

    public static bool MenuLeft()
    {
        return s_keyboard.WasKeyJustPressed(Keys.Left) ||
               s_gamepad.WasButtonJustPressed(Buttons.DPadLeft) ||
               s_gamepad.WasButtonJustPressed(Buttons.LeftThumbstickLeft);
    }

    public static bool MenuRight()
    {
        return s_keyboard.WasKeyJustPressed(Keys.Right) ||
               s_gamepad.WasButtonJustPressed(Buttons.DPadRight) ||
               s_gamepad.WasButtonJustPressed(Buttons.LeftThumbstickRight);
    }

    public static bool MenuAccept()
    {
        return s_keyboard.WasKeyJustPressed(Keys.Enter) ||
               s_gamepad.WasButtonJustPressed(Buttons.A);
    }

    public static bool MenuCancel()
    {
        return s_keyboard.WasKeyJustPressed(Keys.Escape) ||
               s_gamepad.WasButtonJustPressed(Buttons.B);
    }
}
