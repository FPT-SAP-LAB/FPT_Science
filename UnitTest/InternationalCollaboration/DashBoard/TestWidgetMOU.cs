﻿using BLL.InternationalCollaboration.Dashboard;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.Dashboard;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.InternationalCollaboration.DashBoard
{
    [TestFixture]
    public class TestWidgetMOU
    {
        [TestCase]
        public void TestMethod1()
        {
            //Arrange
            int year = 2019;

            //Act
            DashboardRepo dashBoardRepo = new DashboardRepo();
            string widgetMOU = dashBoardRepo.WidgetMou(year);

            //Assert
            Assert.Pass();
        }
        [TestCase]
        public void TestMethod2()
        {
            //Arrange
            int year = 2021;

            //Act
            DashboardRepo dashBoardRepo = new DashboardRepo();
            string widgetMOU = dashBoardRepo.WidgetMou(year);

            //Assert
            Assert.Pass();
        }
    }
}
