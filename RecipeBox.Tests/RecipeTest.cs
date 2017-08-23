using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using RecipeBox.Models;

namespace RecipeBox.Tests
{
  [TestClass]
  public class RecipeTests : IDisposable
  {
    public void Dispose()
    {
      Recipe.DeleteAll();
      Category.DeleteAll();
    }

    public RecipeTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Recipe()
    {
      //Arrange, Act
      Recipe firstRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 4);
      Recipe secondRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 4);

      //Assert
      Assert.AreEqual(firstRecipe, secondRecipe);
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Recipe.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Save_SavesToDatabase_RecipeList()
    {
      //Arrange
      Recipe testRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 4);

      //Act
      testRecipe.Save();
      List<Recipe> result = Recipe.GetAll();
      List<Recipe> testList = new List<Recipe>{testRecipe};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
//
    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Recipe testRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 4);

      //Act
      testRecipe.Save();
      Recipe savedRecipe = Recipe.GetAll()[0];

      int result = savedRecipe.GetId();
      int testId = testRecipe.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsRecipeInDatabase_Recipe()
    {
      //Arrange
      Recipe testRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 4);
      testRecipe.Save();

      //Act
      Recipe foundRecipe = Recipe.Find(testRecipe.GetId());

      //Assert
      Assert.AreEqual(testRecipe, foundRecipe);
    }

      [TestMethod]
      public void AddCategory_AddsCategoryToJoinTable_CategoryList()
      {
        //Arrange
        Recipe testRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 4);
        testRecipe.Save();

        Category testCategory1 = new Category("Dessert");
        testCategory1.Save();
        Category testCategory2 = new Category("Baking");
        testCategory2.Save();

        //Act
        testRecipe.AddCategoryToJoinTable(testCategory1);
        testRecipe.AddCategoryToJoinTable(testCategory2);

        List<Category> result = testRecipe.GetCategories();
        List<Category> testList = new List<Category>{testCategory1, testCategory2};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }

    [TestMethod]
    public void Delete_DeletesRecipeAssociationsFromDatabase_RecipeList()
    {
      //Arrange
      Category testCategory = new Category("Dessert");
      testCategory.Save();

      Recipe testRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 4);
      testRecipe.Save();

      //Act
      testRecipe.AddCategoryToJoinTable(testCategory);
      testRecipe.Delete();

      List<Recipe> resultCategoryRecipes = testCategory.GetRecipes();
      List<Recipe> testCategoryRecipes = new List<Recipe> {};

      //Assert
      CollectionAssert.AreEqual(testCategoryRecipes, resultCategoryRecipes);
    }

    [TestMethod]
    public void GetCategories_ReturnsAllCategoryRecipes_CategoryList()
    {
      //Arrange
      Recipe testRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 4);
      testRecipe.Save();

      Category testCategory1 = new Category("Dessert");
      testCategory1.Save();

      Category testCategory2 = new Category("Baking");
      testCategory2.Save();

      //Act
      testRecipe.AddCategoryToJoinTable(testCategory1);
      testRecipe.AddCategoryToJoinTable(testCategory2);
      List<Category> result = testRecipe.GetCategories();
      List<Category> testList = new List<Category> {testCategory1, testCategory2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
  }
}