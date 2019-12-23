using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Exam.Models
{
    public class Plan
    {
        [Key]
        public int PlanId {get; set;}
        [Required]
        [MinLength(5)]
        [Display(Name="Activity Name: ")]
        public string ActName {get; set;}
        [Required]
        [MinLength(10)]
        [Display(Name="Description: ")]
        public string Description {get; set;}
        [Required]
        [FutureDate]
        public DateTime Date {get; set;}
        [Required]
        public string Duration {get; set;}
        public int UserId {get; set;}
        public User Planner {get; set;}
        public List<Rsvp> ShowingUp {get; set;}
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
    }
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date;
            if(value is DateTime)
            {
                date = (DateTime)value;
            }
            else
            {
                return new ValidationResult("Invalid DateTime");
            }
            if(date < DateTime.Now)
            {
                return new ValidationResult("Dont be a kuncklehead, date has to be in the future! :) ");
            }
            return ValidationResult.Success;
        }
    }
}