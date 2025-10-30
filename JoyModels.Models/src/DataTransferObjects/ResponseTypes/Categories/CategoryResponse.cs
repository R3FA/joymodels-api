namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Categories;

public class CategoryResponse
{
    public Guid Uuid { get; set; }

    public string CategoryName { get; set; } = string.Empty;
}