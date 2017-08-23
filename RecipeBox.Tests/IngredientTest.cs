using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using RecipeBox.Models;

namespace RecipeBox.Tests
{
  [TestClass]
  public class IngredientTests : IDisposable
  {
    public IngredientTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
    }

    public void Dispose()
    {
      Recipe.DeleteAll();
      Ingredient.DeleteAll();
    }

    [TestMethod]
    public void GetAll_IngredientsEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Ingredient.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueForSameName_True()
    {
      //Arrange, Act
      Ingredient firstIngredient = new Ingredient("Dessert");
      Ingredient secondIngredient = new Ingredient("Dessert");

      //Assert
      Assert.AreEqual(firstIngredient, secondIngredient);
    }

  [TestMethod]
    public void Save_DatabaseAssignsIdToIngredient_Id()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("Dessert");
      testIngredient.Save();

      //Act
      Ingredient savedIngredient = Ingredient.GetAll()[0];

      int result = savedIngredient.GetId();
      int testId = testIngredient.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Save_SavesIngredientToDatabase_IngredientList()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("Dessert");
      testIngredient.Save();

      //Act
      List<Ingredient> result = Ingredient.GetAll();
      List<Ingredient> testList = new List<Ingredient>{testIngredient};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Find_FindsIngredientInDatabase_Ingredient()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("Dessert");
      testIngredient.Save();

      //Act
      Ingredient foundIngredient = Ingredient.Find(testIngredient.GetId());

      //Assert
      Assert.AreEqual(testIngredient, foundIngredient);
    }

    [TestMethod]
    public void Delete_DeletesIngredientFromDatabase_IngredientList()
    {
      //Arrange
      string name1 = "Dessert";
      Ingredient testIngredient1 = new Ingredient(name1);
      testIngredient1.Save();

      string name2 = "Physics";
      Ingredient testIngredient2 = new Ingredient(name2);
      testIngredient2.Save();

      //Act
      testIngredient1.Delete();
      List<Ingredient> resultIngredients = Ingredient.GetAll();
      List<Ingredient> testIngredientList = new List<Ingredient> {testIngredient2};

      //Assert
      CollectionAssert.AreEqual(testIngredientList, resultIngredients);
    }

    [TestMethod]
    public void AddRecipeToIngredientJoinTable_AddsRecipeToJoinTable_RecipeList()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("Dessert");
      testIngredient.Save();

      Recipe testRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 4);
      testRecipe.Save();

      Recipe testRecipe2 = new Recipe("Ceviche", "Put the tilapia in a medium bowl. Pour the lime juice over the fish and mix gently to combine.", 4);
      testRecipe2.Save();

      //Act
      testIngredient.AddRecipeToIngredientJoinTable(testRecipe);
      testIngredient.AddRecipeToIngredientJoinTable(testRecipe2);

      List<Recipe> result = testIngredient.GetRecipes();
      List<Recipe> testList = new List<Recipe>{testRecipe, testRecipe2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetRecipes_ReturnsAllRecipesForIngredient_RecipeList()
    {
      //Arrange
      Ingredient testIngredient = new Ingredient("Dessert");
      testIngredient.Save();

      Recipe testRecipe1 = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 5);
      testRecipe1.Save();

      Recipe testRecipe2 = new Recipe("Ceviche", "Put the tilapia in a medium bowl. Pour the lime juice over the fish and mix gently to combine.", 5);
      testRecipe2.Save();

      //Act
      testIngredient.AddRecipeToIngredientJoinTable(testRecipe1);
      testIngredient.AddRecipeToIngredientJoinTable(testRecipe2);

      List<Recipe> savedRecipes = testIngredient.GetRecipes();
      List<Recipe> testList = new List<Recipe> {testRecipe1, testRecipe2};

      //Assert
      CollectionAssert.AreEqual(testList, savedRecipes);
    }

    [TestMethod]
    public void Delete_DeletesIngredientFromIngredientsAndJointTable_IngredientList()
    {
      //Arrange
      Recipe testRecipe = new Recipe("Chocolate Chip Cookies", "Melt butter, mix with sugar, eggs, flour, salt, baking soda, chocolate chips. Bake for 10 minutes", 5);
      testRecipe.Save();

      Ingredient testIngredient = new Ingredient("Dessert");
      testIngredient.Save();

      //Act
      testIngredient.AddRecipeToIngredientJoinTable(testRecipe);
      testIngredient.Delete();

      List<Ingredient> resultRecipeIngredients = testRecipe.GetIngredients();
      List<Ingredient> testRecipeIngredients = new List<Ingredient> {};

      //Assert
      CollectionAssert.AreEqual(testRecipeIngredients, resultRecipeIngredients);
    }

  }
}
