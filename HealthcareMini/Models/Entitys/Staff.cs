using HealthcareMini.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Entitys
{
    public class Staff : User, IEmployee
    {
        public double Salary { get; set; }

        [MaxLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        // Employee can work at multiple centers
        public ICollection<HealthCareCenter> HealthCareCenters { get; set; } = [];
    }
}