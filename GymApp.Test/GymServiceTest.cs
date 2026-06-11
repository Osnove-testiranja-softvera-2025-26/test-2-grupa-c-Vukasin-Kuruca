using GymApp.Exceptions;
using GymApp.Fakes;
using GymApp.Models;
using GymApp.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;



namespace GymApp.Test
{
    //Guid example: "00000000-0000-0000-0000-000000000001"
    [TestFixture]
    public class GymServiceTests
    {
        public FakePaymentService _paymentService;
        public FakeTrainingService _trainingService;
        public FakeTrainerPerformanceService _performanceService;
        public GymService _gymService;

            [SetUp]
            public void Setup()
            {
                _paymentService = new FakePaymentService();
                _trainingService = new FakeTrainingService();
                _performanceService = new FakeTrainerPerformanceService();
            _gymService = new GymService(_paymentService, _trainingService, _performanceService);
                
            }
         
        [Test]
        public void DoStaffBonusPaymentCalculation_NoTrainings_ThrowsException()
        { 

            var paymentService = new FakePaymentService();
            var trainingService = new FakeTrainingService
            {
                Trainings = new List<Training>()
            };
            var performanceService = new FakeTrainerPerformanceService();

            var service = new GymService(paymentService, trainingService, performanceService);

            var trainer = new Trainer { Id = Guid.NewGuid() };

            Assert.Throws<NoTrainingsInTheLastMonthException>((TestDelegate)(() => service.DoStaffBonusPaymentCalculation(trainer)));


        }
        [Test]
        public void DoStaffBonusPaymentCalculation_SecondRank_MoreThan13FreeDays_Bonus150()
        {
            Trainer trainer = new Trainer { Id = Guid.NewGuid() };

            _trainingService.Trainings = new List<Training>
            {
                new Training { Type = TrainingType.Personal }
            };

            _performanceService.Report = new PerformanceReport
            {
                PerformanceRank = PerformanceRank.Second,
                PercentOfTrainingsNotHeld = 5,
                NumberOfFreeDaysLeft = 14
            };

            _gymService.DoStaffBonusPaymentCalculation(trainer);

            Assert.That(_paymentService.Payment.Amount, Is.EqualTo(150));
        }

        [Test]
        public void DoStaffBonusPaymentCalculation_SecondRank_13OrLessFreeDays_Bonus120()
        {
            Trainer trainer = new Trainer { Id = Guid.NewGuid() };

            _trainingService.Trainings = new List<Training>
            {
                new Training { Type = TrainingType.Personal }
            };

            _performanceService.Report = new PerformanceReport
            {
                PerformanceRank = PerformanceRank.Second,
                PercentOfTrainingsNotHeld = 5,
                NumberOfFreeDaysLeft = 13
            };

            _gymService.DoStaffBonusPaymentCalculation(trainer);

            Assert.That(_paymentService.Payment.Amount, Is.EqualTo(120));
        }

        [Test]
        public void DoStaffBonusPaymentCalculation_FirstRank_SevenGroupTrainings_Bonus200()
        {
            Trainer trainer = new Trainer { Id = Guid.NewGuid() };

            _trainingService.Trainings = new List<Training>();

            for (int i = 0; i < 7; i++)
            {
                _trainingService.Trainings.Add(
                    new Training { Type = TrainingType.Group });
            }

            _performanceService.Report = new PerformanceReport
            {
                PerformanceRank = PerformanceRank.First,
                PercentOfTrainingsNotHeld = 20,
                NumberOfFreeDaysLeft = 0
            };

            _gymService.DoStaffBonusPaymentCalculation(trainer);

            Assert.That(_paymentService.Payment.Amount, Is.EqualTo(200));
        }

        [Test]
        public void DoStaffBonusPaymentCalculation_FirstRank_NoConditionMet_Bonus150()
        {
            Trainer trainer = new Trainer { Id = Guid.NewGuid() };

            _trainingService.Trainings = new List<Training>
            {
                new Training { Type = TrainingType.Personal }
            };

            _performanceService.Report = new PerformanceReport
            {
                PerformanceRank = PerformanceRank.First,
                PercentOfTrainingsNotHeld = 10,
                NumberOfFreeDaysLeft = 0
            };

            _gymService.DoStaffBonusPaymentCalculation(trainer);

            Assert.That(_paymentService.Payment.Amount, Is.EqualTo(150));
        }

        [Test]
        public void DoStaffBonusPaymentCalculation_OtherRank_Bonus0()
        {
            Trainer trainer = new Trainer { Id = Guid.NewGuid() };

            _trainingService.Trainings = new List<Training>
            {
                new Training { Type = TrainingType.Personal }
            };

            _performanceService.Report = new PerformanceReport
            {
                PerformanceRank = PerformanceRank.Third,
                PercentOfTrainingsNotHeld = 0,
                NumberOfFreeDaysLeft = 0
            };

            _gymService.DoStaffBonusPaymentCalculation(trainer);

            Assert.That(_paymentService.Payment.Amount, Is.EqualTo(0));
        }


        [TestCaseSource(typeof(PiktParser.PictParser), nameof(PiktParser.PictParser.GetTestCases))]
        public void GetMemberhipType_ReturnsExpectedMembership(int numberOfMonths,bool groupTrainings,double monthlyPriceBudget,TrainingTime trainingTime,MembershipType? expected)
        {
            MembershipType? result = _gymService.GetMemberhipType(numberOfMonths, groupTrainings, monthlyPriceBudget, trainingTime);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
    }



