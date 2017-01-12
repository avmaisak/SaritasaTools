﻿using System;
using System.Configuration;
using System.Linq;
using Autofac;
using NUnit.Framework;
using Saritasa.BoringWarehouse.Domain;
using Saritasa.BoringWarehouse.Domain.Products.Commands;
using Saritasa.BoringWarehouse.Domain.Products.Queries;
using Saritasa.Tools.Commands;

namespace Saritasa.BoringWarehouse.IntegrationTests
{
    public class CompanyTest
    {
        private IContainer container;
        private ICommandPipeline commandPipeline;

        [SetUp]
        public void SetUp()
        {
            container = DIConfig.Container;
            commandPipeline = container.Resolve<ICommandPipeline>();
        }

        [TestCase]
        public void TestCompanyCreation()
        {
            using (var uow = container.Resolve<IAppUnitOfWork>())
            {
                var query = new CompanyQueries(uow);
                var count1 = query.GetAll().Count();

                var command = new CreateCompanyCommand();
                command.CreatedByUserId = GlobalConfig.AdminId;
                command.Name = "Test Company " + DateTime.Now.Ticks;

                commandPipeline.Handle(command);

                var count2 = query.GetAll().Count();
                Assert.AreEqual(count1 + 1, count2, "number of companies");
            }
        }
    }
}