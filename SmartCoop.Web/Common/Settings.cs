using SmartCoop.Infrastructure.Services.MesageService;

namespace SmartCoop.Web.Common
{
    public class Settings : IMessageServiceConfig
    {
        public string MqttUri { get; set; }
        public string MqttUser { get; set; }
        public string MqttPassword { get; set; }
        public int MqttPort { get; set; }
        public bool MqttSecure { get; set; }
        public string MqttBaseTopic { get; set; }
    }
}