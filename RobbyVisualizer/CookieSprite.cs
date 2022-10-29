using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RobbyVisualizer
{
    public class CookieSprite : DrawableGameComponent
    {
        private Game _game;
        private SpriteBatch _spriteBatch;
        private Texture2D _cookieSprite;
        public Boolean IsVisible;
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

        public CookieSprite(Game game, int xPosition, int yPosition) : base(game){
            _game = game;
            _xPosition = xPosition;
            _yPosition = yPosition;
            IsVisible = true;
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
            _cookieSprite = _game.Content.Load<Texture2D>("Cookie");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // Calls base Update
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            if(IsVisible){
                _spriteBatch.Draw(_cookieSprite, new Rectangle(_xPosition, _yPosition, 90, 90), Color.White);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}