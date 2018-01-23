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
        Texture2D BulletSprite;
        Texture2D EnemySprite;
        SpriteFont score;

        Vector2 PlayerVector;
        Vector2 Velocity;

        float x, y; 
        float Vel;
        float UpperBulletLimit = 30;
        float fallspeed = 4;
        float PlayerRotation;

        double PlayerSin;
        double PlayerCos;

        int EspawnX, EspawnY;
        int BulletCD = 0;
        int scoreToPrint = 0;
        int scoreKillCD = 0;
        int scoreTimer;
        int Highscore = 0;

        bool Gamerun = true;

        Rectangle PlayerRec;

        List<Vector2> pbullet;
        List<Vector2> EnemyList;
        List<float> BulletRotation;
        List<float> EnemySpawnCircle;
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
            PlayerVector = new Vector2(x, y);
            Vel = 0;
            Velocity = new Vector2(0,0);
            pbullet = new List<Vector2>();
            EnemyList = new List<Vector2>();
            BulletRotation = new List<float>();
            EnemySpawnCircle = new List<float>();

            PlayerRotation = (float)Math.PI / 2;
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
            BulletSprite = Content.Load<Texture2D>("BulletSprite");
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

                if (scoreToPrint > Highscore)
                    Highscore = scoreToPrint;
                scoreTimer++;
                if (scoreTimer >= 60)
                {
                    scoreToPrint+= 1;
                    scoreTimer = 0;

                }
                
                 
            }

            PlayerSin = Math.Sin(PlayerRotation);
            PlayerCos = Math.Cos(PlayerRotation);

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

                pbullet[i] -= new Vector2((float)Math.Cos(BulletRotation[i]) * 15, (float)Math.Sin(BulletRotation[i]) * 15);

                if (pbullet[i].Y <= -10 || pbullet[i].Y >= 480 || pbullet[i].X <= -10 || pbullet[i].X >= 810)
                {
                    pbullet.RemoveAt(i);
                    BulletRotation.RemoveAt(i);
                }
                    

            }
        }

        void updateEnemies()
        {

            

            while (EnemyList.Count < 16)
            {


                double RandomRotation = random.Next(0, 10000) / 10000d;
                RandomRotation *= Math.PI * 2;

                EnemySpawnCircle.Add((float)RandomRotation);
                EnemyList.Add(new Vector2((float)Math.Cos(RandomRotation) * 200, (float)Math.Sin(RandomRotation) * 200)+new Vector2(400,220));
            }


            for (int i = 0; i < EnemyList.Count; i++)
            {
                    
            }
        }

        void doEnemyBulletCollison()
        {
            for (int i = 0; i < EnemyList.Count; i++)
            {
                Rectangle tmpBullRec = new Rectangle((int)EnemyList[i].X, (int)EnemyList[i].Y, PlayerSprite.Width, PlayerSprite.Height);
                if (tmpBullRec.Intersects(PlayerRec))
                    //Gamerun = false;
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
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (Vel > 9 == false)
                    Vel += 0.2f;
                PlayerVector -= new Vector2((float)PlayerCos *Vel, (float)PlayerSin*Vel);
            }
            else if (Vel > 0.01f == false || Vel > -0.01f)
            {
                Vel *= 0.93f;
                PlayerVector -= new Vector2((float)PlayerCos * Vel, (float)PlayerSin * Vel);
            }


            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                PlayerRotation -= 0.1f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                PlayerRotation += 0.1f;
            }


            prevKs = ks;
            ks = Keyboard.GetState();
            BulletCD++;
            if(prevKs.IsKeyUp(Keys.Space) && ks.IsKeyDown(Keys.Space) && BulletCD > 7)
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
                pbullet.Add(new Vector2(PlayerVector.X, PlayerVector.Y));
                BulletRotation.Add(PlayerRotation);
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            spriteBatch.Begin();

            spriteBatch.DrawString(score, "Radian"+ PlayerRotation, new Vector2(390, 460),Color.White);
            spriteBatch.Draw(PlayerSprite, PlayerVector, null, null, new Vector2(PlayerSprite.Width/2,PlayerSprite.Height/2), PlayerRotation+ (float)Math.PI / 2, null, Color.White, SpriteEffects.None, 1);

            if(Gamerun == false)
            {
                spriteBatch.DrawString(score, "Press R to restart", new Vector2(350, 200),Color.White);
                spriteBatch.DrawString(score, "Highscore: " + Highscore, new Vector2(350, 180), Color.White);
            }

            foreach (Vector2 bulletPos in pbullet)
            {
                spriteBatch.Draw(BulletSprite, bulletPos, null, null, new Vector2(PlayerSprite.Width / 2, PlayerSprite.Height / 2), 0, null, Color.White, SpriteEffects.None, 1);
            }

            foreach(Vector2 EnemyPos in EnemyList)
            {
                spriteBatch.Draw(BulletSprite, EnemyPos, Color.Red);
            }
            

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
