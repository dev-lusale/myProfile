// Utility functions for the portfolio

// Function to download a file
window.downloadFile = (fileUrl, fileName) => {
    const link = document.createElement('a');
    link.href = fileUrl;
    link.download = fileName;
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

// Function to open URL in new tab
window.openInNewTab = (url) => {
    window.open(url, '_blank');
};