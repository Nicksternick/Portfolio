// ===== | Variables | =====

// ===== | Classes | =====
export class CircleVisualizer {
    constructor(canvasWidth, canvasHeight, color = "white", lineColor = "white", lineWidth = 1) {
        Object.assign(this, { canvasWidth, canvasHeight, color, lineColor, lineWidth });
        this.pointList = [];
        this.circleDivisions = 0;
        this.minData = 0;
        this.maxData = 0;
        Object.seal(this);
    }

    setFillColor(value) {
        this.color = value;
    }

    setLineColor(value) {
        this.lineColor = value;
    }

    setMinData(value) {
        this.minData = value;
    }

    setMaxData(value) {
        this.maxData = 256 - value;
    }

    setLineWidth(value) {
        this.lineWidth = value;
    }

    update(audioData) {
        // Clear the points list
        this.pointList = [];

        // each point in the point list represents a radius at each sub division

        // This for loop is effected by the minData and maxData value
        for (let i = this.minData; i <= audioData.length - this.maxData; i++) {
            this.pointList.push(25 + audioData[i]);
        }

        // Calculate the number of subdivisions, equal to how many points were made
        this.circleDivisions = (Math.PI * 2) / (this.pointList.length);
    }

    draw(ctx) {
        // get the middle of the canvas
        let midX = this.canvasWidth / 2;
        let midY = this.canvasHeight / 3;

        // save the current context
        ctx.save();

        // apply to the styles as dictated by the class
        ctx.lineWidth = this.lineWidth;
        ctx.fillStyle = this.color;
        ctx.strokeStyle = this.lineColor;

        // begin drawing
        ctx.beginPath();

        // setup the rect postion and size
        for (let i = 0; i <= this.pointList.length; i++) {
            // Calculate point x and y on the circle
            let x = Math.cos(this.circleDivisions * i) * this.pointList[i];
            let y = Math.sin(this.circleDivisions * i) * this.pointList[i];

            // draw a line to it
            ctx.lineTo((x / 2) + midX, (y / 2) + midY);
        }

        // draw a final line to connect the start and end of the circle
        ctx.lineTo((Math.cos(0) * this.pointList[0] / 2) + midX, (Math.sin(0) * this.pointList[0] / 2) + midY);

        // color the circle
        ctx.fill();

        // line the circle
        ctx.stroke();

        ctx.closePath();
        ctx.restore();
    }
}

export class LineVisualizer {
    constructor(canvasWidth, canvasHeight, color = "white", lineColor = "white", lineWidth = 1) {
        Object.assign(this, { canvasWidth, canvasHeight, color, lineColor, lineWidth });
        this.pointList = [];
        this.height = canvasHeight / 2;
        this.minData = 0;
        this.maxData = 0;
        Object.seal(this);
    }

    setFillColor(value) {
        this.color = value;
    }

    setLineColor(value) {
        this.lineColor = value;
    }

    setMinData(value) {
        this.minData = value;
    }

    setMaxData(value) {
        this.maxData = 256 - value;
    }

    setLineWidth(value) {
        this.lineWidth = value;
    }

    update(audioData) {
        // Create a number of divisions along the width of the canvas
        let width = this.canvasWidth / (audioData.length - this.maxData);

        // clear the point list
        this.pointList = [];

        for (let i = this.minData; i <= audioData.length; i++) {

            // calculate the x & y of the given point
            let finalX = (i - this.minData) * width;
            let finalY = this.height + 256 - audioData[i];

            // clamp Y to Canvas Height is it goes over
            if (finalY >= this.canvasHeight) {
                finalY = this.canvasHeight;
            }

            // Put X & Y into an array
            let xy = [finalX, finalY];

            if (i == audioData.length - 1) {
                xy[0] = this.canvasWidth;
            }

            // push the point to the array
            this.pointList.push(xy);
        }
    }

    draw(ctx) {
        ctx.save();

        // set up the colors
        ctx.lineWidth = this.lineWidth;
        ctx.fillStyle = this.color;
        ctx.strokeStyle = this.lineColor;

        // begin drawing
        ctx.beginPath();

        ctx.moveTo(this.pointList[0][0], this.pointList[0][1]);

        // setup the rect postion and size
        for (let i = 1; i < this.pointList.length; i++) {
            ctx.lineTo(this.pointList[i][0], this.pointList[i][1]);
        }

        // Complete the box, so that the fill shows up correctly
        ctx.lineTo(this.canvasWidth, this.canvasHeight);
        ctx.lineTo(0, this.canvasHeight);
        ctx.lineTo(this.pointList[0][0], this.pointList[0][1]);

        // color the rect
        ctx.fill();

        ctx.stroke();

        ctx.closePath();
        ctx.restore();
    }
}