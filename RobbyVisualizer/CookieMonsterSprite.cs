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
        private int _xPosition;

        public int XPosition {
            get {
                return _xPosition;
            }
            set{
                _xPosition = value;
            }
        }
        private int _yPosition;

         public int YPosition {
            get {
                return _yPosition;
            }
            set{
                _yPosition = value;
            }
        }

        public CookieMonsterSprite(Game game, int xPosition, int yPosition) : base(game){
            _game = game;

            // -10 being the fudge factor for Cookie Monster
            _xPosition = xPosition - 10;
            _yPosition = yPosition - 10;
            _eating = false;
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
            //Calls base Update
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