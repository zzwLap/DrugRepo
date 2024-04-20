using MassTransit;

namespace webapi.Controllers
{
    public class IdGeneratorService
    {
        public string GetNextId()
        {
            return NewId.Next().ToString();
        }
        public string GenerateNextReceiptId(string currentReceiptId)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
