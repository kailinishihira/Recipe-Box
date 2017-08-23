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

    [HttpPost("/category-added")]
    public ActionResult AddCategory()
    {
      string name = Request.Form["category-name"];

      Category newCategory = new Category(name);
      newCategory.Save();

      var allCategories = Category.GetAll();
      return View("Index", allCategories);
    }

    [HttpGet("/categories/{id}")]
    public ActionResult CategoryDetails(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(id);
      List<Recipe> categoryRecipe = selectedCategory.GetRecipes();
      List<Recipe> allRecipes = Recipe.GetAll();
      model.Add("selectedCategory", selectedCategory);
      model.Add("categoryRecipe", categoryRecipe);
      model.Add("allRecipes", allRecipes);
      return View(model);
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

    // [HttpPost('/category/add_recipe')]
    // public ActionResult CategoryAddRecipe()
    // {
    //   Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));
    //   Recipe recipe = Recipe.Find(Int32.Parse(Request.Form["recipe-id"]));
    //
    //   category.AddRecipeToCategoryJoinTable(recipe);
    //
    //   Dictionary<string, object> model = new Dictionary<string, object>();
    //   Category selectedCategory = Category.Find(category.GetId());
    //   List<Recipe> categoryRecipes = selectedCourse.GetRecipes();
    //   List<Recipe> allRecipes = Recipe.GetAll();
    //   model.Add("selectedCategory", selectedCategory);
    //   model.Add("categoryRecipes", categoryRecipes);
    //   model.Add("allRecipes", allRecipes);
    //   return View("CategoryDetail", model);
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
