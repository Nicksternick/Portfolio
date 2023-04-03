#include<SFML/Graphics.hpp>
#include "Heart.h"

using namespace sf;

Heart::Heart(Texture* heartFilled, Texture* heartEmpty, Vector2f position)
{
	hearts[0].setTexture(*heartFilled);
	hearts[1].setTexture(*heartEmpty);

	heartSprite = hearts[0];
	heartSprite.setPosition(position);
	heartSprite.setScale(.2, .2);
}

void Heart::UpdateHeart(int heart)
{
	Vector2f position;
	if (heartActive)
	{
		position = heartSprite.getPosition();
		heartSprite = hearts[1];
		heartSprite.setPosition(position);
		heartSprite.setScale(.2, .2);
		heartActive = false;
	}
	else
	{
		position = heartSprite.getPosition();
		heartSprite = hearts[0];
		heartSprite.setPosition(position);
		heartSprite.setScale(.2, .2);
		heartActive = true;
	}
}

void Heart::Render(RenderWindow& window)
{
	window.draw(heartSprite);
}
