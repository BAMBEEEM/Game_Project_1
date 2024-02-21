using Game_Project_1.Collisions;
using Game_Project_1.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.WIC;
using System.Collections.Generic;

namespace Game_Project_1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private bool _lost = false;

        private CharacterSprite _mainCharacter;
        private SpriteFont spriteFont;
        private ForestSprite _forest;

        private int[] _mushroomsLeft = new int[_maxLevel];
        private int _finalScore;
        private List<MushroomSprite>[] _mushrooms = new List<MushroomSprite>[_maxLevel];

        private bool _calculated = false;

        private int level = 1;

        private bool _won = false;

        private System.TimeSpan time;

        private System.TimeSpan _extraTime;

        private Color _timeColor;

        private const int _maxLevel = 7;



        //for testing purposes

        /*        BoundingRectangle cursor;

                public MouseState CurrentMouseState => currentMouseState;
                public BoundingRectangle Cursor => cursor;
                MouseState currentMouseState;
                MouseState priorMouseState;*/


        /// <summary>
        /// A game demonstrating collision detection
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Initializes the game 
        /// </summary>
        protected override void Initialize()
        {


            _forest = new ForestSprite();

            // TODO: Add your initialization logic here
            for (int i = 0; i < _maxLevel; i++)
            {
                _mushrooms[i] = new List<MushroomSprite>();
                _mushrooms[i].Capacity = 5 + i;

                for (int j = 0; j < _mushrooms[i].Capacity; j++)
                {
                    _mushrooms[i].Add(new MushroomSprite());
                }
            }

            _finalScore = 0;

            _mainCharacter = new CharacterSprite();

            base.Initialize();
        }

        /// <summary>
        /// Loads content for the game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _mainCharacter.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("retro");
            _forest.LoadContent(Content);
            foreach (List<MushroomSprite> LMushroom in _mushrooms)
                foreach (MushroomSprite mushroom in LMushroom) mushroom.LoadContent(Content);
        }

        /// <summary>
        /// Updates the game world
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Update(GameTime gameTime)
        {



            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _mainCharacter.Color = Color.White; // default color
            _mainCharacter.Update(gameTime);

            // responsible for time limit
            double extratime = (level * 8) + level;
            if (!_won && !_lost)
            {
                time = (System.TimeSpan.FromSeconds(5) + System.TimeSpan.FromSeconds(extratime)) - gameTime.TotalGameTime;
            }
            else
            {
                _mainCharacter.Stopped = true; // charecter stops when game ends.
            }

            if (time < System.TimeSpan.FromSeconds(0)) // game is lost if time ends.
                _lost = true;

            // Calculates number of mushrooms left that are not toxic
            // Also resets and respawn the mushroom if it is poisonous and too close another mushroom
            if (!_calculated)
            {
                for (int i = 0; i < _maxLevel; i++)
                {
                    _mushroomsLeft[i] = 5 + i;

                    for (int j = 0; j < _mushrooms[i].Capacity; j++)
                    {

                        if (_mushrooms[i][j].Poisonous)
                        {
                            _mushroomsLeft[i]--;
                            int k = j - 1;

                            while (k > 0)
                            {
                                k--;
                                if (CollisionHelper.Closeness(_mushrooms[i][j].Position, _mushrooms[i][k].Position) < 50)
                                {
                                    _mushrooms[i][j].Respawn();
                                    k = j - 1;
                                }
                            }
                        }
                    }
                }
                _calculated = true;
            }



            //for testing purposes
            /*
                        priorMouseState = currentMouseState;
                        currentMouseState = Mouse.GetState();
                        cursor = new(currentMouseState.Position.X, currentMouseState.Position.Y, 1, 1);
            */
            /*
                        if (_forest.Bounds.CollidesWith(Cursor))
                        {

                            _forest.color = Color.White;

                        }
                        else _forest.color = Color.Black;*/

            // TODO: Add your update logic here



            //sets current level with its assigned mushroom list
            List<MushroomSprite> currentLMushroom = _mushrooms[level - 1];

            foreach (MushroomSprite mushroom in currentLMushroom)
            {
                mushroom.Closeness = CollisionHelper.Closeness(mushroom.Position - new Vector2(-11.1375f, -11.1375f),
                    _mainCharacter.CurrentPosition);
            }

            //Detect and process collisions
            foreach (MushroomSprite mushroom in currentLMushroom)
            {
                if (!mushroom.Collected && mushroom.Bounds.CollidesWith(_mainCharacter.Bounds) && !_lost)
                {
                    if (!mushroom.Poisonous)
                    {
                        _mainCharacter.Color = Color.Gold;
                        mushroom.Collected = true;
                        _mushroomsLeft[level - 1]--;
                        _finalScore++;
                        _mainCharacter.CharBonusSpeed += 0.016f; //become faster with every mushroom
                    }
                    else
                    {
                        _mainCharacter.Poisoned = true;
                        _lost = true;
                    }
                }


            }

            //advances to next level
            if (_mushroomsLeft[level - 1] == 0 && level < _maxLevel) 
            {
                level++;
                _extraTime = gameTime.TotalGameTime + System.TimeSpan.FromSeconds(1);
                foreach (MushroomSprite mushroom in _mushrooms[level - 1])
                {
                    while (CollisionHelper.Closeness(mushroom.Position, _mainCharacter.CurrentPosition) < 85)
                    {
                        mushroom.Respawn();
                    }
                }
            }
            else if (level == _maxLevel && _mushroomsLeft[level - 1] == 0)
            {
                _won = true;
            }

            // makes time green to allude time increase
            if (_extraTime > gameTime.TotalGameTime)

                _timeColor = new Color(75, 200, 75);
            else
                _timeColor = Color.White;



            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game world
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            List<MushroomSprite> currentLMushroom = _mushrooms[level - 1];

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            _forest.Draw(spriteBatch);
            _mainCharacter.Draw(gameTime, spriteBatch);
            foreach (MushroomSprite mushroom in currentLMushroom) mushroom.Draw(gameTime, spriteBatch);
            if (_won)
            {
                spriteBatch.DrawString(spriteFont, "You win!", new Vector2(335, 195), Color.White);
                spriteBatch.DrawString(spriteFont, $"Final Score: {_finalScore}", new Vector2(275, 220), Color.White);
            }
            else if (!_lost)
            {
                //outlining text
                spriteBatch.DrawString(spriteFont, $"Mushrooms left: {_mushroomsLeft[level - 1]}", new Vector2(239, 60), Color.Black);
                spriteBatch.DrawString(spriteFont, $"Mushrooms left: {_mushroomsLeft[level - 1]}", new Vector2(241, 60), Color.Black);
                spriteBatch.DrawString(spriteFont, $"Mushrooms left: {_mushroomsLeft[level - 1]}", new Vector2(240, 59), Color.Black);
                spriteBatch.DrawString(spriteFont, $"Mushrooms left: {_mushroomsLeft[level - 1]}", new Vector2(240, 61), Color.Black);
                spriteBatch.DrawString(spriteFont, $"Mushrooms left: {_mushroomsLeft[level - 1]}", new Vector2(240, 60), Color.White);
                spriteBatch.DrawString(spriteFont, $"Time Left: {time:ss'.'ff}", new Vector2(264, 20), Color.Black);
                spriteBatch.DrawString(spriteFont, $"Time Left: {time:ss'.'ff}", new Vector2(266, 20), Color.Black);
                spriteBatch.DrawString(spriteFont, $"Time Left: {time:ss'.'ff}", new Vector2(265, 19), Color.Black);
                spriteBatch.DrawString(spriteFont, $"Time Left: {time:ss'.'ff}", new Vector2(265, 21), Color.Black);
                spriteBatch.DrawString(spriteFont, $"Time Left: {time:ss'.'ff}", new Vector2(265, 20), _timeColor);
            }
            else if (_lost)
            {
                Vector2 drawPosition; // to adjust text placement and center it
                if (_finalScore < 10) drawPosition = new(310, 195);
                else drawPosition = new(320, 195);


                spriteBatch.DrawString(spriteFont, "You lost!", drawPosition, Color.White);
                spriteBatch.DrawString(spriteFont, $"Final Score: {_finalScore}", new Vector2(275, 220), Color.White);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}