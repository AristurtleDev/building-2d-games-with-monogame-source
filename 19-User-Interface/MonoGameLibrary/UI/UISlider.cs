using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UISlider<T> : UIElement where T : System.Numerics.INumber<T>
{
    /// <summary>
    /// The sprite to draw that represents the visual component of this slider.
    /// </summary>
    public Sprite SliderSprite { get; set; }

    /// <summary>
    /// Gets or Sets the sprite to scale and draw that represents the visual fill of the slider.
    /// </summary>
    public Sprite FillSprite { get; set; }

    /// <summary>
    /// Gets or Sets the bounds, relative to the SliderSprite bounds, that represents the area in which to scale the
    /// FillSprite to visually represent the slider value.
    /// </summary>
    public Rectangle FillBounds { get; set; }

    /// <summary>
    /// Gets or Sets the inclusive minimum value this slider can be stepped down to.
    /// </summary>
    public T MinValue { get; set; }

    /// <summary>
    /// Gets or Sets the inclusive maximum value this slider can be stepped up to.
    /// </summary>
    public T MaxValue { get; set; }

    /// <summary>
    /// Gets or Sets the current value of this slider.
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// Gets or Sets the amount to step this slider by when stepping up or down the value.
    /// </summary>
    public T Step { get; set; }

    /// <summary>
    /// Creates a new UI Slider.
    /// </summary>
    public UISlider() : base() { }

    /// <summary>
    /// Increments the value of this slider by the amount defined int he Step property.
    /// </summary>
    /// <returns>The updated value after stepping up.</returns>
    public T StepUp()
    {
        // Increment the value byt he step.
        Value = Value + Step;

        // Clamp the value within min and max range inclusively
        Clamp();

        // Return the updated value.
        return Value;
    }

    /// <summary>
    /// Decrements the value of this slider by the amount defined in the Step property.
    /// </summary>
    /// <returns>The updated value after stepping down.</returns>
    public T StepDown()
    {
        // Decrement the value by the step.
        Value = Value - Step;

        // CLampe the value within min and max range inclusively.
        Clamp();

        // Return the updated value.
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

    /// <summary>
    /// Draws this UI Slider.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to used for drawing.</param>
    public override void Draw(SpriteBatch spriteBatch)
    {
        // Calculate the position to render the fill sprite at.
        Vector2 fillSpritePosition = AbsolutePosition + FillBounds.Location.ToVector2();

        // Calculate the amount to scale the fill sprite
        float value = Convert.ToSingle(Value);
        float min = Convert.ToSingle(MinValue);
        float max = Convert.ToSingle(MaxValue);
        float percentage = (value - min) / (max - min);
        FillSprite.Scale = new Vector2(FillBounds.Width * percentage, FillBounds.Height);


        // Set the color of the fill sprite based on the enabled property.
        FillSprite.Color = IsEnabled ? EnabledColor : DisabledColor;

        // Draw the fill sprite.
        FillSprite.Draw(spriteBatch, fillSpritePosition);

        // Set the color of the slider sprite based on the enabled property.
        SliderSprite.Color = IsEnabled ? EnabledColor : DisabledColor;

        // Draw the slider sprite on top of the fill sprite.
        SliderSprite.Draw(spriteBatch, AbsolutePosition);

        // Call base draw so any child elements are drawn.
        base.Draw(spriteBatch);
    }
}
