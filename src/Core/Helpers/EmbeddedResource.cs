using System.Reflection;
using System.Text;

namespace CraftersCloud.Core.Helpers;

public static class EmbeddedResource
{
    public static string ReadResourceContent(Assembly assembly, string namespaceAndFileName)
    {
        try
        {
            using var stream = assembly.GetManifestResourceStream(namespaceAndFileName);
            if (stream == null)
            {
                return string.Empty;
            }

            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException($"Failed to read Embedded Resource {namespaceAndFileName}",
                exception);
        }
    }
}