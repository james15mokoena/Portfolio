﻿using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class Administrator
    {
        [Key]
        public int Id { get; set; }
        
        public string? Email { get; set; }
        
        public string? Password { get; set; }

        public string? Phone { get; set; }        
    }
}
