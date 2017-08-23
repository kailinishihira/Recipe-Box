using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RecipeBox.Models
{
  public class  Category
  {
    private int _id;
    private string _name;

    public Category(string name, int id = 0)
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


    public override bool Equals(Object otherCategory)
    {
      if (!(otherCategory is Category))
      {
        return false;
      }
      else
      {
        Category newCategory = (Category) otherCategory;

        bool idEquality = (this.GetId() == newCategory.GetId());
        bool nameEquality = (this.GetName() == newCategory.GetName());


        return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public static List<Category> GetAll()
    {
     List<Category> categoryList = new List<Category> {};

     MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"SELECT * FROM categories;";

     var rdr = cmd.ExecuteReader() as MySqlDataReader;
     while(rdr.Read())
     {
       int categoryId = rdr.GetInt32(0);
       string name = rdr.GetString(1);
       Category newCategory = new Category(name, categoryId);
       categoryList.Add(newCategory);
     }
     conn.Close();
     return categoryList;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO categories (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);


      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
    }

    public static Category Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories WHERE id = @categoryId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@categoryId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int categoryId = 0;
      string categoryName = "";


      while(rdr.Read())
      {
        categoryId = rdr.GetInt32(0);
        categoryName = rdr.GetString(1);

      }
      Category foundCategory = new Category(categoryName, categoryId);
      conn.Close();
      return foundCategory;
    }

    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE categories SET name = @newName WHERE id = @thisId;";

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
      cmd.CommandText = @"DELETE FROM categories WHERE id = @thisId;";

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
      cmd.CommandText = @"DELETE FROM categories;";
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddRecipeToCategoryJoinTable(Recipe newCategory)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO categories_recipes (recipe_id, category_id) VALUES (@RecipeId, @CategoryId);";

      MySqlParameter recipe_id_param = new MySqlParameter();
      recipe_id_param.ParameterName = "@RecipeId";
      recipe_id_param.Value = newCategory.GetId();
      cmd.Parameters.Add(recipe_id_param);

      MySqlParameter category_id_param = new MySqlParameter();
      category_id_param.ParameterName = "@CategoryId";
      category_id_param.Value = _id;
      cmd.Parameters.Add(category_id_param);

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
       FROM categories
       JOIN categories_recipes ON(categories.id = categories_recipes.category_id)
       JOIN recipes ON(recipes.id = categories_recipes.recipe_id)
       WHERE category_id = @categoryId;";

     MySqlParameter categoryIdParameter = new MySqlParameter();
     categoryIdParameter.ParameterName = "@categoryId";
     categoryIdParameter.Value = _id;
     cmd.Parameters.Add(categoryIdParameter);

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
