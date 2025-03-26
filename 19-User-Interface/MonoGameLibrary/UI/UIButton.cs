using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;

namespace MonoGameLibrary.UI;

public class UIButton
{
    private Sprite _currentSprite;

    public Sprite NormalSprite { get; set; }
    public Sprite SelectedSprite { get; set; }

    public Action MoveUp { get; set; }
    public Action MoveDown { get; set; }
    public Action MoveLeft { get; set; }
    public Action MoveRight { get; set; }
    public Action Action { get; set; }
    public bool IsSelected { get; set; }

    public UIButton()
    {

    }

    public void Update(GameTime gameTime)
    {

        if(IsSelected)
        {
            if(SelectedSprite is AnimatedSprite animatedSprite)
            {
                animatedSprite.Update(gameTime);
            }

            if()
        }

    }


}
