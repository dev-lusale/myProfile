// Professional Typing Effect with Continuous Loop
window.startTypingEffect = function(subtitleText, descriptionText) {
    const subtitleElement = document.getElementById('typing-subtitle');
    const descriptionElement = document.getElementById('typing-description');
    const subtitleCursor = document.querySelector('.typing-cursor');
    const descriptionCursor = document.querySelector('.typing-cursor-desc');
    
    if (!subtitleElement || !descriptionElement) return;
    
    // Configuration for realistic typing
    const typeSpeed = 120; // milliseconds per character (readable speed)
    const deleteSpeed = 80; // milliseconds per character when deleting
    const pauseAfterComplete = 2000; // pause after typing complete text
    const pauseBeforeDelete = 1500; // pause before starting to delete
    const initialDelay = 800; // initial delay before starting
    
    // Hide cursors initially
    if (subtitleCursor) subtitleCursor.style.display = 'none';
    if (descriptionCursor) descriptionCursor.style.display = 'none';
    
    // Clear existing content
    subtitleElement.textContent = '';
    descriptionElement.textContent = descriptionText; // Set description immediately
    
    let isTyping = false;
    let currentIndex = 0;
    let isDeleting = false;
    
    // Start the continuous loop after initial delay
    setTimeout(() => {
        startContinuousLoop();
    }, initialDelay);
    
    function startContinuousLoop() {
        if (subtitleCursor) subtitleCursor.style.display = 'inline-block';
        typewriterLoop();
    }
    
    function typewriterLoop() {
        const currentText = subtitleElement.textContent;
        
        if (!isDeleting) {
            // Typing phase
            if (currentIndex < subtitleText.length) {
                subtitleElement.textContent = subtitleText.substring(0, currentIndex + 1);
                currentIndex++;
                setTimeout(typewriterLoop, typeSpeed + Math.random() * 50); // Add slight variation for realism
            } else {
                // Finished typing, pause before deleting
                setTimeout(() => {
                    isDeleting = true;
                    typewriterLoop();
                }, pauseAfterComplete);
            }
        } else {
            // Deleting phase
            if (currentIndex > 0) {
                subtitleElement.textContent = subtitleText.substring(0, currentIndex - 1);
                currentIndex--;
                setTimeout(typewriterLoop, deleteSpeed + Math.random() * 30); // Faster deletion with variation
            } else {
                // Finished deleting, pause before typing again
                isDeleting = false;
                setTimeout(typewriterLoop, pauseBeforeDelete);
            }
        }
    }
};

// Utility function to restart typing effect (for page navigation)
window.restartTypingEffect = function() {
    const subtitleElement = document.getElementById('typing-subtitle');
    const descriptionElement = document.getElementById('typing-description');
    
    if (subtitleElement && descriptionElement) {
        subtitleElement.textContent = '';
        
        const subtitleText = "Software Engineer & Cybersecurity Specialist";
        const descriptionText = "Passionate about cybersecurity analysis and building secure web applications with modern technologies. Specialized in security testing, vulnerability assessment, and .NET development with a focus on creating robust, scalable solutions that protect digital assets and enhance user experiences.";
        
        window.startTypingEffect(subtitleText, descriptionText);
    }
};

// Handle reduced motion preference
if (window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
    window.startTypingEffect = function(subtitleText, descriptionText) {
        const subtitleElement = document.getElementById('typing-subtitle');
        const descriptionElement = document.getElementById('typing-description');
        const subtitleCursor = document.querySelector('.typing-cursor');
        const descriptionCursor = document.querySelector('.typing-cursor-desc');
        
        if (subtitleElement) subtitleElement.textContent = subtitleText;
        if (descriptionElement) descriptionElement.textContent = descriptionText;
        if (subtitleCursor) subtitleCursor.style.display = 'none';
        if (descriptionCursor) descriptionCursor.style.display = 'none';
    };
}