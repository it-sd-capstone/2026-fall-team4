document.addEventListener('DOMContentLoaded', () => {
    const toggleBtn = document.getElementById('theme-toggle');
    const themeIcon = document.getElementById('theme-icon');
    const htmlElement = document.documentElement;

    if (!toggleBtn || !themeIcon) return;

    const updateIcon = (theme) => {
        themeIcon.textContent = theme === 'light' ? '🌙' : '☀️';
    };

    const currentTheme = htmlElement.getAttribute('data-theme') || 'dark';
    updateIcon(currentTheme);

    toggleBtn.addEventListener('click', () => {
        const activeTheme = htmlElement.getAttribute('data-theme');
        const nextTheme = activeTheme === 'light' ? 'dark' : 'light';

        htmlElement.setAttribute('data-theme', nextTheme);
        localStorage.setItem('etf-theme', nextTheme);
        updateIcon(nextTheme);
    });
});