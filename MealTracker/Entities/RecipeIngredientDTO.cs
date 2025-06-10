using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTracker.Entities
{
    class RecipeIngredientDTO
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public int IngredientId { get; set; }

        public double Quantity { get; set; }

        public UnitType Unit { get; set; }

        [Ignore]
        public IngredientDTO? Ingredient { get; set; }
    }
}
