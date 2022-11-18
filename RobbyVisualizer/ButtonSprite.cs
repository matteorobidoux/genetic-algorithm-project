using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class ButtonSprite : DrawableGameComponent
{
  private int _xPosition, _yPosition;
  private Game _game;
  private SpriteBatch _spriteBatch;
  private Texture2D _squareSprite;
  private SpriteFont _spriteFont;
  private bool _isClicked;
  public bool IsClicked
  {
    get
    {
      return _isClicked;
    }
    set
    {
      _isClicked = value;
    }
  }

  private string[] _files;
  public string[] Files
  {
    get
    {
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
    var rectangle = new Rectangle(_xPosition, _yPosition, 400, 80);

    // If the mouse is within the button and it is clicked
    if (rectangle.Contains(mousePoint))
    {
      if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
      {
        FolderBrowserDialog folderDlg = new FolderBrowserDialog();
        folderDlg.ShowNewFolderButton = true;
        DialogResult result = folderDlg.ShowDialog();

        // If user clicks button but does not choose folder assure program does not crash and just wait until they choose a folder
        try
        {
          _files = Directory.GetFiles(folderDlg.SelectedPath);

          // Ensure there are files within the folder
          if (_files.Length == 0)
          {
            throw new Exception();
          }

          Regex regex = new Regex(@"\\generation(\d+).txt$");

          // Ensures files are as expected
          foreach (var file in _files)
          {
            if (!(regex.IsMatch(file)))
            {
              throw new Exception();
            }
          }

          // Sorts the files by ascending order based of the generation number
          Array.Sort(_files, (a, b) =>
          {
            var genA = Int32.Parse(regex.Match(a).Groups[1].Value);
            var genB = Int32.Parse(regex.Match(b).Groups[1].Value);
            return genA.CompareTo(genB);
          });
          _isClicked = true;
        }
        catch (Exception)
        {

          // Message Box shows up if Exceptionn is thrown
          MessageBox.Show("Invalid Folder Entered! Please Enter a Proper Test Folder!");
          _isClicked = false;
        }
      }
    }
  }

  public override void Draw(GameTime gameTime)
  {
    _spriteBatch.Begin();
    _spriteBatch.Draw(_squareSprite, new Rectangle(_xPosition, _yPosition, 400, 80), Color.White);
    _spriteBatch.DrawString(_spriteFont, "Select Folder With Proper Data", new Vector2(680, 960), Color.Black);
    _spriteBatch.End();
    base.Draw(gameTime);
  }
}