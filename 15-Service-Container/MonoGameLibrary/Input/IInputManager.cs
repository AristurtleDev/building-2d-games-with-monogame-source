using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Input;

/// <summary>
/// Interface for the game's input manager that handles keyboard, mouse, and gamepad input.
/// </summary>
public interface IInputManager
{
    /// <summary>
    /// Gets the keyboard info component for checking keyboard state.
    /// </summary>
    KeyboardInfo Keyboard { get; }

    /// <summary>
    /// Gets the mouse info component for checking mouse state.
    /// </summary>
    MouseInfo Mouse { get; }

    /// <summary>
    /// Gets an array of gamepad info components, one for each player.
    /// </summary>
    GamePadInfo[] GamePads { get; }

    /// <summary>
    /// Updates the input states.
    /// </summary>
    /// <param name="gameTime">Time passed since the last update.</param>
    void Update(GameTime gameTime);
}
