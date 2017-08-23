using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using RecipeBox.Models;

namespace RecipeBox.Tests
{
  [TestClass]
  public class CategoryTests : IDisposable
  {
    public CategoryTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
    }

    public void Dispose()
    {
      Recipe.DeleteAll();
      Category.DeleteAll();
    }

    [TestMethod]
    public void GetAll_CategoriesEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Category.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueForSameName_True()
    {
      //Arrange, Act
      Category firstCategory = new Category("Dessert");
      Category secondCategory = new Category("Dessert");

      //Assert
      Assert.AreEqual(firstCategory, secondCategory);
    }

  [TestMethod]
    public void Save_DatabaseAssignsIdToCategory_Id()
    {
      //Arrange
      Category testCategory = new Category("Dessert");
      testCategory.Save();

      //Act
      Category savedCategory = Category.GetAll()[0];

      int result = savedCategory.GetId();
      int testId = testCategory.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Save_SavesCategoryToDatabase_CategoryList()
    {
      //Arrange
      Category testCategory = new Category("Dessert");
      testCategory.Save();

      //Act
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Find_FindsCategoryInDatabase_Category()
    {
      //Arrange
      Category testCategory = new Category("Dessert");
      testCategory.Save();

      //Act
      Category foundCategory = Category.Find(testCategory.GetId());

      //Assert
      Assert.AreEqual(testCategory, foundCategory);
    }

    [TestMethod]
    public void Delete_DeletesCategoryFromDatabase_CategoryList()
    {
      //Arrange
      string name1 = "Dessert";
      Category testCategory1 = new Category(name1);
      testCategory1.Save();

      string name2 = "Physics";
      Category testCategory2 = new Category(name2);
      testCategory2.Save();

      //Act
      testCategory1.Delete();
      List<Category> resultCategories = Category.GetAll();
      List<Category> testCategoryList = new List<Category> {testCategory2};

      //Assert
      CollectionAssert.AreEqual(testCategoryList, resultCategories);
    }

    [TestMethod]
    public void AddRecipeToCategoryJoinTable_AddsRecipeToJoinTable_RecipeList()
    {
      //Arrange
      Category testCategory = new Category("Dessert");
      testCategory.Save();

      Recipe testRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 4);
      testRecipe.Save();

      Recipe testRecipe2 = new Recipe("Ceviche", "Put the tilapia in a medium bowl. Pour the lime juice over the fish and mix gently to combine.", 4);
      testRecipe2.Save();

      //Act
      testCategory.AddRecipeToCategoryJoinTable(testRecipe);
      testCategory.AddRecipeToCategoryJoinTable(testRecipe2);

      List<Recipe> result = testCategory.GetRecipes();
      List<Recipe> testList = new List<Recipe>{testRecipe, testRecipe2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetRecipes_ReturnsAllRecipesForCategory_RecipeList()
    {
      //Arrange
      Category testCategory = new Category("Dessert");
      testCategory.Save();

      Recipe testRecipe1 = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 5);
      testRecipe1.Save();

      Recipe testRecipe2 = new Recipe("Ceviche", "Put the tilapia in a medium bowl. Pour the lime juice over the fish and mix gently to combine.", 5);
      testRecipe2.Save();

      //Act
      testCategory.AddRecipeToCategoryJoinTable(testRecipe1);
      testCategory.AddRecipeToCategoryJoinTable(testRecipe2);

      List<Recipe> savedRecipes = testCategory.GetRecipes();
      List<Recipe> testList = new List<Recipe> {testRecipe1, testRecipe2};

      //Assert
      CollectionAssert.AreEqual(testList, savedRecipes);
    }

    [TestMethod]
    public void Delete_DeletesCategoryFromCategoriesAndJointTable_CategoryList()
    {
      //Arrange
      Recipe testRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 5);
      testRecipe.Save();

      Category testCategory = new Category("Dessert");
      testCategory.Save();

      //Act
      testCategory.AddRecipeToCategoryJoinTable(testRecipe);
      testCategory.Delete();

      List<Category> resultRecipeCategories = testRecipe.GetCategories();
      List<Category> testRecipeCategories = new List<Category> {};

      //Assert
      CollectionAssert.AreEqual(testRecipeCategories, resultRecipeCategories);
    }

  }
}
