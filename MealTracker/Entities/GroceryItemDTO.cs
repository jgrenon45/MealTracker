using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTracker.Entities
{
    class GroceryItemDTO
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int IngredientId { get; set; }

        public double Quantity { get; set; }

        public UnitType UnitType { get; set; }

        public bool IsChecked { get; set; }

        [Ignore]
        public IngredientDTO? Ingredient { get; set; }
    }
}
