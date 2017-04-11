public class BurnableObjectif : Burnable
{
    private void Awake()
    {
        OnBurnRatioProgress += This_OnBurnRatioProgress;
    }

    private void This_OnBurnRatioProgress(Burnable burnable, float newBurnRatio)
    {
        // TODO : Update graphic state
    }

    private void OnDestroy()
    {
        OnBurnRatioProgress -= This_OnBurnRatioProgress;
    }
}