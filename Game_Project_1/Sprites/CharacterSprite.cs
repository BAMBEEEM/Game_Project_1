using Game_Project_1.Collisions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Project_1.Sprites
{

    /// <summary>
    /// A class representing a slime ghost
    /// </summary>
    public class CharacterSprite
    {
        private GamePadState gamePadState;

        private KeyboardState keyboardState;

        private Texture2D texture;

        public Vector2 CurrentPosition = new Vector2(600, 200);

        private Vector2 lastPosition;


        private bool flipped;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(600 - 54, 200 - 60), 54, 60);

        private int _animationFrame;

        private double _flippingTimer;

        private double _animationTimer;

        private float _flippingSpeed = 0.35f;

        public bool Stopped = false;

        private float _animationSpeed = 0.1f;

        private bool _standing = true;

        public bool Poisoned = false;


        public float CharBonusSpeed = 1f;

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// The color to blend with the ghost
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("ninja");
        }

        /// <summary>
        /// Updates the sprite's position based on user input
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();


            #region GamePad Input

            if (!Stopped)
            {
                if (gamePadState.ThumbSticks.Left.Y > 0) //up
                {
                    CurrentPosition += gamePadState.ThumbSticks.Left * new Vector2(2, -2) * CharBonusSpeed;
                    _flippingTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_flippingTimer > _flippingSpeed)
                    {
                        if (flipped)
                            flipped = false;
                        else
                            flipped = true;

                        _flippingTimer -= _flippingSpeed;
                    }
                }

                if (gamePadState.ThumbSticks.Left.Y < 0) //down
                {
                    CurrentPosition += gamePadState.ThumbSticks.Left * new Vector2(2, -2) * CharBonusSpeed;
                    _flippingTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_flippingTimer > _flippingSpeed)
                    {
                        if (flipped)
                            flipped = false;
                        else
                            flipped = true;

                        _flippingTimer -= _flippingSpeed;
                    }
                }


                if (gamePadState.ThumbSticks.Left.X < 0) //left
                {
                    CurrentPosition += gamePadState.ThumbSticks.Left * new Vector2(2, -2) * CharBonusSpeed;
                    if (gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.ThumbSticks.Left.Y < 0)
                    {
                        // 0.75 bevause it's impossible to joystick vector to be (1,1), max it could be is (0.75,0.75)
                        CurrentPosition -= gamePadState.ThumbSticks.Left * 0.75f * new Vector2(2, -2) * CharBonusSpeed;

                        if (_flippingTimer > _flippingSpeed)
                        {
                            if (flipped)
                                flipped = false;
                            else
                                flipped = true;

                            _flippingTimer -= _flippingSpeed;
                        }
                    }
                    else
                        flipped = true;
                }

                if (gamePadState.ThumbSticks.Left.X > 0) //right
                {
                    CurrentPosition += gamePadState.ThumbSticks.Left * new Vector2(2, -2) * CharBonusSpeed;
                    if (gamePadState.ThumbSticks.Left.Y > 0 || gamePadState.ThumbSticks.Left.Y < 0)
                    {
                        // 0.75 bevause it's impossible to joystick vector to be (1,1), max it could be is (0.75,0.75)
                        CurrentPosition -= gamePadState.ThumbSticks.Left * 0.75f * new Vector2(2, -2) * CharBonusSpeed;

                        if (_flippingTimer > _flippingSpeed)
                        {
                            if (flipped)
                                flipped = false;
                            else
                                flipped = true;

                            _flippingTimer -= _flippingSpeed;
                        }
                    }
                    else
                        flipped = false;
                }
            }
            #endregion

            #region Keyboard Input

            if (!Stopped)
            {
                // Apply keyboard movement
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                {
                    //if (position.Y > 170)
                    CurrentPosition += new Vector2(0, -2) * CharBonusSpeed;

                    _flippingTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_flippingTimer > _flippingSpeed)
                    {
                        if (flipped)
                            flipped = false;
                        else
                            flipped = true;

                        _flippingTimer -= _flippingSpeed;
                    }
                }
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                {
                    //if (position.Y < 453)
                    CurrentPosition += new Vector2(0, 2) * CharBonusSpeed;

                    _flippingTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (_flippingTimer > _flippingSpeed)
                    {
                        if (flipped)
                            flipped = false;
                        else
                            flipped = true;

                        _flippingTimer -= _flippingSpeed;
                    }

                }
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                {
                    //if (position.X > 20)

                    CurrentPosition += new Vector2(-2, 0) * CharBonusSpeed;
                    if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)
                        || keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                    {

                        if (_flippingTimer > _flippingSpeed)
                        {
                            if (flipped)
                                flipped = false;
                            else
                                flipped = true;

                            _flippingTimer -= _flippingSpeed;
                        }
                    }
                    else
                        flipped = true;
                }
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    //if (position.X < 780)
                    CurrentPosition += new Vector2(2, 0) * CharBonusSpeed;
                    if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)
                        || keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                    {

                        if (_flippingTimer > _flippingSpeed)
                        {
                            if (flipped)
                                flipped = false;
                            else
                                flipped = true;

                            _flippingTimer -= _flippingSpeed;
                        }
                    }
                    else
                        flipped = false;
                }


                if (!(keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)
                        || keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)
                        || keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)
                        || keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                        && gamePadState.ThumbSticks.Left == new Vector2(0, 0))
                {
                    _standing = true;
                }
                else _standing = false;
            }
            #endregion

            //to limit the sprite from getting out of map
            #region Position Offset


            if (CurrentPosition.X < 20)
                CurrentPosition.X += 20 - CurrentPosition.X;

            if (CurrentPosition.X > 780)
                CurrentPosition.X -= CurrentPosition.X - 780;

            if (CurrentPosition.Y < 170)
                CurrentPosition.Y += 170 - CurrentPosition.Y;

            if (CurrentPosition.Y > 454)
                CurrentPosition.Y -= CurrentPosition.Y - 454;

            #endregion

            //Update the bounds
            bounds.X = CurrentPosition.X - 27f;
            bounds.Y = CurrentPosition.Y - 30f;


        }



        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Poisoned) Color = Color.Green;

            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (!Stopped)
            {
                if (_animationTimer > _animationSpeed)
                {
                    _animationFrame++;
                    if (_standing == true)
                        _animationFrame = 6;

                    else if (_animationFrame > 5)
                    {
                        _animationFrame = 0;
                    }



                    _animationTimer -= _animationSpeed;
                }
            }
            else // stop at animation frame #3 when dead (looks the best) or stay standing if wasn't moving
            {
                if (_standing == true)
                    _animationFrame = 6;
                else
                _animationFrame = 3;
            } 


                var source = new Rectangle(_animationFrame * 36, 0, 36, 40);

            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, CurrentPosition, source, Color, 0, new Vector2(18, 20), 1.5f, spriteEffects, 0);
        }
    }
}
