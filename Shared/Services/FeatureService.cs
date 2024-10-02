using Microsoft.FeatureManagement;

namespace Shared.Services;

public class FeatureService
{
    private readonly IFeatureManager _featureManager;

    public FeatureService(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    public async Task<bool> IsFeatureEnabledAsync(string featureName)
    {
        return await _featureManager.IsEnabledAsync(featureName);
    }
}