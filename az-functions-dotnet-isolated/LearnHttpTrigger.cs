using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LitPlus.Function
{
    public class LearnHttpTrigger
    {
        private readonly ILogger<LearnHttpTrigger> _logger;

        public LearnHttpTrigger(ILogger<LearnHttpTrigger> logger)
        {
            _logger = logger;
        }

        [Function("LearnHttpTrigger")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions! owo");
        }
    }
}
