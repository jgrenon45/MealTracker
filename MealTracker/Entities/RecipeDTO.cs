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

        public string ImagePath { get; set; }

        [Ignore]
        public List<RecipeIngredientDTO> Ingredients { get; set; }

        [Ignore]
        public List<InstructionDTO> Instructions { get; set; }

        public TimeSpan PreparationTime { get; set; }

        public TimeSpan CookingTime { get; set; }

        public int Servings { get; set; }
    }
}
