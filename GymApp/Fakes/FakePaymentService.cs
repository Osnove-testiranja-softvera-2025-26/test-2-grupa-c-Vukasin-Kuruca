using GymApp.Models;
using GymApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Fakes
{
    public class FakePaymentService : IPaymentService 
    {
        
            public Guid TrainerId { get; set; }
            public BonusPayment Payment { get; set; }

            public void UpdateTrainerBonusPayment(Guid trainerId, BonusPayment payment)
            {
                TrainerId = trainerId;
                Payment = payment;
            }
        }
    }
