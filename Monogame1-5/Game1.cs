using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Threading;

namespace Monogame1_5
{
    public class Game1 : Game
    {
        bool charTurn, squirtTurn, isScratching;
        float squirtAlpha, fadeTimer, fadeSpeed, charAlpha;
        Random stats = new Random();
        Random critChance = new Random();
        Random squirtMove = new Random();
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        KeyboardState currentState, oldState;
        Rectangle window, battleSize, movesetSize, moveInfoSize, selectLocation, healthLocation1, healthLocation2, healthTileLocation1, healthTileLocation2, charLocation, squirtLocation;
        Vector2 emberLocation, scratchLocation, growlLocation, ppLocation, ppAmountLocation, typeLocation, ppTotalLocation, typeMoveLocation, totalHealthLocation, healthAmountLocation;
        Texture2D starterBattle, starterMoveset, moveInfo, select, healthBar, healthTile, squirtle, charmander;
        Screen screen;
        SpriteFont pokeFont, healthFont;
        int emberPPAmount, scratchPPAmount, growlPPAmount, healthAmount, charHealth, charAttack, charSAttack, charDefense, charSDefense, charSpeed, squirtAttack, squirtSAttack, squirtSpeed, squirtSDefense, squirtDefense, squirtHealth, crit, squirtChoice, growlCount, tailWhipCount, currentFrame;
        double emberDamage, scratchDamage, waterGunDamage, tackleDamage, growlEffect, tailWhipEffect, squirtHealthBar, charHealthBar, frameTime;
        enum Screen
        {
            Intro, Battlefield
        }
        List<Texture2D> scratch = new List<Texture2D>();
        List<Texture2D> ember = new List<Texture2D>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screen = Screen.Battlefield;
            window = new Rectangle(0, 0, 800, 600);
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.ApplyChanges();

            squirtHealth = stats.Next(20, 26);
            squirtAttack = stats.Next(10, 16);
            squirtSAttack = stats.Next(11, 17);
            squirtDefense = stats.Next(13, 18);
            squirtSDefense = stats.Next(12, 18);
            squirtSpeed = stats.Next(9, 15);

            charHealth = stats.Next(18, 21);
            charAttack = stats.Next(10, 12);
            charSAttack = stats.Next(10, 12);
            charDefense = stats.Next(9, 11);
            charSDefense = stats.Next(10, 13);
            charSpeed = stats.Next(11, 14);
            healthAmount = charHealth;
            squirtHealthBar = 205 / squirtHealth;
            charHealthBar = 205 / charHealth;

            growlEffect = squirtAttack * 0.80;
            tailWhipEffect = charDefense * 0.80;
            growlCount = 0;
            tailWhipCount = 0;
            isScratching = false;

            moveInfoSize = new Rectangle(550, 450, 250, 150);
            battleSize = new Rectangle(0, 0, 800, 450);
            movesetSize = new Rectangle(0, 450, 550, 150);
            selectLocation = new Rectangle(25, 470, 40, 60);
            emberLocation = new Vector2(70, 475);
            scratchLocation = new Vector2(70, 530);
            growlLocation = new Vector2(300, 475);
            typeLocation = new Vector2(570, 520);
            typeMoveLocation = new Vector2(670, 520);
            ppLocation = new Vector2(570, 470);
            ppTotalLocation = new Vector2(670, 470);
            ppAmountLocation = new Vector2(630, 470);
            healthLocation1 = new Rectangle(80, 100, 280, 70);
            healthLocation2 = new Rectangle(495, 340, 280, 70);
            totalHealthLocation = new Vector2(710, 395);
            healthAmountLocation = new Vector2(670, 395);
            healthTileLocation1 = new Rectangle(130, 125, 205, 22);
            healthTileLocation2 = new Rectangle(545, 365, 205, 22);
            charLocation = new Rectangle(80, 251, 250, 200);
            squirtLocation = new Rectangle(500, 100, 200, 200);

