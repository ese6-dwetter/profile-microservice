using System;
using System.Text.Json;
using System.Threading.Tasks;
using MessageBroker;
using ProfileMicroservice.Services;

namespace ProfileMicroservice.MessageHandlers
{
    public class RegisterUserMessageHandler : IMessageHandler<RegisterUserMessage>
    {
        private readonly IProfileService _service;

        public RegisterUserMessageHandler(IProfileService service)
        {
            _service = service;
        }

        public Task HandleMessageAsync(string messageType, RegisterUserMessage message)
        {
            Task.Run(() => { _service.CreateProfileAsync(Guid.Parse(message.userId), message.username); });
            
            return Task.CompletedTask;
        }

        public Task HandleMessageAsync(string messageType, byte[] obj)
        {
            return HandleMessageAsync(
                messageType,
                JsonSerializer.Deserialize<RegisterUserMessage>((ReadOnlySpan<byte>) obj));
        }
    }
}
