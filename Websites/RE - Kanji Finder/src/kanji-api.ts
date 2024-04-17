// ===== | Imports | =====
import { fetchJson } from "./web-handler"
import { randomInt } from "./utils"

// ===== | Interfaces | =====

/** Object that is given by the kanji api */
interface KanjiObject {
    grade: number,
    jlpt: number,
    kanji: string,
    kun_readings: string[],
    meanings: string[],
    name_readings: string[],
    on_readings: string[],
    stroke_count: number
}

interface kanjiLibrary {
    [key: string]: string
}

// ===== | Variables | =====

// Important constistent variables
const KanjiAPI_URL: string = 'https://kanjiapi.dev/v1/';
const libraryKey: string = "kanji-library";
const loadingImgTag:string = "<img src='media/loading.gif' alt='downloading'>";

let kanjiList: string[];
let currentKanji: KanjiObject;
let kanjiLibrary: kanjiLibrary;

// Containers
let kanjiContainer: HTMLDivElement;
let libraryContainer: HTMLDivElement;
let chosenKanjiContainer: HTMLSpanElement;

// ===== | Methods | =====

/** initalizes the KanjiAPI, getting the kanji from local storage */
export const initKanjiAPI = () => {
    // Get a reference to the divContainer for the kanji
    kanjiContainer = document.querySelector('#container') as HTMLDivElement;
    libraryContainer = document.querySelector('#library-kanji') as HTMLDivElement;
    chosenKanjiContainer = document.querySelector('#chosen-kanji') as HTMLSpanElement;

    // ----- | Get the local storage of the browser | -----
    kanjiLibrary = {};

    // Parse the local storage of the page into a variable
    let storage: string | null = localStorage.getItem(libraryKey);

    // If storage is not null
    if (storage) {
        // Parse this information into the kanji library
        kanjiLibrary = JSON.parse(storage)

        updateLibrary();
    }

    // ----- | Initialize the kanjiList to be a list of all the kanji | -----
    // Get the list of every possible kanji
    let url: string = KanjiAPI_URL + `kanji/all`;

    // Fetch the first random kanji when the page loads
    fetchJson(url).then((response) => {
        // set the kanji list to the response
        kanjiList = response;

        // load a random kanji
        loadRandomKanji();
    });
}

/** Updates the list that the random kanji is picked from */
export const updateKanjiList = (newlist:string) => {
    // create the new url
    let url: string = KanjiAPI_URL + newlist;

    // try to fetch the new list
    fetchJson(url).then((response) => {
        // set the new list if successful
        kanjiList = response;
    }).catch(() => {
        // throw a console log if it fails
        console.log("Error Updating List, Make Sure Values Are Correct");
    });
}

/** save the current kanji to the kanji library */
export const saveKanji = () => {
    // If the current kanji isn't already in the library
    if (kanjiLibrary[currentKanji.kanji] == null)
    {
        // Add the current kanji as a new entry
        kanjiLibrary[currentKanji.kanji] = currentKanji.kanji;
        
        // update the library to make it visible to the user
        updateLibrary();

        // Update the local storage
        localStorage.setItem(libraryKey, JSON.stringify(kanjiLibrary));
    }
}

/** removes the selected kanji from the library */
export const removeKanji = () => {
    // Get the key from the value of from the chosen kanji container
    let index: string | undefined = chosenKanjiContainer.dataset.value;
    
    // if the key is not undefined
    if (index)
    {
        // delete that entry from the library
        delete kanjiLibrary[index];

        // reset the chosen kanji container
        chosenKanjiContainer.dataset.value = "";
        chosenKanjiContainer.innerHTML = "";

        // update the library to make it visible to the user
        updateLibrary();

        // Update the local storage
        localStorage.setItem(libraryKey, JSON.stringify(kanjiLibrary));
    }
}

/** search the selected kanji from the library */
export const searchKanji = () => {
    // Get the kanji from the value of from the chosen kanji container
    let kanji: string | undefined = chosenKanjiContainer.dataset.value;
    
    // If the kanji is not undefined
    if (kanji)
    {
        // load that kanji
        loadNewKanji(kanji)
    }
}

