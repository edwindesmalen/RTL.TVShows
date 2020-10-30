using NSwag.Generation.AspNetCore;
using System;
using System.Linq;
using System.Reflection;

namespace RTL.TVShows.Extensions.OpenApi
{
    public static class OpenApiHelper
    {
        private static T GetAttribute<T>(this ICustomAttributeProvider assembly, bool inherit = false) where T : Attribute
        {
            return assembly
                .GetCustomAttributes(typeof(T), inherit)
                .OfType<T>()
                .FirstOrDefault();
        }

        public static Action<AspNetCoreOpenApiDocumentGeneratorSettings> GetOpenApiDocumentSettings()
        {
            return options =>
            {
                var assembly = Assembly.GetEntryAssembly();
                var description = GetAttribute<AssemblyDescriptionAttribute>(assembly)?.Description;
                var company = GetAttribute<AssemblyCompanyAttribute>(assembly)?.Company;

                var version = assembly.GetName().Version;

                options.AllowReferencesWithProperties = true;
                options.Title = assembly.GetName().Name;
                options.DocumentName = $"v{version}";
                options.Version = version.ToString();
                options.Description = description;
                options.PostProcess = document =>
                {
                    document.Info.Version = $"v{version}";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = company,
                        Email = "n/a",
                        Url = "https://github.com/edwindesmalen/RTL.TVShows"
                    };
                };
            };
        }
    }
}
