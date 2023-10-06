using Azure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationWaterSupplySol
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (OrganizationsWaterSupplyContext db = new OrganizationsWaterSupplyContext())
            {
                Select(db);
                Insert(db);
                Delete(db);
                Update(db);
            }

            static void Print(string sqltext, IEnumerable items)
            {
                Console.WriteLine(sqltext);
                Console.WriteLine("Записи: ");
                foreach (var item in items)
                {
                    Console.WriteLine(item.ToString());
                }
                Console.WriteLine();
                Console.ReadKey();
            }
            static void Select(OrganizationsWaterSupplyContext db)
            {

                var linq1 = from cm in db.CounterModels
                                    select new
                                    {
                                        ModelName = cm.ModelName,
                                        Manufacturer = cm.Manufacturer,
                                        ServiceTime = cm.ServiceTime,
                                    };
                string comment1 = "1. Результат выполнения запроса на выборку всех данных из таблицы: \r\n";

                Print(comment1, linq1.ToList());

                var linq2 = from cm in db.CounterModels
                                        where (cm.ServiceTime > 20)
                                        select new
                                        {
                                            ModelName = cm.ModelName,
                                            Manufacturer = cm.Manufacturer,
                                            ServiceTime = cm.ServiceTime,
                                        };
                string comment2 = "2. Результат выполнения запроса на выборку отфильтрованных данных: \r\n";
                Print(comment2, linq2.ToList());

                var linq3 = from c in db.Counters
                            join cm in db.CounterModels
                            on c.ModelId equals cm.ModelId
                            group c.RegistrationNumber by cm.ServiceTime into gr
                            select new
                            {
                                 ModelId = gr.Key,
                                 TimeOfInstallation = gr.Max()
                             };
                string comment3 = "3. Результат выполнения запроса на выборку с группировкой данных из таблицы: \r\n";
                Print(comment3, linq3.ToList());
                var linq4 = from c in db.Counters
                            join cm in db.CounterModels
                            on c.ModelId equals cm.ModelId
                            select new
                            {
                                CounterId = c.RegistrationNumber,
                                Model = cm.ModelName,
                                ServiceTime = cm.ServiceTime,
                                TimeOfIsntallation = c.TimeOfInstallation,
                                OrganizationId = c.OrganizationId
                            };
                string comment4 = "4. Результат выполнения запроса на выборку полей с двух таблиц: \r\n";
                Print(comment4, linq4.ToList());
                var linq5 = from c in db.Counters
                            join cm in db.CounterModels
                            on c.ModelId equals cm.ModelId
                            where (cm.ServiceTime > 20 && cm.ServiceTime < 30)
                            select new
                            {
                                CounterId = c.RegistrationNumber,
                                Model = cm.ModelName,
                                ServiceTime = cm.ServiceTime,
                                TimeOfIsntallation = c.TimeOfInstallation,
                                OrganizationId = c.OrganizationId
                            };
                string comment5 = "5. Результат выполнения запроса на выборку полей с двух таблиц с фильтрацией: \r\n";
                Print(comment5, linq5.ToList());
            };
            static void Insert(OrganizationsWaterSupplyContext db)
            {
                CounterModel newModel = new CounterModel
                {
                    ModelName = "ModelExample",
                    Manufacturer = "ManufacturerExample",
                    ServiceTime = 40

                };
                Organization newOrg = new Organization
                {
                    OrgName = "OrgExample",
                    OwnershipType = "Ownership",
                    Adress = "Adress",
                    DirectorFullname = "Director",
                    DirectorPhone = "+375123456",
                    ResponsibleFullname = "Responsible",
                    ResponsiblePhone = "+3754412341"
                };
                db.CounterModels.Add(newModel);
                db.Organizations.Add(newOrg);
                db.SaveChanges();
                Counter newCounter = new Counter
                {
                    ModelId = newModel.ModelId,
                    TimeOfInstallation = new DateTime(2020,12,24),
                    OrganizationId = newOrg.OrganizationId
                };

                db.Counters.Add(newCounter);
                db.SaveChanges();

            }
            static void Delete(OrganizationsWaterSupplyContext db)
            {
                string namemanufact = "ManufacturerToDelete";
                var manufact = db.CounterModels.Where(cm => cm.Manufacturer == namemanufact);

                string nameorg = "OrgToDelete";
                var org  = db.Organizations
                    .Where(c => c.OrgName == nameorg);

                var someCounters = db.Counters
                    .Include("Organization")
                    .Include("Model")
                    .Where(o => ((o.Organization.OrgName == nameorg)) && (o.Model.Manufacturer == namemanufact));

                db.Counters.RemoveRange(someCounters);
                db.SaveChanges();

                db.CounterModels.RemoveRange(manufact);
                db.Organizations.RemoveRange(org);

                db.SaveChanges();

            }
            static void Update(OrganizationsWaterSupplyContext db)
            {
                
                var oldCounter = db.Counters.Where(c => (c.RegistrationNumber == 6));
                //обновление
                if (oldCounter != null)
                {
                    foreach (var c in oldCounter)
                    {
                        Counter counter = (Counter)c;
                        counter.TimeOfInstallation = new DateTime(2030, 01, 01);
                    };

                    
                }
                db.SaveChanges();

            }


        }
    }
}


