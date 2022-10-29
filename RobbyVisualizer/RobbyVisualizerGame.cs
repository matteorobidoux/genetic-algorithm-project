using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RobbyVisualizer
{
    public class RobbyVisualizerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GridUnitSprite[,] _grid;
        private CookieMonsterSprite _cookieMonster;
        private CookieSprite _cookie;
        private ButtonSprite _buttonSprite;
        private Song _eatingCookie;

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

            _grid = new GridUnitSprite[10,10];

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

                    GridUnitSprite gridUnit = new GridUnitSprite(this, xPos, yPos, color);
                    _grid[i, j] = gridUnit;
                    Components.Add(gridUnit);
                    xPos += 78;
                }
                xPos = 470;
                yPos += 78;
            }

            _cookie = new CookieSprite(this, 460, 98);
            Components.Add(_cookie);

            // Moving square to square is -10 x and -10 y
            _cookieMonster = new CookieMonsterSprite(this, 460, 20);
            Components.Add(_cookieMonster);

            _buttonSprite = new ButtonSprite(this, 650, 850);
            Components.Add(_buttonSprite);

            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _eatingCookie = Content.Load<Song>("Sounds/EatingCookie");

            // TODO: use this.Content to load your game content here
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if(_cookieMonster.XPosition == _cookie.XPosition && _cookieMonster.YPosition == _cookie.YPosition ){
                if(_cookie.IsVisible){
                    MediaPlayer.Play(_eatingCookie);
                }
                _cookie.IsVisible = false;
            }

            if(_buttonSprite.IsClicked){
                _cookieMonster.Run = true;
            }
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
