using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FlappyBird
{
    class UI
    {
        private Texture2D menuTexture, gameOverTexture;
        private List<Texture2D> numbers;
        private Vector2 menu, gameOverPos;
        public static int score;

        public void Initialize(Vector2 menuPos, Vector2 gameOverPos)
        {
            this.menu = menuPos;
            this.gameOverPos = gameOverPos;
        }

        public void LoadContent(ContentManager Content)
        {
            menuTexture = Content.Load<Texture2D>("UI\\message");
            gameOverTexture = Content.Load<Texture2D>("UI\\gameover");
            numbers = new List<Texture2D>();
            for(int i = 0; i < 10; i++)
            {
                numbers.Add(Content.Load<Texture2D>($"Numbers\\{i}"));
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void DrawMenu(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(menuTexture, menu, Color.White);
        }

        public void DrawGameOver(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gameOverTexture, gameOverPos, Color.White);
        }

        public void DrawScore(SpriteBatch spriteBatch)
        {
            if (score < 10)
            {
                spriteBatch.Draw(numbers[score], new Vector2(20, 20), Color.White);
            }else if(score < 100)
            {
                int secondDigit = score - (score / 10 * 10);
                int firstDigit = score / 10;
                spriteBatch.Draw(numbers[firstDigit], new Vector2(20, 20), Color.White);
                spriteBatch.Draw(numbers[secondDigit], new Vector2(45, 20), Color.White);

            }else if(score < 1000)
            {

            }
        }

    }
}
