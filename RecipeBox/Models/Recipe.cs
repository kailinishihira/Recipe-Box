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

    public Student(string name, string instructions, int rating = 0, int id = 0)
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

    public string GetRating()
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
       int rating = rdr.GetString(3);

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
      cmd.CommandText = @"INSERT INTO recipes (name, instructions) VALUES (@name, @instructions);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      MySqlParameter instructions = new MySqlParameter();
      instructions.ParameterName = "@instructions";
      instructions.Value = this._instructions;
      cmd.Parameters.Add(instructions);

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
      Recipe foundRecipe = new Recipe(recipeName, recipeName, recipeInstructions, recipeId);
      conn.Close();
      return foundRecipe;
    }

    public void Update(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE students SET name = @newName WHERE id = @thisId;";

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
      cmd.CommandText = @"DELETE FROM students WHERE id = @thisId;";

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
      cmd.CommandText = @"DELETE FROM students;";
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void AddCourseToJoinTable(Course newCourse)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId, @CourseId);";

      MySqlParameter course_id_param = new MySqlParameter();
      course_id_param.ParameterName = "@CourseId";
      course_id_param.Value = newCourse.GetId();
      cmd.Parameters.Add(course_id_param);

      MySqlParameter student_id_param = new MySqlParameter();
      student_id_param.ParameterName = "@StudentId";
      student_id_param.Value = _id;
      cmd.Parameters.Add(student_id_param);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Course> GetCourses()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT courses.* FROM students
          JOIN students_courses ON (students.id = students_courses.student_id)
          JOIN courses ON (courses.id = students_courses.course_id)
          WHERE students.id = @StudentId;";

        MySqlParameter studentIdParameter = new MySqlParameter();
        studentIdParameter.ParameterName = "@StudentId";
        studentIdParameter.Value = _id;
        cmd.Parameters.Add(studentIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        List<Course> courses = new List<Course>{};

        while(rdr.Read())
        {
          int courseId = rdr.GetInt32(0);
          string courseName = rdr.GetString(1);
          string courseNumber = rdr.GetString(2);
          Course newCourse = new Course(courseName, courseNumber, courseId);
          courses.Add(newCourse);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return courses;
    }

  }
}
