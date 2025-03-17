using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class NineSlice
{
    private TextureRegion _region;
    private int _topMargin;
    private int _bottomMargin;
    private int _leftMargin;
    private int _rightMargin;
    private Rectangle[] _sourceRectangles;
    private Rectangle[] _destinationRectangles;
    private Rectangle _destination;

    public NineSlice(TextureRegion region, int leftMargin, int rightMargin, int topMargin, int bottomMargin)
    {
        _region = region;
        _topMargin = topMargin;
        _bottomMargin = bottomMargin;
        _leftMargin = leftMargin;
        _rightMargin = rightMargin;

        _sourceRectangles = new Rectangle[9];
        CalculateRectangles(_sourceRectangles, region.SourceRectangle);
    }

    public NineSlice(TextureRegion region, int margin)
        : this(region, margin, margin, margin, margin) { }

    private void CalculateRectangles(Rectangle[] rectangles, Rectangle reference)
    {
        int centerWidth = reference.Width - _leftMargin - _rightMargin;
        int centerHeight = reference.Height - _topMargin - _bottomMargin;

        int x = reference.X;
        int y = reference.Y;

        // top-left corner
        rectangles[0] = new Rectangle(x, y, _leftMargin, _topMargin);

        // top-edge
        rectangles[1] = new Rectangle(x + _leftMargin, y, centerWidth, _topMargin);

        // top-right corner
        rectangles[2] = new Rectangle(x + _leftMargin + centerWidth, y, _rightMargin, _topMargin);

        // left-edge
        rectangles[3] = new Rectangle(x, y + _topMargin, _leftMargin, centerHeight);

        // center
        rectangles[4] = new Rectangle(x + _leftMargin, y + _topMargin, centerWidth, centerHeight);

        // right-edge
        rectangles[5] = new Rectangle(x + _leftMargin + centerWidth, y + _topMargin, _rightMargin, centerHeight);

        // bottom-left corner
        rectangles[6] = new Rectangle(x, y + _topMargin + centerHeight, _leftMargin, _bottomMargin);

        // bottom-edge
        rectangles[7] = new Rectangle(x + _leftMargin, y + _topMargin + centerHeight, centerWidth, _bottomMargin);

        // bottom-right
        rectangles[8] = new Rectangle(x + _leftMargin + centerWidth, y + _topMargin + centerHeight, _rightMargin, _bottomMargin);
    }

    public void Draw(SpriteBatch spriteBatch, Rectangle destination, Color color)
    {
        // If destination rectangles haven't been calculated yet, or if the
        // provided destination is different than the previous destination,
        // then calculate them, otherwise use the cached values
        if (_destinationRectangles == null || _destination != destination)
        {
            _destinationRectangles = new Rectangle[9];
            CalculateRectangles(_destinationRectangles, destination);
            _destination = destination;
        }

        for (int i = 0; i < _destinationRectangles.Length; i++)
        {
            spriteBatch.Draw(
                _region.Texture,
                _destinationRectangles[i],
                _sourceRectangles[i],
                color
            );
        }
    }
}
