using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RecipeBox.Models
{
  public class Recipe
  {
    private int _id;
    private string _name;
    private string _instructions;
    private int _rating;

    public Recipe(string name, string instructions, int rating = 0, int id = 0)
    {
      _name = name;
      _instructions = instructions;
      _rating = rating;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }

    public int GetRating()
    {
      return _rating;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetInstructions()
    {
      return _instructions;
    }

    public override bool Equals(Object otherRecipe)
    {
      if (!(otherRecipe is Recipe))
      {
        return false;
      }
      else
      {
        Recipe newRecipe = (Recipe) otherRecipe;

        bool idEquality = (this.GetId() == newRecipe.GetId());
        bool nameEquality = (this.GetName() == newRecipe.GetName());
        bool instructionsEquality = (this.GetInstructions() == newRecipe.GetInstructions());
        bool ratingEquality = (this.GetRating() == newRecipe.GetRating());

        return (idEquality && nameEquality && instructionsEquality && ratingEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public static List<Recipe> GetAll()
    {
     List<Recipe> recipeList = new List<Recipe> {};

     MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"SELECT * FROM recipes;";

     var rdr = cmd.ExecuteReader() as MySqlDataReader;
     while(rdr.Read())
     {
       int recipeId = rdr.GetInt32(0);
       string name = rdr.GetString(1);
       string instructions = rdr.GetString(2);
       int rating = rdr.GetInt32(3);

       Recipe newRecipe = new Recipe(name, instructions, rating, recipeId);
       recipeList.Add(newRecipe);
     }
     conn.Close();
     return recipeList;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO recipes (name, instructions, rating) VALUES (@name, @instructions, @rating);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      MySqlParameter instructions = new MySqlParameter();
      instructions.ParameterName = "@instructions";
      instructions.Value = this._instructions;
      cmd.Parameters.Add(instructions);

      MySqlParameter rating = new MySqlParameter();
      rating.ParameterName = "@rating";
      rating.Value = this._rating;
      cmd.Parameters.Add(rating);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
    }

    public static Recipe Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipes WHERE id = @recipeId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@recipeId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int recipeId = 0;
      string recipeName = "";
      string recipeInstructions = "";
      int recipeRating = 0;

      while(rdr.Read())
      {
        recipeId = rdr.GetInt32(0);
        recipeName = rdr.GetString(1);
        recipeInstructions = rdr.GetString(2);
        recipeRating = rdr.GetInt32(3);

      }
      Recipe foundRecipe = new Recipe(recipeName, recipeInstructions, recipeRating, recipeId);
      conn.Close();
      return foundRecipe;
    }

    public void UpdateName(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE recipes SET name = @newName WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@newName";
      name.Value = newName;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      conn.Close();
      _name = newName;
    }

    public void UpdateInstructions(string newInstructions)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE recipes SET instructions = @newInstructions WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter instructions = new MySqlParameter();
      instructions.ParameterName = "@newInstructions";
      instructions.Value = newInstructions;
      cmd.Parameters.Add(instructions);

      cmd.ExecuteNonQuery();
      conn.Close();
      _instructions = newInstructions;
    }

    public void UpdateRating(int newRating)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE recipes SET rating = @newRating WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter rating = new MySqlParameter();
      rating.ParameterName = "@newRating";
      rating.Value = newRating;
      cmd.Parameters.Add(rating);

      cmd.ExecuteNonQuery();
      conn.Close();
      _rating = newRating;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM recipes WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void DeleteCategoryFromRecipe(Category newCategory)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM categories_recipes WHERE recipe_id = @RecipeId AND category_id = @CategoryId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@RecipeId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter category_id_param = new MySqlParameter();
      category_id_param.ParameterName = "@CategoryId";
      category_id_param.Value = newCategory.GetId();
      cmd.Parameters.Add(category_id_param);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void DeleteIngredientFromRecipe(Ingredient newIngredient)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM ingredients_recipes WHERE recipe_id = @RecipeId AND ingredient_id = @IngredientId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@RecipeId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter ingredient_id_param = new MySqlParameter();
      ingredient_id_param.ParameterName = "@IngredientId";
      ingredient_id_param.Value = newIngredient.GetId();
      cmd.Parameters.Add(ingredient_id_param);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM recipes;";
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddCategoryToJoinTable(Category newCategory)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO categories_recipes (recipe_id, category_id) VALUES (@RecipeId, @CategoryId);";

      MySqlParameter category_id_param = new MySqlParameter();
      category_id_param.ParameterName = "@CategoryId";
      category_id_param.Value = newCategory.GetId();
      cmd.Parameters.Add(category_id_param);

      MySqlParameter recipe_id_param = new MySqlParameter();
      recipe_id_param.ParameterName = "@RecipeId";
      recipe_id_param.Value = _id;
      cmd.Parameters.Add(recipe_id_param);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Category> GetCategories()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT categories.*
          FROM recipes
          JOIN categories_recipes ON (recipes.id = categories_recipes.recipe_id)
          JOIN categories ON (categories.id = categories_recipes.category_id)
          WHERE recipes.id = @RecipeId;";

        MySqlParameter recipeIdParameter = new MySqlParameter();
        recipeIdParameter.ParameterName = "@RecipeId";
        recipeIdParameter.Value = _id;
        cmd.Parameters.Add(recipeIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        List<Category> categories = new List<Category>{};

        while(rdr.Read())
        {
          int categoryId = rdr.GetInt32(0);
          string categoryName = rdr.GetString(1);

          Category newCategory = new Category(categoryName, categoryId);
          categories.Add(newCategory);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return categories;
    }

    public void AddIngredientToJoinTable(Ingredient newIngredient)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO ingredients_recipes (ingredient_id, recipe_id) VALUES (@IngredientId, @RecipeId);";

      MySqlParameter ingredient_id_param = new MySqlParameter();
      ingredient_id_param.ParameterName = "@IngredientId";
      ingredient_id_param.Value = newIngredient.GetId();
      cmd.Parameters.Add(ingredient_id_param);

      MySqlParameter recipe_id_param = new MySqlParameter();
      recipe_id_param.ParameterName = "@RecipeId";
      recipe_id_param.Value = _id;
      cmd.Parameters.Add(recipe_id_param);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Ingredient> GetIngredients()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT ingredients.* FROM recipes
          JOIN ingredients_recipes ON (recipes.id = ingredients_recipes.recipe_id)
          JOIN ingredients ON (ingredients.id = ingredients_recipes.ingredient_id)
          WHERE recipes.id = @RecipeId;";

        MySqlParameter recipeIdParameter = new MySqlParameter();
        recipeIdParameter.ParameterName = "@RecipeId";
        recipeIdParameter.Value = _id;
        cmd.Parameters.Add(recipeIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        List<Ingredient> ingredients = new List<Ingredient>{};

        while(rdr.Read())
        {
          int ingredientId = rdr.GetInt32(0);
          string ingredientName = rdr.GetString(1);

          Ingredient newIngredient = new Ingredient(ingredientName, ingredientId);
          ingredients.Add(newIngredient);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return ingredients;
    }
  }
}
