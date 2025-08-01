using System.Text;
using System.Text.Json;
using RecipesApp.Shared.Dto;

namespace RecipesApp.Web.Api;

public class RecipeService
{
    public RecipeDetailDto? LoadRecipe(string id)
    {
        var recipeDetails = ReadRecipeDetailsFromStream();
        return recipeDetails.SingleOrDefault(r => r.Id == id);
    }

    public RecipeOverviewItemsDto LoadRecipes(int pageSize = 7, int pageIndex = 0)
    {
        var recipeDetails = ReadRecipeDetailsFromStream().ToList();

        var result = new RecipeOverviewItemsDto(recipeDetails.Count, pageSize, pageIndex,
            recipeDetails
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(r => new RecipeOverviewItemDto(r.Id, r.Name, r.Image))
                .ToArray());

        return result;
    }

    private RecipeDetailDto[] ReadRecipeDetailsFromStream()
    {
        var json = string.Empty;
        using (var reader = new StreamReader("recipedetails.json", Encoding.UTF8)) { json = reader.ReadToEnd(); }

        return JsonSerializer.Deserialize<RecipeDetailDto[]>(json) ?? new RecipeDetailDto[0];
    }
}