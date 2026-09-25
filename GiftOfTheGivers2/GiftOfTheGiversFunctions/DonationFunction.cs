using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace GiftOfTheGiversFunctions
{
    public class DonationFunction
    {
        private readonly ILogger<DonationFunction> _logger;

        public DonationFunction(
            ILogger<DonationFunction> logger)
        {
            _logger = logger;
        }

        [Function("DonationFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                "post"
            )] HttpRequestData req)
        {
            _logger.LogInformation(
                "Gift of the Givers donation request received."
            );

            // Read donation information
            string requestBody =
                await new StreamReader(req.Body)
                    .ReadToEndAsync();

            // Generate a dummy certificate number
            string certificateNumber =
                "GOTG-TAX-" +
                DateTime.Now.ToString("yyyyMMddHHmmss");

            // Create response
            var result = new
            {
                success = true,

                message =
                    "Donation successfully recorded.",

                organisation =
                    "Gift of the Givers",

                certificateNumber =
                    certificateNumber,

                donation =
                    requestBody,

                generatedAt =
                    DateTime.UtcNow
            };

            var response =
                req.CreateResponse(
                    HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}