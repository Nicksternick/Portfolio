// Utility Functions that are modular, and can be used in other projects

// ===== | Written Functions | =====

// ===== | Given Utility Functions | =====

/**
 * @Parameters red, green, blue, and alpha (alpha has default of 1)
 * @returns Formmated string `rgba(${red},${green},${blue},${alpha})`
 */
export const makeColor = (red, green, blue, alpha = 1) => {
    return `rgba(${red},${green},${blue},${alpha})`;
};

/**
 * @Parameters min, max
 * @returns Random number between the min and the max value
 */
export const getRandom = (min, max) => {
    return Math.random() * (max - min) + min;
};

/**
 * @returns a random formatted color rgba string
 */
export const getRandomColor = () => {
    const floor = 35; // so that colors are not too bright or too dark 
    const getByte = () => getRandom(floor, 255 - floor);
    return `rgba(${getByte()},${getByte()},${getByte()},1)`;
};

export const getLinearGradient = (ctx, startX, startY, endX, endY, colorStops) => {
    let lg = ctx.createLinearGradient(startX, startY, endX, endY);
    for (let stop of colorStops) {
        lg.addColorStop(stop.percent, stop.color);
    }
    return lg;
};

