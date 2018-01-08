using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AsteroidsTheRealOne
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D PlayerSprite;

        Vector2 PlayerMovement;

        float x, y, Vel;



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
            else
            {
                Vel *= 0.95f;
                x -= Vel;
            }
            if (x < -30)
                x = 800;
            if (x > 800)
                x = 0;
            

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            spriteBatch.Begin();

            spriteBatch.Draw(PlayerSprite, PlayerMovement, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
