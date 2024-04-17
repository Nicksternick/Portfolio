// ===== | Helper Functions | =====
export const randomInt = (min:number = 0, max:number = 1) => {
    return Math.floor(Math.random() * (max - min + 1) + min)
}