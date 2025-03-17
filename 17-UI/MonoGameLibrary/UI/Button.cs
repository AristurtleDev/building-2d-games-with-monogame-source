using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;

namespace MonoGameLibrary.UI;

public class Button
{
    private int _width;
    private int _height;
    private int _minWidth;
    private int _minHeight;
    private Vector2 _textSize;

    public Vector2 Position { get; set; }
    public int VerticalPadding { get; set; }
    public int HorizontalPadding { get; set; }


    public int Width
    {
        get => _width;
        set
        {
            if (_width == value)
            {
                return;
            }

            _width = Math.Max(_minWidth, value);
        }
    }

    public int Height
    {
        get => _height;
        set
        {
            if (_height == value)
            {
                return;
            }
            _height = Math.Max(_minHeight, value);
        }
    }

    public string Text { get; }
    public Color TextColor { get; set; }
    public SpriteFont Font { get; }
    public NineSlice NormalBorder { get; set; }
    public NineSlice HoveredBorder { get; set; }
    public NineSlice ClickedBorder { get; set; }
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

    public bool IsHovered { get; private set; }
    public bool IsPushed { get; private set; }
    public bool IsClicked { get; private set; }

    public Action Click;
    public Action Focused;
    public Action Hovered;

    public Button(SpriteFont font, string text, int width, int height, int horizontalPadding, int verticalPadding, NineSlice normalBorder, NineSlice hoveredBorder, NineSlice clickedBorder)
    {
        Font = font;
        Text = text;
        VerticalPadding = verticalPadding;
        HorizontalPadding = horizontalPadding;
        TextColor = Color.White;

        _textSize = Font.MeasureString(Text);
        _minWidth = (int)_textSize.X + horizontalPadding * 2;
        _minHeight = (int)_textSize.Y + verticalPadding * 2;

        Width = width;
        Height = height;

        NormalBorder = normalBorder;
        HoveredBorder = hoveredBorder;
        ClickedBorder = clickedBorder;
    }

    public void Update()
    {

        if (!IsHovered && Bounds.Contains(Core.Input.Mouse.Position))
        {
            IsHovered = true;
            Hovered?.Invoke();
        }

        if (IsHovered && Core.Input.Mouse.IsButtonDown(MouseButton.Left))
        {
            IsPushed = true;
        }

        if (IsHovered && Core.Input.Mouse.WasButtonJustReleased(MouseButton.Left))
        {
            Click?.Invoke();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (IsHovered)
        {
            HoveredBorder.Draw(spriteBatch, Bounds, Color.White);
        }
        else if(IsPushed)
        {
            Push
        }

        if ((IsFocused || IsHovered) && HoveredBorder != null)
        {
            HoveredBorder.Draw(spriteBatch, Bounds, Color.White);
        }
        else if (NormalBorder != null)
        {
            NormalBorder.Draw(spriteBatch, Bounds, Color.White);
        }

        // Text is drawn centered on button
        Vector2 center = new Vector2(
            Position.X + Width * 0.5f,
            Position.Y + Height * 0.5f
        );

        spriteBatch.DrawString(Font, Text, center, TextColor, 0.0f, _textSize * 0.5f, 1.0f, SpriteEffects.None, 0.0f);
    }
}
