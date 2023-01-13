// NOTE: Two classes will be made for this game.
// Player: Handles health, position, and movement of the player
// Obstacle: Inheritence tree for all obstacles

// ===== | Player | =====

// let animSprite = new PIXI.AnimatedSprite(texture);
// animSprite.loop = true
// animSprite.scale.set(.5);
// gameScene.addChild(animSprite);
// animSprite.play();

class Player extends PIXI.AnimatedSprite
{
    constructor(x = 0, y = 0, size = 0, texture)
    {
        super(texture);
        this.anchor.set(0, 1);
        this.scale.set(size);
        this.animationSpeed = .20;
        this.x = x;
        this.y = y;
        this.rowNum = 2;
        this.zIndex = 2;
        this.health = 100;
        this.bladeOut = false;
        this.bladeCooldown = false;
    }
    // let player1 = new Player(80, sceneHeight - 245, .26, texture);
    MoveUp()
    {
        if (this.rowNum > 1)
        {
            this.rowNum--;
            this.zIndex--;
            //this.x -= 10;
            this.y -= 100;
            //this.scale.set(this.scale -= .2);
        }
    }

    MoveDown()
    {
        if (this.rowNum < 3)
        {
            this.rowNum++;
            this.zIndex++;
            //this.x += 10;
            this.y += 100;
            //this.scale.set(this.scale += .2);
        }
    }

    ChangeSprite(animTex, speed)
    {
        this.textures = animTex;
        this.animationSpeed = speed;
        this.play();
    }
}

// ===== | Obstacles | =====
class Obstacle extends PIXI.AnimatedSprite
{
    constructor(x = 0, y = 0, row, texture, type, speed)
    {
        super(texture);
        this.anchor.set(0, 1);
        this.x = x;
        this.y = y;
        this.speed = speed;
        this.rowNum = row;
        this.zIndex = row;
        this.isAlive = true;
        this.objectType = type;
    }

    Move(dt = 1/60)
    {
        this.x -= this.speed * dt;
    }
}

// ===== | Ground | =====
// const graph = new PIXI.Graphics();
// graph.beginFill(0xffff00);
// graph.drawRect(0, 0, 300, 200);
// startScene.addChild(graph);
// x, y, l, ws
class Ground extends PIXI.Sprite
{
    constructor(x = 0, y = 0, texture)
    {
        super(texture);
        this.x = x;
        this.y = y;
    }
}

// ===== | SwordMeter | =====
class Meter extends PIXI.Graphics
{
    constructor(x = 0, y = 0, l = 0, w = 0, r, color = 0xFFFFFF, tint = 0xFFFFFF)
    {
        super();
        this.beginFill(color);
        this.tint = tint
        this.drawRoundedRect (x, y, l, w, r);
        this.dropShadow = true;
        this.dropShadowBlur = 5;
        this.dropShadowColor = "red";
        this.dropShadowDistance = 4;
    }
}

// ===== | Ground | =====
class Background extends PIXI.Sprite
{
    constructor(x = 0, y = 0, texture)
    {
        super(texture);
        this.x = x;
        this.y = y;
    }

    Move()
    {
        this.x -= .1;
    }
}
