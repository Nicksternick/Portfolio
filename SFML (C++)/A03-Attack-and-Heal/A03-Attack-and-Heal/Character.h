#pragma once
#include <iostream>
#include <SFML/Graphics.hpp>
#include "Heart.h"

using namespace std;
using namespace sf;

class Character
{
private:
	Sprite m_characterSprite;
	int health = 4;

	// Heart Stuff
	Heart* hearts[5];
	bool isKeyPressed = false;
	bool atHeartBounds = false;

	// Text Stuff
	Font characterFont;
	Text controls;
	Text healthText;
public:
	Character(Texture* characterSprite, Vector2f position, Texture* heartFilled, Texture* heartEmpty);
	void Render(RenderWindow& window);
	void Update(RenderWindow& window);
};

