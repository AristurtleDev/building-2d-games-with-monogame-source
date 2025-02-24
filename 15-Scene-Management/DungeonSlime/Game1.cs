using Microsoft.Xna.Framework;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;

namespace DungeonSlime;

public class Game1 : Game
{
    public Game1()
    {
        new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Create and add the input manager component to the game's
        // component collection and services.
        InputManager input = new InputManager(this);
        Components.Add(input);
        Services.AddService<InputManager>(input);

        // Create and add the audio manager component to the game's
        // component collection and services.
        AudioManager audio = new AudioManager(this);
        Components.Add(audio);
        Services.AddService<AudioManager>(audio);

        // Create an add the scene manager component to the game's
        // component collection
        SceneManager scenes = new SceneManager(this);
        Components.Add(scenes);

        // Start the first game scene.
        scenes.ChangeScene(new GameScene(this));
    }
}
