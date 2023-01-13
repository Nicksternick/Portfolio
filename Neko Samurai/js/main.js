"use strict";

// ===== | Screen Initalization | =====
const app = new PIXI.Application({
    width: 800,
    height: 500
});

//document.body.appendChild(app.view);

window.onload = () => {
    document.querySelector('div').appendChild(app.view);
}

WebFont.load({
    google: {
        families : ['Stick']
    },
    active: e => {
        //console.log("Font Loaded");
        app.loader.add(['media/NekoPlaceholder.png',
                    'media/NekoSamurai.png',
                    'media/UndyingWill.png',
                    'media/spritesheet.png',
                    'media/enemySpriteSheet.png',
                    'media/ground1.png',
                    'media/ground2.png',
                    'media/ground3.png',
                    'media/Background.png']);
        //app.loader.onProgress.add(e => { console.log(`progress=${e.progress}`) });
        app.loader.onComplete.add(Setup);
        app.loader.load();        
    }
});

// constants
const sceneWidth = app.view.width;
const sceneHeight = app.view.height;

// ===== | Variables | =====

// ----- | Scenes | -----
// NOTE: There will be three scenes minimum, but I would
// like to make a scene between start and game as an 
// Intro to the game
let startScene, gameScene, gameOverScene;
let stage;

// Text
let healthText;
let distanceText;
let highScoreText

let debugText;
let swordText;

// ----- | Obstacles | -----
// NOTE: Obstacles will all be stored in a list, and will 
// be looped through to see the state of the obstacle
let obstacles = [];

let player;
let health = 100;
let distance = 0;
let highScore;
let swordTimer = 100;
let swingingBlade = false;
let overusedBlade = false;
let wasHit = false;
let hitTimer;
let dt;

let swordSound, hitSound, damageSound;

let randomValue;

let healthMeter;
let swordMeter;
let music;

let paused;
let playerSheet = {};
let enemySheet = {};

let background = [];

const hexValues = [0,1,2,3,4,5,6,7,8,9,'A','B','C','D','E','F']; 
const enemyTypes = ["breakable", "breakable", "unbreakable", "mover"];


// ===== | Setup | =====

// Setup(): Runs after the Apllication is 
// complete, and sets up all of the scenes
function Setup()
{
    stage = app.stage;

    // Create the different scene views
    startScene = new PIXI.Container();
    stage.addChild(startScene);

    // TEMPORARAY
    startScene.visible = true;

    gameScene = new PIXI.Container();
    gameScene.visible = false;
    gameScene.sortableChildren = true;
    stage.addChild(gameScene);

    gameOverScene = new PIXI.Container();
    gameOverScene.visible = false;
    stage.addChild(gameOverScene);

    // shootSound = new Howl({
    //     src: ['sounds/shoot.wav']
    // });
    swordSound = new Howl({
        src: ['sounds/Swing.mp3'],
        loop: true,
        volume: 0.05
    });

    hitSound = new Howl({
        src: ['sounds/Hit.mp3'],
        volume: 0.1
    });

    music = new Howl({
        src: ['sounds/Music.mp3'],
        loop: true,
        volume: .25
    })

    // Create the text and the background sprites
    CreateEnviorment();

    paused = true;

    app.ticker.add(GameLoop);
}

// ===== | Game Loop | =====

