using FeatureFlipping.Infrastructure.Database;
using FeatureFlipping.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FeatureFlipping.Infrastructure.Services;

public class FeatureFlagObserver : IFeatureFlagObserver
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<FeatureFlagObserver> _logger;
    private readonly TimeSpan _pollingInterval;
    private bool _lastKnownState;

    public FeatureFlagObserver(
        IServiceScopeFactory scopeFactory, 
        ILogger<FeatureFlagObserver> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _pollingInterval = TimeSpan.FromSeconds(30);
        _lastKnownState = false;
    }

    public async Task MonitorFeatureFlagAsync(string featureName, Func<bool, Task> onChanged)
    {
        _logger.LogInformation($"FeatureFlagObserver started monitoring {featureName}");

        while (true)
        {
            try
            {
                // Crée un scope manuel pour accéder aux services Scoped
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var featureService = scope.ServiceProvider.GetRequiredService<FeatureService>();
                    
                    var featureFlag = await featureService.IsFeatureEnabledAsync(featureName);

                    if (featureFlag != _lastKnownState)
                    {
                        _lastKnownState = featureFlag;
                        _logger.LogInformation($"Feature {featureName} changed to: {featureFlag}");

                        // Appeler le callback pour réagir au changement
                        await onChanged(featureFlag);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error monitoring feature flag {featureName}: {ex.Message}");
            }

            await Task.Delay(_pollingInterval);
        }
    }
}