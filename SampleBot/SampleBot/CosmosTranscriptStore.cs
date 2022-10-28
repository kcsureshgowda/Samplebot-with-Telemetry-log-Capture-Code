using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SampleBot
{
    public class CosmosTranscriptStore : ITranscriptLogger
    {
        //variable to hold CosmosDB storage 
        private CosmosDbPartitionedStorage _cosmosDBStorage;

        //Constructor to initilize the CosmosDB storage
        public CosmosTranscriptStore(CosmosDbPartitionedStorageOptions dbStorageOption)
        {
            _cosmosDBStorage = new CosmosDbPartitionedStorage(dbStorageOption);
        }

        //Method to log the telemetry data into CosmosDB
        public async Task LogActivityAsync(IActivity activity)
        {

            // Validate that activity object is null or it has data
            var isMessage = activity.AsMessageActivity() != null ? true : false;
            if (isMessage)
            {
                // Customize data before store into CosmosDB
                var data = new
                {
                    Activity = activity
                };
                var document = new Dictionary<string, object>();
                // activity.Id is being used as the Cosmos Document Id
                document.Add(activity.Id, data);
                await _cosmosDBStorage.WriteAsync(document, new CancellationToken());
            }
        }

    }
}
