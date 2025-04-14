using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Input;

namespace DungeonSlime;

public class GameController
{
    private KeyboardInfo _keyboard;
    private GamePadInfo _gamePad;

    public GameController()
    {
        _keyboard = Core.Input.Keyboard;
        _gamePad = Core.Input.GamePads[0];
    }

    public bool MoveUp()
    {
        return _keyboard.WasKeyJustPressed(Keys.Up) ||
               _keyboard.WasKeyJustPressed(Keys.W) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadUp) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickUp);
    }

    public bool MoveDown()
    {
        return _keyboard.WasKeyJustPressed(Keys.Down) ||
               _keyboard.WasKeyJustPressed(Keys.S) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadDown) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickDown);
    }

    public bool MoveLeft()
    {
        return _keyboard.WasKeyJustPressed(Keys.Left) ||
               _keyboard.WasKeyJustPressed(Keys.A) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadLeft) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickLeft);
    }

    public bool MoveRight()
    {
        return _keyboard.WasKeyJustPressed(Keys.Right) ||
               _keyboard.WasKeyJustPressed(Keys.D) ||
               _gamePad.WasButtonJustPressed(Buttons.DPadRight) ||
               _gamePad.WasButtonJustPressed(Buttons.LeftThumbstickRight);
    }

    public bool Pause()
    {
        return _keyboard.WasKeyJustPressed(Keys.Escape) ||
               _gamePad.WasButtonJustPressed(Buttons.Start);
    }

    public bool Action()
    {
        return _keyboard.WasKeyJustPressed(Keys.Enter) ||
               _gamePad.WasButtonJustPressed(Buttons.A);
    }
}
