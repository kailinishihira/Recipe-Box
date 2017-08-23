using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RecipeBox.Models
{
  public class  Ingredient
  {
    private int _id;
    private string _name;

    public Ingredient(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }


    public override bool Equals(Object otherIngredient)
    {
      if (!(otherIngredient is Ingredient))
      {
        return false;
      }
      else
      {
        Ingredient newIngredient = (Ingredient) otherIngredient;

        bool idEquality = (this.GetId() == newIngredient.GetId());
        bool nameEquality = (this.GetName() == newIngredient.GetName());


        return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public static List<Ingredient> GetAll()
    {
     List<Ingredient> ingredientList = new List<Ingredient> {};

     MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"SELECT * FROM ingredients;";

     var rdr = cmd.ExecuteReader() as MySqlDataReader;
     while(rdr.Read())
     {
       int ingredientId = rdr.GetInt32(0);
       string name = rdr.GetString(1);
       Ingredient newIngredient = new Ingredient(name, ingredientId);
       ingredientList.Add(newIngredient);
     }
     conn.Close();
     return ingredientList;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO ingredients (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);


      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
    }

    public static Ingredient Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM ingredients WHERE id = @ingredientId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@ingredientId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int ingredientId = 0;
      string ingredientName = "";


      while(rdr.Read())
      {
        ingredientId = rdr.GetInt32(0);
        ingredientName = rdr.GetString(1);

      }
      Ingredient foundIngredient = new Ingredient(ingredientName, ingredientId);
      conn.Close();
      return foundIngredient;
    }

    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE ingredients SET name = @newName WHERE id = @thisId;";

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

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM ingredients WHERE id = @thisId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM ingredients;";
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddRecipeToIngredientJoinTable(Recipe newIngredient)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO ingredients_recipes (ingredient_id, recipe_id) VALUES (@IngredientId, @RecipeId);";

      MySqlParameter recipe_id_param = new MySqlParameter();
      recipe_id_param.ParameterName = "@RecipeId";
      recipe_id_param.Value = newIngredient.GetId();
      cmd.Parameters.Add(recipe_id_param);

      MySqlParameter ingredient_id_param = new MySqlParameter();
      ingredient_id_param.ParameterName = "@IngredientId";
      ingredient_id_param.Value = _id;
      cmd.Parameters.Add(ingredient_id_param);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Recipe> GetRecipes()
    {
     MySqlConnection conn = DB.Connection();
     conn.Open();
     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"SELECT recipes.*
       FROM ingredients
       JOIN ingredients_recipes ON(ingredients.id = ingredients_recipes.ingredient_id)
       JOIN recipes ON(recipes.id = ingredients_recipes.recipe_id)
       WHERE ingredient_id = @ingredientId;";

     MySqlParameter ingredientIdParameter = new MySqlParameter();
     ingredientIdParameter.ParameterName = "@ingredientId";
     ingredientIdParameter.Value = _id;
     cmd.Parameters.Add(ingredientIdParameter);

     var rdr = cmd.ExecuteReader() as MySqlDataReader;
     List<Recipe> allRecipes = new List<Recipe> {};
       while(rdr.Read())
       {
         int thisRecipeId = rdr.GetInt32(0);
         string recipeName = rdr.GetString(1);
         string recipeInstructions = rdr.GetString(2);
         int recipeRating = rdr.GetInt32(3);
         Recipe foundRecipe = new Recipe(recipeName, recipeInstructions, recipeRating, thisRecipeId);
         allRecipes.Add(foundRecipe);
       }
     conn.Close();
     if (conn != null)
     {
       conn.Dispose();
     }
     return allRecipes;
   }

  }
}
