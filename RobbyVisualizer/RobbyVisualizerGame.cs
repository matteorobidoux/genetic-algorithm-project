using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace RobbyVisualizer
{
    public class RobbyVisualizerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SimulationSprite[,] _grid;
        private CookieMonsterSprite _cookieMonster;
        private CookieSprite _cookie;

        public RobbyVisualizerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1770;
            _graphics.PreferredBackBufferHeight = 980;
            _graphics.ApplyChanges();

            _grid = new SimulationSprite[10,10];

            int xPos = 470;
            int yPos = 30;

            for(int i = 0; i< 10; i++){
                for(int j = 0; j< 10; j++){
                    Color color;
                    if((i % 2 == 0 && j % 2 == 0) || (i % 2 == 1 && j % 2 == 1)){
                        color = Color.DarkRed;
                    } else {
                        color = Color.White;
                    }

                    SimulationSprite gridUnit = new SimulationSprite(this, xPos, yPos, color);
                    _grid[i, j] = gridUnit;
                    Components.Add(gridUnit);
                    xPos += 78;
                }
                xPos = 470;
                yPos += 78;
            }

            _cookie = new CookieSprite(this, 463, 23);
            Components.Add(_cookie);

            // Moving square to square is -10 x and -10 y
            _cookieMonster = new CookieMonsterSprite(this, 200, 20);
            Components.Add(_cookieMonster);
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
