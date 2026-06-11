using GymApp.Models;
using GymApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Fakes
{
    public class FakeTrainerPerformanceService : ITrainerPerformanceService
    {
        
            public PerformanceReport Report { get; set; }

            public PerformanceReport GetTrainerPerformanceReport(Guid trainerId)
            {
                return Report;
            }
        }
    }

