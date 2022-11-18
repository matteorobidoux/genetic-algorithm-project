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
    private string[] _fileDetails;
    private int _numOfMoves;
    private IRobbyTheRobot _robby;
    private ContentsOfGrid[,] _contentGrid;
    private int _baseX;
    private int _baseY;
    private int _sizeChange;
    private bool _displayNewGrid;
    private Random _rand;
    private int _xStarting;
    private int _yStarting;
    private double _points;
    private int _gridSize;
    private bool _validFile;

    public RobbyVisualizerGame()
    {
      _graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
      _fileDetails = null;
      _numOfMoves = 0;
      _robby = Robby.CreateRobby(200, 1, 100, 0.5, 0.5, 200, null);
      _baseX = 470;
      _baseY = 30;
      _sizeChange = 78;
      _displayNewGrid = true;
      _rand = new Random();
      _points = 0;
      _gridSize = 10;
      _validFile = false;
    }

    protected override void Initialize()
    {
      _graphics.PreferredBackBufferWidth = 1770;
      _graphics.PreferredBackBufferHeight = 1060;
      _graphics.ApplyChanges();

      _grid = new GridUnitSprite[_gridSize, _gridSize];
      _cookies = new CookieSprite[_gridSize, _gridSize];

      int xPos = _baseX;
      int yPos = _baseY;

      for (int i = 0; i < _gridSize; i++)
      {
        for (int j = 0; j < _gridSize; j++)
        {
          Color color;
          if ((i % 2 == 0 && j % 2 == 0) || (i % 2 == 1 && j % 2 == 1))
          {
            color = Color.DarkRed;
          }
          else
          {
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
      // Occurs when ever it goes to a new file, it must create a new grid, new starting position and reset all variables used.
      if (_displayNewGrid)
      {
        MediaPlayer.Stop();
        RemoveCookies();
        _numOfMoves = 0;
        _points = 0;
        _contentGrid = _robby.GenerateRandomTestGrid();
        _cookies = new CookieSprite[_gridSize, _gridSize];

        // Adds cookie to the cookie grid matching the ContentsGrid and then adding it to the components
        for (int i = 0; i < _gridSize; i++)
        {
          for (int j = 0; j < _gridSize; j++)
          {
            if (_contentGrid[i, j] == ContentsOfGrid.Can)
            {
              CookieSprite cookie = new CookieSprite(this, _grid[i, j].XPosition, _grid[i, j].YPosition);
              _cookies[i, j] = cookie;
              Components.Add(cookie);
            }
          }
        }

        // Random starting position on the grid
        _yStarting = _rand.Next(0, 10);
        _xStarting = _rand.Next(0, 10);

        // If sprite already exists, remove it from the components
        if (_cookieMonster != null)
        {
          Components.Remove(_cookieMonster);
        }

        _cookieMonster = new CookieMonsterSprite(this, _grid[_yStarting, _xStarting].XPosition, _grid[_yStarting, _xStarting].YPosition);
        Components.Add(_cookieMonster);
        _displayNewGrid = false;
      }

      // If files are chosen, start moving
      if (_buttonSprite.IsClicked)
      {
        _buttonSprite.IsClicked = false;
        MoveCookieMonster();
      }

      // TODO: Add your update logic here

      base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      _spriteBatch.Begin();
      _spriteBatch.Draw(_background, new Rectangle(0, 0, 1950, 1125), Color.White);
      if (_validFile)
      {
        _spriteBatch.DrawString(_infoFontSprite, $"Generation: {_fileDetails[0]}", new Vector2(470, 820), Color.White);
        _spriteBatch.DrawString(_infoFontSprite, $"Move: {_numOfMoves}/{_fileDetails[2]}", new Vector2(470, 855), Color.White);
        _spriteBatch.DrawString(_infoFontSprite, $"Points: {_points}/{_fileDetails[1]}", new Vector2(470, 890), Color.White);
      }
      _spriteBatch.End();
      // TODO: Add your drawing code here
      base.Draw(gameTime);
    }

    private void MoveCookieMonster()
    {
      var task = new Task(() =>
      {

        // Loop through all files within file list
        foreach (string file in _buttonSprite.Files)
        {
          int[] moves = null;
          try
          {
            _validFile = false;
            _fileDetails = System.IO.File.ReadAllText(file).Split(",");


            moves = new int[_fileDetails.Length - 3];

            // Retrieve only the moves section from the file
            for (int i = 3; i < _fileDetails.Length; i++)
            {
              moves[i - 3] = Convert.ToInt32(_fileDetails[i]);
            }
            _validFile = true;
          }
          catch (Exception)
          {
            MessageBox.Show("Invalid Folder Content... Skipping File!");
            _validFile = false;
            continue;
          }

          // Used to keep track of previous parameter values
          int previousX = _xStarting;
          int previousY = _yStarting;
          double previousPoints = _points;

          // Loop the amount of time the file says to loop through
          for (int i = 0; i < Int32.Parse(_fileDetails[2]); i++)
          {
            _points += RobbyHelper.ScoreForAllele(moves, _contentGrid, new Random(), ref _yStarting, ref _xStarting);

            _cookieMonster.Eating = false;

            // If a cookie is eaten
            if (previousPoints + 10 == _points)
            {
              EatCookie(previousY, previousX);
            }

            // Checks weather he moves right or left in the grid to correspond it to the GUI
            if (previousX > _xStarting)
            {
              _cookieMonster.XPosition -= _sizeChange;
            }
            else if (previousX < _xStarting)
            {
              _cookieMonster.XPosition += _sizeChange;
            }

            // Checks weather he moves up or down in the grid to correspond it to the GUI
            if (previousY > _yStarting)
            {
              _cookieMonster.YPosition -= _sizeChange;
            }
            else if (previousY < _yStarting)
            {
              _cookieMonster.YPosition += _sizeChange;
            }

            // Set the new previous parameters
            previousX = _xStarting;
            previousY = _yStarting;
            previousPoints = _points;


            Thread.Sleep(200);
            _numOfMoves++;
          }

          // New file will be looped through, therefor display a new grid and reset
          _displayNewGrid = true;

          Thread.Sleep(2000);
        }
      });
      task.Start();
    }

    // Takes current location and makes him eat the good, play the eating sound and ensure the cookie is no longer visible
    private void EatCookie(int y, int x)
    {
      _cookieMonster.Eating = true;
      MediaPlayer.Play(_eatingCookie);
      _cookies[y, x].IsVisible = false;
    }

    // Loops through the cookie grid and removes all cookies from Components
    private void RemoveCookies()
    {
      for (int i = 0; i < _gridSize; i++)
      {
        for (int j = 0; j < _gridSize; j++)
        {
          if (_cookies[i, j] != null)
          {
            Components.Remove(_cookies[i, j]);
          }
        }
      }
    }

  }
}
