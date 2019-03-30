using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Database
{
    public class Program
    {
        public static string ConfigPath { get; } = "config.json";
        public static DbConfig Config { get; set; }

        private static DbContextOptionsBuilder<AudioContext> optionsBuilder = new DbContextOptionsBuilder<AudioContext>();

        public static void Main(string[] args)
        {
            var json = string.Empty;

            // Check for config file
            if (!File.Exists(ConfigPath))
            {
                json = JsonConvert.SerializeObject(GenerateNewConfig(), Formatting.Indented);
                File.WriteAllText("config.json", json, new UTF8Encoding(false));
                return;
            }

            //If Config.json exists, get the values and store it in a parameter.
            json = File.ReadAllText(ConfigPath, new UTF8Encoding(false));
            Config = JsonConvert.DeserializeObject<DbConfig>(json);

            if (Config.DbType == "sqlserver")
            {
                optionsBuilder.UseSqlServer(GetConnectionString(Config));
            }

            // Now build the database.
            SetupDatabase();

            // Next we should check the local filesystem for new audio files.
            if (Directory.Exists("audiofiles"))
            {
                // since the directory is here, lets iterate through all the files and add them to our db.
                PopulateDatabase();
            }

            // Now we exit, currently we dont have anything else to do!
        }

        private static void PopulateDatabase()
        {
            // TODO: Refactor this to actually log any errors that may occur. also needs to be Async.
            using (var dbContext = new AudioContext(optionsBuilder.Options))
            {
                foreach (var file in Directory.EnumerateFiles(@"E:\audiofiles", "*.mp3", SearchOption.AllDirectories))
                {
                    var fi = new FileInfo(file);
                    if (fi.Extension == ".mp3")
                    {
                        var fileid3 = TagLib.File.Create(file);

                        var myAudioFile = new AudioFile
                        {
                            Artist = fileid3.Tag.FirstPerformer,
                            Title = fileid3.Tag.Title,
                            Filename = fi.Name,
                            Path = fi.DirectoryName,
                            FullPath = file
                        };
                        dbContext.AudioFiles.Add(myAudioFile);


                        Console.WriteLine($"Title: {fileid3.Tag.Title} {Environment.NewLine}" +
                                          $"      Artist: {fileid3.Tag.FirstPerformer}");
                    }
                }

                dbContext.SaveChanges();
            }
        }

        private static bool SetupDatabase()
        {
            bool toReturn = false;

            try
            {
                using (var context = new AudioContext(optionsBuilder.Options))
                {
                    context.Database.EnsureCreated();
                }

                toReturn = true;
            }
            catch (Exception ex)
            {
                // TODO: Refactor this so the errors are logged.
                Console.WriteLine($"Error Occured while building the database. {ex.ToString()}");
            }

            return (toReturn);
        }

        private static DbConfig GenerateNewConfig() => new DbConfig()
        {
            DbType = "sqlserver",
            Host = "192.168.5.184",
            Port = "1433",
            Instance = "",
            Database = "pbAudioStore",
            Trusted_Connection = "false",
            Username = "",
            Password = ""
        };

        private static string GetConnectionString(DbConfig config)
        {
            var connStr = new SqlConnectionStringBuilder();

            if (!string.IsNullOrEmpty(config.Host))
                connStr.DataSource = config.Host;

            // Port cant be used in sqlserver??
            // if (!string.IsNullOrEmpty(config.Port))

            if (!string.IsNullOrEmpty(config.Username))
                connStr.UserID = config.Username;

            if (!string.IsNullOrEmpty(config.Password))
                connStr.Password = config.Password;

            if (!string.IsNullOrEmpty(config.Database))
                connStr.InitialCatalog = config.Database;

            return (connStr.ToString());
        }
    }
}