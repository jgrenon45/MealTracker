using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTracker.Entities
{
    public enum UnitType
    {
        Grams,
        Kilograms,
        Lbs,
        Milliliters,
        Liters,
        Units,
        Packs,
        Tablespoons,
        Teaspoons,
        Cups
    }

    public partial class Ingredient : ObservableObject
    {
        public int Id { get; }
        
        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public bool isNeeded;

        // Simple constructor for basic ingredient creation
        public Ingredient(int id, string name)
        {
            Id = id;
            Name = name;
        }
        
    }      
}