// GameLoop(): The method 
// for the gameplay loop
function GameLoop()
{
    if (paused) return;

    dt = 1/app.ticker.FPS;
    if (dt > 1/24) dt = 1/24;

    background[0].Move();
    background[1].Move();

    // ----- | For every obstacle on screen | -----
    for(let obstacle of obstacles)
    {
        // Move the object
        obstacle.Move(dt);

        // Check to see if it's a mover obstacle and then move to another
        // land if it's halfway down the path
        if (obstacle.objectType == "mover" && obstacle.getBounds().x < 500)
        {
            switch (obstacle.rowNum)
            {
                case 3:
                    obstacle.y = sceneHeight - 140;
                    obstacle.speed = 350;
                    obstacle.rowNum--;
                    obstacle.zIndex--;
                    break;
                case 2:
                    obstacle.y = sceneHeight - (40 + (200 * getRandomInt(2)));
                    if (obstacle.y == sceneHeight - 240)
                    {
                        obstacle.rowNum--;
                        obstacle.zIndex--;
                        obstacle.speed = 300;
                    }
                    else
                    {
                        obstacle.rowNum++;
                        obstacle.zIndex++;
                        obstacle.speed = 400;
                    }
                    break;
                case 1:
                    obstacle.y = sceneHeight - 140;
                    obstacle.speed = 350;
                    obstacle.rowNum++;
                    obstacle.zIndex++;
                    break;
            }

            obstacle.objectType = "moveCheck";
        }

        // Check to see if it's intersection with the chracter and if it's in the same row.
        if (characterIntersect(player, obstacle) && player.rowNum == obstacle.rowNum && obstacle.isAlive)
        {
            obstacle.isAlive = false;
            gameScene.removeChild(obstacle);

            // Check to see if the player is swingint their blade and if the object is unbreakable
            if (!swingingBlade || obstacle.objectType == "unbreakable")
            {
                LowerHealth(20);
                wasHit = true;
                hitTimer = 10;
                hitSound.play();
            }
        }

        // check to see if the object is out of bounds
        if (obstacle.getBounds().x < -100)
        {
            obstacle.isAlive = false
            gameScene.removeChild(obstacle);
        }
    }

    // remove any dead objects
    obstacles = obstacles.filter(o=>o.isAlive);
    
    // increment the distance by 1
    IncreaseDistance(1);

    // let obstacle1 = new Obstacle(800, sceneHeight - 240, 1);
    // let obstacle1 = new Obstacle(800, sceneHeight - 140, 2);
    // let obstacle1 = new Obstacle(800, sceneHeight - 40, 3);
    // gameScene.addChild(obstacle1);
    // obstacles.push(obstacle1);

    // Check to see the distance, and random value accordingly
    if (distance < 2000)
    {
        randomValue = 51;
    }
    else if (distance > 2000 && distance < 4000)
    {
        randomValue = 41;
    }
    else if (distance > 4000 && distance < 6000)
    {
        randomValue = 31;
    }
    else if (distance > 6000 && distance < 8000)
    {
        randomValue = 21;
    }
    else if (distance > 8000 && distance < 10000)
    {
        randomValue = 11;
    }
    else
    {
        randomValue = 1;
    }

    // Random object spawning
    let random = getRandomInt(randomValue);
    if (random == 0)
    {
        let obstacle
        let row = getRandomInt(3);
        switch (row)
        {
            case 0:
                obstacle = CreateNewObstacle(row);
                break;

            case 1:
                obstacle = CreateNewObstacle(row);
                break;

            case 2:
                obstacle = CreateNewObstacle(row);
                break;
        }

        obstacles.push(obstacle);
        gameScene.addChild(obstacle);

        // Check to see if the object is interesting with an object, 
        //and remove it if so
        if (obstacles.length != 0)
        {
            for(let o of obstacles)
            {
                if (rectsIntersect(obstacle, o) && o != obstacle && o.rowNum == obstacle.rowNum)
                {
                    obstacle.isAlive = false;
                    gameScene.removeChild(obstacle);
                }            
            }            
        }
    }

    if (!overusedBlade)
    {
        // If your swinging your blade, decrement the swordMeter
        // Increase it if you aren't swinging
        if (!swingingBlade)
        {
            if (swordTimer < 100)
            LowerBladeMeter(-.1);
        }
        else
        {
            LowerBladeMeter(1);
        }
    }
    else
    {
        // If you overuse your blade then wait 
        //for the sword meter to reach max
        LowerBladeMeter(-.05);
        swordMeter.tint = GetRandomColor();

        if (swordTimer > 100)
        {
            overusedBlade = false;
            swordMeter.tint = 0x2645FF;
        }
    }

    // If the player was hit, play hit animation (Which is just flashing colors)
    if (wasHit)
    {
        hitTimer--;
        player.tint = GetRandomColor();
        if (hitTimer < 0)
        {
            player.tint = 0xFFFFFF;
            wasHit = false;
        }
    }

    // If the health is less than 20, then make health meter flash
    if (health <= 20){healthMeter.tint = GetRandomColor();}
    // If player has 0 health, GameOver()
    if (health <= 0){GameOver()}
}

// ===== | Methods | =====

