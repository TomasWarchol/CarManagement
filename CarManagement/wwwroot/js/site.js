document.addEventListener('DOMContentLoaded', function () {

  
        const navLinks = document.querySelectorAll('.nav-link');
        const currentUrl = window.location.pathname; 
        navLinks.forEach(link => {
            if (link.getAttribute('href') === currentUrl) {
                link.classList.add('active'); 
            }
        });
  


    const scrollTopButton = document.createElement('button');
    scrollTopButton.textContent = '↑';
    scrollTopButton.style.position = 'fixed';
    scrollTopButton.style.bottom = '20px';
    scrollTopButton.style.right = '20px';
    scrollTopButton.style.backgroundColor = '#4CAF50';
    scrollTopButton.style.color = 'white';
    scrollTopButton.style.padding = '10px';
    scrollTopButton.style.border = 'none';
    scrollTopButton.style.borderRadius = '50%';
    scrollTopButton.style.cursor = 'pointer';
    scrollTopButton.style.fontSize = '18px';
    scrollTopButton.style.display = 'none'; // ukryty
    document.body.appendChild(scrollTopButton);


    window.addEventListener('scroll', function () {
        if (window.scrollY > 300) {
            scrollTopButton.style.display = 'block';
        } else {
            scrollTopButton.style.display = 'none';
        }
    });


    scrollTopButton.addEventListener('click', function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });


    const tableRows = document.querySelectorAll('table tr');
    tableRows.forEach(row => {
        row.addEventListener('mouseenter', function () {
            this.style.transition = 'all 0.3s ease-in-out';
            this.style.transform = 'scale(1.02)';
        });

        row.addEventListener('mouseleave', function () {
            this.style.transform = 'scale(1)';
        });
    });
});
