// Analytics JavaScript utilities
window.portfolioAnalytics = {
    // Track page visibility changes
    trackVisibility: function() {
        document.addEventListener('visibilitychange', function() {
            if (document.hidden) {
                console.log('Page hidden - could track session pause');
            } else {
                console.log('Page visible - could track session resume');
            }
        });
    },

    // Track scroll depth
    trackScrollDepth: function() {
        let maxScroll = 0;
        window.addEventListener('scroll', function() {
            const scrollPercent = Math.round((window.scrollY / (document.body.scrollHeight - window.innerHeight)) * 100);
            if (scrollPercent > maxScroll) {
                maxScroll = scrollPercent;
                if (maxScroll % 25 === 0) { // Track at 25%, 50%, 75%, 100%
                    console.log(`Scroll depth: ${maxScroll}%`);
                }
            }
        });
    },

    // Initialize analytics
    init: function() {
        this.trackVisibility();
        this.trackScrollDepth();
        console.log('Portfolio analytics initialized');
    }
};

// Auto-initialize when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function() {
        window.portfolioAnalytics.init();
    });
} else {
    window.portfolioAnalytics.init();
}