// StartGame(): Sets up the game to be played again
function StartGame()
{
    distanceText.text = 'Distance: 0000';
    health = 100;
    LowerHealth(0);
    healthMeter.tint = 0xFF4526;
    swordTimer = 100;
    LowerBladeMeter(0)
    swordMeter.tint = 0x2645FF;
    distance = 0;
    player.tint = 0xFFFFFF;
    wasHit = false;

    for(let obstacle of obstacles)
    {
        gameScene.removeChild(obstacle);
    }

    obstacles = [];

    music.play();

    paused = false;
    startScene.visible = false;
    gameOverScene.visible = false;
    gameScene.visible = true;
}

// GameOver(): Sends the player to the game overscreen
function GameOver()
{
    music.stop();
    paused = true;
    highScore = distance;
    gameScene.visible = false;
    gameOverScene.visible = true;
    highScoreText.text = `Highscore: ${highScore}`;
}

// CreateEnviorment(): Creates the text for 
// the game and adds any background to the game
function CreateEnviorment()
{
    // ----- | Setup Keyboard Input | -----
    document.addEventListener('keydown', SwingStart);
    document.addEventListener('keypress', PlayerControl);
    document.addEventListener('keyup', SwingEnd);

    // ----- | Title Screen | -----

    // Background Logo
    const nekoSamurai = PIXI.Sprite.from('media/NekoSamurai.png');
    nekoSamurai.x = 425;
    nekoSamurai.y = 40;
    nekoSamurai.alpha = .5;
    startScene.addChild(nekoSamurai);

    const undyingWill = PIXI.Sprite.from('media/UndyingWill.png');
    undyingWill.x = 300;
    undyingWill.y = 40;
    undyingWill.alpha = .5;
    startScene.addChild(undyingWill);

    // Title
    const titleStyle = new PIXI.TextStyle({
        dropShadow: true,
        dropShadowBlur: 5,
        dropShadowColor: "red",
        dropShadowDistance: 4,
        fill: "white",
        fontFamily: "Stick",
        fontSize: 60,
        padding: 3
    });
    const titleText = new PIXI.Text('Neko Samurai', titleStyle);
    titleText.x = 400;
    titleText.y = 125;
    titleText.anchor.set(0.5);
    startScene.addChild(titleText);

    // Sub Title
    const subTitleStyle = new PIXI.TextStyle({
        dropShadow: true,
        dropShadowBlur: 5,
        dropShadowColor: "red",
        dropShadowDistance: 4,
        fill: "white",
        fontFamily: "Stick",
        fontSize: 50,
        padding: 3
    });
    const subTitleText = new PIXI.Text('Undying Will', subTitleStyle);
    subTitleText.x = 400;
    subTitleText.y = 180;
    subTitleText.anchor.set(0.5);
    startScene.addChild(subTitleText);

    // Start Button
    const buttonStyle = new PIXI.TextStyle({
        dropShadow: true,
        dropShadowBlur: 5,
        dropShadowColor: "red",
        dropShadowDistance: 4,
        fill: "white",
        fontFamily: "Stick",
        fontSize: 40,
        padding: 3
    });
    const startButton = new PIXI.Text('Begin The Journey', buttonStyle);
    startButton.x = 400;
    startButton.y = 400;
    startButton.interactive = true;
    startButton.buttonMode = true;
    startButton.on("pointerup", StartGame);
    startButton.on('pointerover', e => e.target.style.fill = 'red');
    startButton.on('pointerout', e => e.currentTarget.style.fill = 'white');
    startButton.anchor.set(0.5);
    startScene.addChild(startButton);

    // ----- | Game Over | -----
    const GameOverText = new PIXI.Text('Game Over', titleStyle);
    GameOverText.x = 400;
    GameOverText.y = 125;
    GameOverText.anchor.set(0.5);
    gameOverScene.addChild(GameOverText);

    highScoreText = new PIXI.Text(`High Score: 0000`, subTitleStyle);
    highScoreText.x = 400;
    highScoreText.y = 200;
    highScoreText.anchor.set(0.5);
    gameOverScene.addChild(highScoreText);

    const GameOverButton = new PIXI.Text('Restart The Journey?', buttonStyle);
    GameOverButton.x = 400;
    GameOverButton.y = 400;
    GameOverButton.interactive = true;
    GameOverButton.buttonMode = true;
    GameOverButton.on("pointerup", StartGame);
    GameOverButton.on('pointerover', e => e.target.style.fill = 'red');
    GameOverButton.on('pointerout', e => e.currentTarget.style.fill = 'white');
    GameOverButton.anchor.set(0.5);
    gameOverScene.addChild(GameOverButton);

    
    // ----- | Game Screen | -----
    // const graph = new PIXI.Graphics();
    // graph.beginFill(0xffff00);
    // graph.drawRect(0, 0, 300, 200);
    // startScene.addChild(graph);

    background.push(new Background(0, 0, app.loader.resources['media/Background.png'].texture));
    background.push(new Background(sceneWidth, 0, app.loader.resources['media/Background.png'].texture));

    gameScene.addChild(background[0]);
    gameScene.addChild(background[1]);

    let platorm1 = new Ground(0, sceneHeight - 130, app.loader.resources['media/ground1.png'].texture);
    gameScene.addChild(platorm1);

    let platorm2 = new Ground(0, sceneHeight - 230, app.loader.resources['media/ground2.png'].texture);
    gameScene.addChild(platorm2);

    let platorm3 = new Ground(0, sceneHeight - 300, app.loader.resources['media/ground3.png'].texture);
    gameScene.addChild(platorm3);

    // HUD for the game
    const hudStyle = new PIXI.TextStyle({
        dropShadow: true,
        dropShadowBlur: 5,
        dropShadowColor: "red",
        dropShadowDistance: 4,
        fill: "white",
        fontFamily: "Stick",
        fontSize: 25,
        padding: 3
    })

    distanceText = new PIXI.Text('Distance: 0000', hudStyle);
    distanceText.x = 200;
    distanceText.y = 5;
    //distanceText.anchor.set(0.5);
    gameScene.addChild(distanceText);

    healthText = new PIXI.Text('Health:', hudStyle);
    healthText.x = 200;
    healthText.y = 35;
    //healthText.anchor.set(0.5);
    gameScene.addChild(healthText);

    swordText = new PIXI.Text('Sword:', hudStyle);
    swordText.x = 200;
    swordText.y = 65;
    //swordText.anchor.set(0.5);
    gameScene.addChild(swordText);

    healthMeter = new Meter (286, 38, 300, 25, 10, 0xFFFFFF, 0xFF4526);
    gameScene.addChild(healthMeter);

    swordMeter = new Meter (286, 70, 300, 25, 10, 0xFFFFFF, 0x2645FF);
    gameScene.addChild(swordMeter);

    LoadSpriteSheet();

    player = new Player(70, sceneHeight - 140, .28, playerSheet.running);
    player.loop = true;
    player.visible = true;
    gameScene.addChild(player);
    player.play();

    // let obstacle1 = new Obstacle(800, sceneHeight - 140, 2);
    // gameScene.addChild(obstacle1);
    // obstacles.push(obstacle1);

    // let obstacle2 = new Obstacle(810, sceneHeight - 140, 2);
    // gameScene.addChild(obstacle2);
    // obstacles.push(obstacle2);

    // ----- | Game Over Screen | -----
}