/** updates the kanji library UI */
const updateLibrary = () => {
    // clear the library
    let newElement: string = "";

    // For every kanji entry
    for (let kanji in kanjiLibrary)
    {
        // Add them to the library
        newElement += `<div class='library-item is-clickable is-size-1 p-2'>${kanji}</div>`;
    }

    // Put it in the container on the page
    libraryContainer.innerHTML = newElement;

    // Get all the library items
    let libraryItems = document.querySelectorAll('.library-item') as NodeListOf<HTMLDivElement>;

    // For each item
    libraryItems.forEach((kanji) => {
        // Add a new event to update the chosen kanji when a new kanji is clicked
        kanji.addEventListener('click', () => {
            chosenKanjiContainer.dataset.value = kanji.innerHTML;
            chosenKanjiContainer.innerHTML = `${kanji.innerHTML} is selected.`;
        })
    })
}

// ----- | Load Kanji Functions | -----

/** Takes a kanji and the div container, 
 * load the file and adds it content in a list */
export const loadNewKanji = (kanji: string) => {
    // Create the url used for the fetch
    let url: string = KanjiAPI_URL + `kanji/${kanji}`

    kanjiContainer.innerHTML = loadingImgTag;

    // fetch the json, and continue if it succeeds
    fetchJson(url).then((response) => {
        // Set the current kanji to the newly aquired one
        currentKanji = response;

        // Prepare the information variables 
        let kanji:string = `<p class="is-size-1">${currentKanji.kanji}</p>`;
        let stroke:string = `Stroke Count: ${currentKanji.stroke_count}`;
        let grade:string = "Grade: ";
        let jlpt:string = "JLPT: ";
        let kunReadings:string = "Kun Readings: ";
        let onReadings:string = "On Readings: ";
        let nameReadings:string = "Name Readings: ";
        let meanings:string = "Meanings: ";
        
        // Set the grade of the kanji
        grade += currentKanji.grade != null? currentKanji.grade.toString() : "None";

        // Set the jlpt level of the kanji
        jlpt += currentKanji.jlpt != null? currentKanji.jlpt.toString() : "None";

        // Set the kun readings (array) of the kanji
        if (currentKanji.kun_readings.length != 0)
        {
            kunReadings += currentKanji.kun_readings.join(', ');
        }
        else
        {
            kunReadings += "None"
        }

        // Set the on readings (array) of the kanji
        if (currentKanji.on_readings.length != 0)
        {
            onReadings += currentKanji.on_readings.join(', ');
        }
        else
        {
            onReadings += "None"
        }

        // Set the name readings (array) of the kanji
        if (currentKanji.name_readings.length != 0)
        {
            nameReadings += currentKanji.name_readings.join(', ');
        }
        else
        {
            nameReadings += "None"
        }

        // Set the meanings (array) of the kanji
        if (currentKanji.meanings.length != 0)
        {
            meanings += currentKanji.meanings.join(', ');
        }
        else
        {
            meanings += "None"
        }

        // Create a new unordered list and fill it with the contents
        let newElement: string = `<ul class="">`;
        newElement += `<li>${stroke}</li>`;
        newElement += `<li>${grade}</li>`;
        newElement += `<li>${jlpt}</li>`;
        newElement += `<li>${kunReadings}</li>`;
        newElement += `<li>${onReadings}</li>`;
        newElement += `<li>${nameReadings}</li>`;
        newElement += `<li>${meanings}</li>`;
        newElement += `</ul>`;

        // Set the containers contents equal to the new elements
        kanjiContainer.innerHTML = `${kanji + newElement}`;
    });
}

/**Load a random kanji */
export const loadRandomKanji = () => {
    // Get a random number
    let index: number = randomInt(0, kanjiList.length - 1);

    // display a random Kanji
    loadNewKanji(kanjiList[index]);
}