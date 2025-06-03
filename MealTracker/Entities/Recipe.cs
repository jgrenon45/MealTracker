using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTracker.Entities
{
    public class Recipe
    {
        public int Id { get; }

        public string Name { get; }

        public string? Description { get; }

        public List<string>? Ingredients { get; } = new List<string>();

        public string? Instructions { get; }

        public TimeSpan? PreparationTime { get; }

        public TimeSpan? CookingTime { get; }

        public int? Servings { get; }

        //Simple constructor for basic recipe creation
        public Recipe(int id, string name)
        {
            Id = id;
            Name = name;           
        }

        //Full constructor for detailed recipe creation
        public Recipe(int id, string name, string description, List<string> ingredients, string instructions, TimeSpan preparationTime, TimeSpan cookingTime, int servings)
        {
            Id = id;
            Name = name;
            Description = description;
            Ingredients = ingredients;
            Instructions = instructions;
            PreparationTime = preparationTime;
            CookingTime = cookingTime;
            Servings = servings;
        }
    }
}
