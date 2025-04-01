namespace MonoGameLibrary.UI;

/// <summary>
/// Represents the controller for UI element.
/// </summary>
public interface IUIElementController
{
    /// <summary>
    /// Returns a value that indicates whether the ui element this controller is for should trigger the navigate up
    /// action.
    /// </summary>
    /// <returns>true if the navigate up action should trigger; otherwise, false.</returns>
    bool NavigateUp();

    /// <summary>
    /// Returns a value that indicates whether the ui element this controller is for should trigger the navigate down
    /// action.
    /// </summary>
    /// <returns>true if the navigate down action should trigger; otherwise, false.</returns>
    bool NavigateDown();

    /// <summary>
    /// Returns a value that indicates whether the ui element this controller is for should trigger the navigate left
    /// action.
    /// </summary>
    /// <returns>true if the navigate left action should trigger; otherwise, false.</returns>
    bool NavigateLeft();

    /// <summary>
    /// Returns a value that indicates whether the ui element this controller is for should trigger the navigate right
    /// action.
    /// </summary>
    /// <returns>true if the navigate right action should trigger; otherwise, false.</returns>
    bool NavigateRight();

    /// <summary>
    /// Returns a value that indicates whether the ui element this controller is for should trigger the confirm action.
    /// </summary>
    /// <returns>true if the confirm action should trigger; otherwise, false.</returns>
    bool Confirm();

    /// <summary>
    /// Returns a value that indicates whether the ui element this controller is for should trigger the cancel action.
    /// </summary>
    /// <returns>true if the cancel action should trigger; otherwise, false.</returns>
    bool Cancel();
}