// IncreaseDistance(): Increases distance by an amount, and then updates the Hud
function IncreaseDistance(amount)
{
    distance += amount;
    distanceText.text = `Distance: ${distance}`;
}

// BladeOverused(): Stops the player swinging state
function BladeOverused()
{
    swordTimer = 0; 
    overusedBlade = true;
    swingingBlade = false;
    player.ChangeSprite(playerSheet.running, .20);
    swordSound.stop();
}

// LowerBladeMeter(); Decrease the sword meter, if it goes below zero then call BladeOveruse()
function LowerBladeMeter(amount)
{
    swordTimer -= amount;
    if (swordTimer < 0) 
    {
        BladeOverused();
    };
    let temp = (swordTimer / 100) * 300;
    let temp2 = 300 - temp;
    swordMeter.width = temp;
    swordMeter.x = temp2;
}

// ----- | Life Detection | -----

// LowerHealth(): Lowers the health by an amount
function LowerHealth(amount)
{
    health -= amount;
    let temp = (health / 100) * 300;
    let temp2 = 300 - temp;
    healthMeter.width = temp;
    healthMeter.x = temp2;
}

// ----- | Key Detection | -----

// SwingStart(): Swings Blade if space is pressed
function SwingStart(key)
{
    if (!overusedBlade && key.keyCode == 32)
    {
        swingingBlade = true;
        player.ChangeSprite(playerSheet.slashing, .20);
        swordSound.play();
    }
}

