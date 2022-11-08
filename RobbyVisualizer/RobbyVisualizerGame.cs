using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace RobbyVisualizer
{
    public class RobbyVisualizerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GridUnitSprite[,] _grid;
        private CookieMonsterSprite _cookieMonster;
        private CookieSprite[,] _cookies;
        private ButtonSprite _buttonSprite;
        private Song _eatingCookie;
        private SpriteFont _infoFontSprite;
        private Texture2D _background;
        private bool _run;
        public RobbyVisualizerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _run = false;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1770;
            _graphics.PreferredBackBufferHeight = 1060;
            _graphics.ApplyChanges();

            _grid = new GridUnitSprite[10,10];
            _cookies = new CookieSprite[10,10];

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

                    if((i % 2 == 0 && j % 2 == 0) || (i % 2 == 1 && j % 2 == 1)){
                        CookieSprite cookie = new CookieSprite(this, xPos-10, yPos-10);
                        _cookies[i, j] = cookie;
                        Components.Add(cookie);
                    }
                    xPos += 78;
                }
                xPos = 470;
                yPos += 78;
            }           

            // Moving square to square is -10 x and -10 y
            _cookieMonster = new CookieMonsterSprite(this, 460, 20);
            Components.Add(_cookieMonster);

            _buttonSprite = new ButtonSprite(this, 650, 930);
            Components.Add(_buttonSprite);


            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _eatingCookie = Content.Load<Song>("Sounds/EatingCookie");
            _infoFontSprite = Content.Load<SpriteFont>("Fonts/Info");
            _background = Content.Load<Texture2D>("CookieMonsterBackground");

            // TODO: use this.Content to load your game content here
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            OnCookie();

            if(_buttonSprite.IsClicked){
                _buttonSprite.IsClicked = false;
                _run = true;
                MoveCookieMonster();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Rectangle(0,0,1950, 1125),Color.White);
            _spriteBatch.DrawString(_infoFontSprite, $"Generation: ", new Vector2(470, 820), Color.White);
            _spriteBatch.DrawString(_infoFontSprite, "Move: ", new Vector2(470, 855), Color.White);
            _spriteBatch.DrawString(_infoFontSprite, "Points: ", new Vector2(470, 890), Color.White);
            _spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        public void MoveCookieMonster(){
            var task = new Task(() => {
                foreach(string file in _buttonSprite.Files){
                    string moves = System.IO.File.ReadAllText(file);
                    foreach(char move in moves){
                        if(move == '1' && _cookieMonster.YPosition - 78 >= 20){
                            _cookieMonster.YPosition -= 78;
                        } else if(move == '2' && _cookieMonster.YPosition + 78 <= 722){
                        _cookieMonster.YPosition += 78;
                        } else if(move == '3' && _cookieMonster.XPosition - 78 >= 460){
                            _cookieMonster.XPosition -= 78;
                        } else if(move == '4' && _cookieMonster.XPosition + 78 <= 1162){
                            _cookieMonster.XPosition += 78;
                        }
                        Thread.Sleep(500);
                    }
                    _cookieMonster.XPosition = 460;
                    _cookieMonster.YPosition = 20;
                    MediaPlayer.Stop();
                    Thread.Sleep(2000);
                } 
            });
            task.Start();
        }

        public void OnCookie(){
            for(int i =0; i< _cookies.GetLength(0); i++){
                for(int j =0; j< _cookies.GetLength(1); j++){
                    if( _cookies[i,j] != null){
                        if(_cookieMonster.XPosition == _cookies[i,j].XPosition && _cookieMonster.YPosition == _cookies[i,j].YPosition){
                            if(_cookies[i,j].IsVisible && _run){
                                _cookieMonster.Eating = true;
                                MediaPlayer.Play(_eatingCookie);
                                _cookies[i,j].IsVisible = false;
                            }
                        }
                    } else {
                        if(_cookieMonster.XPosition + 10 == _grid[i,j].XPosition && _cookieMonster.YPosition + 10 == _grid[i,j].YPosition){
                            _cookieMonster.Eating = false;
                        }
                    }
                }
            }
        }
    }
}
