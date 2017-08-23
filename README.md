# _Recipe Box_

#### _A web app for storing recipes, Aug 18, 2017_

#### By _**Charlie Kelson**_

## Description

_This web app will allow a hair salon owner to add a list of stylist, and for each stylist, add recipes who see that stylist. The stylists work independently, so each client only belongs to a single stylist._


### User Story

| User Behavior | Input | Output |
|----|----|----|  
| As a user, I want to add a recipe with ingredients and instructions, so I remember how to prepare my favorite dishes. | Chocolate Chip Cookies, Flour - Sugar - Egg...., Add Flour... | Chocolate Chip Cookies, Flour - Sugar - Egg...., Add Flour... |
| As a user, I want to tag my recipes with different categories, so recipes are easier to find. A recipe can have many tags and a tag can have many recipes. |Dessert | Dessert - Chocolate Chip Cookies...|
| As a user, I want to be able to update and delete tags, so I can have flexibility with how I categorize recipes. | Dessert - 15 Minutes and under / Delete tag without deleting recipes under tag | Update/Delete |
| As a user, I want to edit my recipes, so I can make improvements or corrections to my recipes. | Chocolate Chip Cookies, Flour(Gluten Free) - Sugar - Egg...., Add Flour...  | Chocolate Chip Cookies, Flour(Gluten Free) - Sugar - Egg...., Add Flour...|
| As a user, I want to be able to delete recipes I don't like or use, so I don't have to see them as choices. | Delete Chocolate Chip Cookie Recipe | Chocolate Chip Cookie deleted|
|As a user, I want to rate my recipes, so I know which ones are the best. | Chocolate Chip Cookies - 5 stars |Chocolate Chip Cookies - 5 stars|
|As a user, I want to list my recipes by highest rated so I can see which ones I like the best. | Chocolate Chip Cookies - 5 stars, Turkey Sandwich - 4 Stars -- Sort by highest rating |Chocolate Chip Cookies - 5 stars, Turkey Sandwich - 4 Stars -- Sort by highest rating|
|As a user, I want to see all recipes that use a certain ingredient, so I can more easily find recipes for the ingredients I have. | Flour - Chocolate Chip Cookies, Apple Pie, Fried Chicken |Flour - Chocolate Chip Cookies, Apple Pie, Fried Chicken|

![](/schema.png)

### Technical Specs

| App Behavior | Expected | Actual |
|----|----|----|  
|  Get all categories at first position in database | 0 | Database List<Category> count start at 0 |
|  Save Category to database|  Local List<Category> = {Dessert}  | Database List<Category> = {Dessert}   |
|  Find Category from database by id|  Dessert  |  Dessert  |
| Get all recipes at first position in database | 0 | Database List<Recipes> count start at 0|
|  Save recipe to database | List with one recipe: Chocolate Chip Cookies | List with one recipe: Chocolate Chip Cookies |
|  Find recipe from database by id| Chocolate Chip Cookies  |  Chocolate Chip Cookies  |
| Add category to recipe | Dessert: Chocolate Chip Cookies | Dessert: Chocolate Chip Cookies
| Add multiple categories to a recipe | Baking, Dessert: Chocolate Chip Cookies | Baking, Dessert: Chocolate Chip Cookies |
|  Get all recipes of a specific category by category name |  Dessert: Chocolate Chip Cookies  |  Dessert: Chocolate Chip Cookies  |
|  Update recipes name | Chocolate Chip Walnut Cookies | Chocolate Chip Walnut Cookies |
|  Delete recipe | A list of only one recipe rather than two | A database query that only returns one recipe after delete method has been called |
| Remove category from recipe | A recipe with two categories, now has one | A recipe with one category tag, previously with two |
| Add rating to a recipe | Rating is created ||
| Sort recipes by rating | Query sorts table by rating | Sorts table by rating |


## Setup/Installation Requirements

* _Download and install [.NET Core 1.1 SDK](https://www.microsoft.com/net/download/core)_
* _Download and install [Mono](http://www.mono-project.com/download/)_
* _Download and install [MAMP](https://www.mamp.info/en/)_
* _Clone repository_

#### MySQL commands to create database


---

## Known Bugs

_No known Bugs_



## Technologies Used

* _ASP.NET MVC_
* MySQL

### License

MIT License

Copyright (c) 2015 **_Charlie Kelson_**
