using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AsteroidsTheRealOne
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        KeyboardState ks, prevKs = Keyboard.GetState();

        Texture2D PlayerSprite;
        Texture2D EnemySprite;
        SpriteFont score;

        Vector2 PlayerMovement;

        float x, y, Vel;
        float UpperBulletLimit = 30;
        float fallspeed = 4;

        int EspawnX, EspawnY;
        int BulletCD = 0;
        int scoreToPrint = 0;
        int scoreKillCD = 0;
        int scoreTimer;

        bool Gamerun = true;

        Rectangle PlayerRec;

        List<Vector2> pbullet;
        List<Vector2> EnemyList;
        Random random = new Random();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            x = 400; y = 420;
            PlayerMovement = new Vector2(x, y);
            Vel = 0;
            pbullet = new List<Vector2>();
            EnemyList = new List<Vector2>();
            Gamerun = true;
            scoreToPrint = 0;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            PlayerSprite = Content.Load<Texture2D>("PlayerSprite");
            EnemySprite = Content.Load<Texture2D>("EnemySprite");
            score = Content.Load<SpriteFont>("Score");
            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent()
        {
            
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Gamerun)
            {
                Player();
                updateBullets();
                updateEnemies();
                doEnemyBulletCollison();
                scoreKillCD++;
                fallspeed += 0.003f;


                scoreTimer++;
                if (scoreTimer >= 60)
                {
                    scoreToPrint+= 1;
                    scoreTimer = 0;

                }

            }

            if (Keyboard.GetState().IsKeyDown(Keys.R) && Gamerun == false)
            {
                Initialize();
            }


            base.Update(gameTime);
        }
        void updateBullets()
        {
            for (int i = 0; i < pbullet.Count; i++)
            {
                pbullet[i] += new Vector2(0, -10);

                if (pbullet[i].Y <= -10)
                    pbullet.RemoveAt(i);

            }
        }

        void updateEnemies()
        {
            EspawnX = random.Next(0, 800);
            EspawnY = random.Next(-400, -15);


            while (EnemyList.Count < 16)
            {
                EnemyList.Add(new Vector2(EspawnX, EspawnY));
            }


            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i] += new Vector2(0, fallspeed);
                if (EnemyList[i].Y >= 530)
                    EnemyList.RemoveAt(i);
            }
        }

        void doEnemyBulletCollison()
        {
            for (int i = 0; i < EnemyList.Count; i++)
            {
                Rectangle tmpBullRec = new Rectangle((int)EnemyList[i].X, (int)EnemyList[i].Y, PlayerSprite.Width, PlayerSprite.Height);
                if (tmpBullRec.Intersects(PlayerRec))
                    Gamerun = false;
                for (int j = 0; j < pbullet.Count; j++)
                {
                    Rectangle tmpEmyRec = new Rectangle((int)pbullet[j].X, (int)pbullet[j].Y, EnemySprite.Width, EnemySprite.Height);
                    if (tmpBullRec.Intersects(tmpEmyRec))
                    {
                        EnemyList.RemoveAt(i);

                        
                        if (scoreKillCD > 15)
                        {
                            scoreToPrint += 2;
                            scoreKillCD = 0;
                        }
                    }
                }

            }
        }

        ///Player Logic
        void Player()
        {
            PlayerMovement = new Vector2(x, y);
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (Vel > 9 == false)
                    Vel += 0.5f;
                x -= Vel;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (Vel < -9 == false)
                    Vel -= 0.5f;
                x -= Vel;
            }
            else if (Vel > 0.01f == false || Vel > -0.01f)
            {
                Vel *= 0.93f;
                x -= Vel;
            }
            if (x < -30)
                x = 800;
            if (x > 800)
                x = 0;
            prevKs = ks;
            ks = Keyboard.GetState();
            BulletCD++;
            if(prevKs.IsKeyUp(Keys.Space) && ks.IsKeyDown(Keys.Space) && BulletCD > 15)
            {
                Fire();
            }

            PlayerRec = new Rectangle((int)x, (int)y,PlayerSprite.Width,PlayerSprite.Height);
        }

        ///Fire method
        void Fire()
        {
            BulletCD = 0;
            if (pbullet.Count < UpperBulletLimit)
            {
                pbullet.Add(new Vector2(x, y));
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            spriteBatch.Begin();

            spriteBatch.DrawString(score, "Score: " + scoreToPrint, new Vector2(390, 460),Color.White);
            spriteBatch.Draw(PlayerSprite, PlayerMovement, Color.White);

            foreach (Vector2 bulletPos in pbullet)
            {
                spriteBatch.Draw(PlayerSprite, bulletPos, Color.White);
            }

            foreach(Vector2 EnemyPos in EnemyList)
            {
                spriteBatch.Draw(EnemySprite, EnemyPos, Color.White);
            }
            

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
