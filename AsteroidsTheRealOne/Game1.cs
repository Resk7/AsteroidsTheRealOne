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

        Vector2 PlayerMovement;
        Vector2 PBullet;

        float x, y, Vel;
        float BulletSpeed, yB;
        float UpperBulletLimit = 30000;
        List<Vector2> pbullet = new List<Vector2>();
        List<int> bulletInt = new List<int>();


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




            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            PlayerSprite = Content.Load<Texture2D>("PlayerSprite");
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

            

            for (int i = 0; i < pbullet.Count; i++)
            {
                yB -= 1;
               pbullet[i] = new Vector2(x, yB);
            }

            base.Update(gameTime);
        }

        ///Player Logic
        void Player()
        {
            PlayerMovement = new Vector2(x, y);
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (Vel > 8 == false)
                    Vel += 0.5f;
                x -= Vel;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (Vel < -8 == false)
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

            if(prevKs.IsKeyUp(Keys.Space) && ks.IsKeyDown(Keys.Space))
            {
                Fire();
            }
        }

        ///Fire method
        void Fire()
        {
            if (pbullet.Count < UpperBulletLimit)
            {
                yB = y;
                pbullet.Add(new Vector2(x, yB));
                
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            spriteBatch.Begin();

            spriteBatch.Draw(PlayerSprite, PlayerMovement, Color.White);
            spriteBatch.Draw(PlayerSprite, PBullet, Color.White);
            foreach (Vector2 bulletPos in pbullet)
            {
                spriteBatch.Draw(PlayerSprite, bulletPos, Color.White);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
