#include "Character.h"
#include <SFML/Graphics.hpp>
#include <iostream>

using namespace sf;

Character::Character(Texture* characterSprite, Vector2f position, Texture* heartFilled, Texture* heartEmpty)
{
	m_characterSprite.setTexture(*characterSprite);
    // init origin
    m_characterSprite.setOrigin(characterSprite->getSize().x / 2.0f, characterSprite->getSize().y / 2.35f);
    // init position
    m_characterSprite.setPosition(position);
    // init rotation
    m_characterSprite.setRotation(0);
    // init scale
    m_characterSprite.setScale(.4f, .4f);

    characterFont.loadFromFile("Fonts/arial.ttf");

    controls.setFont(characterFont);
    controls.setString("A: Attack\nH: Heal");
    controls.setFillColor(Color(0, 0, 0));
    controls.setPosition(Vector2f(10, 0));

    // Heart Initialization Area
    int offset = 0;
    for (int i = 0; i < 5; i++)
    {
        hearts[i] = new Heart(heartFilled, heartEmpty, Vector2f(150 + offset, -5));
        offset += 100;
    }
}

void Character::Render(RenderWindow& window)
{
    window.draw(m_characterSprite);
    window.draw(controls);
    for (int i = 0; i < 5; i++)
    {
        hearts[i]->Render(window);
    }
}

void Character::Update(RenderWindow& window)
{
    if (!isKeyPressed && Keyboard::isKeyPressed(Keyboard::A))
    {
        if (health != 0)
        {
            hearts[health]->UpdateHeart(health);
            health--;
            atHeartBounds = false;
        }
        else
        {
            if (!atHeartBounds)
            {
                hearts[health]->UpdateHeart(health);
                atHeartBounds = true;
            }
        }
    }

    if (!isKeyPressed && Keyboard::isKeyPressed(Keyboard::H))
    {
        if (!hearts[health]->heartActive && health == 0)
        {
            hearts[health]->UpdateHeart(health);
        }
        else if (health != 4)
        {
            health++;
            hearts[health]->UpdateHeart(health);
            atHeartBounds = false;
        }
    }

    isKeyPressed = Keyboard::isKeyPressed(Keyboard::A) || Keyboard::isKeyPressed(Keyboard::H);
}
