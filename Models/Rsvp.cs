using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Exam.Models
{
    public class Rsvp
    {
        [Key]
        public int RsvpId {get; set;}
        public int UserId {get; set;}
        public int PlanId {get; set;}
        public User Participent {get; set;}
        public Plan Plans {get; set;}
    }
}