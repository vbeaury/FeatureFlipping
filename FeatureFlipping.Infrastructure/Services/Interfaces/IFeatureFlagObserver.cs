namespace FeatureFlipping.Infrastructure.Services.Interfaces;

public interface IFeatureFlagObserver
{
    Task MonitorFeatureFlagAsync(string featureName, Func<bool, Task> onChanged);
}