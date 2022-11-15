using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using System;
using System.IO;

public class ButtonSprite : DrawableGameComponent
{
    private int _xPosition, _yPosition;
    private Game _game;
    private SpriteBatch _spriteBatch;
    private Texture2D _squareSprite;
    private SpriteFont _spriteFont;
    private bool _isClicked;
    public bool IsClicked{
        get{
            return _isClicked;
        }
        set{
            _isClicked = value;
        }
    }

    private string[] _files;
    public string[] Files{
        get{
            return _files;
        }
    }

    public ButtonSprite(Game game, int xPosition, int yPosition) : base(game)
    {
        _game = game;
        _xPosition = xPosition;
        _yPosition = yPosition;
        _isClicked = false;
    }

    public override void Initialize()
    {
        // Calls base Initialize
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _squareSprite = _game.Content.Load<Texture2D>("Square");
        _spriteFont = _game.Content.Load<SpriteFont>("Fonts/ButtonText");

        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        var mousePoint = new Point(mouseState.X, mouseState.Y);
        var rectangle = new Rectangle(_xPosition,_yPosition,400,80);

        if (rectangle.Contains(mousePoint))
        {
            if(mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed){  
                FolderBrowserDialog folderDlg = new FolderBrowserDialog();  
                folderDlg.ShowNewFolderButton = true;   
                DialogResult result = folderDlg.ShowDialog();
                _isClicked = true;
                _files = Directory.GetFiles(folderDlg.SelectedPath);
                Array.Sort(_files);
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin();
        _spriteBatch.Draw(_squareSprite, new Rectangle(_xPosition,_yPosition,400,80), Color.White);
        _spriteBatch.DrawString(_spriteFont, "Select Folder With Proper Data", new Vector2(680, 960), Color.Black);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}