            growlPPAmount = 40;
            emberPPAmount = 25;
            scratchPPAmount = 35;
            squirtAlpha = 1f;
            charAlpha = 1f;
            fadeTimer = 0f;
            fadeSpeed = 10f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //  redIntro = Content.Load<Video>("pokeRedIntro");
            //introPlayer = new VideoPlayer();
            starterBattle = Content.Load<Texture2D>("starterBattle");
            starterMoveset = Content.Load<Texture2D>("starterMoveset");
            moveInfo = Content.Load<Texture2D>("starterMoveset");
            pokeFont = Content.Load<SpriteFont>("font");
            select = Content.Load<Texture2D>("select");
            healthBar = Content.Load<Texture2D>("healthBar");
            healthFont = Content.Load<SpriteFont>("healthFont");
            healthTile = Content.Load<Texture2D>("tile");
            scratch.Add(Content.Load<Texture2D>("scratch1"));
            scratch.Add(Content.Load<Texture2D>("scratch2"));
            scratch.Add(Content.Load<Texture2D>("scratch3"));
            scratch.Add(Content.Load<Texture2D>("scratch4"));
            ember.Add(Content.Load<Texture2D>("Ember"));
            ember.Add(Content.Load<Texture2D>("onFire"));
            charmander = Content.Load<Texture2D>("charmander");
            squirtle = Content.Load<Texture2D>("squirtle");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // TODO: Add your update logic here
            if (charSpeed >= squirtSpeed)
            {
                charTurn = true;
                squirtTurn = false;
            }
            else
            {
                charTurn = false;
                squirtTurn = true;
            }
            if (charTurn == true && squirtTurn == false)
            {
                crit = critChance.Next(1, 25);
                currentState = Keyboard.GetState();
                if (selectLocation.X == 25 && selectLocation.Y == 470 && currentState.IsKeyDown(Keys.Down))
                {
                    selectLocation.Y = 525;
                }
                else if (selectLocation.X == 25 && selectLocation.Y == 525 && currentState.IsKeyDown(Keys.Up))
                {
                    selectLocation.Y = 470;
                }
                else if (selectLocation.X == 25 && selectLocation.Y == 470 && currentState.IsKeyDown(Keys.Right))
                {
                    selectLocation.X = 255;
                }
                else if (selectLocation.X == 255 && selectLocation.Y == 470 && currentState.IsKeyDown(Keys.Left))
                {
                    selectLocation.X = 25;
                }

                if (selectLocation.X == 25 && selectLocation.Y == 525)
                {
                    currentState = Keyboard.GetState();
                    if (currentState.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A))
                    {
                        isScratching = true;
                        currentFrame = 0;
                        frameTime = 0;
                        scratchPPAmount -= 1;
                        if (crit == 1)
                        {
                            scratchDamage = ((((2 * 5 * 2 / 5 + 2) * charAttack * 40 / squirtDefense) / 50) + 2);
                            healthTileLocation1.Width -= (int)(squirtHealthBar * scratchDamage);
                            charTurn = false;
                            squirtTurn = true;
                        }
                        else
                        {
                            scratchDamage = ((((2 * 5 / 5 + 2) * charAttack * 40 / squirtDefense) / 50) + 2);
                            healthTileLocation1.Width -= (int)(squirtHealthBar * scratchDamage);
                            charTurn = false;
                            squirtTurn = true;
                        }
                    }
                    oldState = currentState;
                }
                else if (selectLocation.X == 255 && selectLocation.Y == 470)
                {
                    currentState = Keyboard.GetState();
                    if (currentState.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A))
                    {
                        growlPPAmount -= 1;
                        squirtAttack -= (int)growlEffect;
                        growlCount += 1;
                        if (growlCount == 3)
                        {
                            growlEffect = 0;
                        }
                        charTurn = false;
                        squirtTurn = true;
                    }
                    oldState = currentState;
                }
                else if (selectLocation.X == 25 && selectLocation.Y == 470)
                {
                    currentState = Keyboard.GetState();
                    if (currentState.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A))
                    {
                        emberPPAmount -= 1;
                        if (crit == 1)
                        {
                            emberDamage = ((((2 * 5 * 2 / 5 + 2) * charSAttack * 40 / squirtSDefense) / 50) + 2) * 0.5;
                            healthTileLocation1.Width -= (int)(squirtHealthBar * emberDamage);
                            charTurn = false;
                            squirtTurn = true;
                        }
                        else
                        {
                            emberDamage = ((((2 * 5 / 5 + 2) * charSAttack * 40 / squirtSDefense) / 50) + 2) * 0.5;
                            healthTileLocation1.Width -= (int)(squirtHealthBar * emberDamage);
                            charTurn = false;
                            squirtTurn = true;
                        }
                    }
                    oldState = currentState;
                }
            }
            if (squirtTurn == true && charTurn == false)
            {
                squirtChoice = squirtMove.Next(1, 4);
                crit = critChance.Next(1, 25);
                if (squirtChoice == 1)
                {
                    if (crit == 1)
                    {
                        waterGunDamage = (((((2 * 5 * 2 / 5 + 2) * squirtSAttack * 40 / charSDefense) / 100) + 2) * 1.5);
                        healthTileLocation2.Width -= (int)(charHealthBar * waterGunDamage);
                        healthAmount -= (int)waterGunDamage;
                        charTurn = true;
                        squirtTurn = false;
                    }
                    else
                    {
                        waterGunDamage = (((((2 * 5 / 5 + 2) * squirtSAttack * 40 / charSDefense) / 100) + 2) * 1.5);
                        healthTileLocation2.Width -= (int)(charHealthBar * waterGunDamage);
                        healthAmount -= (int)waterGunDamage;
                        charTurn = true;
                        squirtTurn = false;
                    }
                }
                if (squirtChoice == 2)
                {
                    if (crit == 1)
                    {
                        tackleDamage = ((((2 * 5 * 2 / 5 + 2) * squirtAttack * 40 / charDefense) / 100) + 2);
                        healthTileLocation2.Width -= (int)(charHealthBar * tackleDamage);
                        healthAmount -= (int)tackleDamage;
                        charTurn = true;
                        squirtTurn = false;
                    }
                    else
                    {
                        tackleDamage = ((((2 * 5 / 5 + 2) * squirtAttack * 40 / charDefense) / 100) + 2);
                        healthTileLocation2.Width -= (int)(charHealthBar * tackleDamage);
                        healthAmount -= (int)tackleDamage;
                        charTurn = true;
                        squirtTurn = false;
                    }
                }
                if (squirtChoice == 3)
                {
                    charDefense -= (int)tailWhipEffect;
                    tailWhipCount += 1;
                    if (tailWhipCount == 3)
                    {
                        tailWhipEffect = 0;
                    }
                    charTurn = true;
                    squirtTurn = false;
                }
            }
            if (isScratching)
            {
                frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (frameTime >= 0.1)
                {
                    currentFrame++;
                    frameTime = 0;

                    if (currentFrame >= scratch.Count)
                    {
                        isScratching = false;
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            float maxTimer = 3f * MathF.PI / fadeSpeed;
          // Texture2D introRed = introPlayer.GetTexture();
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            if (screen == Screen.Battlefield)
            {
                _spriteBatch.Draw(starterBattle, battleSize, Color.White);
                _spriteBatch.Draw(starterMoveset, movesetSize, Color.White);
                _spriteBatch.Draw(moveInfo, moveInfoSize, Color.White);
                _spriteBatch.Draw(squirtle, squirtLocation, Color.White * squirtAlpha);
                _spriteBatch.Draw(charmander, charLocation, Color.White * charAlpha);
                if (isScratching && currentFrame < scratch.Count)
                {
                    fadeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    squirtAlpha = (float)(0.5f + 0.5f * Math.Sin(fadeTimer * fadeSpeed));
                    _spriteBatch.Draw(scratch[currentFrame], new Vector2(530, 150), Color.White);
                }
                else
                {
                    squirtAlpha = 1f;
                    fadeTimer = 0f;
                }
                if (healthTileLocation2.Width >= 152)
                {
                    _spriteBatch.Draw(healthTile, healthTileLocation2, Color.LimeGreen);
                }
                else if (healthTileLocation2.Width <= 152 && healthTileLocation2.Width > 40)
                {
                    _spriteBatch.Draw(healthTile, healthTileLocation2, Color.GreenYellow);
                }
                else if (healthTileLocation2.Width <= 40)
                {
                    _spriteBatch.Draw(healthTile, healthTileLocation2, Color.OrangeRed);
                }
                if (healthTileLocation1.Width >= 152)
                {
                    _spriteBatch.Draw(healthTile, healthTileLocation1, Color.LimeGreen);
                }
                else if (healthTileLocation1.Width <= 152 && healthTileLocation1.Width > 40)
                {
                    _spriteBatch.Draw(healthTile, healthTileLocation1, Color.GreenYellow);
                }
                else if (healthTileLocation1.Width <= 40)
                {
                    _spriteBatch.Draw(healthTile, healthTileLocation1, Color.OrangeRed);
                }
                _spriteBatch.Draw(healthBar, healthLocation1, Color.White);
                _spriteBatch.Draw(healthBar, healthLocation2, Color.White);
                _spriteBatch.DrawString(healthFont, healthAmount.ToString() + "/", healthAmountLocation, Color.Black);
                _spriteBatch.DrawString(healthFont, Convert.ToString(charHealth), totalHealthLocation, Color.Black);
                _spriteBatch.DrawString(pokeFont, "Ember", emberLocation, Color.Black);
                _spriteBatch.DrawString(pokeFont, "Scratch", scratchLocation, Color.Black);
                _spriteBatch.DrawString(pokeFont, "Growl", growlLocation, Color.Black);
                _spriteBatch.DrawString(pokeFont, "PP", ppLocation, Color.Black);
                _spriteBatch.DrawString(pokeFont, "Type/", typeLocation, Color.Black);
                if (selectLocation.X == 25 && selectLocation.Y == 470)
                {
                    _spriteBatch.DrawString(pokeFont, emberPPAmount.ToString(), ppAmountLocation, Color.Black);
                    _spriteBatch.DrawString(pokeFont, "/25", ppTotalLocation, Color.Black);
                    _spriteBatch.DrawString(pokeFont, "Fire", typeMoveLocation, Color.Black);
                }
                if (selectLocation.X == 255 && selectLocation.Y == 470)
                {
                    _spriteBatch.DrawString(pokeFont, growlPPAmount.ToString(), ppAmountLocation, Color.Black);
                    _spriteBatch.DrawString(pokeFont, "/40", ppTotalLocation, Color.Black);
                    _spriteBatch.DrawString(pokeFont, "Normal", typeMoveLocation, Color.Black);
                }
                if (selectLocation.X == 25 && selectLocation.Y == 525)
                {
                    _spriteBatch.DrawString(pokeFont, scratchPPAmount.ToString(), ppAmountLocation, Color.Black);
                    _spriteBatch.DrawString(pokeFont, "/35", ppTotalLocation, Color.Black);
                    _spriteBatch.DrawString(pokeFont, "Normal", typeMoveLocation, Color.Black);
                }
                _spriteBatch.Draw(select, selectLocation, Color.Black);
            }
            /*if (screen == Screen.Intro)
            {
                _spriteBatch.Draw(introRed, introSize, Color.White);
            }*/
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
