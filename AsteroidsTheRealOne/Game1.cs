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

        Vector2 PlayerMovement;

        float x, y, Vel;
        float UpperBulletLimit = 30;
        float fallspeed = 4;

        int EspawnX, EspawnY;
        int BulletCD = 0;

        Rectangle PlayerRec;

        List<Vector2> pbullet;
        List<Vector2> EnemyList;
        List<Rectangle> BulletRecList;
        List<Rectangle> EnemyRecList;
        Random random = new Random();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Player Initialize
            x = 400; y = 420;
            PlayerMovement = new Vector2(x, y);
            Vel = 0;
            pbullet = new List<Vector2>();
            EnemyList = new List<Vector2>();
            BulletRecList = new List<Rectangle>();
            EnemyRecList = new List<Rectangle>();

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            PlayerSprite = Content.Load<Texture2D>("PlayerSprite");
            EnemySprite = Content.Load<Texture2D>("EnemySprite");
            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent()
        {
            
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Player();
            Enemy();

            for (int i = 0; i < pbullet.Count; i++)
            {
               pbullet[i] += new Vector2(0, -10);
                BulletRecList[i] = new Rectangle((int)pbullet[i].X, (int)pbullet[i].Y, PlayerSprite.Width, PlayerSprite.Height);
                //Rectangle BulletRec = new Rectangle((int)pbullet[i].X, (int)pbullet[i].Y, PlayerSprite.Width, PlayerSprite.Height);
                if (pbullet[i].Y == -10)
                    pbullet.RemoveAt(i);
                
            }

            base.Update(gameTime);
        }

        ///Enemy Rectangle logic, Movement, and spawning
        void Enemy()
        {
            fallspeed += 0.001f;

            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i] += new Vector2(0, fallspeed);
                EnemyRecList[i] = new Rectangle((int)EnemyList[i].X, (int)EnemyList[i].Y, EnemySprite.Width, EnemySprite.Height);
                if (EnemyRecList[i].Intersects(PlayerRec))
                    EnemyList.RemoveAt(i);
                if (EnemyList[i].Y >= 530)
                    EnemyList.RemoveAt(i);
                if (EnemyRecList[i].IsEmpty == false)
                    EnemyList.RemoveAt(i);
                for (int j = 0; j < EnemyRecList.Count; j++)
                {
                    for (int k = 0; k < BulletRecList.Count; k++)
                    {
                        if (EnemyRecList[j].Intersects(BulletRecList[k]))
                        {
                            EnemyList.RemoveAt(j);
                        }
                        
                    }
                }
            }

            EspawnX = random.Next(0, 800);
            EspawnY = random.Next(-400, 15);
            while (EnemyList.Count < 16)
            {
                EnemyList.Add(new Vector2(EspawnX, EspawnY));
                EnemyRecList.Add(new Rectangle(EspawnX, EspawnY, EnemySprite.Width, EnemySprite.Height));
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
                BulletRecList.Add(new Rectangle((int)x, (int)y, PlayerSprite.Width, PlayerSprite.Height));
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            spriteBatch.Begin();

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
