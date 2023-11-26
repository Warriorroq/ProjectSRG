namespace ProjectSRG.LevelGeneration.PlanetGeneration
{
    public static class NoiseFilterFactory
    {
        public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
            => settings.filterType switch
            {
                NoiseSettings.FilterType.ridgid => new RidgidNoiseFilter(settings),
                NoiseSettings.FilterType.simple => new SimpleNoiseFilter(settings),
                _ => null
            };
    }
}
