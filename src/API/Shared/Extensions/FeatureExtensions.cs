namespace API.Shared.Extensions
{
    public interface IFeature
    {
        IServiceCollection RegisterFeature(IServiceCollection services);
    }

    public static class FeatureExtensions
    {
        // this could also be added into the DI container
        private static readonly List<IFeature> RegisteredFeatures = new List<IFeature>();

        public static IServiceCollection RegisterFeatures(this IServiceCollection services)
        {
            var features = DiscoverFeatures();
            foreach (var feature in features)
            {
                if (RegisteredFeatures.Find(e => e.GetType() == feature.GetType()) == null)
                {
                    RegisteredFeatures.Add(feature);
                }

                feature.RegisterFeature(services);
            }

            return services;
        }

        private static IEnumerable<IFeature> DiscoverFeatures()
        {
            return typeof(IFeature).Assembly
                .GetTypes()
                .Where(p => p.IsClass && p.IsAssignableTo(typeof(IFeature)))
                .Select(Activator.CreateInstance)
                .Cast<IFeature>();
        }
    }
}
