using GymApp.Exceptions;
using GymApp.Models;
using GymApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Fakes
{
    public class FakeTrainingService : ITrainingService
    {
        public List<Training> Trainings { get; set; } = new List<Training>();

        public NoTrainingsInTheLastMonthException ex = new NoTrainingsInTheLastMonthException();

        public List<Training> GetTrainingsInTheLastMonth(Guid trainerId)
        {
            if (ex != null)
            {
                throw new NoTrainingsInTheLastMonthException("Bonus payment cannot be calculated");
            }

            return Trainings;
        }
    }
}
