namespace ProductService.Entities;

public class Category
{
    public int MyProperty { get; set; }
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }
    
}