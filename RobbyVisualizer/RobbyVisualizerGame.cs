using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System;
using RobbyTheRobot;

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
        private string[] _fileDetails;
        private int _numOfMoves;
        private IRobbyTheRobot _robby;
        private ContentsOfGrid[,] _contentGrid;
        private int _baseX;
        private int _baseY;
        private int _sizeChange;
        private bool _displayNewGrid;
        private Random rand;
        private int _xStarting;
        private int _yStarting;
        private double _points;

        public RobbyVisualizerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _run = false;
            _fileDetails = null;
            _numOfMoves = 0;
            _robby = Robby.CreateRobby(200, 1, 10, 100, 0.5, 0.5, 200, null);
            _contentGrid =  _robby.GenerateRandomTestGrid();
            _baseX = 470;
            _baseY = 30;
            _sizeChange = 78;
            _displayNewGrid = true;
            rand = new Random();
            _points = 0;

        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1770;
            _graphics.PreferredBackBufferHeight = 1060;
            _graphics.ApplyChanges();

            _grid = new GridUnitSprite[10,10];
            _cookies = new CookieSprite[10,10];

            int xPos = _baseX;
            int yPos = _baseY;

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

                    xPos += _sizeChange;
                }
                xPos = _baseX;
                yPos += _sizeChange;
            }           

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
            if(_displayNewGrid){
                for(int i = 0; i< 10; i++){
                    for(int j = 0; j< 10; j++){
                        if(_contentGrid[i,j] == ContentsOfGrid.Can){
                            CookieSprite cookie = new CookieSprite(this, _grid[i,j].XPosition-10, _grid[i,j].YPosition-10);
                            _cookies[i, j] = cookie;
                            Components.Add(cookie);
                        }
                    }
                }

                _yStarting = rand.Next(0,10);
                _xStarting = rand.Next(0,10);
                _cookieMonster = new CookieMonsterSprite(this, _grid[_yStarting,_xStarting].XPosition - 10, _grid[_yStarting,_xStarting].YPosition - 10);
                Components.Add(_cookieMonster);
                _displayNewGrid = false;
            }

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
            if(_fileDetails != null){
                _spriteBatch.DrawString(_infoFontSprite, $"Generation: {_fileDetails[0]}", new Vector2(470, 820), Color.White);
                _spriteBatch.DrawString(_infoFontSprite, $"Move: {_numOfMoves}/{_fileDetails[2]}", new Vector2(470, 855), Color.White);
                _spriteBatch.DrawString(_infoFontSprite, $"Points: {_points}/{_fileDetails[1]}", new Vector2(470, 890), Color.White);
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        public void MoveCookieMonster(){
            var task = new Task(() => {
                foreach(string file in _buttonSprite.Files){
                    _fileDetails = System.IO.File.ReadAllText(file).Split(",");
                    int[] moves = new int[_fileDetails.Length-3];
                    for(int i = 3; i < _fileDetails.Length; i++){
                        moves[i-3] = Convert.ToInt32(_fileDetails[i]);
                    }

                    int previousX = _xStarting;
                    int previousY = _yStarting;

                    for(int i = 3; i < Int32.Parse(_fileDetails[2])+3; i++){
                        _points += RobbyHelper.ScoreForAllele(moves,_contentGrid, new Random(),ref _xStarting, ref _yStarting);
                        
                        if(previousX > _xStarting){
                            _cookieMonster.XPosition -= _sizeChange;
                        } else if(previousX < _xStarting){
                            _cookieMonster.XPosition += _sizeChange;
                        }

                        if(previousY > _yStarting){
                            _cookieMonster.YPosition -= _sizeChange;
                        } else if(previousY < _yStarting){
                            _cookieMonster.YPosition += _sizeChange;
                        }

                        previousX = _xStarting;
                        previousY = _yStarting;
    
                        Thread.Sleep(500);
                        _numOfMoves++;
                    }
                    _cookieMonster.XPosition = 460;
                    _cookieMonster.YPosition = 20;
                    MediaPlayer.Stop();
                    Thread.Sleep(2000);
                    _numOfMoves = 0;
                } 
            });
            task.Start();
        }

        public void OnCookie(){
            for(int i =0; i< _cookies.GetLength(0); i++){
                for(int j =0; j< _cookies.GetLength(1); j++){
                    if( _cookies[i,j] != null){
                        if(_cookieMonster.XPosition == _cookies[i,j].XPosition && _cookieMonster.YPosition == _cookies[i,j].YPosition && _cookieMonster.Eating == true){
                            if(_cookies[i,j].IsVisible && _run){
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
