using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
  public class Class1
  {
    private static ILog log;
    static Class1()
    {
      LoadElasticSearchConfigFile();
      //log = LogManager.GetLogger("ElasticSearchAppender");
      log = LogManager.GetLogger(typeof(Class1));
    }
    public static void Method()
    {
      log.Error("Test");
    }
    private static void LoadElasticSearchConfigFile()
    {
      //log4net.ElasticSearch.ElasticSearchAppender appender = new log4net.ElasticSearch.ElasticSearchAppender();
      //log4net.Appender.IAppender iAppender = appender;
      String directory = typeof(Class1).Assembly.Location;
      // Environment.CurrentDirectory  : passe en local dans un scénario run.csx
      if (!ConfigFileExistsIndir(directory))
      {
        directory = Environment.CurrentDirectory;
        if (!ConfigFileExistsIndir(directory))
        {
          String functionName = "HttpTriggerCSharp1";
          directory = Path.Combine(Environment.CurrentDirectory, functionName);
          if (!ConfigFileExistsIndir(directory))
            directory = Path.Combine(directory, "bin");
        }
      }
      String configFilePath = MakeConfigFilePath(directory);
      var configFile = new FileInfo(configFilePath);
      Debug.Assert(configFile.Exists);
      XmlConfigurator.Configure(configFile);
    }
    private static bool ConfigFileExistsIndir(string directory)
    {
      String configFilePath = MakeConfigFilePath(directory);
      var configFile = new FileInfo(configFilePath);
      return configFile.Exists;
    }
    private static string MakeConfigFilePath(string directory)
    {
      String configFileName = "elasticsearch.config";
      String configFilePath = directory == null ? configFileName : Path.Combine(directory, configFileName);
      return configFilePath;
    }
  }
}
