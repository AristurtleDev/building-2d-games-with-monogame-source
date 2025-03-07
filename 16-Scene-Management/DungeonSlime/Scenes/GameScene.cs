using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;

namespace DungeonSlime.Scenes;

public class GameScene : Scene
{
    private IInputManager _input;
    private IAudioManager _audio;

    public GameScene(Game game) : base(game)
    {

    }

    public override void Initialize()
    {
        _input = Game.Services.GetService<IInputManager>();
        _audio = Game.Services.GetService<IAudioManager>();

        base.Initialize();

    }

    public override void LoadContent()
    {
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {

    }
}
