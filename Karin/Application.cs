using ImGuiNET;
using Karin.Events;
using Karin.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.ImGuiNet;

namespace Karin;

public class Application
{
    public static Application Instance { get; private set; } = null!;
    public InputManager InputManager { get; private set; } = new InputManager();
    public Scene CurrentScene { get; set; } = null!;
    public Game Game { get; private set; }
    float fixedDeltaTime = 1f / 50f;
    float accumulatedTime = 0f;
    public ImGuiRenderer ImGuiRenderer;
    private SpriteBatch _spriteBatch;
    public List<ImGuiTool> Tools = new List<ImGuiTool>();
    public GameStats GameStats = new GameStats();

    public delegate void ApplicationEventHandler(Event e);
    public event ApplicationEventHandler? OnApplicationEvent;

    public Application(Game game)
    {
        if (Instance != null)
        {
            throw new InvalidOperationException("Application instance already exists.");
        }
        Instance = this;
        Game = game;
        InputManager.OnEvent += OnEvent;
        ImGuiRenderer = new ImGuiRenderer(game);
        GameStats = new GameStats();
    }

    public void LoadContent()
    {
        ImGuiRenderer.RebuildFontAtlas();

        CurrentScene.LoadContent();
    }

    public void Update(GameTime gameTime)
    {
        GameStats.Update(gameTime);
        InputManager.Update();
        CurrentScene.BeforeUpdate(gameTime);
        CurrentScene.Update(gameTime);
        CurrentScene.AfterUpdate(gameTime);
        TryFixedUpdate(gameTime);
    }
    
    public void Draw(GameTime gameTime)
    {
        GameStats.Draw(gameTime);
        CurrentScene.Draw(gameTime);

        ImGuiRenderer.BeginLayout(gameTime);

        Tools.ForEach(tool => tool.Render());

        ImGuiRenderer.EndLayout();

    }

    public void FixedUpdate(GameTime gameTime)
    {
        GameStats.FixedUpdate(gameTime);
        CurrentScene.FixedUpdate(gameTime);
    }

    public void OnEvent(Event e)
    {
        OnApplicationEvent?.Invoke(e);
    }

    public void SetScene(Scene scene)
    {
        var previousScene = CurrentScene;
        previousScene?.Destroy();
        CurrentScene = scene;
    }

    // ----------------------------
    // Private Methods
    // ----------------------------

    private void TryFixedUpdate(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        accumulatedTime += elapsed;

        while (accumulatedTime >= fixedDeltaTime)
        {
            TimeSpan totalGameTime = gameTime.TotalGameTime + TimeSpan.FromSeconds(accumulatedTime);
            GameTime fixedGameTime = new GameTime(totalGameTime, TimeSpan.FromSeconds(fixedDeltaTime));
            FixedUpdate(fixedGameTime);
            accumulatedTime -= fixedDeltaTime;
        }
    }

}
