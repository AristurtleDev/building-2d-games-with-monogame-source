using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime.UI;

public class GameSelectMenu
{
    private GameOptions _options;

    // The select label
    private Sprite _selectLabelSprite;
    private Vector2 _selectSpritePosition;

    // The enter label
    private Sprite _enterLabelSprite;
    private Vector2 _enterLabelSpritePosition;

    // The escape label
    private Sprite _escapeLabelSprite;
    private Vector2 _escapeLabelSpritePosition;

    // The speed panel
    private Sprite _speedPanelSprite;
    private Vector2 _speedPanelSpritePosition;

    // The speed panel label
    private Sprite _speedPanelLabelSprite;
    private Vector2 _speedPanelLabelSpritePosition;

    // The slow speed button
    private Sprite _slowSpeedButtonSprite;
    private AnimatedSprite _slowSpeedButtonSelectedSprite;
    private Vector2 _slowSpeedButtonPosition;
    private bool _isSlowSpeedButtonSelected;

    // The normal speed button
    private Sprite _normalSpeedButtonSprite;
    private AnimatedSprite _normalSpeedButtonSelectedSprite;
    private Vector2 _normalSpeedButtonPosition;
    private bool _isNormalSpeedButtonSelected;

    // The fast speed button
    private Sprite _fastSpeedButtonSprite;
    private AnimatedSprite _fastSpeedButtonSelectedSprite;
    private Vector2 _fastSpeedButtonPosition;
    private bool _isFastSpeedButtonSelected;

    // The mode panel
    private Sprite _modePanelSprite;
    private Vector2 _modePanelSpritePosition;

    // The mode panel label
    private Sprite _modePanelLabelSprite;
    private Vector2 _modePanelLabelSpritePosition;

    // The normal mode button
    private Sprite _normalModeButtonSprite;
    private AnimatedSprite _normalModeButtonSelectedSprite;
    private Vector2 _normalModeButtonPosition;
    private bool _isNormalModeButtonSelected;

    // The dark mode button
    private Sprite _darkModeButtonSprite;
    private AnimatedSprite _darkModeButtonSelectedSprite;
    private Vector2 _darkModeButtonPosition;
    private bool _isDarkModeButtonSelected;

    // The accept button
    private Sprite _acceptButtonSprite;
    private AnimatedSprite _acceptButtonSelectedSprite;
    private Vector2 _acceptButtonPosition;
    private bool _isAcceptButtonSelected;

    // The cancel button
    private Sprite _cancelButtonSprite;
    private AnimatedSprite _cancelButtonSelectedSprite;
    private Vector2 _cancelButtonPosition;
    private bool _isCancelButtonSelected;

    // The color to use for elements that are enabled
    private Color _enabledColor = Color.White;

    // The color to use for elements that are disabled
    private Color _disabledColor = new Color(70, 86, 130, 255);

    // Raised when the accept button is clicked
    public event EventHandler Accepted;

    // Raised when the cancel button is clicked
    public event EventHandler Cancelled;

