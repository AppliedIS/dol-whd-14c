﻿using System;
using DOL.WHD.Section14c.Domain.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace DOL.WHD.Section14c.Test.Domain.ViewModels
{
    [TestClass]
    public class AddApplicationSaveTests
    {
        [TestMethod]
        public void AddApplicationSave_PublicProperties()
        {
            // Arrange
            var ein = "30-1234567";
            var state = JObject.Parse(@"
                {
                    ""userId"" : ""5"",
                    ""email"" : ""foo@bar.com""
                }
            ");
            

            // Act
            var obj = new AddApplicationSave
            {
                EIN = ein,
                State = state
            };

            // Assert
            Assert.AreEqual(ein, obj.EIN);
            Assert.AreEqual(state, obj.State);
        }
    }
}
