using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class NineSlice
{
    private TextureRegion _region;
    private int _top;
    private int _bottom;
    private int _left;
    private int _right;
    private Rectangle[] _sourceRect;

    public NineSlice(TextureRegion region, int left, int right, int top, int bottom)
    {
        _region = region;
        _top = top;
        _bottom = bottom;
        _left = left;
        _right = right;

        CalculateSourceRects();
    }

    public NineSlice(TextureRegion region, int edgeSize)
        : this(region, edgeSize, edgeSize, edgeSize, edgeSize)
    {

    }

    private void CalculateSourceRects()
    {
        _sourceRect = new Rectangle[9];

        int centerWidth = _region.Width - _left - _right;
        int centerHeight = _region.Height - _top - _bottom;

        if (centerWidth <= 0 || centerHeight <= 0)
        {
            throw new ArgumentException("Edge sizes are too large for the provided texture region");
        }

        int sourceX = _region.SourceRectangle.X;
        int sourceY = _region.SourceRectangle.Y;

        // top-left corner
        _sourceRect[0] = new Rectangle(sourceX, sourceY, _left, _top);

        // top-edge
        _sourceRect[1] = new Rectangle(sourceX + _left, sourceY, centerWidth, _top);

        // top-right corner
        _sourceRect[2] = new Rectangle(sourceX + _left + centerWidth, sourceY, _right, _top);

        // left-edge
        _sourceRect[3] = new Rectangle(sourceX, sourceY + _top, _left, centerHeight);

        // center
        _sourceRect[4] = new Rectangle(sourceX + _left, sourceY + _top, centerWidth, centerHeight);

        // right-edge
        _sourceRect[5] = new Rectangle(sourceX + _left + centerWidth, sourceY + _top, _right, centerHeight);

        // bottom-left corner
        _sourceRect[6] = new Rectangle(sourceX, sourceY + _top + centerHeight, _left, _bottom);

        // bottom-edge
        _sourceRect[7] = new Rectangle(sourceX + _left, sourceY + _top + centerHeight, centerWidth, _bottom);

        // bottom-right
        _sourceRect[7] = new Rectangle(sourceX + _left + centerWidth, sourceY + _top + centerHeight, _right, _bottom);
    }




}
