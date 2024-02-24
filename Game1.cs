using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlappyBird;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D render;
    const int WIDTH = 258, HEIGHT = 512;
    private Level level;
    private UI ui;
    public static bool isGameOver;
    public static bool isGamePaused;
    public static bool isGameRunning;
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = WIDTH;
        _graphics.PreferredBackBufferHeight = HEIGHT;

    }

    protected override void Initialize()
    {
        level = new Level();
        ui = new UI();
        ui.Initialize(new Vector2(WIDTH / 6,HEIGHT / 4), new Vector2(WIDTH / 6, HEIGHT / 3));
        level.Initialize();
        isGamePaused = true;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        render = new RenderTarget2D(GraphicsDevice, 258, 512);
        level.LoadContent(Content);
        ui.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
        level.CheckScore();
        var keyboard = Keyboard.GetState();

        if( keyboard.IsKeyDown(Keys.Space) && !isGameRunning)
        {
            level.ResetLevel();
            isGameRunning = true;
            isGamePaused = false;
        }
        if(keyboard.IsKeyDown(Keys.Space) && isGameOver)
        {
            level.ResetLevel();
            UI.score = 0;
            isGamePaused = false;
            isGameOver = false;
        }

        if(!isGameOver && isGameRunning)
        {
            level.Update(gameTime);
        }
        


        base.Update(gameTime);

        
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        GraphicsDevice.SetRenderTarget(render);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        if (!isGamePaused)
        {
            level.Draw(_spriteBatch);
            ui.DrawScore(_spriteBatch);
        }else
        {
            if (!isGameOver)
            {
                level.Draw(_spriteBatch);
                ui.DrawMenu(_spriteBatch);
            }else
            {
                level.Draw(_spriteBatch);
                ui.DrawGameOver(_spriteBatch);
                ui.DrawScore(_spriteBatch);
            }
        }
        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);

        _spriteBatch.Begin();
        _spriteBatch.Draw(render,Vector2.Zero,Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void StateMachine()
    {

    }
}
