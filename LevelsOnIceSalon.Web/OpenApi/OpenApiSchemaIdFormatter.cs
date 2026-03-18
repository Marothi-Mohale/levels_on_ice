namespace LevelsOnIceSalon.Web.OpenApi;

internal static class OpenApiSchemaIdFormatter
{
    public static string Build(Type type)
    {
        if (!type.IsGenericType)
        {
            return type.Name.Replace("+", ".");
        }

        var genericTypeName = type.GetGenericTypeDefinition().Name;
        var backtickIndex = genericTypeName.IndexOf('`');
        if (backtickIndex >= 0)
        {
            genericTypeName = genericTypeName[..backtickIndex];
        }

        var genericArguments = string.Join("And", type.GetGenericArguments().Select(Build));
        return $"{genericTypeName}Of{genericArguments}";
    }
}
