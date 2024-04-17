// ===== | Variables | =====

import * as utils from './utils.js';
import * as visualizer from './visualizerUtils.js';

let ctx, canvasWidth, canvasHeight, gradient, analyserNode, audioData;

let lineVisualizer;
let circleVisualizer;

let basefill;
let baseStroke;

let gradientTimer = 0;

// ===== | Methods | =====

const setupCanvas = (canvasElement, analyserNodeRef, defaultFill, defaultStroke) => {
    // create drawing context
    ctx = canvasElement.getContext("2d");
    canvasWidth = canvasElement.width;
    canvasHeight = canvasElement.height;
    // keep a reference to the analyser node
    analyserNode = analyserNodeRef;
    // this is the array where the analyser data will be stored
    audioData = new Uint8Array(analyserNode.fftSize / 2);

    // set base fill and stroke to the defaults passed in
    basefill = defaultFill;
    baseStroke = defaultStroke;

    // Set up the visualizers
    setupVisualizers();
}

const setupVisualizers = () => {
    // Declare the two visualizer classes
    lineVisualizer = new visualizer.LineVisualizer(canvasWidth,
        canvasHeight, basefill, baseStroke, 1);
    circleVisualizer = new visualizer.CircleVisualizer(canvasWidth,
        canvasHeight, basefill, baseStroke, 1);
}

const draw = (params = {}) => {
    // 1 - populate the audioData array with the frequency data from the analyserNode
    // notice these arrays are passed "by reference" 

    // Depending on the boolean, use Frequency or Wave data
    if (params.useFrequency) {
        analyserNode.getByteFrequencyData(audioData);
    }
    else {
        analyserNode.getByteTimeDomainData(audioData);
    }

    // 2 - draw background
    ctx.save();
    ctx.fillStyle = "black";
    ctx.globalAlpha = .1;
    ctx.fillRect(0, 0, canvasWidth, canvasHeight);
    ctx.restore();

    // ===== | ===== | ===== | =====

    // 3 - draw gradient
    if (params.showGradient) {
        // create a gradient that runs top to bottom

        // Initalize these variables
        let step = 1 // how many gradient steps there will be
        let gradientStop = 100 / step; // how many stops will be added to the array
        let stops = []; // The gradient stop array

        for (let i = 0; i < gradientStop; i++) {
            // Calculate percentage
            let percentage = (step * i) / 100;
            //calculate the audio at the current percent
            let readAudio = audioData[Math.round((audioData.length - 1) * percentage)];
            // create a hsl value
            let value = `hsl(${(readAudio + gradientTimer) % 360}, 30%, 70%)`;
            // push the new gradient stop
            stops.push({ percent: percentage, color: value });
        }

        // Increment gradient timer (This is how I get the color wave effect)
        gradientTimer++;

        //Set the gradient
        gradient = utils.getLinearGradient(ctx, 0, canvasHeight, 0, 0, stops);

        // apply the gradient
        effectGradient();
    }

    // 4 - draw bars
    if (params.showBars) {
        lineVisualizer.update(audioData);
        lineVisualizer.draw(ctx);
    }

    // 5 - draw circles
    if (params.showCircle) {
        circleVisualizer.update(audioData);
        circleVisualizer.draw(ctx);
    }
}

//#region Effect Methods
// ----- | Effect methods | -----
const effectGradient = () => {
    ctx.save();
    ctx.fillStyle = gradient;
    ctx.globalAlpha = .3;
    ctx.fillRect(0, 0, canvasWidth, canvasHeight);
    ctx.restore();
}
//#endregion

export { setupCanvas, draw, circleVisualizer, lineVisualizer };