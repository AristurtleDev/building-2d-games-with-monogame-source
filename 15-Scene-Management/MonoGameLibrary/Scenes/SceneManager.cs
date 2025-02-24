using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Scenes;

public class SceneManager : DrawableGameComponent
{
    private Scene _activeScene;
    private Scene _nextScene;
    private SpriteBatch _spriteBatch;

    public SceneManager(Game game) : base(game) { }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
    }

    public override void Update(GameTime gameTime)
    {
        // If there is a next scene waiting to be switched to, transition to
        // that scene
        if (_nextScene != null)
        {
            TransitionScene();
        }

        // If there is an active scene, update it.
        if (_activeScene != null)
        {
            _activeScene.BeforeUpdate(gameTime);
            _activeScene.Update(gameTime);
            _activeScene.AfterUpdate(gameTime);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        if (_activeScene != null)
        {
            _activeScene.BeforeDraw(_spriteBatch, gameTime);
            _activeScene.Draw(_spriteBatch, gameTime);
            _activeScene.AfterDraw(_spriteBatch, gameTime);
        }
    }

    public void ChangeScene(Scene nextScene)
    {
        if (_activeScene != nextScene)
        {
            _nextScene = nextScene;
        }
    }

    private void TransitionScene()
    {
        // Unload content from active scene before going to the next scene
        if (_activeScene != null)
        {
            _activeScene.UnloadContent();
        }

        // Perform a garbage collection
        GC.Collect();

        // Swap to the next scene
        _activeScene = _nextScene;
        _nextScene = null;

        //  Initialize the new active scene
        _activeScene.Initialize();
    }
}
