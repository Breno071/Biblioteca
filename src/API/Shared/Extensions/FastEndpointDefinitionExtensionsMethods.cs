using FastEndpoints;

namespace API.Shared.Extensions
{
    public static class FastEndpointDefinitionExtensionsMethods
    {
        public static void CustomConfig(this EndpointDefinition definition, int apiVersion, string[] tags, string summary = "", string description = "")
        {
            definition.EndpointVersion(apiVersion);

            if (tags.IsCollectionNotEmpty())
            {
                definition.Options(x => x.WithTags(tags));
            }

            if (summary.IsNotEmpty() || description.IsNotEmpty())
            {
                definition.Summary(s =>
                {
                    s.Summary = summary;
                    s.Description = description;
                });
            }
        }

        public static void CustomConfig(this EndpointDefinition definition, string[] authenticationScheme, int apiVersion, string[] tags, string summary = "", string description = "", params string[] permissions)
        {
            definition.Permissions(permissions);
            definition.CustomConfig(apiVersion, tags, summary, description);
            definition.AuthSchemes(authenticationScheme);
        }
    }
}
