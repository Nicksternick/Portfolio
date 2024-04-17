// ===== | Variables | =====
/**The Web Audio Context*/
let audioCtx

/** the audio element instantiated in code */
let audioElement

let sourceNode;
/** The nodes used to analyize the audio data */
let analyserNode;
/** The nodes used to effect the audio sound */
let gainNode;

let highshelfNode;
let lowshelfNode;

const DEFAULTS = Object.freeze({
    gain: .5,
    numSamples: 256
});

// ===== | Methods | =====

/** 
* Sets up the audio nodes to work on a new audio file
* @param filePath the path to the audio file
*/
export const setupWebaudio = (filePath) => {
    // Create a new audioContext, and have a fallback incase window.AudioContex is not supported
    const AudioContext = window.AudioContext || window.webkitAudioContext;

    // Fill the audioCtx with this new context
    audioCtx = new AudioContext();

    // fill the audio element with a new audio tag
    audioElement = new Audio();

    // load the audio element with the selected file
    loadSoundFile(filePath);

    // create an a source node that points at the <audio> element
    sourceNode = audioCtx.createMediaElementSource(audioElement);

    // create an analyser node
    analyserNode = audioCtx.createAnalyser(); // note the UK spelling of "Analyser"

    // fft stands for Fast Fourier Transform
    analyserNode.fftSize = DEFAULTS.numSamples;

    // create a gain (volume) node
    gainNode = audioCtx.createGain();
    gainNode.gain.value = DEFAULTS.gain;

    // create highshelf and lowshelf nodes
    highshelfNode = audioCtx.createBiquadFilter();
    highshelfNode.type = "highshelf";
    highshelfNode.frequency.setValueAtTime(1000, audioCtx.currentTime);

    lowshelfNode = audioCtx.createBiquadFilter();
    lowshelfNode.type = "lowshelf";
    lowshelfNode.frequency.setValueAtTime(1000, audioCtx.currentTime);

    // connect the nodes - we now have an audio graph
    sourceNode.connect(highshelfNode);
    highshelfNode.connect(lowshelfNode);
    lowshelfNode.connect(analyserNode);
    analyserNode.connect(gainNode);
    gainNode.connect(audioCtx.destination);
}

export const loadSoundFile = (filePath) => {
    audioElement.src = filePath;
}

export const playCurrentSound = () => {
    audioElement.play();
}

export const pauseCurrentSound = () => {
    audioElement.pause();
}

export const setVolume = (value) => {
    value = Number(value); // make sure that it's a Number rather than a String
    gainNode.gain.value = value;
}

export const setHighshelf = (value) => {
    
    highshelfNode.gain.setValueAtTime(value, audioCtx.currentTime);
}

export const setLowshelf = (value) => {
    
    lowshelfNode.gain.setValueAtTime(value, audioCtx.currentTime);
}

export { audioCtx, analyserNode};