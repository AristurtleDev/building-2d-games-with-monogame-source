using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UISlider<T> : UIElement where T : System.Numerics.INumber<T>
{
    public Sprite SliderSprite { get; set; }
    public Sprite FillSprite { get; set; }
    public Rectangle FillBounds { get; set; }
    public T MinValue { get; set; }
    public T MaxValue { get; set; }
    public T Value { get; set; }
    public T Step { get; set; }

    public UISlider() : base() { }

    public override void Update(GameTime gameTime)
    {
        // Get the current percentage of the value within the min and max range.
        float value = Convert.ToSingle(Value);
        float min = Convert.ToSingle(MinValue);
        float max = Convert.ToSingle(MaxValue);
        float percentage = (value - min) / (max - min);

        // Calculate the x-axis scale of the fill sprite based on teh width
        // of hte fill boundary and the percentage
        FillSprite.Scale = new Vector2(
            FillBounds.Width * percentage,
            1.0f
        );

        base.Update(gameTime);
    }

    public T StepUp()
    {
        Value = Value + Step;
        Clamp();
        return Value;
    }

    public T StepDown()
    {
        Value = Value - Step;
        Clamp();
        return Value;
    }

    private void Clamp()
    {
        if (Value < MinValue)
        {
            Value = MinValue;
        }
        else if (Value > MaxValue)
        {
            Value = MaxValue;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 fillSpritePosition = AbsolutePosition + FillBounds.Location.ToVector2();

        // Draw the fill sprite
        FillSprite.Draw(spriteBatch, fillSpritePosition);

        SliderSprite.Draw(spriteBatch, AbsolutePosition);

        base.Draw(spriteBatch);
    }
}
