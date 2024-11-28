namespace VehicleInsuranceAPI.Controllers
{
    public class DocumentUploader
    {
        private readonly HttpClient _httpClient;

        public DocumentUploader()
        {
            _httpClient = new HttpClient();
        }

        public void UploadDocumentsForPolicies(List<string> policyNumbers)
        {
            foreach (var policyNumber in policyNumbers)
            {
                // Simulate document upload for each policy
                var documentPath = GetDocumentPathForPolicy(policyNumber);
                var documentContent = File.ReadAllBytes(documentPath);

                // Upload the document (poor performance due to synchronous call)
                UploadDocumentSynchronously(policyNumber, documentContent);
            }
        }

        private string GetDocumentPathForPolicy(string policyNumber)
        {
            // Logic to determine the document path based on policy number
            // For demonstration, assume all documents are in the same folder
            return $"C:\\Documents\\Policy_{policyNumber}.pdf";
        }

        private void UploadDocumentSynchronously(string policyNumber, byte[] documentContent)
        {
            // Simulate document upload to a server (poor performance due to synchronous call)
            var uploadUrl = $"https://api.example.com/upload?policyNumber={policyNumber}";
            var content = new ByteArrayContent(documentContent);
            var response = _httpClient.PostAsync(uploadUrl, content).Result;

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Document for Policy {policyNumber} uploaded successfully.");
            }
            else
            {
                Console.WriteLine($"Error uploading document for Policy {policyNumber}. Status code: {response.StatusCode}");
            }
        }
    }
}
