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
            $"https://localhost:5001/api/product/{productId}");

        return response.IsSuccessStatusCode;
    }
}