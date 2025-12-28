// Continuous Typing Effect for Subtitle
window.startTypingEffect = function(subtitleText, descriptionText) {
    console.log('Starting typing effect with:', subtitleText);
    
    const subtitleElement = document.getElementById('typing-subtitle');
    const descriptionElement = document.getElementById('typing-description');
    const subtitleCursor = document.querySelector('.typing-cursor');
    const descriptionCursor = document.querySelector('.typing-cursor-desc');
    
    console.log('Elements found:', {
        subtitle: !!subtitleElement,
        description: !!descriptionElement,
        subtitleCursor: !!subtitleCursor,
        descriptionCursor: !!descriptionCursor
    });
    
    if (!subtitleElement) {
        console.error('Subtitle element not found!');
        return;
    }
    
    // Set description immediately and hide its cursor
    if (descriptionElement) {
        descriptionElement.textContent = descriptionText;
    }
    if (descriptionCursor) {
        descriptionCursor.style.display = 'none';
    }
    
    // Configuration
    const typeSpeed = 100;
    const deleteSpeed = 50;
    const pauseEnd = 2000;
    const pauseStart = 1000;
    
    let i = 0;
    let isDeleting = false;
    
    // Show cursor and start typing
    if (subtitleCursor) {
        subtitleCursor.style.display = 'inline-block';
    }
    
    function type() {
        const current = subtitleText.substring(0, i);
        subtitleElement.textContent = current;
        
        if (!isDeleting && i < subtitleText.length) {
            // Typing
            i++;
            setTimeout(type, typeSpeed);
        } else if (isDeleting && i > 0) {
            // Deleting
            i--;
            setTimeout(type, deleteSpeed);
        } else if (!isDeleting && i === subtitleText.length) {
            // Finished typing, start deleting after pause
            console.log('Finished typing, will delete after pause');
            setTimeout(() => {
                isDeleting = true;
                type();
            }, pauseEnd);
        } else if (isDeleting && i === 0) {
            // Finished deleting, start typing after pause
            console.log('Finished deleting, will type after pause');
            setTimeout(() => {
                isDeleting = false;
                type();
            }, pauseStart);
        }
    }
    
    // Start after initial delay
    console.log('Starting typing in 500ms...');
    setTimeout(type, 500);
};

// Restart function
window.restartTypingEffect = function() {
    const subtitleText = "Software Engineer & Cybersecurity Specialist";
    const descriptionText = "Passionate about cybersecurity analysis and building secure web applications with modern technologies. Specialized in security testing, vulnerability assessment, and .NET development with a focus on creating robust, scalable solutions that protect digital assets and enhance user experiences.";
    
    window.startTypingEffect(subtitleText, descriptionText);
};

// Handle reduced motion
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