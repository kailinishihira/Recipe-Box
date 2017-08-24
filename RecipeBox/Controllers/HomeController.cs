using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System;

namespace RecipeBox.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      var allCategories = Category.GetAll();
      return View(allCategories);
    }

    [HttpGet("/category-form")]
    public ActionResult CategoryForm()
    {
      return View();
    }

    [HttpGet("/recipe-form")]
    public ActionResult RecipeForm()
    {
      return View("RecipeForm");
    }


    [HttpPost("/category-added")]
    public ActionResult AddCategory()
    {
      string name = Request.Form["category-name"];

      Category newCategory = new Category(name);
      newCategory.Save();

      List<Category> allCategories = Category.GetAll();
      return View("Index", allCategories);
    }

    [HttpPost("/category/{id}/recipe/add")]
    public ActionResult AddRecipe(int id)
    {
      string recipeName = Request.Form["recipe-name"];
      string instructions = Request.Form["instructions"];
      Category thisCategory = Category.Find(id);
      Recipe newRecipe = new Recipe(recipeName, instructions);
      newRecipe.Save();
      List<Recipe> allRecipes = Recipe.GetAll();
      return View("AllRecipes", allRecipes);
    }

    [HttpGet("/recipes/all")]
    public ActionResult AllRecipes()
    {
      List<Recipe> allRecipes = Recipe.GetAll();
      return View(allRecipes);
    }

    [HttpGet("/recipes/{id}")]
    public ActionResult RecipeDetails(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Recipe thisRecipe = Recipe.Find(id);
      List<Category> allCategories = Category.GetAll();
      List<Ingredient> ingredientsRecipes = thisRecipe.GetIngredients();
      List<Category> categoriesRecipes = thisRecipe.GetCategories();

      model.Add("thisRecipe", thisRecipe);
      model.Add("allCategories", allCategories);
      model.Add("ingredientsRecipes", ingredientsRecipes);
      model.Add("categoriesRecipes", categoriesRecipes);
      return View(model);
    }

    [HttpPost("/recipes/{id}/add-ingredient")]
    public ActionResult AddIngredients(int id)
    {
      string ingredient = Request.Form["ingredient"];
      Ingredient newIngredient = new Ingredient(ingredient);
      newIngredient.Save();

      Dictionary<string, object> model = new Dictionary<string, object>();
      Recipe thisRecipe = Recipe.Find(id);
      List<Category> allCategories = Category.GetAll();
      List<Ingredient> ingredientsRecipes = thisRecipe.GetIngredients();
      List<Category> categoriesRecipes = thisRecipe.GetCategories();
      model.Add("thisRecipe", thisRecipe);
      model.Add("allCategories", allCategories);
      model.Add("ingredientsRecipes", ingredientsRecipes);
      model.Add("categoriesRecipes", categoriesRecipes);
      return View("RecipeDetails", model);
    }

    [HttpPost("/recipe/add_category_to_recipe")]
    public ActionResult AddCategoryToRecipe()
    {
        Recipe recipe = Recipe.Find(Int32.Parse(Request.Form["recipe-id"]));
        Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));

        recipe.AddCategoryToJoinTable(category);

        Dictionary<string, object> model = new Dictionary<string, object>();
        Recipe thisRecipe = Recipe.Find(recipe.GetId());
        List<Category> allCategories = Category.GetAll();
        List<Ingredient> ingredientsRecipes = thisRecipe.GetIngredients();
        List<Category> categoriesRecipes = thisRecipe.GetCategories();
        model.Add("thisRecipe", thisRecipe);
        model.Add("allCategories", allCategories);
        model.Add("ingredientsRecipes", ingredientsRecipes);
        model.Add("categoriesRecipes", categoriesRecipes);

        return View("RecipeDetails", model);
      }

    [HttpGet("/categories/{id}")]
    public ActionResult CategoryDetails(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(id);
      List<Recipe> categoriesRecipes = selectedCategory.GetRecipes();
      List<Recipe> allRecipes = Recipe.GetAll();
      model.Add("selectedCategory", selectedCategory);
      model.Add("categoriesRecipes", categoriesRecipes);
      model.Add("allRecipes", allRecipes);
      return View(model);
    }

    [HttpPost("/category/add_recipe")]
    public ActionResult CategoryAddRecipe()
    {
      Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));
      Recipe recipe = Recipe.Find(Int32.Parse(Request.Form["recipe-id"]));
      category.AddRecipeToCategoryJoinTable(recipe);
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(category.GetId());
      List<Recipe> categoriesRecipes = selectedCategory.GetRecipes();
      List<Recipe> allRecipes = Recipe.GetAll();
      model.Add("selectedCategory", selectedCategory);
      model.Add("categoriesRecipes", categoriesRecipes);
      model.Add("allRecipes", allRecipes);
      return View("CategoryDetails", model);
    }

    [HttpGet("/categories/{id}/add-recipe")]
    public ActionResult CategoryToRecipeForm(int id)
    {
      Category thisCategory = Category.Find(id);
      return View("RecipeForm", thisCategory);
    }

    [HttpGet("/recipe/{id}/delete")]
    public ActionResult RecipeDelete(int id)
    {
      Recipe thisRecipe = Recipe.Find(id);
      thisRecipe.Delete();
      return View();
    }

    [HttpGet("/category/{id}/delete")]
    public ActionResult CategoryDelete(int id)
    {
      Category thisCategory = Category.Find(id);
      thisCategory.Delete();
      return View();
    }

    // [HttpPost ("/category-added")]
    // public ActionResult AddCategory(int id)
    // {
    //   Category newCategory = new Category(Request.Form["category-name"]);
    //   newCategory.Save();
    //
    //   Dictionary<string, object> model = new Dictionary<string, object>();
    //   Category selectedCategory = Category.Find(id);
    //   List<Recipe> categoryRecipe = selectedCategory.GetRecipes();
    //   List<Recipe> allRecipes = Recipe.GetAll();
    //   model.Add("selectedCategory", selectedCategory);
    //   model.Add("categoryRecipe", categoryRecipe);
    //   model.Add("allRecipes", allRecipes);
    //   return View("CategoryDetails", model);
    // }



    // [HttpPost("/recipe/{id}")]
    // public ActionResult AddRecipeDetails(int id)
    // {
    //   Recipe newRecipe = new Recipe (Request.Form[""]);
    //   newRecipe.Save();
    //   Recipe selectedRecipe = Recipe.Find(id)
    //
    //   return View("RecipeDetails", )
    // }
  }
}
