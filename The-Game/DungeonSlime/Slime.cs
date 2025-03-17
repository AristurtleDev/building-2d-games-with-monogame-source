using Microsoft.Xna.Framework;

namespace DungeonSlime;

public struct Slime
{
    /// <summary>
    /// The position the slime is at.
    /// </summary>
    public Vector2 At;

    /// <summary>
    /// The position the slime is moving toward.
    /// </summary>
    public Vector2 To;

    /// <summary>
    /// The direction the slime is moving.
    /// </summary>
    public Vector2 Direction;

    /// <summary>
    /// The opposite direction the slime is moving.
    /// </summary>
    public Vector2 ReverseDirection => new Vector2(-Direction.X, -Direction.Y);
}
