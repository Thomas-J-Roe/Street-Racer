using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using static System.Formats.Asn1.AsnWriter;
using SharpDX.Direct2D1;
using Color = Microsoft.Xna.Framework.Color;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SharpDX.WIC;
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel.DataAnnotations;

namespace Street_Racer_V3
{
    public class NewGame : Game
    {
        //general variables
        private GraphicsDeviceManager graphics;//initialises graphics
        private SpriteBatch spriteBatch;//initialises spritebatch
        public static int ScreenHeight; //stores screenheight
        public static int ScreenWidth; //stores screen width
        private Random Random = new Random();//initialises random function

        //menu and leaderboard variables
        private SpriteFont menufont;//initialises menufont
        private SpriteFont titlefont;//initialises titlefont
        private enum MenuState { MainMenu, Playing, Leaderboard, Exit, Instructions }//sets up the game states
        private MenuState currentMenuState = MenuState.MainMenu; //sets up the state where the game starts
        private string[] menuItems = { "Enter Game", "Leaderboard", "Exit" };//stores the display names and order of each state
        private int selectedMenuState = 0;//stores where we are in the menu items
        private KeyboardState currentKeyState;//initialises current keyboard state
        private KeyboardState previousKeyState;//initialises previous keyboard state
        private bool LoadMenu = true;//initialises load menu
        private bool DrawMenu = true;//initialises draw menu
        private Leaderboard leaderboard;//initialises leaderboard

        //game variables
        private SpriteFont gamefont;//initialises gamefont
        private string filePath = "map.txt";//stores the file where the map is
        private Camera camera;//initialises camera
        private List<Enemy> enemies = new List<Enemy>();//initialises enemies list
        private List<MapTile> mapTiles = new List<MapTile>();//initialises maptiles list
        private string nameadd = " ";//initialises nameadd
        private bool HasWon = false;//initialises haswon boolean
        private bool HasLost = false;//initialises haslost boolean
        private bool DrawnEnd = false;//initialises drawnend boolean
        private bool AddName = false;//initialises addname boolean
        public List<Vector2> ValidSpawns = new List<Vector2>();//initialises validspawns list
        private int TimeLeft = 120000;//initialises timeleft
        private int delay = 100;//initialises delay
        private Texture2D PlayerTexture;//initialises player texture up
        public Texture2D PlayerTextureDown;//initialises player texture down
        public Texture2D PlayerTextureLeft;//initialises player texture left
        public Texture2D PlayerTextureRight;//initialises player texture right
        private Texture2D[] EnemyTextures;//initialises enemy textures array
        private Texture2D[] CityTilesTextures;//initialises citytiles textures array
        public bool[,] ValidTiles = new bool[25, 25];//initialises validtiles array
        private Texture2D RoadTexture;//initialises road texture in the sideways direction
        private Texture2D RoadTextureUp;//initialises road texture in the upwards direction
        private Texture2D TokenTexture;//initialises token texture
        private Texture2D HeartTexture;//initialises heart texture
        private Texture2D BlockageTexture;//initialises blockage texture in the sideways direction
        private Texture2D BlockageTextureUp;//initialises blockage texture in the upwards direction
        private bool IsRoad;//initialises isroad boolean
        private Token Token;//initialises token
        private Heart Heart;//initialises heart
        private Player Player;//initialises player
        private char lastEnteredChar = '\0';//initialises last entered char
        private bool HasEnteredScore = false;//initialises has entered score
        private bool HasEnteredName = false;//initialises has entered name
        private bool NameCorrect = false;//initialises name correct
        public NewGame()//initialises the game
        {
            graphics = new GraphicsDeviceManager(this);//initialises graphics
            Content.RootDirectory = "Content";//sets the content directory
            IsMouseVisible = true;//sets the mouse to be visible
        }

        protected override void Initialize()//initialises game variables
        {
            ScreenHeight = graphics.PreferredBackBufferHeight; //gets screenheight
            ScreenWidth = graphics.PreferredBackBufferWidth; //gets screenwidth
            camera = new Camera();//starts the camera
            string leaderboardFilePath = "leaderboard.txt";//stores the file path of the leaderboard
            leaderboard = new Leaderboard(leaderboardFilePath);//initialises the leaderboard

            base.Initialize();
        }

