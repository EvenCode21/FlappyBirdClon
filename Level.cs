using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;

namespace FlappyBird
{
    class Level
    {
        private Texture2D background;
        private Texture2D ground;
        private Texture2D pipe, pipeDown;
        private Vector2 backgroundPosition1, backgroundPosition2, groundPosition1, groundPosition2,playerPos;
        private Vector2 pipeUpPos, pipeDownPos,pipeUp2Pos,pipeDown2Pos;
        private Rectangle pipeUpRect, pipeDownRect, pipeUp2,pipeDown2;

        private float rotation;
        private float rotationSpeed,velocityY;
        private const float GRAVITY = 10f,JUMP_SPEED = -6f;
        int distance = 100, pipeLength, distanceBetweenPipes = 130;

        private Texture2D player;
        private Rectangle playerRect,groundRect1,groundRect2;

        private KeyboardState keyboard, prevKey;

        private Random random;

        private SoundEffect jump, hit, point;

        public void Initialize()
        {
            backgroundPosition1 = new Vector2(0, 0);
            prevKey = Keyboard.GetState();
            random = new Random(Environment.TickCount);
        }

        public void LoadContent(ContentManager Content)
        {
            background = Content.Load<Texture2D>("background");
            ground = Content.Load<Texture2D>("ground");

            //pipe assets
            pipe = Content.Load<Texture2D>("pipe");
            pipeDown = Content.Load<Texture2D>("pipeDown");
            pipeUpPos = new Vector2(400, -random.Next(80, 241));
            pipeLength = (int)pipeUpPos.Y + pipe.Height;

            pipeDownPos = new Vector2(pipeUpPos.X, pipeLength + distance);  
            pipeUpRect = new Rectangle((int)pipeUpPos.X, (int)pipeUpPos.Y,pipe.Width,pipe.Height);
            pipeDownRect = new Rectangle((int)pipeDownPos.X,(int)pipeDownPos.Y,pipe.Width,pipe.Height);

            pipeUp2Pos = new Vector2((pipeUpRect.X + pipe.Width) + distanceBetweenPipes, pipeUpRect.Y);
            pipeDown2Pos = new Vector2(pipeUp2Pos.X, (pipeUp2Pos.Y + pipe.Height) + distance);

            pipeUp2 = new Rectangle((int)pipeUp2Pos.X,(int)pipeUp2Pos.Y, pipe.Width, pipe.Height);
            pipeDown2 = new Rectangle((int)pipeDown2Pos.X,(int)pipeDown2Pos.Y, pipeDown.Width, pipeDown.Height);

            groundPosition1 = new Vector2(0, 512 - ground.Height);
            groundPosition2 = new Vector2(ground.Width, groundPosition1.Y);

            backgroundPosition2 = new Vector2(background.Width, backgroundPosition1.Y);
            player = Content.Load<Texture2D>(@"player\playerMid");
            playerPos = new Vector2(50, 200);
            playerRect = new Rectangle((int)playerPos.X,(int)playerPos.Y,player.Width,player.Height);

            groundRect1 = new Rectangle((int)groundPosition1.X,(int)groundPosition1.Y,ground.Width,ground.Height);
            groundRect2 = new Rectangle((int)groundPosition2.X, (int)groundPosition2.Y, ground.Width, ground.Height);

            jump = Content.Load<SoundEffect>("Sounds\\wing");
            hit = Content.Load<SoundEffect>("Sounds\\hit");
            point = Content.Load<SoundEffect>("Sounds\\point");

        }

        public void Update(GameTime gameTime)
        {

            if (!Game1.isGamePaused && Game1.isGameRunning)
            {
                CheckCollision();
            }
            keyboard = Keyboard.GetState();

            PlayerUpdate(gameTime);
            ParallaxEffect(2f);
            PipesUpdate(gameTime,2f);


            prevKey = keyboard;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Game1.isGameRunning = true; 
            spriteBatch.Draw(background, backgroundPosition1, Color.White);
            spriteBatch.Draw(background, backgroundPosition2, Color.White);

            spriteBatch.Draw(pipe, pipeUpRect, Color.White);
            spriteBatch.Draw(pipeDown, pipeDownRect,Color.White);

            spriteBatch.Draw(pipe, pipeUp2, Color.White);
            spriteBatch.Draw(pipeDown, pipeDown2, Color.White);

            spriteBatch.Draw(ground,groundRect1,Color.White);
            spriteBatch.Draw(ground, groundRect2, Color.White);
            spriteBatch.Draw(player, playerRect, null, Color.White, rotation , Vector2.Zero, SpriteEffects.None,0); 

        }

