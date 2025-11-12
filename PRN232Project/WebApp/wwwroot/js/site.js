// Mobile menu toggle
window.toggleMobileMenu = function() {
    const mobileNav = document.querySelector('.mobile-nav');
    mobileNav.classList.toggle('active');
}

// Initialize AOS animation library
window.initAOS = function() {
    if (typeof AOS !== 'undefined') {
        AOS.init({
            duration: 800,
            easing: 'ease-in-out',
            once: true
        });
    }
}

// Document ready
document.addEventListener('DOMContentLoaded', function() {
    // Initialize any components that need to be set up on page load
}); 