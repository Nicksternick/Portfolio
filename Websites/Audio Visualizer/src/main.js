// ===== | Variables | =====

import * as utils from './utils.js';
import * as audio from './audio.js';
import * as visualizer from './visualizer.js';

const drawParams = {
    showGradient: true,
    showBars: true,
    showCircle: true,
    useFrequency: true
}

// 1 - here we are faking an enumeration
const DEFAULTS = Object.freeze({
    sound1: "media/New Adventure Theme.mp3"
});

//#region HTML UI References
// ----- | HTML UI References | -----
// Play button: Responsible for starting the audio
const playButton = document.querySelector("#btn-play");

// Pause button: Responsible for stopping the audio
const pauseButton = document.querySelector("#btn-pause");

// ListPlayButton & FilePlayButton: Used to choose 
// whether you want to play from a file or play from the list
const listPlayButton = document.querySelector("#btn-list-play");
const filePlayButton = document.querySelector("#btn-file-play");

// Volume slider & label: Responsible for controlling the volume
// (Gain Node) of the audio
const volumeSlider = document.querySelector("#slider-volume");
const volumeLabel = document.querySelector("#label-volume");

// High & Low Shelf Slider: Responsible for controlling the trebel and base node
const highshelfSlider = document.querySelector("#slider-highshelf");
const lowshelfSlider = document.querySelector("#slider-lowshelf");

// Track Select: Responsible for holding the selected track
const trackSelect = document.querySelector("#select-song");

// file input: Responsible for holding the users inputed file
const fileSongInput = document.querySelector("#input-song");

// Frequency, Line, Circle, Gradient Checkbox: Resposible for
// keeping track of what visualizers to draw
const checkboxFrequency = document.querySelector("#cb-frequency");
const checkboxLine = document.querySelector("#cb-line");
const checkboxCircle = document.querySelector("#cb-circle");
const checkboxGradient = document.querySelector("#cb-gradient");

// THIS SECTION HOLDS ALL OF THE LINE MODIFIER CODE, AND THEIR LABELS
const lineMinSlider = document.querySelector("#select-line-min");
const lineMinLabel = document.querySelector("#label-line-min");

const lineMaxSlider = document.querySelector("#select-line-max");
const lineMaxLabel = document.querySelector("#label-line-max");

const lineWidthSlider = document.querySelector("#select-line-width");
const lineWidthLabel = document.querySelector("#label-line-width");

const lineFillSelect = document.querySelector("#select-line-fill");
const lineStrokeSelect = document.querySelector("#select-line-stroke");

// ====================================================================

// THIS SECTION HOLDS ALL OF THE CIRCLE MODIFIER CODE, AND THEIR LABELS
const circleMinSlider = document.querySelector("#select-circle-min");
const circleMinLabel = document.querySelector("#label-circle-min");

const circleMaxSlider = document.querySelector("#select-circle-max");
const circleMaxLabel = document.querySelector("#label-circle-max");

const circleWidthSlider = document.querySelector("#select-circle-width");
const circleWidthLabel = document.querySelector("#label-circle-width");

const circleFillSelect = document.querySelector("#select-circle-fill");
const circleStrokeSelect = document.querySelector("#select-circle-stroke");
// ====================================================================
//#endregion


// ===== | Methods | =====

const init = (defaultFill, defaultStroke) => {
    // Setup the audio node
    audio.setupWebaudio(DEFAULTS.sound1);

    // get a reference to the canvas element
    let canvasElement = document.querySelector("canvas"); // hookup <canvas> element

    // setup the UI
    setupUI();

    // Set the audio file the currently selected track
    audio.loadSoundFile(trackSelect.value);
    
    // setup the canvas in the visualizer
    visualizer.setupCanvas(canvasElement, audio.analyserNode, defaultFill, defaultStroke);

    // Begin the loop
    loop();
}

const setupUI = () => {
    // set up the buttons
    setupButtons();

    // set up the sliders
    setupSliders();

    // set up selectors
    setupSelect();

    // set up checkboxes
    setupCheckboxes();
    
} // end setupUI

const loop = () => {
    /* NOTE: This is temporary testing code that we will delete in Part II */
    setTimeout(loop);

    // draw the visualizer
    visualizer.draw(drawParams);
}

// ----- | Setup Functions For Init To Organize It Better | -----

