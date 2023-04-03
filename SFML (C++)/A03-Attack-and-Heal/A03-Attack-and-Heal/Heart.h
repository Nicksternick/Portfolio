#pragma once
#include<iostream>
#include<SFML/Graphics.hpp>

using namespace sf;

class Heart
{
private:
	Sprite hearts[2];
	Sprite heartSprite;
public:
	bool heartActive = true;;
	Heart(Texture* heartFilled, Texture* heartEmpty, Vector2f position);
	void Render(RenderWindow& window);
	void UpdateHeart(int heart);
};