        protected override void LoadContent()//Loads the game content
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);//loads the spritebatch

            ValidSpawns.Add(new Vector2(385, 260));//adds 8 valid spawn positions
            ValidSpawns.Add(new Vector2(410, 410));
            ValidSpawns.Add(new Vector2(135, 135));
            ValidSpawns.Add(new Vector2(235, 160));
            ValidSpawns.Add(new Vector2(260, 260));
            ValidSpawns.Add(new Vector2(185, 410));
            ValidSpawns.Add(new Vector2(475, 360));
            ValidSpawns.Add(new Vector2(475, 260));

            menufont = Content.Load<SpriteFont>("MenuFont");//stores the font used in the menu
            titlefont = Content.Load<SpriteFont>("TitleFont");//stores the font used in the title

            PlayerTexture = Content.Load<Texture2D>("Race car");//Loads player's up texture
            PlayerTextureDown = Content.Load<Texture2D>("Race car down");//Loads player's down texture
            PlayerTextureLeft = Content.Load<Texture2D>("Race car left");//Loads player's left texture
            PlayerTextureRight = Content.Load<Texture2D>("Race car right");//Loads player's right texture
            Player = new Player(PlayerTexture, CheckOrigin(), PlayerTextureDown, PlayerTextureLeft, PlayerTextureRight);//loads the player

            TokenTexture = Content.Load<Texture2D>("Token");//Loads the texture for the token
            Vector2 Origin = CheckOrigin();//checks if the origin is valid
            while (Origin == Player.Position)//if the origin is the same as the player then change the origin
            {
                Origin = CheckOrigin();
            }
            Token = new Token(TokenTexture, Origin);//loads the token

            HeartTexture = Content.Load<Texture2D>("Health");//Loads the texture for the heart
            Origin = CheckOrigin();
            while (Origin == Player.Position)
            {
                Origin = CheckOrigin();
            }
            Heart = new Heart(HeartTexture, Origin);//loads the heart

            RoadTexture = Content.Load<Texture2D>("Road");//Loads the texture for the road in the sideways direction
            RoadTextureUp = Content.Load<Texture2D>("RoadUp");//Loads the texture for the road in the upwards direction

            BlockageTexture = Content.Load<Texture2D>("blockage");//Loads the texture for the blockage in the sideways direction
            BlockageTextureUp = Content.Load<Texture2D>("BlockageUp");//Loads the texture for the blockage in the upwards direction

            EnemyTextures = new Texture2D[]//Loads the textures for the enemies
            {
                Content.Load<Texture2D>("Enemy Car White"),
                Content.Load<Texture2D>("Enemy Car Red"),
                Content.Load<Texture2D>("Enemy Car Blue")

            };

            for (int i = 0; i < 6; i++)//loads 6 enemies

            {
                Texture2D EnemyTexture = EnemyTextures[Random.Next(EnemyTextures.Length)];
                Origin = CheckOrigin();
                while (Origin == Player.Position)
                {
                    Origin = CheckOrigin();
                }
                enemies.Add(new Enemy(EnemyTexture, Origin));
            }

            CityTilesTextures = new Texture2D[]//Loads the textures for the city tiles
            {
                Content.Load<Texture2D>("City Tile 1"),
                Content.Load<Texture2D>("City Tile 2"),
                Content.Load<Texture2D>("City Tile 3"),
                Content.Load<Texture2D>("City Tile 4"),
                Content.Load<Texture2D>("City Tile 5")
            };