const setupCheckboxes = () => {
    
    // FOR ALL OF THESE 4 EVENTS:
    // Toggling them will set the related param boolean 
    // to true or false depending on the checked attribute

    checkboxFrequency.onclick = () => {
        drawParams.useFrequency = checkboxFrequency.checked;
    }

    checkboxLine.onclick = () => {
        drawParams.showBars = checkboxLine.checked;
    }

    checkboxCircle.onclick = () => {
        drawParams.showCircle = checkboxCircle.checked;
    }

    checkboxGradient.onclick = () => {
        drawParams.showGradient = checkboxGradient.checked;
    }
}

const setupButtons = () => {
    // Get references to the buttons, then setup their on click event

    // Create an event when the play button is clicked
    playButton.onclick = e => {
        // check if the contet i in suspended space (autoplay policy)
        if (audio.audioCtx.state == "suspended") {
            audio.audioCtx.resume();
        }

        // play the audio
        audio.playCurrentSound();
    };

    // Create an event when the pause button is clicked
    pauseButton.onclick = e => {
        // pause the audio
        audio.pauseCurrentSound();
    }

    listPlayButton.onclick = e => {
        // load in the audio
        audio.loadSoundFile(trackSelect.value);
        // click the play button
        playButton.dispatchEvent(new MouseEvent("click"));
    }

    filePlayButton.onclick = e => {
        // split by file by the ., to find the file extension
        let words = fileSongInput.value.split('.');

        // If the last value in the array (The file extension) is an mp3
        if (words[words.length - 1] == "mp3") {
            // Aquire that file
            const file = fileSongInput.files[0];
            // load that file, making sure to turn it into a URL
            audio.loadSoundFile(URL.createObjectURL(file));
            // click the play button
            playButton.dispatchEvent(new MouseEvent("click"));
        }
    }
}

const setupSelect = () => {
    // ALL OF THESE EVENTS WORK THE SAME
    // when the select value changes, set the fill or stroke appropitally based on the value

    lineFillSelect.onchange = () => {
        visualizer.lineVisualizer.setFillColor(lineFillSelect.value);
    }

    lineStrokeSelect.onchange = () => {
        visualizer.lineVisualizer.setLineColor(lineStrokeSelect.value);
    }

    circleFillSelect.onchange = () => {
        visualizer.circleVisualizer.setFillColor(circleFillSelect.value);
    }

    circleStrokeSelect.onchange = () => {
        visualizer.circleVisualizer.setLineColor(circleStrokeSelect.value);
    }
}

const setupSliders = () => {
    // ALL OF THESE EVENTS WORK THE SAME
    // When the range changes, set the new value based on the new range value

    lineMinSlider.oninput = e => {
        visualizer.lineVisualizer.setMinData(e.target.value);
        lineMinLabel.innerHTML = `Min Analyzed Data: ${e.target.value}`;
        
    }

    lineMaxSlider.oninput = e => {
        visualizer.lineVisualizer.setMaxData(e.target.value);
        lineMaxLabel.innerHTML = `Max Analyzed Data: ${e.target.value}`;
    }

    lineWidthSlider.oninput = e => {
        visualizer.lineVisualizer.setLineWidth(e.target.value);
        lineWidthLabel.innerHTML = `Line Width: ${e.target.value}`;
    }

    circleMinSlider.oninput = e => {
        visualizer.circleVisualizer.setMinData(e.target.value);
        circleMinLabel.innerHTML = `Min Analyzed Data: ${e.target.value}`;
    }

    circleMaxSlider.oninput = e => {
        visualizer.circleVisualizer.setMaxData(e.target.value);
        circleMaxLabel.innerHTML = `Max Analyzed Data: ${e.target.value}`;
    }

    circleWidthSlider.oninput = e => {
        visualizer.circleVisualizer.setLineWidth(e.target.value);
        circleWidthLabel.innerHTML = `Line Width: ${e.target.value}`;
    }

    volumeSlider.oninput = e => {
        // Set the gain
        audio.setVolume(e.target.value);

        // update value of label to math value of slider
        volumeLabel.innerHTML = Math.round((e.target.value / 2 * 100));
    };

    // set value of label to match inital value of slider
    volumeSlider.dispatchEvent(new Event("input"));

    highshelfSlider.oninput = e => {
        audio.setHighshelf(e.target.value);
    }

    lowshelfSlider.oninput = e => {
        audio.setLowshelf(e.target.value);
    }
}

export { init };