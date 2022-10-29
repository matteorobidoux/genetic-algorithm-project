using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        public CookieMonsterSprite(Game game, int xPosition, int yPosition) : base(game){
            _previous = Keyboard.GetState();
            _current = Keyboard.GetState();
            _game = game;
            _xPosition = xPosition;
            _yPosition = yPosition;
            _run = false;
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

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _previous = _current;
            _current = Keyboard.GetState();

            if(_run){
                    if (_previous.IsKeyDown(Keys.W) && _current.IsKeyUp(Keys.W) && _yPosition - 78 >= 20) {
                        _yPosition -= 78;
                    }
                    if (_previous.IsKeyDown(Keys.S) && _current.IsKeyUp(Keys.S) && _yPosition + 78 <= 722) {
                    _yPosition += 78;
                    }
                    if (_previous.IsKeyDown(Keys.A) && _current.IsKeyUp(Keys.A) && _xPosition - 78 >= 460) {
                        _xPosition -= 78;
                    }
                    if (_previous.IsKeyDown(Keys.D) && _current.IsKeyUp(Keys.D) && _xPosition + 78 <= 1162) {
                        _xPosition += 78;
                    }
                
            }

            // Calls base Update
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_cookieMonsterSprite, new Rectangle(_xPosition, _yPosition, 100, 100), Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}