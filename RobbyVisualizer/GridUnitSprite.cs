using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RobbyVisualizer
{
    public class GridUnitSprite : DrawableGameComponent
    {
        private Game _game;
        private SpriteBatch _spriteBatch;
        private Texture2D _squareSprite;
        private int _xPosition;
        private int _yPosition;
        private Color _color;

        public GridUnitSprite(Game game, int xPosition, int yPosition, Color color) : base(game){
            _game = game;
            _xPosition = xPosition;
            _yPosition = yPosition;
            _color = color;
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
            _squareSprite = _game.Content.Load<Texture2D>("Square");

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
            _spriteBatch.Draw(_squareSprite, new Rectangle(_xPosition,_yPosition,78,78), _color);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}