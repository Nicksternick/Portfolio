// Mobile Menu
const burgerIcon = document.querySelector("#burger") as HTMLDivElement;
const navbarMenu = document.querySelector("#nav-links") as HTMLDivElement;

// Add functionality to the taskbar
burgerIcon.addEventListener('click', () => {
    navbarMenu.classList.toggle('is-active');
});