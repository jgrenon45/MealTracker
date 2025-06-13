using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealTracker.Entities
{
    class InstructionDTO
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey("RecipeId")]
        public int RecipeId { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public TimeSpan EstimatedTime { get; set; }

        public bool IsCompleted { get; set; }
    }
}
