namespace OrderService.Services;

public class ProductServiceClient
{
    private readonly HttpClient _httpClient;

    public ProductServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }




    public async Task<bool> ProductExists(int productId)
    {
        var response = await _httpClient.GetAsync(
            $"http://localhost:5001/api/Product/{productId}");
            
        return response.IsSuccessStatusCode;
    }

}