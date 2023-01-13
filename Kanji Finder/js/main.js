    // ===== | Top | =====

    // ===== | Variables | =====

    window.onload = startUp;

    // ----- | KanjiAPI | -----

    // Link to the website used: https://kanjiapi.dev/#!/

    const KanjiAPI_URL = 'https://kanjiapi.dev/v1/';

    // Kanji list has default content to generate the initial kanji
    let kanjiLists = ["一","右","雨","円","王","音","下","火","花","貝","学","気","休","玉","金","九","空","月","犬","見","五","口","校","左","三","山","四","子","糸","字","耳","七","車","手","十","出","女","小","上","森","人","水","正","生","青","石","赤","先","千","川","早","草","足","村","大","男","竹","中","虫","町","天","田","土","二","日","入","年","白","八","百","文","本","名","木","目","夕","立","力","林","六"];
    
    // Holds the information of the kanji object
    let kanjiObject;

    let prevKanji;
    let prevKanjiText;

    // This object holds the save Kanji to show on the page
    let kanjiLibrary = {};

    // Create a key to use for sending the library to local storage
    const libraryKey = "nsd3407-library";
    const storedLibrary = localStorage.getItem(libraryKey);

    // ===== | Functions | =====
    
    // ----- | Misc Section | -----
    
    // startUp(): Called during windows onLoad, attaches events to all of the buttons and search info
    function startUp()
    {
        document.querySelector("#searchButton").onclick = searchKanji;
        document.querySelector("#KanjiButton").onclick = generateKanji;
        document.querySelector("#KanjiSelection").onchange = getKanjiLists;
        document.querySelector("#KanjiSave").onclick = saveKanji;
        getKanjiLists();
        generateKanji();

        if (storedLibrary)
        {
            kanjiLibrary = JSON.parse(storedLibrary);
            generateLibrary();
        }
    }

    function generateLibrary()
    {
        // Get the library from the HTML and create the header
        let library = document.querySelector("#ListArea");
        library.innerHTML = "";

        // Create a new ordered list element
        let libraryList = document.createElement('ul');

        // For every kanji in the kanjiLibrary, add it to the list
        for (const kanji in kanjiLibrary)
        {
            let li = document.createElement('li');
            li.innerHTML = kanji;
            li.onclick = selectkanji;
            libraryList.appendChild(li);
        }

        // add the list to the library in the HTML
        library.appendChild(libraryList);

        // Search for the add and remove button and attach the respective event
        let search = document.querySelector("#kanjiLibrary #search");
        search.onclick = searchLibraryKanji;

        let remove = document.querySelector("#kanjiLibrary #remove");
        remove.onclick = removeLibraryKanji;
    }

    // ----- | KanjiAPI Query Section | -----

    // getKanjiLists(): Gets the list of the kanji based 
    function getKanjiLists()
    {
        // Get the value of the Kanji selection in the HTML
        let kanjiCategory = document.querySelector("#KanjiSelection").value;

        // Create the url to get the list of Kanji
        let url = KanjiAPI_URL + "kanji/" + kanjiCategory;
        
        let xhrList = new XMLHttpRequest();

        xhrList.onload = (e) =>
        {
            let xhrList = e.target;

            kanjiLists = JSON.parse(xhrList.responseText);
        }

        xhrList.onerror = dataError;

        xhrList.open("GET", url);
        xhrList.send();
    }

    // saveKanji(): Saves the current Kanji on screen to a library
    function saveKanji()
    {
        //adds a Kanji symbol as the value with a key of the same symbol
        kanjiLibrary[kanjiObject.kanji] = kanjiObject.kanji;

        // Get the library from the HTML and create the header
        let library = document.querySelector("#ListArea");
        library.innerHTML = "";

        // Create a new ordered list element
        let libraryList = document.createElement('ul');

        // For every kanji in the kanjiLibrary, add it to the list
        for (const kanji in kanjiLibrary)
        {
            let li = document.createElement('li');
            li.innerHTML = kanji;
            li.onclick = selectkanji;
            libraryList.appendChild(li);
        }

        // add the list to the library in the HTML
        library.appendChild(libraryList);

        // Search for the add and remove button and attach the respective event
        let search = document.querySelector("#kanjiLibrary #search");
        search.onclick = searchLibraryKanji;

        let remove = document.querySelector("#kanjiLibrary #remove");
        remove.onclick = removeLibraryKanji;

        localStorage.setItem(libraryKey, JSON.stringify(kanjiLibrary));
    }

    // selectKanji(): gets the Kanji that is click and makes to the selected Kanji for the library
    function selectkanji(e)
    {
        let kanji = e.target.innerHTML;
        let selection = document.querySelector("#Selection");

        selection.innerHTML = kanji;
    }

    // removeLibraryKanji(): remove a list item from the Kanji List by index
    function removeLibraryKanji()
    {
        // get the value of the input (subtracted by one to account for the index)
        let kanji = document.querySelector("#Selection").innerHTML;

        // if the number is in range, remove the list item from the list
        delete kanjiLibrary[kanji];

        localStorage.setItem(libraryKey, JSON.stringify(kanjiLibrary));

        generateLibrary();
    }

    // searchLibraryKanji(): search a list item from the Kanji List by the index
    function searchLibraryKanji()
    {
        // get the value of the input (subtracted by one to account for the index)
        let kanji = document.querySelector("#Selection").innerHTML;

        // if the number is in range, call getKanjiObect with the kanji symbol as the parameter
        getKanjiObject(kanji);
    }

    // searchKanji(): searches Kanji based on user input
    function searchKanji()
    {
        // Make sure to set the search term back
        let searchHead = document.querySelector("#SearchDiv")
        searchHead.innerHTML = "Input a single Kanji to get it's info!";
        
        // gets value from the input section and call getKanjiObject with said parameter
        let kanji = document.querySelector("#KanjiInput").value;
        getKanjiObject(kanji);            
    }


    // generateKanji(): gets a random kanji to show on screen
    function generateKanji()
    {
        // generates a random Kanji based on the list and calls getKanjiObect with said parameter
        let kanji = kanjiLists[Math.floor(Math.random() * kanjiLists.length)];
        getKanjiObject(kanji);
    }

    // getKanjiObject(): gets the object for a specific Kanji and displays it's info on screen
    function getKanjiObject(kanji)
    {        
        // create the URL
        let url = KanjiAPI_URL + "kanji/" + kanji;
        
        let xhrKanji = new XMLHttpRequest();

        xhrKanji.onload = (e) =>
        {
            // set xhrKanji to the string returned by send()
            let xhrKanji = e.target;

            // set the kanjiObject with the parsed information
            kanjiObject = JSON.parse(xhrKanji.responseText);

            let kanjiSymbol = document.querySelector("#kanjiSymbol");
            prevKanji = kanjiSymbol.innerHTML;
            kanjiSymbol.innerHTML = `<div class='kanjiSymbol'>${kanjiObject.kanji}</div>`;

            // set kanjiText to the text block
            let kanjiText = document.querySelector("#KanjiText");
            prevKanjiText = kanjiText.innerHTML;
            kanjiText.innerHTML = "";
            
            // create a new div in kanjiList
            let kanjiList = document.createElement("div")
            
            // for each data in the object
            for (const data in kanjiObject)
            {
                // if the data is the data we care about, format it into the HTML
                switch (data)
                {
                    case "stroke_count":
                        kanjiList = `<div class='kanjiProperties'>Stroke Count: ${kanjiObject.stroke_count}</div>`;
                    break;

                    case "meanings":
                        let meaningArray = kanjiObject.meanings;

                        if (meaningArray.length != 0)
                        {
                            kanjiList += `<div class='kanjiProperties'> Meanings: `;
                            for (const meaning of meaningArray)
                            {
                                kanjiList += `"${meaning}", `;
                            }
                            kanjiList += '</div>';                            
                        }
                        else
                        {
                            kanjiList += `<div class='kanjiProperties'> Meanings: None</div> `;
                        }
                    break;

                    case "kun_readings":
                        let kunyomiArray = kanjiObject.kun_readings;

                        if (kunyomiArray.length != 0)
                        {
                            kanjiList += `<div class='kanjiProperties'> Kunyomi: `;
                            for (const kunyomi of kunyomiArray)
                            {
                                kanjiList += `"${kunyomi}", `;
                            }
                            kanjiList += '</div>';                            
                        }
                        else
                        {
                            kanjiList += `<div class='kanjiProperties'> Kunyomi: None</div> `;
                        }
                    break;

                    case "on_readings":
                        let onyomiArray = kanjiObject.on_readings;

                        if (onyomiArray.length != 0)
                        {
                            kanjiList += `<div class='kanjiProperties'> Onyomi: `;
                            for (const onyomi of onyomiArray)
                            {
                                kanjiList += `"${onyomi}", `;
                            }
                            kanjiList += '</div>';                            
                        }
                        else
                        {
                            kanjiList += `<div class='kanjiProperties'> Onyomi: None</div> `;
                        }
                    break;
                }
            }

            kanjiText.innerHTML += kanjiList;
        }

        // check to see if the link was not right, and returned a 404 error
        xhrKanji.onloadend = (e) =>
        {
            let kanjiText = document.querySelector("#KanjiText");

            // if there was a 404 error, print an error message and do not change the Kanji 
            // (The code remeber to previous Kanji and just reprints it)
            if (xhrKanji.status == 404)
            {
                let kanjiSymbol = document.querySelector("#kanjiSymbol");
                kanjiSymbol.innerHTML = prevKanji;

                let kanjiText = document.querySelector("#KanjiText");
                kanjiText.innerHTML = prevKanjiText;

                let searchHead = document.querySelector("#SearchDiv")
                searchHead.innerHTML = "Invalid, please input a single Kanji!";
            }
        }

        xhrKanji.onerror = dataError;
        xhrKanji.open("GET", url);
        xhrKanji.send();
    }

    function dataError(e)
    {
        console.log("error");
    }