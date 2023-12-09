using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.EntityFrameworkCore;
using FitiChanBot.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Data.Common;

namespace ServicesTests
{
    [TestClass]
    public class ServicesCreation
    {
        private readonly string dbString = "server=localhost; user=root; password=example; database=FitiTestBD;";
        private readonly string relativePath = "appsettings.json";
        private readonly DiscordSocketConfig dsSocketConf = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.All,
            AlwaysDownloadUsers = true
        };


        [TestMethod("FitiDBContext")]
        [Description("FitiDBContext creation test.")]
        public void FitiDBContextCreate()
        {
            DbContextOptionsBuilder<FitiDBContext> optionsBuilder = new DbContextOptionsBuilder<FitiDBContext>();
            //Hardcode connection string to track changes in connection path
            optionsBuilder.UseMySql(dbString, new MySqlServerVersion(new Version(8, 1, 0)));
            FitiDBContext db = new FitiDBContext(optionsBuilder.Options);
            //Assert
            Assert.IsNotNull(db);
        }

        [TestMethod("FitiSettings")]
        [Description("FitiSettings creation test.")]
        public void FitiSettingsCreate()
        {
            //Hardcode relative path to track changes
            FitiSettings fitiSettings = FitiUtilities.ReadJsonSettings<FitiSettings>(relativePath);
            //Assert
            Assert.IsNotNull(fitiSettings);
        }

        [TestMethod("DiscordSocketClient")]
        [Description("DiscordSocketClient creation test.")]
        public void DiscordSocketClientCreate()
        {
            //Hardcode DiscordSocketConfig
            DiscordSocketClient dsClient = new DiscordSocketClient(dsSocketConf);
            //Assert
            Assert.IsNotNull(dsClient);
        }

        [TestMethod("CommandService")]
        [Description("CommandService creation test.")]
        public void CommandServiceCreate()
        {
            CommandService CMDService = new CommandService();
            //Assert
            Assert.IsNotNull(CMDService);
        }

        [TestMethod("CommandHandler")]
        [Description("CommandHandler creation test.")]
        public void CommandHandlerCreate()
        {

            CommandHandler CMDHandler = new CommandHandler(new DiscordSocketClient(), new CommandService(), new ServiceCollection().BuildServiceProvider());
            //Assert
            Assert.IsNotNull(CMDHandler);
        }

        [TestMethod("MessageManager")]
        [Description("MessageManager creation test.")]
        public void MessageManagerCreate()
        {
            DbContextOptionsBuilder<FitiDBContext> optionsBuilder = new DbContextOptionsBuilder<FitiDBContext>();
            //Hardcode connection string to track changes in connection path
            optionsBuilder.UseMySql(dbString, new MySqlServerVersion(new Version(8, 1, 0)));
            FitiDBContext db = new FitiDBContext(optionsBuilder.Options);

            MessageManager MSGManager = new MessageManager(new DiscordSocketClient(dsSocketConf), db, FitiUtilities.ReadJsonSettings<FitiSettings>(relativePath));
            //Assert
            Assert.IsNotNull(MSGManager);
        }

        [TestMethod("BackgroundMonitor")]
        [Description("BackgroundMonitor creation test.")]
        public void BackgroundMonitorCreate()
        {
            DbContextOptionsBuilder<FitiDBContext> optionsBuilder = new DbContextOptionsBuilder<FitiDBContext>();
            //Hardcode connection string to track changes in connection path
            optionsBuilder.UseMySql(dbString, new MySqlServerVersion(new Version(8, 1, 0)));
            FitiDBContext db = new FitiDBContext(optionsBuilder.Options);

            BackgroundMonitor BackMonitor = new BackgroundMonitor(new MessageManager(new DiscordSocketClient(dsSocketConf), db, FitiUtilities.ReadJsonSettings<FitiSettings>(relativePath)),
                                                                  FitiUtilities.ReadJsonSettings<FitiSettings>(relativePath));
            //Assert
            Assert.IsNotNull(BackMonitor);
        }
    }
}