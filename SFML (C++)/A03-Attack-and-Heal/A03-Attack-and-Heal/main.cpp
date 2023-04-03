#include <iostream>
#include <SFML/Window.hpp>
#include "Character.h"

Character* g_character_ptr;

Time g_preTime;
Clock g_deltaClock;

void Init(sf::RenderWindow& window) {
    Texture* characterTexture_ptr = new Texture();
    characterTexture_ptr->loadFromFile("Images/Pongo.png");

    Texture* heartFilled_ptr = new Texture();
    heartFilled_ptr->loadFromFile("Images/heart.png");
    Texture* heartEmpty_ptr = new Texture();
    heartEmpty_ptr->loadFromFile("Images/heart_gray.png");
        
    g_character_ptr = new Character(characterTexture_ptr, sf::Vector2f(window.getSize().x / 2, window.getSize().y / 2), heartFilled_ptr, heartEmpty_ptr);
}

void Update(sf::RenderWindow& window) {
    // DeltaTime
    Time currentTime = g_deltaClock.getElapsedTime();
    Time deltaTime = currentTime - g_preTime;
    g_preTime = currentTime;

    g_character_ptr->Update(window);

}

void Render(sf::RenderWindow& window) {
    g_character_ptr->Render(window);
}

int main()
{
    sf::RenderWindow window(sf::VideoMode(800, 600), "My window");

    Init(window);

    // run the program as long as the window is open
    while (window.isOpen())
    {
        // check all the window's events that were triggered since the last iteration of the loop
        sf::Event event;
        while (window.pollEvent(event))
        {
            // "close requested" event: we close the window
            if (event.type == sf::Event::Closed)
                window.close();
        }

        Update(window);

        window.clear(Color(244, 244, 244));

        Render(window);

        window.display();
    }

    return 0;
}