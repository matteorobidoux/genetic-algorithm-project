using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RobbyVisualizer
{
    public class BackgroundSprite : DrawableGameComponent
    {
        private Game _game;
        private SpriteBatch _spriteBatch;
        private Texture2D _background;

        public BackgroundSprite(Game game) : base(game){
            _game = game;
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
            _background = _game.Content.Load<Texture2D>("CookieMonsterBackground");

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
            _spriteBatch.Draw(_background, new Rectangle(0,100, 1000, 1000),Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}