using HealthcareMini.Data;
using HealthcareMini.DTOs;
using HealthcareMini.Models.Entitys;
using HealthcareMini.Models.Enums;
using HealthcareMini.Models.Interfaces;
using HealthcareMini.Models.Objects;
using HealthcareMini.Services.HealthCareCenterServices;
using Microsoft.EntityFrameworkCore;

namespace HealthcareMini.Controllers.CreationalPattrens.Factory
{
    public class AccountTypeFactory
    {
        private readonly HealthcareDbContext _context;
        private HealthCareCenterServices _centerService;

        public AccountTypeFactory(HealthcareDbContext context)
        {
            _context = context;
            _centerService = new HealthCareCenterServices(context);
        }

       
    }
}