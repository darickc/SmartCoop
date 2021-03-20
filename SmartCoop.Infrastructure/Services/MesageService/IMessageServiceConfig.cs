namespace SmartCoop.Infrastructure.Services.MesageService
{
    public interface IMessageServiceConfig
    {
        string MqttUri { get; }
        string MqttUser { get; }
        string MqttPassword { get; }
        int MqttPort { get; }
        bool MqttSecure { get; }
        string MqttBaseTopic { get; }
    }
}