            for (int x = 0; x < 25; x++)
            {
                for (int y = 0; y < 24; y++)
                {
                    Texture2D CityTileTexture = CityTilesTextures[Random.Next(CityTilesTextures.Length)];//loads the city tiles

                    if (File.Exists(filePath))
                    {
                        string[] lines = File.ReadAllLines(filePath);
                        foreach (var line in lines)
                        {
                            string[] parts = line.Split(':');
                            if (int.Parse(parts[0]) == x && int.Parse(parts[1]) == y)
                            {
                                if (parts[2] == "R")//checks if tile is road sideways
                                {
                                    IsRoad = true;
                                    mapTiles.Add(new MapTile(RoadTexture, new Vector2(x * 25, y * 25), IsRoad));//adds the road sideway tile
                                    ValidTiles[x, y] = true;
                                }
                                else if (parts[2] == "B")//checks if tile is blockage sideways
                                {
                                    IsRoad = false;
                                    mapTiles.Add(new MapTile(BlockageTexture, new Vector2(x * 25, y * 25), IsRoad));//adds the blockage sideway tile
                                    ValidTiles[x, y] = false;
                                }
                                else if (parts[2] == "RU")//checks if tile is road upwards
                                {
                                    IsRoad = true;
                                    mapTiles.Add(new MapTile(RoadTextureUp, new Vector2(x * 25, y * 25), IsRoad));//adds the road upwards tile
                                    ValidTiles[x, y] = true;
                                }
                                else if (parts[2] == "BU")//checks if tile is blockage upwards
                                {
                                    IsRoad = true;
                                    mapTiles.Add(new MapTile(BlockageTextureUp, new Vector2(x * 25, y * 25), IsRoad));//adds the blockage upwards tile
                                    ValidTiles[x, y] = true;
                                }
                                else 
                                {
                                    IsRoad = false;
                                    mapTiles.Add(new MapTile(CityTileTexture, new Vector2(x * 25, y * 25), IsRoad));//adds the city tiles
                                    ValidTiles[x, y] = false;
                                }
                            }

                        }
                    }
                }
            }
            Sprite.ValidTiles = ValidTiles;//assigns the valid tiles to the static valid tiles in the sprite class
            Sprite.ValidSpawns = ValidSpawns;//assigns the valid spawns to the static valid spawns in the sprite class
            gamefont = Content.Load<SpriteFont>("Game font");//loads the game font
        }

        protected override void Update(GameTime gameTime)//Updates all game components
        {
            if (LoadMenu)//if on menu input, start menu
            {
                currentKeyState = Keyboard.GetState();//stores input of keyboard

                if (currentMenuState == MenuState.MainMenu)//if on menu statw, start menu
                {
                    HandleMenuNavigation();
                }
                else if (currentMenuState == MenuState.Leaderboard)//if on leaderboard state, start leaderboard
                {
                    if (IsKeyPressed(Keys.Escape))//if escapes on leaderboard, go back to main menu
                    {
                        currentMenuState = MenuState.MainMenu;
                    }
                }
                else if (currentMenuState == MenuState.Instructions)//if on instructions state, start instructions
                {
                    if (IsKeyPressed(Keys.Escape))//if escapes on instructions, go back to main menu
                    {
                        currentMenuState = MenuState.MainMenu;
                    }
                    else if (IsKeyPressed(Keys.Enter))//if enters on instructions, go to playing state
                    {
                        currentMenuState = MenuState.Playing;
                    }
                }
                else if (currentMenuState == MenuState.Playing)//if on playing state, starts game
                {
                    LoadMenu = false;
                }
                else if (currentMenuState == MenuState.Exit)//if on exit input, exit
                {
                    Exit();
                }
                previousKeyState = currentKeyState;//stores current state as previous state
            }
            else if (AddName)//if adding score, checks user input
            {

                KeyboardState state = Keyboard.GetState();//stores keyboard state
                Keys[] keys = state.GetPressedKeys();//stores array of pressed keys
                foreach (Keys key in keys)//adds the key to the name
                {
                    if (key == Keys.Back && nameadd.Length > 0) //handles backspace
                    {
                        nameadd = nameadd.Substring(0, nameadd.Length - 1);
                        lastEnteredChar = nameadd.Length > 0 ? nameadd[nameadd.Length - 1] : '\0';
                    }
                    else if (key >= Keys.A && key <= Keys.Z) //handles letters
                    {
                        char currentChar = key.ToString().ToUpper()[0];// converts to uppercase
                        
                            nameadd += currentChar; //adds the letter to the name 
                        lastEnteredChar = currentChar;
                        
                    }
                    else if (key == Keys.Escape)//if escape pressed, name is correct
                    {
                        NameCorrect = true;
                    }
                    else if (key == Keys.Enter) //handles enter key, storing name if correct
                    {
                        if (NameCorrect)
                        {
                            leaderboard.AddScore(nameadd, TimeLeft);
                            HasEnteredScore = true;
                        }
                        else
                        {
                            HasEnteredName = true;
                        }
                    }
                    else
                    {

                    }
                }

            }
            else//updates game components
            {

                Player.Update();//updates player
                Token.Update(Player);//updates token
                Heart.Update(Player);//updates heart
                foreach (var enemy in enemies)//updates enemies
                {
                    enemy.Update(Player);
                }
                camera.Follow(Player.Position);//follows player with camera
                if (Player.EscapeInGame)//if escape pressed in game, exit game
                {
                    Exit();
                }

                if (Player.Health == 0 || TimeLeft < 0)//if player has no health or time left, player has lost
                {
                    HasLost = true;
                    if (DrawnEnd)
                    {
                        Exit();
                    }
                }
                if (Player.TokensLeft == 0)//if player has no tokens left, player has won
                {
                    HasWon = true;
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)//Draws all game components
        {
            GraphicsDevice.Clear(Color.Gray);//clears the screen


            if (DrawMenu)//draws the menu if on menu state
            {
                spriteBatch.Begin();//starts the spritebatch

                if (currentMenuState == MenuState.MainMenu)
                {
                    DrawTheMenu(); //draws the menu if that is the state
                }
                else if (currentMenuState == MenuState.Instructions)
                {
                    DrawInstructions();
                    //draws the instructions if that is the state
                }
                else if (currentMenuState == MenuState.Playing)
                {
                    DrawMenu = false;
                    //draws the game if that is the state
                }
                else if (currentMenuState == MenuState.Leaderboard)
                {
                    leaderboard.Draw(spriteBatch, menufont);
                    //draws the leaderboard if that is the state
                }
            }
            else if (HasWon)//if haswon, displays congratulations message
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(titlefont, "Congratulations.", new Vector2(50, 25), Color.Black);
                spriteBatch.DrawString(menufont, $"You completed the game with {TimeLeft / 1000} seconds left.", new Vector2(50, 125), Color.Black);
                spriteBatch.DrawString(menufont, "Please enter your name so your score can be saved.", new Vector2(50, 165), Color.Black);
                spriteBatch.DrawString(menufont, "Please tap each key lightly to add to name.", new Vector2(50, 205), Color.Black);
                spriteBatch.DrawString(menufont, "Press enter to see name.", new Vector2(50, 245), Color.Black);
                AddName = true;
                if (HasEnteredScore)
                {
                    Exit();
                }
                else if (HasEnteredName)
                {
                    spriteBatch.DrawString(menufont, $"Name: {nameadd}", new Vector2(50, 285), Color.Black);
                    spriteBatch.DrawString(menufont, "Press escape and enter to save.", new Vector2(50, 325), Color.Black);
                    spriteBatch.DrawString(menufont, "Use backspace to remove letters.", new Vector2(50, 365), Color.Black);
                }
            }
            else if (HasLost)//if haslost, displays commiserations message
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(titlefont, "Commiserations.", new Vector2(50, 25), Color.Black);
                spriteBatch.DrawString(menufont, "You didn't complete the game.", new Vector2(50, 125), Color.Black);
                spriteBatch.DrawString(menufont, "Better luck next time.", new Vector2(50, 165), Color.Black);
                if (delay != 0)
                {
                    delay--;
                }
                else
                {
                    Exit();
                }
            }

            else//draws the game
            {
                spriteBatch.Begin(transformMatrix: camera.Transformation);//assigns Transformation to transformationmatrix

                foreach (var tile in mapTiles)//draws each tile
                {
                    tile.Draw(spriteBatch);
                }
                Player.Draw(spriteBatch);//draws the player
                Token.Draw(spriteBatch);//draws the token
                Heart.Draw(spriteBatch);//draws the heart
                foreach (var enemy in enemies)//draws each enemy
                {
                    enemy.Draw(spriteBatch);

                }
                spriteBatch.DrawString(gamefont, $"Health left: {Player.Health}          Tokens Left: {Player.TokensLeft}          Time Left: {TimeLeft / 1000} seconds", new Vector2(Player.Position.X - 125, Player.Position.Y - 60), Color.White);//displays the health, tokens left and time left
                TimeLeft -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;//decreases the time left
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void HandleMenuNavigation()
        {
            if (IsKeyPressed(Keys.Up) || IsKeyPressed(Keys.W))
            {
                selectedMenuState--;
                if (selectedMenuState < 0)
                    selectedMenuState = menuItems.Length - 1; //if up is pressed, the previous state is selected, with the first looping to the last
            }
            if (IsKeyPressed(Keys.Down) || IsKeyPressed(Keys.S))
            {
                selectedMenuState++;
                if (selectedMenuState >= menuItems.Length)
                    selectedMenuState = 0; //if down is pressed, the next state is selected, with the last looping to the first
            }
            if (IsKeyPressed(Keys.Enter))
            {
                switch (selectedMenuState)//goes to the state selected from the menu
                {
                    case 0:
                        currentMenuState = MenuState.Instructions;
                        break;
                    case 1:
                        currentMenuState = MenuState.Leaderboard;
                        break;
                    case 2:
                        currentMenuState = MenuState.Exit;
                        break;
                }
            }
        }

        private bool IsKeyPressed(Keys key)//checks if a key is pressed
        {
            return currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyUp(key);//displays whether a key has been pressed
        }
        private void DrawTheMenu()//draws the menu
        {
            Vector2 position = new Vector2(250, 120);//initialises the position for menu
            spriteBatch.DrawString(titlefont, "Street Racer", position, Color.Black);//writes the title
            position = new Vector2(300, 90);//moves position up to other part of the title
            spriteBatch.DrawString(menufont, "Welcome To", position, Color.Black);//writes welcome line
            position = new Vector2(0, 250);//moves position to where menu starts
            spriteBatch.DrawString(menufont, "Please select the state you want to enter", position, Color.Black);//draws start of the menu part 1
            position.Y += 40;//changes position of the menu
            spriteBatch.DrawString(menufont, "Use WASD or Arrow Keys to move and Enter to select state", position, Color.Black);//draws start of the menu part 2
            position = new Vector2(100, 368);//moves position to where the menu items start
            for (int i = 0; i < menuItems.Length; i++)
            {
                Color textColour = (i == selectedMenuState) ? Color.White : Color.Black; //creates the base colour for stated in the menu and a colour for the selected item
                spriteBatch.DrawString(menufont, menuItems[i], position, textColour);//creates a line for each menu state
                position.Y += 40;//changes position of the menu item
            }
        }
        private void DrawInstructions()//draws the instructions
        {
            Vector2 position = new Vector2(50, 25);//initialises the position for instructions
            spriteBatch.DrawString(titlefont, "Instructions", position, Color.Black);//writes the title
            position.Y += 60;//moves position to where the instructions start
            spriteBatch.DrawString(menufont, "1) To w in the game you need to collect 3 tokens.", position, Color.Black);//writes the instructions
            position.Y += 40;
            spriteBatch.DrawString(menufont, "   Only 1 token exists at each moment and in a random place", position, Color.Black);
            position.Y += 40;
            spriteBatch.DrawString(menufont, "2) 3 enemy cars w ill try and cause you damage.", position, Color.Black);
            position.Y += 40;
            spriteBatch.DrawString(menufont, "   If you get too close they will damage you and respawn", position, Color.Black);
            position.Y += 40;
            spriteBatch.DrawString(menufont, "3) There w ill be a heart that can fully heal your damage", position, Color.Black);
            position.Y += 40;
            spriteBatch.DrawString(menufont, "   Only 1 heart exists in each moment", position, Color.Black);
            position.Y += 40;
            spriteBatch.DrawString(menufont, "4) You lose if you run out of time or have no health", position, Color.Black);
            position.Y += 40;
            spriteBatch.DrawString(menufont, "5) Your score is 1000 times your time left out of 2 minutes", position, Color.Black);
            position.Y += 45;
            spriteBatch.DrawString(menufont, "Good Luck :)", position, Color.Black);//writes the good luck message
            position.Y += 40;
            spriteBatch.DrawString(menufont, "Press enter to start and escape to go back", position, Color.Black);//writes the start and escape message
        }
        public bool ValidPos(Vector2 nextpos)//checks if the next position is valid
        {
            int posX = (int)nextpos.X / 25;
            int posY = (int)nextpos.Y / 25;

            return ValidTiles[posX, posY];
        }
        public Vector2 CheckOrigin()//checks if the origin of the sprite is valid
        {
            int choice = Random.Next(0, ValidSpawns.Count - 1);
            Vector2 Origin = ValidSpawns[choice];
            return Origin;
        }
    }
}