    public void Initialize()
    {
        Rectangle screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        // Position the select label sprite
        _selectSpritePosition = new Vector2(112, 20);

        // Position the enter label sprite
        _enterLabelSpritePosition = new Vector2(640, 52);

        // Position the escape label sprite
        _escapeLabelSpritePosition = new Vector2(804, 52);

        // Position the speed panel sprite
        _speedPanelSpritePosition = new Vector2(198, 139);

        // Position the speed label sprite
        _speedPanelLabelSpritePosition = new Vector2(240, 181);

        // Position the slow speed button
        _slowSpeedButtonPosition = new Vector2(238, 272);
        _slowSpeedButtonSprite.CenterOrigin();
        _slowSpeedButtonSelectedSprite.CenterOrigin();

        // Position the normal speed button
        _normalSpeedButtonPosition = new Vector2(511, 272);
        _normalSpeedButtonSprite.CenterOrigin();
        _normalSpeedButtonSelectedSprite.CenterOrigin();

        // Position the fast speed button
        _fastSpeedButtonPosition = new Vector2(511, 272);
        _fastSpeedButtonSprite.CenterOrigin();
        _fastSpeedButtonSelectedSprite.CenterOrigin();

        // Position the mode panel sprite
        _modePanelSpritePosition = new Vector2(198, 406);

        // Position the model label sprite
        _modePanelLabelSpritePosition = new Vector2(240, 448);

        // Position the normal mode button sprite
        _normalModeButtonPosition = new Vector2(395, 535);
        _normalModeButtonSprite.CenterOrigin();
        _normalModeButtonSelectedSprite.CenterOrigin();

        // Position the fast mode button sprite
        _darkModeButtonPosition = new Vector2(684, 535);
        _darkModeButtonSprite.CenterOrigin();
        _darkModeButtonSelectedSprite.CenterOrigin();

        // Position the accept button
        _acceptButtonPosition = new Vector2(screenBounds.Center.X - _acceptButtonSprite.Width, screenBounds.Bottom - 50);
        _acceptButtonSprite.CenterOrigin();
        _acceptButtonSelectedSprite.CenterOrigin();

        // Position the cancel button
        _cancelButtonPosition = new Vector2(screenBounds.Center.X + _cancelButtonSprite.Width, screenBounds.Bottom - 50);
        _cancelButtonSprite.CenterOrigin();
        _cancelButtonSelectedSprite.CenterOrigin();

    }

    public void LoadContent()
    {
        // Load the ui texture atlas from the XML configuration file
        TextureAtlas atlas = TextureAtlas.FromFile(Core.Content, "images/ui-atlas-definition.xml");

        // Create the select label
        _selectLabelSprite = atlas.CreateSprite("select-label");

        // Create the enter label
        _enterLabelSprite = atlas.CreateSprite("enter-label");

        // Create the escape label
        _escapeLabelSprite = atlas.CreateSprite("escape-label");

        // Create the speed panel
        _speedPanelSprite = atlas.CreateSprite("panel");

        // Create the speed panel label
        _speedPanelLabelSprite = atlas.CreateSprite("speed-label");

        // Create the slow speed button
        _slowSpeedButtonSprite = atlas.CreateSprite("slow-button");
        _slowSpeedButtonSelectedSprite = atlas.CreateAnimatedSprite("slow-button-selected");

        // Create the normal speed button
        _normalSpeedButtonSprite = atlas.CreateSprite("normal-button");
        _normalSpeedButtonSelectedSprite = atlas.CreateAnimatedSprite("normal-button-selected");

        // Create the fast speed button
        _fastSpeedButtonSprite = atlas.CreateSprite("fast-button");
        _fastSpeedButtonSelectedSprite = atlas.CreateAnimatedSprite("fast-button-selected");

        // Create the mode panel
        _modePanelSprite = atlas.CreateSprite("panel");

        // Create the speed panel label
        _modePanelLabelSprite = atlas.CreateSprite("mode-label");

        // Create the normal mode button
        _normalModeButtonSprite = atlas.CreateSprite("normal-button");
        _normalModeButtonSelectedSprite = atlas.CreateAnimatedSprite("normal-button-selected");

        // Create the dark mode button
        _darkModeButtonSprite = atlas.CreateSprite("dark-button");
        _darkModeButtonSelectedSprite = atlas.CreateAnimatedSprite("dark-button-selected");

        // Create the accept button
        _acceptButtonSprite = atlas.CreateSprite("accept-button");
        _acceptButtonSelectedSprite = atlas.CreateAnimatedSprite("accept-button-selected");

        // Create the cancel button
        _cancelButtonSprite = atlas.CreateSprite("cancel-button");
        _cancelButtonSelectedSprite = atlas.CreateAnimatedSprite("cancel-button-selected");

    }

    public void Update(GameTime gameTime)
    {
        if(_isSpeedPanelSelected)
        {
            UpdateSpeedPanel();
        }
        else if(_isModePanelSelected)
        {
            UpdateModePanel();
        }
        else if(_isAcceptButtonSelected)
        {
            UpdateAcceptButton(gameTime);
        }
        else if(_isCancelButtonSelected)
        {
            UpdateCancelButton(gameTime);
        }
    }

