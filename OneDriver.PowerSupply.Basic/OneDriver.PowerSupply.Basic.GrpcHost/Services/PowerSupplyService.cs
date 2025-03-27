using Grpc.Core;
using Microsoft.Extensions.Logging;
using OneDriver.PowerSupply.Basic;
using OneDriver.PowerSupply.Basic.Products;
using OneDriver.PowerSupply.Basic.GrpcHost.Protos;
using OneDriver.Framework.Libs.Validator;
using System.Threading.Tasks;
using OneDriver.Framework.Libs;


namespace OneDriver.PowerSupply.Basic.GrpcHost.Services
{
    public class PowerSupplyService : OneDriver.PowerSupply.Basic.GrpcHost.Protos.PowerSupply.PowerSupplyBase

    {
        private readonly Device _device;

        public PowerSupplyService(ILogger<PowerSupplyService> logger)
        {
            var validator = new ComportValidator();
            var hal = new Kd3005p();
            _device = new Device("Korad", validator, hal);
        }

        public override Task<StatusReply> OpenConnection(OpenRequest request, ServerCallContext context)
        {
            var code = _device.Connect(request.Port);
            return Task.FromResult(new StatusReply
            {
                Code = (int)code,
                Message = code == 0 ? "Connection opened." : "Failed to open connection."
            });
        }

        public override Task<StatusReply> SetVolts(SetRequest request, ServerCallContext context)
        {
            var code = _device.SetVolts(request.Channel, request.Value);
            return Task.FromResult(new StatusReply
            {
                Code = code,
                Message = code == 0 ? "Voltage set." : "Failed to set voltage."
            });
        }

        public override Task<StatusReply> SetAmps(SetRequest request, ServerCallContext context)
        {
            var code = _device.SetAmps(request.Channel, request.Value);
            return Task.FromResult(new StatusReply
            {
                Code = code,
                Message = code == 0 ? "Current set." : "Failed to set current."
            });
        }

        public override Task<StatusReply> AllChannelsOn(Empty request, ServerCallContext context)
        {
            var code = _device.AllChannelsOn();
            return Task.FromResult(new StatusReply
            {
                Code = code,
                Message = "All channels turned ON."
            });
        }

        public override Task<StatusReply> AllChannelsOff(Empty request, ServerCallContext context)
        {
            var code = _device.AllChannelsOff();
            return Task.FromResult(new StatusReply
            {
                Code = code,
                Message = "All channels turned OFF."
            });
        }
    }
}
