using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Threading;
using System;

namespace RobbyVisualizer
{
    public class CookieMonsterSprite : DrawableGameComponent
    {
        private KeyboardState _previous;
        private KeyboardState _current;
        private Game _game;
        private SpriteBatch _spriteBatch;
        private Texture2D _cookieMonsterSprite;
        private Texture2D _cookieMonsterEatingSprite;
        private bool _eating;
        public bool Eating{
            get{
                return _eating;
            }
            set {
                _eating = value;
            }
        }
        private bool _run;
        public bool Run{
            get{
                return _run;
            }
            set {
                _run = value;
            }
        }
        private int _xPosition;

        public int XPosition {
            get {
                return _xPosition;
            }
        }
        private int _yPosition;

         public int YPosition {
            get {
                return _yPosition;
            }
        }
        private string[] _files;
        public string[] Files{
            get{
                return _files;
            }
            set{
                _files = value;
            }
        }
        private int _timeElapsed;

        public CookieMonsterSprite(Game game, int xPosition, int yPosition) : base(game){
            _previous = Keyboard.GetState();
            _current = Keyboard.GetState();
            _game = game;
            _xPosition = xPosition;
            _yPosition = yPosition;
            _run = false;
            _eating = false;
            _timeElapsed = 0;
        }

        public override void Initialize()
        {
            // Calls base Initialize
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Created keySprite with the keyType (Image name) as the image
            _cookieMonsterSprite = _game.Content.Load<Texture2D>("CookieMonster");
            _cookieMonsterEatingSprite = _game.Content.Load<Texture2D>("CookieMonsterEating");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _previous = _current;
            _current = Keyboard.GetState();

            if(_run){
                for(int i =0; i< _files.Length; i++){
                    string moves =  System.IO.File.ReadAllText(_files[i]);
                    for(int j =0; j < moves.Length; j++){
                        if(_timeElapsed == 600){
                            if(moves[j] == '1' && _yPosition - 78 >= 20){
                                _yPosition -= 78;
                            } else if(moves[j] == '2' && _yPosition + 78 <= 722){
                                _yPosition += 78;
                            } else if(moves[j] == '3' && _xPosition - 78 >= 460){
                                _xPosition -= 78;
                            } else if(moves[j] == '4' && _xPosition + 78 <= 1162){
                                _xPosition += 78;
                            }
                            if(i == _files.Length-1 && j == moves.Length-1){
                                _run = false;
                            }
                            _timeElapsed = 0;
                        } else{
                            _timeElapsed++;
                        }
                    }
                }
            }

            // Calls base Update
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            if(_eating){
                _spriteBatch.Draw(_cookieMonsterEatingSprite, new Rectangle(_xPosition, _yPosition, 100, 100), Color.White);
            } else {
                _spriteBatch.Draw(_cookieMonsterSprite, new Rectangle(_xPosition, _yPosition, 100, 100), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}