using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using TodoListDaemonWithCert;


/// <summary>
/// Provides a limited emulation of the Configuration manager for .NET Core
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


namespace TodoListDaemonWithCert
{
    /// <summary>
    /// In .NET Core 1.2, provides Close() for X509Store 
    /// </summary>
    public static class Extension
    {
        public static void Close(this X509Store store)
        {
            store.Dispose();
        }
    }

    /// <summary>
    /// Simple implementation of the configuration adapter
    /// </summary>
    public class ConfigurationAdapter
    {
        public string this[string config]
        {
            get
            {
                string value;
                var elements = xmlDocument.GetElementsByTagName("add").OfType<XmlElement>();
                XmlElement element = elements.FirstOrDefault(e => e.Attributes.OfType<XmlAttribute>().Any(a => a.Name == "key" && a.Value == config));
                if (element != null)
                {
                    value = element.Attributes.OfType<XmlAttribute>().FirstOrDefault(a => a.Name == "value").Value;
                }
                else
                {
                    value = null;
                }
                return value;
            }
        }

        private readonly XmlDocument xmlDocument;

        public ConfigurationAdapter(string configPath)
        {
            xmlDocument = new XmlDocument();
            xmlDocument.Load(File.OpenText(configPath));
        }
    }
}