// SwingEnd(): Stop Swinging the blade if space is released
function SwingEnd(key)
{
    if (key.keyCode == 32)
    {
        swingingBlade = false;
        player.ChangeSprite(playerSheet.running, .20);
        swordSound.stop();
    }
}

// PlayerControls(): controls movement of the player
function PlayerControl(key)
{
    switch (key.keyCode)
    {
        case 119:
            player.MoveUp();
            break;
        case 115:
            player.MoveDown();
            break;
    }
}

// GetRandomColors(): Gets a random hexidecimal color
function GetRandomColor()
{
    let color = "0x";

    for (let i = 0; i < 6; i++)
    {
        color += hexValues[Math.floor(Math.random() * hexValues.length)];
    }

    return color;
}

//CreateNewObstacle(): Creates a new obsatcle and returns it
function CreateNewObstacle(row)
{
    let enemyType = enemyTypes[getRandomInt(enemyTypes.length)];
    let obstacle;

    switch(enemyType)
    {
        case "breakable":
            obstacle = new Obstacle(800, sceneHeight - (40 + (100 * row)), 3 - row, enemySheet.box, "breakable", 300 - (50 * row));
            break;

        case "unbreakable":
            obstacle = new Obstacle(800, sceneHeight - (40 + (100 * row)), 3 - row, enemySheet.wall, "unbreakable", 300 - (50 * row));
            break;

        case "mover":
            obstacle = new Obstacle(800, sceneHeight - (40 + (100 * row)), 3 - row, enemySheet.mover, "mover", 400 - (50 * row));
            obstacle.animationSpeed = .1;
            obstacle.play()
            break;
    }
    
    return obstacle;
}

// ----- | Animation Method | -----

// LoadSpriteSheet(): Creates the player and enemy sprite sheet
function LoadSpriteSheet()
{
    // Loading the player spriteSheet
    let basePlayerSheet = PIXI.BaseTexture.from('media/spritesheet.png');
    let w = 400;
    let h = 800;

    playerSheet["running"] = 
    [
        new PIXI.Texture(basePlayerSheet, new PIXI.Rectangle(0 * w, 0, w, h)),
        new PIXI.Texture(basePlayerSheet, new PIXI.Rectangle(1 * w, 0, w, h)),
        new PIXI.Texture(basePlayerSheet, new PIXI.Rectangle(2 * w, 0, w, h))
    ];
    
    playerSheet["slashing"] = 
    [
        new PIXI.Texture(basePlayerSheet, new PIXI.Rectangle(3 * w, 0, w, h)),
        new PIXI.Texture(basePlayerSheet, new PIXI.Rectangle(4 * w, 0, w, h)),
        new PIXI.Texture(basePlayerSheet, new PIXI.Rectangle(5 * w, 0, w, h)),
        new PIXI.Texture(basePlayerSheet, new PIXI.Rectangle(6 * w, 0, w, h))
    ];

    // Loding the enemy spriteSheet
    let baseEnenmySheet = PIXI.BaseTexture.from('media/enemySpriteSheet.png');

    enemySheet["wall"] =
    [
        new PIXI.Texture(baseEnenmySheet, new PIXI.Rectangle(0, 0, 100, 200))
    ];

    enemySheet["box"] = 
    [
        new PIXI.Texture(baseEnenmySheet, new PIXI.Rectangle(100, 0, 100, 200))
    ];

    enemySheet["mover"] =
    [
        new PIXI.Texture(baseEnenmySheet, new PIXI.Rectangle(200, 0, 120, 200)),
        new PIXI.Texture(baseEnenmySheet, new PIXI.Rectangle(320, 0, 120, 200)),
        new PIXI.Texture(baseEnenmySheet, new PIXI.Rectangle(440, 0, 120, 200)),
        new PIXI.Texture(baseEnenmySheet, new PIXI.Rectangle(560, 0, 120, 200))
    ];
}

