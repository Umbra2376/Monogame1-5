using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        bool squirtleAction, canAct;
        float squirtAlpha, fadeTimer, fadeSpeed, charAlpha;
        Random stats = new Random();
        Random critChance = new Random();
        Random squirtMove = new Random();
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        KeyboardState currentState, oldState;
        Rectangle window, battleSize, movesetSize, moveInfoSize, selectLocation, healthLocation1, healthLocation2, healthTileLocation1, healthTileLocation2, charLocation, squirtLocation, emberMove, onFireLocation, waterMove, wetLocation, battleInfo, introSize;
        Vector2 emberLocation, scratchLocation, growlLocation, ppLocation, ppAmountLocation, typeLocation, ppTotalLocation, typeMoveLocation, totalHealthLocation, healthAmountLocation, battleText, endTextLocation;
        Texture2D starterBattle, starterMoveset, moveInfo, select, healthBar, healthTile, squirtle, charmander, ember, onFire, waterGun, wet, growl, tailWhip, pokeEnd, pokeLose;
        Screen screen;
        SpriteFont pokeFont, healthFont, endFont;
        SoundEffect battleMusic, introMusic, scratchSound, emberSound, tackleSound, waterSound, growlSound, whipSound, endMusic;
        SoundEffectInstance battleInstance, introInstance, scratchInstance, emberInstance, tackleInstance, waterInstance, growlInstance, whipInstance, endInstance;
        int emberPPAmount, scratchPPAmount, growlPPAmount, healthAmount, charHealth, charAttack, charSAttack, charDefense, charSDefense, charSpeed, squirtAttack, squirtSAttack, squirtSpeed, squirtSDefense, squirtDefense, squirtHealth, crit, squirtChoice, growlCount, tailWhipCount, currentFrame, introFrame;
        double emberDamage, scratchDamage, waterGunDamage, tackleDamage, growlEffect, tailWhipEffect, squirtHealthBar, charHealthBar, frameTime, emberInterval, onFireTime, waterInterval, wetTime, effectTime, battleTime, textTime;
        enum Screen
        {
            Intro, Battlefield, End
        }
        enum Action
        {
            None, Scratch, Ember, Growl, Tackle, WaterGun, Whip
        }
        enum TextShown
        {
            None, Scratch, Ember, Growl, Tackle, WaterGun, Whip
        }
        enum Turn
        {
            charTurn, squirtTurn
        }
        List<Texture2D> scratch = new List<Texture2D>();
        List<Texture2D> intro = new List<Texture2D>();
        private Action currentAction;
        private TextShown currentText;
        private Turn currentTurn;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screen = Screen.Intro;
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
            healthAmount = Math.Max(0, healthAmount);
            squirtHealthBar = 205f / squirtHealth;
            charHealthBar = 205f / charHealth;
            if (charSpeed >= squirtSpeed)
            {
                currentTurn = Turn.charTurn;
            }
            else
            {
                currentTurn = Turn.squirtTurn;
            }

            growlEffect = squirtAttack * 0.80;
            tailWhipEffect = charDefense * 0.80;
            growlCount = 0;
            tailWhipCount = 0;
            currentAction = Action.None;
            currentText = TextShown.None;
            squirtleAction = false;
            canAct = true;

            introSize = new Rectangle(0, 0, 800, 600);
            moveInfoSize = new Rectangle(550, 450, 250, 150);
            battleSize = new Rectangle(0, 0, 800, 450);
            movesetSize = new Rectangle(0, 450, 550, 150);
            battleInfo = new Rectangle(0, 450, 800, 150);
            selectLocation = new Rectangle(25, 470, 40, 60);
            battleText = new Vector2 (50, 480);
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
            charLocation = new Rectangle(80, 250, 250, 200);
            squirtLocation = new Rectangle(500, 100, 200, 200);
            emberMove = new Rectangle(250, 350, 50, 50);
            onFireLocation = new Rectangle(500, 100, 200, 200);
            waterMove = new Rectangle(380, 200, 50, 50);
            wetLocation = new Rectangle(80, 250, 200, 200);
            endTextLocation = new Vector2(10, 400);

            growlPPAmount = 40;
            growlPPAmount = Math.Max(0, growlPPAmount);
            emberPPAmount = 25;
            emberPPAmount = Math.Max(0, emberPPAmount);
            scratchPPAmount = 35;
            scratchPPAmount = Math.Max(0, scratchPPAmount);
            squirtAlpha = 1f;
            charAlpha = 1f;
            fadeTimer = 0f;
            fadeSpeed = 10f;
            emberInterval = 0.03;
            waterInterval = 0.03;
            healthLocation1.Width = Math.Max(0, healthLocation1.Width);
            healthLocation2.Width = Math.Max(0, healthLocation2.Width);

            frameTime = 0;
            onFireTime = 0;
            battleTime = 0;
            wetTime = 0;
            textTime = 0;
            effectTime = 0;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            intro.Add(Content.Load<Texture2D>("pokeRedIntro(1)"));
            intro.Add(Content.Load<Texture2D>("pokeRedIntro(2)"));
            starterBattle = Content.Load<Texture2D>("starterBattle");
            starterMoveset = Content.Load<Texture2D>("starterMoveset");
            moveInfo = Content.Load<Texture2D>("starterMoveset");
            pokeFont = Content.Load<SpriteFont>("font");
            select = Content.Load<Texture2D>("select");
            healthBar = Content.Load<Texture2D>("healthBar");
            healthFont = Content.Load<SpriteFont>("healthFont");
            endFont = Content.Load<SpriteFont>("endFont");
            healthTile = Content.Load<Texture2D>("tile");
            scratch.Add(Content.Load<Texture2D>("scratch1"));
            scratch.Add(Content.Load<Texture2D>("scratch2"));
            scratch.Add(Content.Load<Texture2D>("scratch3"));
            scratch.Add(Content.Load<Texture2D>("scratch4"));
            ember = Content.Load<Texture2D>("Ember");
            onFire = Content.Load<Texture2D>("onFire");
            waterGun = Content.Load<Texture2D>("waterGun");
            wet = Content.Load<Texture2D>("wet");
            growl = Content.Load<Texture2D>("growled");
            tailWhip = Content.Load<Texture2D>("tailWhip");
            charmander = Content.Load<Texture2D>("charmander");
            squirtle = Content.Load<Texture2D>("squirtle");
            pokeEnd = Content.Load<Texture2D>("pokeEnd");
            pokeLose = Content.Load<Texture2D>("pokeLose");
            battleMusic = Content.Load<SoundEffect>("battleMusic");
            battleInstance = battleMusic.CreateInstance();
            introMusic = Content.Load<SoundEffect>("pokeIntroMusic");
            introInstance = introMusic.CreateInstance();
            scratchSound = Content.Load<SoundEffect>("scratch");
            scratchInstance = scratchSound.CreateInstance();
            scratchInstance.IsLooped = false;
            emberSound = Content.Load<SoundEffect>("emberSound");
            emberInstance = emberSound.CreateInstance();
            emberInstance.IsLooped = false;
            tackleSound = Content.Load<SoundEffect>("tackle");
            tackleInstance = tackleSound.CreateInstance();
            tackleInstance.IsLooped = false;
            waterSound = Content.Load<SoundEffect>("water-gun");
            waterInstance = waterSound.CreateInstance();
            waterInstance.IsLooped = false;
            growlSound = Content.Load<SoundEffect>("growl");
            growlInstance = growlSound.CreateInstance();
            growlInstance.IsLooped = false;
            whipSound = Content.Load<SoundEffect>("whipSound");
            whipInstance = whipSound.CreateInstance();
            whipInstance.IsLooped = false;
            endMusic = Content.Load<SoundEffect>("pokeTheme");
            endInstance = endMusic.CreateInstance();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            currentState = Keyboard.GetState();
            // TODO: Add your update logic here
            if (screen == Screen.Intro)
            {
                introInstance.Play();
                frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (frameTime >= 0.5)
                {
                    introFrame = 1;
                }
                if (frameTime >= 1)
                {
                    introFrame = 0;
                    frameTime = 0;
                }
                if (currentState.IsKeyDown(Keys.Enter))
                {
                    introInstance.Stop();
                    battleInstance.IsLooped = true;
                    battleInstance.Play();
                    screen = Screen.Battlefield;
                }
            }
            else if (screen == Screen.Battlefield)
            {
                Battle1(gameTime);
            }   
                
            if (healthAmount <= 0 || healthTileLocation1.Width <= 0)
            {
                battleInstance.Stop();
                screen = Screen.End;
                endInstance.IsLooped = true;
                endInstance.Play();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            float maxTimer = 3f * MathF.PI / fadeSpeed;
            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            if (screen == Screen.Intro)
            {
                _spriteBatch.Draw(intro[introFrame], introSize, Color.White);
            }
            if (screen == Screen.Battlefield)
            {
                _spriteBatch.Draw(starterBattle, battleSize, Color.White);
                _spriteBatch.Draw(starterMoveset, movesetSize, Color.White);
                _spriteBatch.Draw(moveInfo, moveInfoSize, Color.White);
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
                _spriteBatch.Draw(squirtle, squirtLocation, Color.White * squirtAlpha);
                _spriteBatch.Draw(charmander, charLocation, Color.White * charAlpha);
                if (currentAction == Action.Scratch && currentFrame < scratch.Count)
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
                if (currentText == TextShown.Scratch && battleTime <= 3)
                {
                    _spriteBatch.Draw(starterMoveset, battleInfo, Color.White);
                    _spriteBatch.DrawString(pokeFont, "Charmander used Scratch!", battleText, Color.Black);
                    if (crit == 1)
                        _spriteBatch.DrawString(pokeFont, "It was a crit!", new Vector2(50, 510), Color.Black);
                }
                if (currentAction == Action.Ember && onFireTime <= 2)
                {
                    _spriteBatch.Draw(ember, emberMove, Color.White);
                    if (onFireTime >= 0.4)
                    {
                        _spriteBatch.Draw(onFire, onFireLocation, Color.White);
                    }
                }
                if (currentText == TextShown.Ember && textTime <= 3)
                {
                    _spriteBatch.Draw(starterMoveset, battleInfo, Color.White);
                    _spriteBatch.DrawString(pokeFont, "Charmander used Ember!", battleText, Color.Black);
                    if (crit == 1)
                        _spriteBatch.DrawString(pokeFont, "It was a crit!", new Vector2(50, 510), Color.Black);
                }
                if (currentAction == Action.WaterGun && wetTime <= 2)
                {
                    _spriteBatch.Draw(waterGun, waterMove, Color.White);
                    if (wetTime >= 0.4)
                    {
                        _spriteBatch.Draw(wet, wetLocation, Color.White);
                    }
                }
                if (currentText == TextShown.WaterGun && textTime <= 3)
                {
                    _spriteBatch.Draw(starterMoveset, battleInfo, Color.White);
                    _spriteBatch.DrawString(pokeFont, "Squirtle used Water Gun!", battleText, Color.Black);
                    if (crit == 1)
                        _spriteBatch.DrawString(pokeFont, "It was a crit!", new Vector2(50, 510), Color.Black);
                }
                if (currentAction == Action.Growl && effectTime <= 2)
                    _spriteBatch.Draw(growl, squirtLocation, Color.White);
                if (currentText == TextShown.Growl && textTime <= 3)
                {
                    _spriteBatch.Draw(starterMoveset, battleInfo, Color.White);
                    _spriteBatch.DrawString(pokeFont, "Charmander used Growl!", battleText, Color.Black);
                    _spriteBatch.DrawString(pokeFont, "Squirtle's attack fell.", new Vector2(50, 510), Color.Black);
                }
                if (currentAction == Action.Whip && effectTime <= 2)
                    _spriteBatch.Draw(tailWhip, charLocation, Color.White);
                if (currentText == TextShown.Whip && textTime <= 3)
                {
                    _spriteBatch.Draw(starterMoveset, battleInfo, Color.White);
                    _spriteBatch.DrawString(pokeFont, "Squirtle used Tail Whip!", battleText, Color.Black);
                    _spriteBatch.DrawString(pokeFont, " Charmander's defense fell.", new Vector2(50, 510), Color.Black);
                }
                if (currentText == TextShown.Tackle && textTime <= 3)
                {
                    _spriteBatch.Draw(starterMoveset, battleInfo, Color.White);
                    _spriteBatch.DrawString(pokeFont, "Squirtle used Tackle!", battleText, Color.Black);
                    if (crit == 1)
                        _spriteBatch.DrawString(pokeFont, "It was a crit!", new Vector2(50, 510), Color.Black);
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
            }
            if (screen == Screen.End)
            {
                if (healthAmount <= 0)
                {
                    _spriteBatch.Draw(pokeLose, window, Color.White);
                    _spriteBatch.DrawString(endFont, "You Lose:(", endTextLocation, Color.Red);
                    _spriteBatch.DrawString(endFont, "Better Luck Next Time", new Vector2(10, 300), Color.Red);
                }
                if (healthTileLocation1.Width <= 0)
                {
                    _spriteBatch.Draw(pokeEnd, window, Color.White);
                    _spriteBatch.DrawString(endFont, "You Win! Congratulations!!!", endTextLocation, Color.Green);
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }


        public void Battle1(GameTime gameTime)
        {
            if (currentTurn == Turn.charTurn && canAct == true)
            {
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
                    if (currentState.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A))
                    {
                        canAct = false;
                        scratchInstance.Play();
                        crit = critChance.Next(1, 25);
                        currentAction = Action.Scratch;
                        currentText = TextShown.Scratch;
                        battleTime = 0;
                        currentFrame = 0;
                        frameTime = 0;
                        scratchPPAmount -= 1;
                        if (crit == 1)
                        {
                            scratchDamage = ((((2 * 5 * 2 / 5 + 2) * charAttack * 40 / squirtDefense) / 50) + 2);
                            healthTileLocation1.Width -= (int)(squirtHealthBar * scratchDamage);
                        }
                        else
                        {
                            scratchDamage = ((((2 * 5 / 5 + 2) * charAttack * 40 / squirtDefense) / 50) + 2);
                            healthTileLocation1.Width -= (int)(squirtHealthBar * scratchDamage);
                        }
                    }
                }
                else if (selectLocation.X == 255 && selectLocation.Y == 470)
                {
                    if (currentState.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A))
                    {
                        canAct = false;
                        growlInstance.Play();
                        currentAction = Action.Growl;
                        currentText = TextShown.Growl;
                        growlPPAmount -= 1;
                        squirtAttack -= (int)growlEffect;
                        growlCount += 1;
                        if (growlCount == 6)
                        {
                            growlEffect = 0;
                        }
                    }
                }
                else if (selectLocation.X == 25 && selectLocation.Y == 470)
                {
                    if (currentState.IsKeyDown(Keys.A) && oldState.IsKeyUp(Keys.A))
                    {
                        canAct = false;
                        emberInstance.Play();
                        crit = critChance.Next(1, 25);
                        currentAction = Action.Ember;
                        currentText = TextShown.Ember;
                        emberPPAmount -= 1;
                        if (crit == 1)
                        {
                            emberDamage = ((((2 * 5 * 2 / 5 + 2) * charSAttack * 40 / squirtSDefense) / 50) + 2) * 0.5;
                            healthTileLocation1.Width -= (int)(squirtHealthBar * emberDamage);
                        }
                        else
                        {
                            emberDamage = ((((2 * 5 / 5 + 2) * charSAttack * 40 / squirtSDefense) / 50) + 2) * 0.5;
                            healthTileLocation1.Width -= (int)(squirtHealthBar * emberDamage);
                        }
                    }
                }
            }
            if (currentTurn == Turn.squirtTurn && !squirtleAction && canAct == true)
            {
                squirtleAction = true;
                squirtChoice = squirtMove.Next(1, 4);
                crit = critChance.Next(1, 25);
                if (squirtChoice == 1)
                {
                    canAct = false;
                    waterInstance.Play();
                    currentAction = Action.WaterGun;
                    currentText = TextShown.WaterGun;
                    if (crit == 1)
                    {
                        waterGunDamage = (((((2 * 5 * 2 / 5 + 2) * squirtSAttack * 40 / charSDefense) / 100) + 2) * 1.5);
                        healthTileLocation2.Width -= (int)(charHealthBar * waterGunDamage);
                        healthAmount -= (int)waterGunDamage;
                    }
                    else
                    {
                        waterGunDamage = (((((2 * 5 / 5 + 2) * squirtSAttack * 40 / charSDefense) / 100) + 2) * 1.5);
                        healthTileLocation2.Width -= (int)(charHealthBar * waterGunDamage);
                        healthAmount -= (int)waterGunDamage;
                    }
                }
                else if (squirtChoice == 2)
                {
                    canAct = false;
                    tackleInstance.Play();
                    currentAction = Action.Tackle;
                    currentText = TextShown.Tackle;
                    if (crit == 1)
                    {
                        tackleDamage = ((((2 * 5 * 2 / 5 + 2) * squirtAttack * 40 / charDefense) / 100) + 2);
                        healthTileLocation2.Width -= (int)(charHealthBar * tackleDamage);
                        healthAmount -= (int)tackleDamage;
                    }
                    else
                    {
                        tackleDamage = ((((2 * 5 / 5 + 2) * squirtAttack * 40 / charDefense) / 100) + 2);
                        healthTileLocation2.Width -= (int)(charHealthBar * tackleDamage);
                        healthAmount -= (int)tackleDamage;
                    }
                }
                else if (squirtChoice == 3)
                {
                    canAct = false;
                    waterInstance.Play();
                    currentAction = Action.Whip;
                    currentText = TextShown.Whip;
                    charDefense -= (int)tailWhipEffect;
                    tailWhipCount += 1;
                    if (tailWhipCount == 3)
                    {
                        tailWhipEffect = 0;
                    }
                }
            }

            if (currentAction == Action.Scratch)
            {
                frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (frameTime >= 0.1)
                {
                    currentFrame++;
                    frameTime -= 0.1f;
                    if (currentFrame >= scratch.Count && battleTime >= 1.0f)
                    {
                        currentAction = Action.None;
                        frameTime = 0;
                    }
                }
            }
            if (currentAction == Action.Ember)
            {
                battleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                onFireTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (frameTime >= emberInterval)
                {
                    emberMove.X += 35;
                    emberMove.Y -= 15;
                    frameTime -= emberInterval;
                }
                if (onFireTime >= 0.4)
                {
                    frameTime = 1;
                    if (onFireTime >= 2 && battleTime >= 3)
                    {
                        currentAction = Action.None;
                        emberMove.X = 250;
                        emberMove.Y = 350;
                        battleTime = 0;
                        frameTime = 0;
                        onFireTime = 0;
                    }
                }
            }
            if (currentAction == Action.Tackle)
            {
                battleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (frameTime <= 0.5f)
                {
                    squirtLocation.X -= 10;
                    squirtLocation.Y += 5;
                }
                else if (frameTime <= 1f)
                {
                    squirtLocation.X += 10;
                    squirtLocation.Y -= 5;
                }
                else if (battleTime >= 3f)
                {
                    currentAction = Action.None;
                    squirtLocation.X = 500;
                    squirtLocation.Y = 100;
                    battleTime = 0;
                    frameTime = 0;
                }
            }
            if (currentAction == Action.WaterGun)
            {
                battleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                wetTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (frameTime >= waterInterval)
                {
                    waterMove.X -= 35;
                    waterMove.Y += 20;
                    frameTime -= waterInterval;
                }
                if (wetTime >= 0.4)
                {
                    frameTime = 1;
                    if (wetTime >= 2 && battleTime >= 3)
                    {
                        currentAction = Action.None;
                        waterMove.X = 380;
                        waterMove.Y = 200;
                        battleTime = 0;
                        frameTime = 0;
                        wetTime = 0;
                    }
                }
            }
            if (currentAction == Action.Growl)
            {
                battleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                effectTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (effectTime >= 2 && battleTime >= 3)
                {
                    currentAction = Action.None;
                    effectTime = 0;
                    battleTime = 0;
                }
            }
            if (currentAction == Action.Whip)
            {
                battleTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                effectTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (effectTime >= 2 && battleTime >= 3)
                {
                    currentAction = Action.None;
                    effectTime = 0;
                    battleTime = 0;
                }
            }

            if (currentText == TextShown.Scratch)
            {
                textTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (textTime >= 3)
                {
                    currentText = TextShown.None;
                    currentTurn = Turn.squirtTurn;
                    canAct = true;
                    textTime = 0;
                }
            }
            if (currentText == TextShown.Tackle)
            {
                textTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (textTime >= 3)
                {
                    currentText = TextShown.None;
                    currentTurn = Turn.charTurn;
                    canAct = true;
                    squirtleAction = false;
                    textTime = 0;
                }
            }
            if (currentText == TextShown.WaterGun)
            {
                textTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (textTime >= 3)
                {
                    currentText = TextShown.None;
                    currentTurn = Turn.charTurn;
                    canAct = true;
                    squirtleAction = false;
                    textTime = 0;
                }
            }
            if (currentText == TextShown.Ember)
            {
                textTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (textTime >= 3)
                {
                    currentText = TextShown.None;
                    currentTurn = Turn.squirtTurn;
                    canAct = true;
                    textTime = 0;
                }
            }
            if (currentText == TextShown.Growl)
            {
                textTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (textTime >= 3)
                {
                    currentText = TextShown.None;
                    currentTurn = Turn.squirtTurn;
                    canAct = true;
                    textTime = 0;
                }
            }
            if (currentText == TextShown.Whip)
            {
                textTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (textTime >= 3)
                {
                    currentText = TextShown.None;
                    currentTurn = Turn.charTurn;
                    squirtleAction = false;
                    canAct = true;
                    textTime = 0;
                }
            }
            oldState = currentState;
        }
    }
}