        private void ParallaxEffect(float speed)
        {
            backgroundPosition1.X -= speed * 0.5f;
            backgroundPosition2.X -= speed * 0.5f;

            groundPosition1.X -= speed;
            groundPosition2.X -= speed;

            groundRect1.X = (int)groundPosition1.X;
            groundRect2.X = (int)groundPosition2.X;

            if(backgroundPosition2.X == 0)
            {
                backgroundPosition1.X = backgroundPosition2.X;
                backgroundPosition2.X = background.Width;
                
            }else if(backgroundPosition1.X == 1)
            {
                backgroundPosition2.X = backgroundPosition1.X;
                backgroundPosition1.X = background.Width;
                
            }

            if(groundPosition2.X == 0)
            {
                groundPosition1.X = groundPosition2.X;
                groundPosition2.X = ground.Width;
            }else if(groundPosition1.X == 1)
            {
                groundPosition2.X = groundPosition1.X;
                groundPosition1.X = ground.Width;
            }

            
        }

        private void PlayerUpdate(GameTime gameTime)
        {
            Gravity(gameTime);
            if(keyboard.IsKeyDown(Keys.Space) && !prevKey.IsKeyDown(Keys.Space))
            {
                jump.Play();
                velocityY += JUMP_SPEED;
            }

        }

        private void Gravity(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocityY += GRAVITY * delta;

            playerPos.Y += velocityY;
            playerRect.Y = (int)playerPos.Y;
        }

        private void CheckCollision()
        {
            if(playerPos.Y <= 0)
            {
                Game1.isGameOver = true;
                Game1.isGamePaused = true;
                Game1.isGameRunning = false;
                
                hit.Play();
                
                //ResetLevel();
            }else if(playerRect.Intersects(groundRect1) || playerRect.Intersects(groundRect2) || playerRect.Intersects(pipeUp2)
                || playerRect.Intersects(pipeUpRect) || playerRect.Intersects(pipeDown2) || playerRect.Intersects(pipeDownRect))
            {
                Game1.isGameOver = true;
                Game1.isGamePaused = true;
                Game1.isGameRunning = false;
                
                hit.Play();
                
                //ResetLevel();
            }
        }

        private void PipesUpdate(GameTime gameTime,float speed)
        {
            pipeUpPos.X -= speed;
            pipeUp2Pos.X -= speed;

            pipeUpRect.X = (int)pipeUpPos.X;
            pipeUp2.X = (int)pipeUp2Pos.X;

            pipeDownRect.X = pipeUpRect.X;
            pipeDown2.X = pipeUp2.X;

            if (pipeUpRect.X < -pipe.Width)
            {
                ResetLevel();
            }else if(pipeUp2Pos.X < -pipe.Width)
            {
                ResetLevel();
            }

        }

        public void ResetLevel()
        {
            if (pipeUpRect.X < -pipe.Width)
            {
                pipeUpPos = new Vector2(pipeUp2.X + pipeUp2.Width + distanceBetweenPipes, -random.Next(80, 250));
                pipeUpRect.Y = (int)pipeUpPos.Y;
                pipeDownRect.Y = (int)(pipeUpRect.Y + pipe.Height) + distance;
            }
            else if (pipeUp2Pos.X < -pipe.Width)
            {
                pipeUp2Pos = new Vector2(pipeUpRect.X + pipeUpRect.Width + distanceBetweenPipes, -random.Next(80, 250));
                pipeUp2.Y = (int)pipeUp2Pos.Y;
                pipeDown2.Y = (int)(pipeUp2.Y + pipe.Height) + distance;
            }else if(Game1.isGameOver)
            {
                pipeUpPos = new Vector2(pipeUp2.X + pipeUp2.Width + distanceBetweenPipes, -random.Next(80, 250));
                pipeUp2Pos = new Vector2(pipeUpRect.X + pipeUpRect.Width + distanceBetweenPipes, -random.Next(80, 250));
                playerPos.Y = 200;
                velocityY = 0;
            }
            pipeDownRect.X = pipeUpRect.X;
            pipeDown2.X = pipeUp2.X;
        }

        public void CheckScore()
        {
            if (Math.Abs((playerRect.X - (pipeUpRect.X + pipe.Width))) < 1)
            {
                point.Play();
                UI.score++;
            } 
            if (Math.Abs((playerRect.X - (pipeUp2.X + pipe.Width))) < 1 )
            {
                point.Play();
                UI.score++;
            }

        }
    }
}
