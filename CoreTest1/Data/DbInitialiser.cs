using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTest1.Models;

namespace CoreTest1.Data
{
    public class DbInitialiser
    {
        public static void Initialise(RocketContext context)
        {

            if(!context.PartTypes.Any())
            {
                var TestTypes = new PartType[]
                {
                    new PartType {Name = "Двигун", Units = "од."},
                    new PartType {Name = "Паливо", Units = "л."},
                    new PartType {Name = "Окиснювач", Units = "л."}
                };

                foreach(var TestType in TestTypes)
                {
                    context.PartTypes.Add(TestType);
                }

                context.SaveChanges();
            }

            if (!context.Parts.Any())
            {
                var TestParts = new Part[]
                {
                new Part {Name = "RD-120", Type = 1 },
                new Part {Name = "RD-110", Type = 1 },
                new Part {Name = "RD-100", Type = 1 },
                new Part {Name = "RD-150A", Type = 1 },
                new Part {Name = "RD-113", Type = 1 }
                };

                foreach (var TestPart in TestParts)
                {
                    TestPart.PartType = context.PartTypes.First();
                    context.Parts.Add(TestPart);
                }

                context.SaveChanges();
            }

            if (!context.Customers.Any())
            {
                var TestCustomers = new Customer[]
                {
                    new Customer {Name = "SpaceK"},
                    new Customer {Name = "KASA"},
                    new Customer {Name = "CAS"}
                };
                
                foreach (var TestCustomer in TestCustomers)
                {
                    context.Customers.Add(TestCustomer);
                }
            }

            if(!context.Positions.Any())
            {
                var TestPos = new Position[]
                {
                    new Position{EmployeeID = context.Employees.First().ID, StockID  = context.Stocks.First().ID}
                };
                foreach(var TP in TestPos)
                {
                    context.Positions.Add(TP);
                }
            }

            if(!context.Employees.Any())
            {
                var TestEmpl = new Employee[]
                {
                    new Employee{FirstName = "Андрій", Surname = "Клячкін"},
                    new Employee{FirstName = "НеАндрій", Surname = "НеКлячкін" }
                };
                foreach (var TE in TestEmpl)
                {
                    context.Employees.Add(TE);
                }
            }

            if(!context.Stocks.Any())
            {
                var TestStocks = new Stock[]
                {
                    new Stock{Address = "Київ, Флоренції 12Б"},
                    new Stock{Address = "Донецьк, Взльотна 2"}
                };
                foreach (var TS in TestStocks)
                {
                    context.Stocks.Add(TS);
                }

                context.SaveChanges();
            }

            if (!context.Contracts.Any())
            {
                var TestContr = new Contract[]
                {
                    new Contract{ CustomerID = context.Customers.First().ID, SignDate =  DateTime.Parse("04.11.2018")},
                    new Contract{ CustomerID = context.Customers.First().ID, SignDate =  DateTime.Parse("02.06.2014")}
                };
                foreach (var Contract in TestContr)
                {
                    context.Contracts.Add(Contract);
                }

                context.SaveChanges();
            }

            if(!context.ContractItems.Any())
            {
                var Ci = new PartInContract { ContractID = context.Contracts.First().ID, PartID = context.Parts.First().ID, Quantity = 1};
                context.ContractItems.Add(Ci);
                context.Contracts.First().PartsInContr.Add(Ci);
            }

            context.SaveChanges();
        }
    }
}
