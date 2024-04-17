import * as main from "./main.js";

// 1 - do preload here - load fonts, images, additional sounds, etc...

// Set the url to the data file
const url = "data/av-data.json";
// create a new XMLHttpRequest
const xhr = new XMLHttpRequest();
// If the data loads
xhr.onload = (e) => {
    console.log(`In onload - HTTPS Status Code = ${e.target.status}`);

    // Get the text and parse it to JSON
    const text = e.target.responseText;
    const json = JSON.parse(text);

    // parse it's data into these vriables
    const appName = json.name;
    const songs = json.songs;
    const colors = json.colors;

    // also the default colors from the file as well
    let defaultFill = json.defaultFill;
    let defaultStroke = json.defaultStroke;

    // Set the apps name in the title, as well as the header
    document.querySelector("title").innerHTML = appName;
    document.querySelector("header h1").innerHTML = appName;

    // map the songs into a collection of option tags, and make the first one the select one
    let options = songs.map(songData => (
        `<option value="${songData.link}">${songData.name}</option>`));
    options[0] = `<option value="${songs[0].link}" selected">${songs[0].name}</option>`;

    // add each one of them to the song list
    options.forEach(song => {
        document.querySelector("#select-song").innerHTML += song;
    });

    // Set solor options to a empty string
    let colorOptions = "";

    // for each color
    for (let color of colors)
    {
        // chheck to see if it's the deafult stroke if the same as the color name
        if (color.color != defaultStroke)
        {
            // If it isn't just add the color option
            colorOptions += `<option value="${color.color}">${color.name}</option>`;
        }
        else
        {
            // if it is make sure to also add the selected attribute
            colorOptions += `<option value="${color.color}" selected>${color.name}</option>`;
        }
    }

    // Get all (4) the color selectors
    const colorSelectors = document.querySelectorAll(".colors");

    for (let selectors of colorSelectors)
    {
        // assign their option to the color options
        selectors.innerHTML = colorOptions;
    }
    
    // 2 - start up app, passing in default fill and stroke
    main.init(defaultFill, defaultStroke);
};
xhr.onerror = e => console.log(`In onerror - HTTP Status Code = ${e.target.status}`);
xhr.open("GET", url);
xhr.send();