    public void SetSpeedPanelState(bool selected)
    {
        // Determine the color based on the selected state of the panel
        Color color = selected ? _enabledColor : _disabledColor;

        // Set the color of each element for this panel to that color.
        _speedPanelSprite.Color = color;
        _speedPanelLabelSprite.Color = color;
        _slowSpeedButtonSprite.Color = color;
        _slowSpeedButtonSelectedSprite.Color = color;
        _normalSpeedButtonSprite.Color = color;
        _normalSpeedButtonSelectedSprite.Color = color;
        _fastSpeedButtonSprite.Color = color;
        _fastSpeedButtonSelectedSprite.Color = color;
    }

    public void SetModePanelState(bool selected)
    {
        // Determine the color based on the selected state of the panel
        Color color = selected ? _enabledColor : _disabledColor;

        // Set the color of each element for this panel to that color
        _modePanelSprite.Color = color;
        _modePanelLabelSprite.Color = color;
        _normalModeButtonSprite.Color = color;
        _normalModeButtonSelectedSprite.Color = color;
        _darkModeButtonSprite.Color = color;
        _darkModeButtonSelectedSprite.Color = color;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // Draw the select label sprite
        _selectLabelSprite.Draw(spriteBatch, _selectSpritePosition);

        // Draw the enter label sprite
        _enterLabelSprite.Draw(spriteBatch, _enterLabelSpritePosition);

        // Draw the escape label sprite
        _escapeLabelSprite.Draw(spriteBatch, _escapeLabelSpritePosition);

        // Draw the speed panel sprite
        _speedPanelSprite.Draw(spriteBatch, _speedPanelSpritePosition);

        // Draw the speed panel label spaire
        _speedPanelLabelSprite.Draw(spriteBatch, _speedPanelLabelSpritePosition);

        // Choose which version of the slow, normal, and fast speed button
        // sprites to draw based on if they are currently selected.
        Sprite slowSpeedButton = _isSlowSpeedButtonSelected ? _slowSpeedButtonSelectedSprite : _slowSpeedButtonSprite;
        Sprite normalSpeedButton = _isNormalSpeedButtonSelected ? _normalSpeedButtonSelectedSprite : _normalSpeedButtonSprite;
        Sprite fastSpeedButton = _isFastSpeedButtonSelected ? _fastSpeedButtonSelectedSprite : _fastSpeedButtonSprite;

        // Draw the slow, normal, and fast speed buttons.
        slowSpeedButton.Draw(spriteBatch, _slowSpeedButtonPosition);
        normalSpeedButton.Draw(spriteBatch, _normalSpeedButtonPosition);
        fastSpeedButton.Draw(spriteBatch, _fastSpeedButtonPosition);

        // Draw the mode panel sprite
        _modePanelSprite.Draw(spriteBatch, _modePanelSpritePosition);

        // Draw the mode panel label sprite
        _modePanelLabelSprite.Draw(spriteBatch, _modePanelLabelSpritePosition);

        // Choose which version of the normal and dark mode button sprites to
        // draw based on if they are currently selected.
        Sprite normalModeButton = _isNormalModeButtonSelected ? _normalModeButtonSelectedSprite : _normalModeButtonSprite;
        Sprite darkModeButton = _isDarkModeButtonSelected ? _darkModeButtonSelectedSprite : _darkModeButtonSprite;

        // Draw the normal and dark mode button sprites.
        normalModeButton.Draw(spriteBatch, _normalModeButtonPosition);
        darkModeButton.Draw(spriteBatch, _darkModeButtonPosition);

        // Choose which version of the accept and cancel button sprites to draw
        // based on if they are currently selected.
        Sprite acceptButton = _isAcceptButtonSelected ? _acceptButtonSelectedSprite : _acceptButtonSprite;
        Sprite cancelButton = _isCancelButtonSelected ? _cancelButtonSelectedSprite : _cancelButtonSprite;

        // Draw the accept and cancel button sprites
        acceptButton.Draw(spriteBatch, _acceptButtonPosition);
        cancelButton.Draw(spriteBatch, _cancelButtonPosition);
    }
}

