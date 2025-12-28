// Professional Typing Effect
window.startTypingEffect = function(subtitleText, descriptionText) {
    const subtitleElement = document.getElementById('typing-subtitle');
    const descriptionElement = document.getElementById('typing-description');
    const subtitleCursor = document.querySelector('.typing-cursor');
    const descriptionCursor = document.querySelector('.typing-cursor-desc');
    
    if (!subtitleElement || !descriptionElement) return;
    
    // Configuration
    const subtitleSpeed = 80; // milliseconds per character
    const descriptionSpeed = 30; // milliseconds per character
    const pauseBetween = 1000; // pause between subtitle and description
    const initialDelay = 500; // initial delay before starting
    
    // Hide cursors initially
    if (subtitleCursor) subtitleCursor.style.display = 'none';
    if (descriptionCursor) descriptionCursor.style.display = 'none';
    
    // Clear existing content
    subtitleElement.textContent = '';
    descriptionElement.textContent = '';
    
    // Start typing after initial delay
    setTimeout(() => {
        typeSubtitle();
    }, initialDelay);
    
    function typeSubtitle() {
        if (subtitleCursor) subtitleCursor.style.display = 'inline-block';
        
        let i = 0;
        const timer = setInterval(() => {
            if (i < subtitleText.length) {
                subtitleElement.textContent += subtitleText.charAt(i);
                i++;
            } else {
                clearInterval(timer);
                // Hide subtitle cursor and start description after pause
                setTimeout(() => {
                    if (subtitleCursor) subtitleCursor.style.display = 'none';
                    typeDescription();
                }, pauseBetween);
            }
        }, subtitleSpeed);
    }
    
    function typeDescription() {
        if (descriptionCursor) descriptionCursor.style.display = 'inline-block';
        
        let i = 0;
        const timer = setInterval(() => {
            if (i < descriptionText.length) {
                descriptionElement.textContent += descriptionText.charAt(i);
                i++;
                
                // Add natural pauses at punctuation
                if (descriptionText.charAt(i - 1) === '.' || 
                    descriptionText.charAt(i - 1) === ',' || 
                    descriptionText.charAt(i - 1) === ';') {
                    // Pause briefly at punctuation
                    setTimeout(() => {}, 200);
                }
            } else {
                clearInterval(timer);
                // Keep description cursor blinking
                if (descriptionCursor) {
                    descriptionCursor.style.display = 'inline-block';
                }
            }
        }, descriptionSpeed);
    }
};

// Utility function to restart typing effect (for page navigation)
window.restartTypingEffect = function() {
    const subtitleElement = document.getElementById('typing-subtitle');
    const descriptionElement = document.getElementById('typing-description');
    
    if (subtitleElement && descriptionElement) {
        subtitleElement.textContent = '';
        descriptionElement.textContent = '';
        
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