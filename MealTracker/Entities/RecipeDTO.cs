using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MealTracker.Entities
{
    class RecipeDTO
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        [Ignore]
        public List<string>? Ingredients { get; set; } = new List<string>();

        // Backing property stored in the DB
        public string IngredientsJson
        {
            get => JsonSerializer.Serialize(Ingredients);
            set => Ingredients = string.IsNullOrEmpty(value)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(value);
        }

        public string? Instructions { get; set; }

        public TimeSpan PreparationTime { get; set; }

        public TimeSpan CookingTime { get; set; }

        public int Servings { get; set; }
    }
}
