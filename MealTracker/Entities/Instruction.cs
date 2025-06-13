using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealTracker.Entities
{
    public partial class Instruction : ObservableObject
    {
        public int Id { get; }

        [ObservableProperty]
        public string description;

        [ObservableProperty]
        public int order;
        
        [ObservableProperty]
        public TimeSpan estimatedTime;

        [ObservableProperty]
        public bool isCompleted;

        public Instruction(int id, string stepDescription, int stepNumber, TimeSpan estimatedTime, bool isCompleted)
        {
            Id = id;
            Description = stepDescription;
            Order = stepNumber;
            EstimatedTime = estimatedTime;
            IsCompleted = isCompleted;
        }

        public Instruction(string description, int order)
        {
            Description = description;
            Order = order;
        }
    }
}
