using HealthcareMini.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace HealthcareMini.Models.Entitys
{
    public class Receptionist : User, IEmployee
    {
        public double Salary { get; set; }

        // Employee can work at multiple centers
        public ICollection<HealthCareCenter> HealthCareCenters { get; set; } = [];
    }
}