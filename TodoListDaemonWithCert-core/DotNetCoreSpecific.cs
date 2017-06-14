using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using TodoListDaemonWithCert;

namespace TodoListDaemonWithCert
{
    /// <summary>
    /// Provides Close() for X509Store.
    /// </summary>
    public static class Extension
    {
        public static void Close(this X509Store store)
        {
            store.Dispose();
        }
    }

    public class ConfigurationAdapter
    {
        public string this[string config]
        {
            get
            {
                string value = null;
                var elements = xmlDocument.GetElementsByTagName("add").OfType<XmlElement>();
                XmlElement element = elements.FirstOrDefault(e => e.Attributes.OfType<XmlAttribute>().Any(a => a.Name == "key" && a.Value == config));
                if (element != null)
                {
                    value = element.Attributes.OfType<XmlAttribute>().FirstOrDefault(a => a.Name == "value").Value;
                }
                return value;
            }
        }

        readonly XmlDocument xmlDocument;

        public ConfigurationAdapter(string configPath)
        {
            xmlDocument = new XmlDocument();
            xmlDocument.Load(File.OpenText(configPath));
        }
    }
}

/// <summary>
/// Provides the Configuration manager for .NET Core
/// </summary>
namespace System.Configuration
{
    public static class ConfigurationManager
    {
        public static ConfigurationAdapter AppSettings
        {
            get
            {
                if (configurationProvider == null)
                {
                    configurationProvider = new ConfigurationAdapter(@"..\TodoListDaemonWithCert\App.config");
                }
                return configurationProvider;
            }
        }

        static ConfigurationAdapter configurationProvider;
    }
}
